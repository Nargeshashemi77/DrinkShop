using DrinkShop.Data;
using DrinkShop.Models;
using DrinkShop.Models.View_Models;
using DrinkShop.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrinkShop.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class BuyerController : Controller
    {
        private DrinkShopDbContext _context;
        private UserManager<User> _userManager;
        private const string BaseUrl = "assets/images/user-image";
        public BuyerController(DrinkShopDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region User Profile
        [HttpGet]
        public async Task<IActionResult> Profile(string? message, string? Url)
        {
            if (message == "Order")
            {
                ViewData["OrderMessage"] = message;
                ViewData["Url"] = Url;
            }
            else
            {
                ViewData["OrderMessage"] = "";
                ViewData["Url"] = "";
            }


            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null) { return NotFound(); }

            UserProfileViewModel userModel = new UserProfileViewModel
            {
                user = user
            };
            return View(userModel);
        }
        [HttpPost]
        public async Task<IActionResult> Profile([FromForm] UserProfileViewModel updateUser, string? message, string? Url)
        {
            try
            {
                if (message == "Order")
                {
                    ViewData["OrderMessage"] = message;
                    ViewData["Url"] = Url;
                }
                else
                {
                    ViewData["OrderMessage"] = "";
                    ViewData["Url"] = "";
                }

                #region Validation
                bool isNumber;
                isNumber = long.TryParse(updateUser.user.PhoneNumber, out long phone);
                long number = phone;
                int count = 0;
                while (number > 0)
                {
                    number = number / 10;
                    count++;
                }
                if (updateUser.user.Name == null)
                {
                    ModelState.AddModelError("", "لطفا نام را وارد کنید");
                    return View(updateUser);
                }
                if (updateUser.user.Family == null)
                {
                    ModelState.AddModelError("", "لطفا نام خانوادگی را وارد کنید");
                    return View(updateUser);
                }
                if (updateUser.user.Address == null)
                {
                    ModelState.AddModelError("", "لطفا آدرس را وارد کنید");
                    return View(updateUser);
                }
                if (updateUser.user.PhoneNumber == null)
                {
                    ModelState.AddModelError("", "لطفا شماره تماس را وارد کنید");
                    return View(updateUser);
                }
                if (isNumber == false || count != 10)
                {
                    ModelState.AddModelError("", "شماره تماس معتبر نیست");
                    return View(updateUser);
                }
                #endregion
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                if (user == null) { return NotFound(); }

                user.Name = updateUser.user.Name;
                user.Family = updateUser.user.Family;
                user.Address = updateUser.user.Address;
                user.PhoneNumber = updateUser.user.PhoneNumber;

                if (updateUser.userImage != null)
                {
                    string userImageUrl = saveImages.createImage(updateUser.userImage.FileName.ToString(), updateUser.userImage, BaseUrl);
                    user.userImage = userImageUrl;
                }

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (message == "Order")
                        return Redirect(Url);
                    else
                        return RedirectToAction("Profile", "Buyer");
                }
                else
                {
                    ModelState.AddModelError("", "مشکلی در سمت سرور پیش آمده است لطفا بعدا مجددا تلاش کنید");
                    return View(updateUser);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ModelState.AddModelError("", "مشکلی در سمت سرور پیش آمده است لطفا بعدا مجددا تلاش کنید");
                return View(updateUser);
            }
        }
        [HttpGet]
        public IActionResult ChangePassword(string? message)
        {
            if (message != null)
                ViewData["SuccessMessage"] = message;

            else
                ViewData["SuccessMessage"] = "";

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model, string? message)
        {
            try
            {
                if (message != null)
                    ViewData["SuccessMessage"] = message;

                else
                    ViewData["SuccessMessage"] = "";


                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                    bool checkOldPass = await _userManager.CheckPasswordAsync(user, model.oldPassword);
                    if (checkOldPass == false)
                    {
                        ModelState.AddModelError("", "رمز عبور قدیمی اشتباه است");
                        return View(model);

                    }
                    var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
                    if (result.Succeeded)
                        return RedirectToAction("ChangePassword", "Buyer", new { message = "Success" });
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        //ModelState.AddModelError("", "مشکلی در سمت سرور پیش آمده است لطفا بعدا مجددا تلاش کنید");
                        return View(model);
                    }
                }
                else
                    return View(model);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                ModelState.AddModelError("", "مشکلی در سمت سرور پیش آمده است لطفا بعدا مجددا تلاش کنید");
                return View(model);
            }
        }
        #endregion
        #region Favorites
        public async Task<IActionResult> Favorites()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) { return NotFound(); }

                var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
                if (buyer == null) { return NotFound(); }

                var favorites = _context.favorites.Where(f => f.buyerId == buyer.id)
                    .Include(f => f.product)
                    .Select(p => new Product
                    {
                        id = p.product.id,
                        Name = p.product.Name,
                        productImage = p.product.productImage,
                        Price = p.product.Price,
                        Unit = p.product.Unit,
                        Stock = p.product.Stock
                    }).ToList();
                return View(favorites);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public async Task<IActionResult> AddItemToFavorites(int itemId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return NotFound();
                var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
                if (buyer == null) return NotFound();
                var ExistItem = _context.favorites.Where(f => f.productId == itemId && f.buyerId == buyer.id).SingleOrDefault();
                if (ExistItem == null)
                {
                    Favorite newItem = new Favorite
                    {
                        productId = itemId,
                        buyerId = buyer.id
                    };
                    _context.favorites.Add(newItem);
                    _context.SaveChanges();
                }
                return RedirectToAction("Favorites", "Buyer");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public async Task<IActionResult> DeleteItemFromFavorites(int itemId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) return NotFound();
                var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
                if (buyer == null) return NotFound();

                var favorite = _context.favorites.Where(f => f.buyerId == buyer.id && f.productId == itemId).SingleOrDefault();
                if (favorite == null) return NotFound();
                _context.favorites.Remove(favorite);
                var result = _context.SaveChanges();
                if (result == 1)
                    return RedirectToAction("Favorites", "Buyer");
                else return StatusCode(404);
            }
            catch (Exception e)
            {
                Console.WriteLine("catched Internal Server Error: " + e.Message);
                return StatusCode(500);
            }
        }
        #endregion
    }
}
