using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Mvc3ToolsUpdateWeb_Default.Models;
using MvcMusicStore.Models;

namespace Mvc3ToolsUpdateWeb_Default.Controllers
{
    public class AccountController : Controller
    {

        private void MigrateShoppingCart(string UserName)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    MigrateShoppingCart(model.UserName); 
                    
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El nombre de usuario o contraseña es incorrecta.");
                }
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, "question", "answer", true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    MigrateShoppingCart(model.UserName); 
                    
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "La contraseña actual es incorrecta o la nueva contraseña es invalida.");
                }
            }
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "El usuario ya existe. Por favor ingrese un nombre de usuario diferente.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Un nombre de usuario para ese correo ya existe. Por favor ingrese un correo electronico diferente.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña ingresada es invalida. Por favor ingrese una contraseña valida.";

                case MembershipCreateStatus.InvalidEmail:
                    return "El correo electronico es invalido. Por favor verifique el valor e intentelo de nuevo.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La respuesta a la pregunta de la recuperacion de contraseña es invalida. Por favor verifique el valor e intentelo de nuevo.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La pregunta de la recuperacion de contraseña es invalida. Por favor verifique el valor e intentelo de nuevo.";

                case MembershipCreateStatus.InvalidUserName:
                    return "El nombre de usuario provisto es invalido. Por favor verifique el valor e intentelo de nuevo.";

                case MembershipCreateStatus.ProviderError:
                    return "La autenticacion provista ha retornado un error. Por favor verifique e intentelo de nuevo. Si el problema persiste, por favor contacte al administrador.";

                case MembershipCreateStatus.UserRejected:
                    return "La creacion de usuario ha sido cancelada. Por favor verifique e intentelo de nuevo. Si el problema persiste, por favor contacte al administrador.";

                default:
                    return "Un error desconocido ha ocurrido. Por favor verifique e intentelo de nuevo. Si el problema persiste, por favor contacte al administrador.";
            }
        }
        #endregion
    }
}
