using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Squirrel.Contexts;
using Squirrel.Entities;
using Squirrel.Models;
using Squirrel.Services;
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
        public AccountController(UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IStringLocalizer<SharedResource> localizer,
                                 IEmailSender emailSender,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _emailSender = emailSender;
            _ = RoleInitializer.RoleInit(userManager, roleManager);
        }


        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            return result.Succeeded ? Ok() : BadRequest(new[] { _localizer["Invalid login and/or password"].Value });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterModel model, string callbackUrl)
        {
            if (model.Same)
            {
                User user = new(model);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    callbackUrl = Url.ActionLink("ConfirmEmail", "Account", new { user.Id, code, callbackUrl });
                    await _emailSender.SendEmailAsync(email: model.Email,
                                                     subject: _localizer["Confirm your email"].Value,
                                                     htmlMessage: _localizer["Please confirm your account by <a href='{0}'>clicking here</a>.", HtmlEncoder.Default.Encode(callbackUrl!)].Value);

                    return Ok();
                }
                return BadRequest(result.Errors.Select(e => e.Description));
            }
            return BadRequest(new[] { _localizer["Password are not the same"].Value });
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


        [HttpPatch]
        public async Task<IActionResult> ConfirmEmailAsync(string id, string code, string callbackUrl)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
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
        public async Task<IActionResult> ForgotPassword(string email, string path)
        {
            var callbackUrl = Request.Headers["Origin"].FirstOrDefault();
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                callbackUrl += $"{path}?email={email}&code={code}";
                await _emailSender.SendEmailAsync(email: email,
                                                         subject: _localizer["Password reset"].Value,
                                                         htmlMessage: _localizer["Please confirm your password reset by <a href='{0}'>clicking here</a>.", HtmlEncoder.Default.Encode(callbackUrl!)].Value);

            }
            return Ok();

        }

        [HttpPatch]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            var callbackUrl = Request.Headers["Origin"].FirstOrDefault();
            if (model.Same)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null)
                    return BadRequest(new[] { _localizer["No user found"].Value });
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                return result.Succeeded ? Ok() : BadRequest(result.Errors.Select(e => e.Description));
            }
            return BadRequest(new[] { _localizer["Passwords are not the same"].Value });
        }
    }
}