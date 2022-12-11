using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ViewBagTempDataSessionCookies.Models;

namespace ViewBagTempDataSessionCookies.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.UserName == "user@gmail.com" && model.Password == "123456")
                {
                    TempData["Name"] = model.UserName;
                    TempData["Message"] = "Your Welcome!";

                    //for session we need to enable it In program.cs file
                    string str = JsonSerializer.Serialize(model);
                    HttpContext.Session.SetString("user", str);

                    Response.Cookies.Append("cookie1", "Non persistent cookie");
                    Response.Cookies.Append("cookie2", "Persistent cookie", new CookieOptions { Expires = DateTime.Now.AddDays(7) });

                    return RedirectToAction("Dashboard", "Account");
                }
                ViewBag.error = "Username or Password doesn’t exist";
                return View();
            }
            return View();
        }
        public ViewResult Dashboard()
        {

            ViewData["AuthorName"] = "Scholarhat";
            ViewBag.Subtitle = "Dotnettricks";

            String Message = (String)TempData["Message"]; //Read once, mark as dirty=>GC
            string Name = (string)TempData.Peek("Name"); // Read many time becuase of peek method

            TempData.Keep("Message"); //costly, reclaim phase in GC.

            string strdata = HttpContext.Session.GetString("user");
            LoginViewModel data = JsonSerializer.Deserialize<LoginViewModel>(strdata);

            string Cookie1 = Request.Cookies["cookie1"];
            string Cookie2 = Request.Cookies["cookie2"];

            return View();
        }

        public ViewResult Logout()
        {
            Response.Cookies.Delete("cookie1");
            Response.Cookies.Delete("cookie2");
            return View();  
        }
    }
}
