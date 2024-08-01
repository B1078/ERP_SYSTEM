using Microsoft.AspNetCore.Mvc;

namespace ERP_System.Controllers.Sales
{
    public class SalesBlanketAgreementController : Controller
    {
        public IActionResult SalesBlanketAgreement()
        {
            if (HttpContext.Session.GetString("User_Id") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
