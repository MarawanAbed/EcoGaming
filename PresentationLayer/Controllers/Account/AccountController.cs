using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helper;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Extend;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using PresentationLayer.ActionRequests.Account;
using System.Net;

namespace PresentationLayer.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailServices _emailServices;
        private readonly ICartServices _cartServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,EmailServices emailServices, ICartServices cartServices, IHttpContextAccessor httpContextAccessor)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _roleManager = roleManager;
            _emailServices = emailServices;
            _cartServices = cartServices;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginActionRequest loginAction)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(loginAction.Email);

                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, loginAction.Password, loginAction.RememberMe, false);

                    if (result.Succeeded)
                    {
                        var session = _httpContextAccessor.HttpContext.Session;
                        var guestCart = session.GetGuestCart(); // Get guest cart from session

                        // ✅ Merge guest cart with user cart
                        await _cartServices.MergeGuestCartWithUserCart(user.Id, guestCart);

                        session.ClearGuestCart(); // Clear guest cart from session after merging

                        return loginAction.ReturnUrl != null ? LocalRedirect(loginAction.ReturnUrl) : RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User not found.");
                }
            }

            return View(loginAction);
        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public async Task<IActionResult> Register()
        {

            var model = new RegisterActionRequest
            {
                Roles = await GetRolesAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterActionRequest registerAction)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerAction.UserName,
                    Email = registerAction.Email,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(user, registerAction.Password);

                if (result.Succeeded)
                {
                    if (registerAction.Role == "Customer" || registerAction.Role == "Buyer")
                    {
                        await userManager.AddToRoleAsync(user, registerAction.Role);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid role selected.");
                        registerAction.Roles = await GetRolesAsync();
                        return View(registerAction);
                    }

                    return RedirectToAction("Login");
                }

                // Handle errors and reload roles
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            // Reload roles before returning the view
            registerAction.Roles = await GetRolesAsync();
            return View(registerAction);
        }




        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordActionRequest forgetPasswordAction)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(forgetPasswordAction.Email);
                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    var url = Url.Action("ResetPassword", "Account", new { Email = forgetPasswordAction.Email, Token = token }, Request.Scheme);

                    _emailServices.SendEmail(forgetPasswordAction.Email, "Reset Password", url);
                }
                return RedirectToAction("ConfirmForgetPassword");
            }
            return View(forgetPasswordAction);
        }

        public IActionResult ConfirmForgetPassword()
        {
            return View();
        }

        public IActionResult ResetPassword(string? Email, string? Token)
        {
            if (Email != null && Token != null)
            {
                return View();
        }


            return RedirectToAction("ForgetPassword");
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordActionRequest model)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {

                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ConfirmResetPassword");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
            return RedirectToAction("ConfirmResetPassword");
        }
        public IActionResult ConfirmResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        private async Task<List<SelectListItem>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Where(r => r.Name != "Admin")
                .ToListAsync();

            return roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();
        }

    }
}
