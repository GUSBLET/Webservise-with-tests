namespace UI.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IMailService _mailService;

    public AccountController(
        IAccountService accountService, 
        IMailService mailService)
    {
        _accountService = accountService;
        _mailService = mailService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(ModelState.IsValid)
        {
            var response = await _accountService.LoginAsync(model);
            if(response.StatusCode == HttpStatusCode.OK)
            { 
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", response.Description);
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult SendResetPasswordRequist()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SendResetPasswordRequist(SendResetPasswordRequistViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _accountService.SendResetPasswordCodeAsync(model);
            if(result.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelState.AddModelError("", result.Description);
                return View(model);
            }

            string emailConfirmationUrl = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { userId = result.Data.Id, code = result.Data.EmailConfirmedToken.ToString() },
                    protocol: HttpContext.Request.Scheme);
            await _mailService.SendEmailAsync(model.Email, "Reset your password", emailConfirmationUrl);

            return View("Success", "Check your email for reset password message");
        }
        return View();
    }

    [HttpGet]
    public IActionResult ResetPassword(ulong userId)
    {
        ViewBag.Id = userId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        ViewBag.Id = model.Id;
        if (ModelState.IsValid)
        {
            var response = await _accountService.ResetPasswordAsync(model.Id, model.Password);
            if (response == HttpStatusCode.OK)
                return View("Success", "Password have been changed");
            if (response == HttpStatusCode.BadRequest)
                return View("Error", "User dosen't exist");
            ModelState.AddModelError("", "Password has to contain chars and numbers");
        }
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {

        if (userId == null || code == null)
        {
            return View("Error", "Link is wrong");
        }
        var result = await _accountService.ConfirmEmailAsync(Convert.ToInt32(userId), code);
        if (result == HttpStatusCode.OK)
            return RedirectToAction("Login", "Account");

        else
            return View("Error", "User don't exist");
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registration(RegistrationViewModel model)
    {
        if (ModelState.IsValid)
        {
            var response = await _accountService.RegistrationAsync(model);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var user = _accountService.GetDataForMailByEmailAsync(model.Email);
                string emailConfirmationUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Result.Id, code = user.Result.EmailConfirmedToken.ToString() },
                        protocol: HttpContext.Request.Scheme);

                await _mailService.SendEmailAsync(user.Result.Email, "Confirm your email", emailConfirmationUrl);
                
                return View("Success", "SuccessRegistrationMessge");
            }
            ModelState.AddModelError("Error", response.Description);
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

}
