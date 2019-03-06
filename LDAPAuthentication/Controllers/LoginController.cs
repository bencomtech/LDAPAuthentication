using LDAPAuthentication.Constants;
using LDAPAuthentication.Models;
using LDAPAuthentication.Services;
using LDAPAuthentication.ViewModels;
using Microsoft.Owin.Security;
using MvcFlashMessages;
using System;
using System.Web;
using System.Web.Mvc;

namespace LDAPAuthentication.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel viewModel, string returnUrl)
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            ADAuthenticationService authenticationService = new ADAuthenticationService(authenticationManager);

            try
            {
                AuthenticationResult authenticationResult = authenticationService.SignIn(viewModel.Username, viewModel.Password);

                if (!authenticationResult.IsSuccess)
                {
                    this.Flash(FLASH_MESSAGE_TYPE.Error, authenticationResult.ErrorMessage);
                    return View(viewModel);
                }

                this.Flash(FLASH_MESSAGE_TYPE.Success, "Log In successful");
                return RedirectToLocal(returnUrl);
            }
            catch (Exception ex)
            {
                this.Flash(FLASH_MESSAGE_TYPE.Error, ex.Message);
                return View(viewModel);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

            try
            {
                authenticationManager.SignOut(AUTHENTICATION.ApplicationCookie);

                this.Flash(FLASH_MESSAGE_TYPE.Success, "Log Off successful");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.Flash(FLASH_MESSAGE_TYPE.Error, ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
    }
}