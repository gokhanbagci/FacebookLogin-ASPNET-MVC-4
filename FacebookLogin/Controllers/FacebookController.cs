using System;
using System.Web.Mvc;
using FacebookLogin.Models;
namespace FacebookLogin.Controllers
{
    public class FacebookController : Controller
    {
        FacebookManager fm = new FacebookManager();
        public ActionResult Login()
        {
            // Facebook'a yönlendiren action
            var link = fm.GetLoginUrl();
            return Redirect(link);
        }
        public ActionResult Callback(string code)
        {
            string x = Request.RawUrl;
            dynamic token = fm.GetAccessToken(code);
            var u = fm.GetUserInfo(token);
            return View(u);
        }
    }
}