using DrinkShop.Convertor;
using DrinkShop.Data;
using DrinkShop.Enum;
using DrinkShop.Models;
using DrinkShop.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DrinkShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private DrinkShopDbContext _context;
        private readonly UserManager<User> _userManager;

        public ReportsController(DrinkShopDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult UserOrders()
        {
            return View();
        }
        public async Task<IActionResult> UserCertainOrders(string username, int? orderprice, string startdate, string enddate)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound();

            var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
            if (buyer == null)
                return NotFound();
            DateTime StartDate = DateTime.Parse(startdate);
            DateTime EndDate = DateTime.Parse(enddate);

            List<UserCertainOrderViewModel> orders;

            if (orderprice != null)
                orders = _context.orders.IgnoreQueryFilters()
                .Include(o => o.product).Where(o => o.buyerId == buyer.id && o.product.Price > orderprice && (StartDate < o.createdAt && o.createdAt < EndDate))
                .Select(o => new UserCertainOrderViewModel
                {
                    orderId = o.Id,
                    productId = o.productId,
                    productImage = o.product.productImage,
                    productName = o.product.Name,
                    productPrice = o.product.Price,
                    orderDatetime = o.createdAt
                }).ToList();

            else
                orders = _context.orders.IgnoreQueryFilters()
                           .Include(o => o.product).Where(o => o.buyerId == buyer.id && (StartDate < o.createdAt && o.createdAt < EndDate))
                           .Select(o => new UserCertainOrderViewModel
                           {
                               orderId = o.Id,
                               productId = o.productId,
                               productImage = o.product.productImage,
                               productName = o.product.Name,
                               productPrice = o.product.Price,
                               orderDatetime = o.createdAt
                           }).ToList();

            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> OrderChart(string? orderStatus, string? period)
        {
            try
            {
                IQueryable<Order> ordersQueryable;
                DateTime startPoint;
                DateTime endPoint;

                if (period == "Today" || period == null)
                {
                    switch (orderStatus)
                    {
                        case "registered":
                            ordersQueryable = _context.orders.IgnoreQueryFilters().Include(p => p.product)
                           .Where(o => DateTime.Now.Date <= o.createdAt && o.createdAt.Date < DateTime.Now.Date.AddDays(1));
                            break;
                        case "finished":
                            ordersQueryable = _context.orders.IgnoreQueryFilters().Include(p => p.product)
                           .Where(o => (DateTime.Now.Date <= o.createdAt && o.createdAt.Date < DateTime.Now.Date.AddDays(1)) && (o.Status == OrderStatus.finished));
                            break;
                        case "canceled":
                            ordersQueryable = _context.orders.IgnoreQueryFilters().Include(p => p.product)
                           .Where(o => (DateTime.Now.Date <= o.createdAt && o.createdAt.Date < DateTime.Now.Date.AddDays(1)) && (o.Status == OrderStatus.canceled));
                            break;
                        case "pending":
                            ordersQueryable = _context.orders.IgnoreQueryFilters().Include(p => p.product)
                           .Where(o => (DateTime.Now.Date <= o.createdAt && o.createdAt.Date < DateTime.Now.Date.AddDays(1)) && (o.Status == OrderStatus.Pending));
                            break;
                        case "doing":
                            ordersQueryable = _context.orders.IgnoreQueryFilters().Include(p => p.product)
                           .Where(o => (DateTime.Now.Date <= o.createdAt && o.createdAt.Date < DateTime.Now.Date.AddDays(1)) && (o.Status == OrderStatus.doing));
                            break;
                        case "rejected":
                            ordersQueryable = _context.orders.IgnoreQueryFilters().Include(p => p.product)
                           .Where(o => (DateTime.Now.Date <= o.createdAt && o.createdAt.Date < DateTime.Now.Date.AddDays(1)) && (o.Status == OrderStatus.rejected));
                            break;
                        case "reffered":
                            ordersQueryable = _context.orders.IgnoreQueryFilters().Include(p => p.product)
                           .Where(o => (DateTime.Now.Date <= o.createdAt && o.createdAt.Date < DateTime.Now.Date.AddDays(1)) && (o.Status == OrderStatus.reffered));
                            break;
                        default:
                            ordersQueryable = _context.orders.IgnoreQueryFilters().Include(p => p.product)
                           .Where(o => DateTime.Now.Date <= o.createdAt && o.createdAt.Date < DateTime.Now.Date.AddDays(1));
                            break;
                    }

                }
                else
                {
                    switch (orderStatus)
                    {
                        case "registered":
                            ordersQueryable = _context.orders.IgnoreQueryFilters();
                            break;
                        case "finished":
                            ordersQueryable = _context.orders.IgnoreQueryFilters()
                           .Where(o => o.Status == OrderStatus.finished);
                            break;
                        case "canceled":
                            ordersQueryable = _context.orders.IgnoreQueryFilters()
                           .Where(o => o.Status == OrderStatus.canceled);
                            break;
                        case "pending":
                            ordersQueryable = _context.orders.IgnoreQueryFilters()
                           .Where(o => o.Status == OrderStatus.Pending);
                            break;
                        case "doing":
                            ordersQueryable = _context.orders.IgnoreQueryFilters()
                           .Where(o => o.Status == OrderStatus.doing);
                            break;
                        case "rejected":
                            ordersQueryable = _context.orders.IgnoreQueryFilters()
                           .Where(o => o.Status == OrderStatus.rejected);
                            break;
                        case "reffered":
                            ordersQueryable = _context.orders.IgnoreQueryFilters()
                           .Where(o => o.Status == OrderStatus.reffered);
                            break;
                        default:
                            ordersQueryable = _context.orders.IgnoreQueryFilters();
                            break;
                    }
                }

                List<DateTime> orders;

                orders = ordersQueryable.OrderBy(o => o.createdAt).Select(o => o.createdAt).ToList();

                List<LineChartViewModel> chart = new List<LineChartViewModel>();

                if (orders.Count != 0)
                {
                    if (period == "Today" || period == null)
                    {
                        startPoint = DateTime.Now.Date;
                        endPoint = DateTime.Now.Date.AddDays(1);
                    }
                    else
                    {
                        startPoint = orders.First();
                        endPoint = orders.Last();
                    }

                    switch (period)
                    {
                        case "Today":
                            chart = CreateChart(startPoint, endPoint, orders, "Today");
                            break;
                        case "Weekly":
                            chart = CreateChart(startPoint, endPoint, orders, "Weekly");
                            break;
                        case "Monthly":
                            chart = CreateChart(startPoint, endPoint, orders, "Monthly");
                            break;
                        case "Yearly":
                            chart = CreateChart(startPoint, endPoint, orders, "Yearly");
                            break;
                        default:
                            chart = CreateChart(startPoint, endPoint, orders, "Today");
                            break;
                    }

                    return View(chart);
                }
                else
                {
                    chart.Add(new LineChartViewModel { });
                }
                return View(chart);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> MostOrderedProducts()
        {
            try
            {
                List<BarChartViewModel> barChart;

                barChart = _context.products.IgnoreQueryFilters()
                   .Include(o => o.orders)
                   .Select(p => new BarChartViewModel
                   {
                       Lable = ("#" + p.id + " " + p.Name),
                       Quantity = p.orders.Count(o => o.Status == OrderStatus.finished)
                   }).ToList();

                int topSellers;
                if (barChart.Count >= 10)
                    topSellers = 10;
                else
                    topSellers = barChart.Count;
                barChart = barChart.OrderByDescending(b => b.Quantity).Take(topSellers).ToList();
                return View(barChart);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        #region ExtraMethod
        private List<LineChartViewModel> CreateChart(DateTime startPoint, DateTime endPoint, List<DateTime> orders, string filter)
        {
            DateTime temp = new DateTime();
            List<DateTime> ChartLable = new List<DateTime>();
            List<int> SoldNumber = new List<int>();
            ChartLable.Add(startPoint);
            switch (filter)
            {
                case "Today":
                    temp = startPoint.AddHours(1);
                    break;
                case "Weekly":
                    temp = (startPoint.AddDays(7)).Date.AddDays(1);
                    break;
                case "Monthly":
                    temp = (startPoint.AddMonths(1)).Date.AddDays(1);
                    break;
                case "Yearly":
                    temp = (startPoint.AddYears(1)).Date.AddDays(1);
                    break;

            }
            while (temp < endPoint)
            {
                ChartLable.Add(temp);
                switch (filter)
                {
                    case "Today":
                        temp = temp.AddHours(1);
                        break;
                    case "Weekly":
                        temp = temp.AddDays(7);
                        break;
                    case "Monthly":
                        temp = temp.AddMonths(1);
                        break;
                    case "Yearly":
                        temp = temp.AddYears(1);
                        break;

                }
            }
            ChartLable.Add(temp);
            if (ChartLable.Count == 1)
            {
                var sumOfNumber = orders.Where(o => o < ChartLable[0]).Count();
                SoldNumber.Add(sumOfNumber);
            }
            else
            {
                for (int i = 0; i < ChartLable.Count; i++)
                {

                    if (i != ChartLable.Count - 1)
                    {
                        var sumOfNumber = orders.Where(o => ChartLable[i] <= o && o < ChartLable[i + 1]).Count();
                        SoldNumber.Add(sumOfNumber);
                    }
                }
            }
            List<LineChartViewModel> chart = new List<LineChartViewModel>();
            for (int i = 1; i < ChartLable.Count; i++)
            {
                if (filter == "Today")
                {
                    chart.Add(new LineChartViewModel
                    {
                        lable = (ChartLable[i].TimeOfDay).ToString(),
                        data = SoldNumber[i - 1],
                        //filterMessage = filter
                    });
                }
                else
                {
                    chart.Add(new LineChartViewModel
                    {
                        lable = ChartLable[i].ToShamsi(),
                        data = SoldNumber[i - 1],
                        //filterMessage = filter
                    });
                }
            }
            return chart;
        }
        #endregion
    }
}
