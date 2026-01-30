using Library_Management_System.Common;
using Library_Management_System.Models;
using Library_Management_System.Services;
using Library_Management_System.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IMemberService _memberService;

    public AccountController(IAccountService accountService, IMemberService memberService)
    {
        _accountService = accountService;
        _memberService = memberService;
    }

    // -------------------- LOGIN --------------------
    [HttpPost]
   
public IActionResult Login(string email, string password)
    {
        var user = _accountService.ValidateUser(email, password);
        if (user == null)
        {
            TempData["LoginError"] = "Invalid email or password";
            return RedirectToAction("Index", "Home");
        }

        HttpContext.Session.Clear();

        if (!user.TwoFactorEnabled)
        {
            // Two-step verification is OFF → go to dashboard
            CompleteLogin(user);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Two-step verification is ON → redirect to 2FA verification page
            HttpContext.Session.SetInt32("2FA_UserId", user.Id);
            return RedirectToAction("Verify2FA");
        }
    }
    // -------------------- ENABLE 2FA --------------------
    [HttpGet]
    public async Task<IActionResult> Enable2FA()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Index", "Home");

        var user = await _memberService.GetByIdAsync(userId.Value);
        if (user == null)
            return RedirectToAction("Index", "Home");
        if (user.TwoFactorEnabled)
            return RedirectToAction("Index", "Home");


        if (string.IsNullOrEmpty(user.TwoFactorSecret))
        {
            var key = OtpNet.KeyGeneration.GenerateRandomKey(20); // 20 byte
            user.TwoFactorSecret = OtpNet.Base32Encoding.ToString(key);
            user.TwoFactorEnabled = false;

            await _memberService.UpdateAsync(user); // ✅ use service
        }

        string otpUri =
            $"otpauth://totp/LibrarySystem:{user.Email}?secret={user.TwoFactorSecret}&issuer=LibrarySystem";

        ViewBag.QrCodeUrl =
            $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={otpUri}";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Enable2FA(string code)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Index", "Home");

        var user = await _memberService.GetByIdAsync(userId.Value);

        var totp = new OtpNet.Totp(
            OtpNet.Base32Encoding.ToBytes(user.TwoFactorSecret)
        );

        bool valid = totp.VerifyTotp(code, out _);

        if (!valid)
        {
            ViewBag.Error = "Invalid authentication code";

            string otpUri =
                $"otpauth://totp/LibrarySystem:{user.Email}?secret={user.TwoFactorSecret}&issuer=LibrarySystem";

            ViewBag.QrCodeUrl =
                $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={otpUri}";

            return View();
        }

        user.TwoFactorEnabled = true;
        await _memberService.UpdateAsync(user);

        return RedirectToAction("Index", "Home");
    }
    // -------------------- VERIFY 2FA --------------------
    [HttpGet]
    public IActionResult Verify2FA()
    {
        if (HttpContext.Session.GetInt32("2FA_UserId") == null)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public IActionResult Verify2FA(string code)
    {
        var userId = HttpContext.Session.GetInt32("2FA_UserId");
        // var userId2 = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Index", "Home");

        var user = _memberService.GetByIdAsync(userId.Value).Result;

        var totp = new OtpNet.Totp(
            OtpNet.Base32Encoding.ToBytes(user.TwoFactorSecret)
        );

        bool valid = totp.VerifyTotp(code, out _);

        if (!valid)
        {
            ViewBag.Error = "Invalid authentication code";
            return View();
        }
        HttpContext.Session.Remove("2FA_UserId");

        CompleteLogin(new UserViewModel
        {
            Id = user.Id,
            Role = user.RoleId == 1 ? "Admin" : "Member"
        });

        return RedirectToAction("Index", "Home");
    }

    private void CompleteLogin(UserViewModel user)
    {
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserRole", user.Role);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // clear all session
        return RedirectToAction("Index", "Home"); // go back to login page
    }


}