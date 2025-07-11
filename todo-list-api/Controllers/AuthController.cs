using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoListApi.Application.Dtos;
using TodoListApi.Application.Services;

namespace TodoListApi.Controllers;

public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration configuration, IAuthService authService)
    {
        _authService = authService;
        _configuration = configuration;
    }

    [HttpGet("allUsers")]
    public IActionResult GetAllUsers()
    {
        return Ok(_authService.GetAllUsersAsync().Result);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {

        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Username and password are required.");
        }
        try
        {
            var user = _authService.RegisterUserAsync(request).Result;
            if (user == null)
            {
                return BadRequest("User registration failed.");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(new { message = "User registered successfully" });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (_authService.ValidateUserCredentials(request.Username, request.Password))
        {
            var user  = _authService.GetUserByUsernameAsync(request.Username).Result;
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            var token = _authService.GenerateJwtToken(request.Username, user.Id);
            return Ok(token );
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPost("changePassword")]
    public IActionResult ChangePassword([FromBody] RegisterRequest request)
    {
        if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.Password))
        {
            try
            {
                if (!_authService.ChangePasswordAsync(request.Username, request.Password).Result)
                {
                    return BadRequest("Password change failed.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new { message = "Password changed successfully" });
        }

        return BadRequest("Username and new password are required.");
    }

    [HttpPost("users/generate")]
    public IActionResult GenerateUsers()
    {
        if(_authService.GetAllUsersAsync().Result.Count > 0)
        {
            return Ok("Users already exist. No need to generate.");
        }
        var users = new[]
        {
            new RegisterRequest { Username = "admin", Password = "admin" },
            new RegisterRequest { Username = "talarek", Password = "OneDirection123" },
            new RegisterRequest { Username = "shavack", Password = "admin" },
            new RegisterRequest { Username = "dominik", Password = "talarek" },
        };
        foreach (var user in users)
        {
            try
            {
                _authService.RegisterUserAsync(user).Wait();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating user {user.Username}: {ex.Message}");
            }
        }
        return Ok(users);
    }
}