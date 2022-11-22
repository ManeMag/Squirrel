using AutoMapper;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Squirrel.Extensions;
using Squirrel.Requests.User;
using Squirrel.Services;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Squirrel.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IEmailSender _emailSender;
        private readonly BaseCategorySeeder _seeder;
        private readonly IMapper _mapper;

        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IStringLocalizer<SharedResource> localizer,
                                 IEmailSender emailSender,
                                 RoleManager<IdentityRole> roleManager,
                                 IConfiguration configuration,
                                 BaseCategorySeeder seeder,
                                 IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _emailSender = emailSender;
            _seeder = seeder;
            _mapper = mapper;
            _ = RoleInitializer.RoleInit(userManager, roleManager, configuration);
        }


        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync(LoginRequest model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            return result.Succeeded ? Ok() : "Invalid login and/or password".ToBadRequestUsing(_localizer);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterRequest model, string callbackUrl)
        {
            if (model.Same)
            {
                var user = _mapper.Map<User>(model);
                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var seedingResult = await _seeder.SeedCategories(user.Id);

                        if (!seedingResult.IsSuccess)
                        {
                            return BadRequest(seedingResult.Errors);
                        }

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        callbackUrl = Url.ActionLink("ConfirmEmail", "Account", new { user.Id, code, callbackUrl });
                        await _emailSender.SendEmailAsync(email: model.Email,
                                                         subject: _localizer["Confirm your email"].Value,
                                                         htmlMessage: _localizer["Please confirm your account by <a href='{0}'>clicking here</a>.", HtmlEncoder.Default.Encode(callbackUrl!)].Value);

                        return CreatedAtAction(nameof(RegisterAsync), user);
                    }
                    return BadRequest(result.Errors.Select(e => e.Description));
                }
                catch
                {
                    await _userManager.DeleteAsync(user);
                    return BadRequest(new[] { _localizer["Something went wrong. Please try again"].Value });
                }
            }
            return "Passwords are not the same".ToBadRequestUsing(_localizer);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EmailAsync() => Ok((await _userManager.GetUserAsync(User)).Email);

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CurrencyAsync() => Ok((await _userManager.GetUserAsync(User)).Currency);


        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAsync(string id, string code, string callbackUrl)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Redirect($"http://{callbackUrl}?success=0");
            var result = await _userManager.ConfirmEmailAsync(user!, code);
            await _userManager.AddToRoleAsync(user!, "user");
            return Redirect($"http://{callbackUrl}?success={(result.Succeeded ? 1 : 0)}");
        }

        [Authorize(Roles = "user")]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            User user = await _userManager.GetUserAsync(User);
            if (user is null)
                return BadRequest(new[] { _localizer["User not found"].Value });
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? Ok() : BadRequest(result.Errors.Select(e => e.Description));
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] string email, string path)
        {
            var callbackUrl = Request.Headers["Origin"].FirstOrDefault();
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                callbackUrl += $"{path}?email={email}&code={code}";
                await _emailSender.SendEmailAsync(email: email,
                                                         subject: _localizer["Password reset"].Value,
                                                         htmlMessage: _localizer["Please confirm your password reset by <a href='{0}'>clicking here</a>.", HtmlEncoder.Default.Encode(callbackUrl!)].Value);
                return Ok();
            }
            return BadRequest(new[] { _localizer["User not found"].Value });

        }

        [HttpPatch]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var callbackUrl = Request.Headers["Origin"].FirstOrDefault();
            if (model.Same)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null)
                    return BadRequest(new[] { _localizer["User not found"].Value });
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                return result.Succeeded ? Ok() : BadRequest(result.Errors.Select(e => e.Description));
            }
            return BadRequest(new[] { _localizer["Passwords are not the same"].Value });
        }


        [HttpGet]
        public IActionResult GoogleAuth(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("/");
            string? redirectUrl = Url.Action("GoogleResponse", "Account", new { returnUrl });
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponseAsync(string returnUrl)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is not null)
            {
                var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
                if (!signInResult.Succeeded)
                {
                    User user = new(info.Principal.FindFirstValue(ClaimTypes.Email));
                    var createResult = await _userManager.CreateAsync(user);
                    if (createResult.Succeeded)
                    {
                        var loginResult = await _userManager.AddLoginAsync(user, info);
                        if (loginResult.Succeeded)
                            await _signInManager.SignInAsync(user, false);
                    }
                }
            }
            return Redirect(returnUrl);
        }
    }
}