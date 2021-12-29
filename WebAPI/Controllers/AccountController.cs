using AutoMapper;
using Common.Extensions.CustomExceptions;
using Common.Identity;
using Domain.Core.Identity;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Interfaces;
using Services.Contracts.Interfaces.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Models.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using System.Collections.Generic;

namespace WebAPI.Controllers
{

    /// <summary>
    /// Контроллер аккаунтов
    /// </summary>
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<RoleEntity> _roleManager;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<UserEntity> userManager,
            RoleManager<RoleEntity> roleManager,
            SignInManager<UserEntity> signInManager,
            IMapper mapper,
            IAuthenticationService authenticationService,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
        }

    // new 22-23.12.2021 ---------------------------   
        /// <summary>
        /// Получение списка пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            try
             {
                //var entityList = await _userManager.GetUsersInRoleAsync(IdentityRoles.Admin);                                               
                //return Ok(entityList);
                var users = _userManager.Users;
                return Ok(users);
            }
             catch (Exception ex)
             {
                 _logger.Error(ex, "Ошибка при получении списка пользователей");
                 return StatusCode(500);
             }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userEntity = await _userManager.FindByIdAsync(id.ToString());
                if (userEntity is null)
                    return BadRequest();
                await _userManager.DeleteAsync(userEntity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при удалении пользователя");
                return StatusCode(500);
            }
        }
    // end new 22-23.12.2021 --------------------

        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        /// <param name="id"></param>
        /// <response code="404">Not Found</response>
        //  [Authorize(Roles = IdentityRoles.Admin)]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var userEntity = await _userManager.FindByIdAsync(id.ToString());

                if (userEntity is null)
                    return NoContent();

                var result = _mapper.Map<UserViewModel>(userEntity);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при получении информации пользователя {id}");
                return StatusCode(500, "Error get user information");
            }
        }

        /// <summary>
        /// Авторизация в системе
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            try
            {
                var userEntity = await _userManager.FindByEmailAsync(model.Email);

                if (userEntity == null)
                    return StatusCode(403, "Authorization failed. \n Possible reasons: \n Email confirmation required. \n Incorrect data.");

                SignInResult signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                if (!signInResult.Succeeded)
                {
                    return StatusCode(403, "Authorization failed. \n Possible reasons: \n Email confirmation required. \n Incorrect data.");
                }

                if (userEntity.LockoutEnabled is true)
                    return StatusCode(403, "User is blocked");

                var token = _authenticationService.GetAuthToken(userEntity);

                return Ok(new UserLoginResponseModel { Id = userEntity.Id, AuthToken = token });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при авторизации {model.Email}");
                return StatusCode(500, "Login error");
            }
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <returns></returns>
        /// <response code="204">No Content</response>
        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error on exit");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="201" cref="int">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(int), 201)]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            try
            {
                //Проверяем нет ли уже пользователя в системе
                var searchUser = await _userManager.FindByEmailAsync(model.Email);
                if (searchUser is not null)
                {
                    return BadRequest();
                }
                var userEntity = _mapper.Map<UserEntity>(model);

                //Создаем пользователя
                var result = await _userManager.CreateAsync(userEntity, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                var user = await _userManager.FindByEmailAsync(model.Email);

                //Добавляем пользователю роль Юзера
                try
                {
                    await _userManager.AddToRoleAsync(userEntity, IdentityRoles.User);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, $"Ошибка при добавлении роли");
                    await _userManager.DeleteAsync(user);
                    throw;
                }
                // генерация токена для пользователя
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, code = token }, Request.Scheme);

                //Отправка письма для подтверждения регистрации
              //  try
              //  {
              //      await SendEmailConfirmLink(confirmationLink, user.Email);
              //  }
              //  catch (SmtpException ex)
              //  {
              //      _logger.Error(ex, $"Ошибка при отправке почты");
              //      await _userManager.DeleteAsync(user);
              //      throw;
              //  }

                return Created($"{HttpContext.Request.Path.Value}/{user.Id}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при регистрации");
                return StatusCode(500, "Registration error \nContact your administrator");
            }
        }

        /// <summary>
        /// Запрос новой ссылки для подтверждения почты
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Roles = IdentityRoles.Admin)]
        [HttpGet]
        [Route("{userId}/email/confirm/resend")]
        public async Task<IActionResult> ResendConfirmEmail(Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());

                if (user is null || user.EmailConfirmed is true)
                {
                    return BadRequest();
                }

                var currentUserRole = GetCurrentUserRole(User);

                if (currentUserRole.Role == IdentityRoles.User && currentUserRole.UserId != user.Id)
                {
                    return BadRequest();
                }

                // генерация токена для пользователя
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ResendConfirmEmail), "Account", new { userId = user.Id, code = token }, Request.Scheme);

                await SendEmailConfirmLink(confirmationLink, user.Email);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при подтверждении почты");
                return StatusCode(500);
            }
        }


        /// <summary>
        /// Подтверждение почты
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("email/confirm")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    return BadRequest();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user is null)
                {
                    return BadRequest();
                }

                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (!result.Succeeded)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при подтверждении почты");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Восстановление пароля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(nameof(ForgotPassword))]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // пользователь с данным email может отсутствовать в бд
                    // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                    // наличие или отсутствие пользователя в бд
                    return BadRequest();
                }

                // генерация токена для пользователя
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

#pragma warning disable CS1030 // Директива #warning
#warning Изменить URL (чтобы стучался сразу на фронт)        
                var resetPasswordLink = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, code = token }, protocol: Request.Scheme);
#pragma warning restore CS1030 // Директива #warning

                await _emailSender.SendAsync(user.Email, "Reset Password",
                $"Для сброса пароля пройдите по ссылке: <a href='{resetPasswordLink}'>link</a>");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при восстановлении пароля");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Сброс пароля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(nameof(ResetPassword))]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return BadRequest();
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (!result.Succeeded)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Ошибка при сбросе пароля");
                return StatusCode(500);
            }
        }

        private static UserRoleModel GetCurrentUserRole(ClaimsPrincipal user)
        {
            ClaimsPrincipal currentUserClaim = user;
            var currentUserId = Guid.Parse(currentUserClaim.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUserRole = currentUserClaim.FindFirstValue(ClaimTypes.Role);
            var currentUser = new UserRoleModel
            {
                UserId = currentUserId,
                Role = currentUserRole
            };

            return currentUser;
        }

        //TODO вынести из контроллера, с возможными шаблонами
        private async Task SendEmailConfirmLink(string confirmationLink, string email)
        {
            await _emailSender.SendAsync(email, "Подтвердите регистрацию",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{confirmationLink}'>link</a>");
        }
    }
}
