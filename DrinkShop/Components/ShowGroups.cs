using DrinkShop.Data;
using DrinkShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrinkShop.Components
{
    public class ShowGroups : ViewComponent
    {
        private DrinkShopDbContext _context;
        public ShowGroups(DrinkShopDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string whichView)
        {
            var groups = _context.groups.Include(sg => sg.subGroups).Select(g =>
            new GroupAndSubGroupViewModel()
            {
                groupId = g.id,
                groupName = g.Name,
                subGroops = g.subGroups.ToList()
            }).ToList();
            string viewAddress = "~/Views/Component/ShowGroups.cshtml";
            if (whichView == "Responsive")
                viewAddress = "~/Views/Component/ShowGroupsResponsive.cshtml";
            else if (whichView == "Sidebar")
                viewAddress = "~/Views/Component/SidebarGroupBlock.cshtml";
            else if (whichView == "Navbar")
                viewAddress = "~/Views/Component/ShowGroupsNavbar.cshtml";
            return View(viewAddress, groups);
        }
    }
}
