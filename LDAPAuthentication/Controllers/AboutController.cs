using System.Web.Mvc;

namespace LDAPAuthentication.Controllers
{
    [Authorize]
    public class AboutController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}