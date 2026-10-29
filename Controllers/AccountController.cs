using DoctorAppointmentSystem.Models;
using DoctorAppointmentSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace DoctorAppointmentSystem.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly SignInManager<ApplicationUser> _signIn;
    public AccountController(UserManager<ApplicationUser> users, SignInManager<ApplicationUser> signIn) { _users = users; _signIn = signIn; }
    [HttpGet] public IActionResult Register() => User.Identity?.IsAuthenticated == true ? RedirectToAction("Index", "Home") : View();
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName, PhoneNumber = model.PhoneNumber };
        var result = await _users.CreateAsync(user, model.Password);
        if (result.Succeeded) { await _users.AddToRoleAsync(user, "Patient"); await _signIn.SignInAsync(user, false); return RedirectToAction("Index", "Home"); }
        foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        return View(model);
    }
    [HttpGet] public IActionResult Login(string? returnUrl = null) { ViewBag.ReturnUrl = returnUrl; return View(); }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (!ModelState.IsValid) return View(model);
        var result = await _signIn.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded) { if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl); return User.IsInRole("Admin") ? RedirectToAction("Index", "Admin") : RedirectToAction("Index", "Home"); }
        ModelState.AddModelError("", "Invalid login attempt."); return View(model);
    }
    [HttpPost, ValidateAntiForgeryToken] public async Task<IActionResult> Logout() { await _signIn.SignOutAsync(); return RedirectToAction("Index", "Home"); }
    public IActionResult AccessDenied() => View();
}
