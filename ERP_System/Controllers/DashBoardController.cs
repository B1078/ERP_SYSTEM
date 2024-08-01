using Microsoft.AspNetCore.Mvc;

namespace ERP_System.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult DashBoard()
        {
            if (HttpContext.Session.GetString("User_Id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
