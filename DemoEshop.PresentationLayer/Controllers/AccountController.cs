﻿using DemoEshop.BusinessLayer.DataTransferObjects;
using DemoEshop.BusinessLayer.Facades;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DemoEshop.PresentationLayer.Helpers.Cookies;
using DemoEshop.PresentationLayer.Models.Accounts;

namespace DemoEshop.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        #region Facades

        public CustomerFacade CustomerFacade { get; set; }
        public OrderFacade OrderFacade { get; set; }

        #endregion

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserCreateDto userCreateDto)
        {
            try
            {
                await CustomerFacade.RegisterCustomer(userCreateDto);
                //FormsAuthentication.SetAuthCookie(userCreateDto.Username, false);
                
                var authTicket = new FormsAuthenticationTicket(1, userCreateDto.Username, DateTime.Now,
                    DateTime.Now.AddMinutes(30), false, "");
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");
            }
            catch(ArgumentException)
            {
                ModelState.AddModelError("Username", "Account with that username already exists!");
                return View();
            }
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            (bool success, string roles) = await CustomerFacade.Login(model.Username, model.Password);
            if (success)
            {
                //FormsAuthentication.SetAuthCookie(model.Username, false);

                var authTicket = new FormsAuthenticationTicket(1, model.Username, DateTime.Now,
                    DateTime.Now.AddMinutes(30), false, roles);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                var decodedUrl = "";
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    decodedUrl = Server.UrlDecode(returnUrl);
                }

                if (Url.IsLocalUrl(decodedUrl))
                {
                    return Redirect(decodedUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Wrong username or password!");
            return View();
        }

        public async Task<ActionResult> Logout()
        {
            var customer = await CustomerFacade.GetUserAccordingToUsernameAsync(User.Identity.Name);
            Response.ClearAllShoppingCartItems(customer.Username);
            OrderFacade.ReleaseReservations(customer.Id);

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}