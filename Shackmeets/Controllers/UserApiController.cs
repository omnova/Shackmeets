﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Shackmeets.Models;
using Shackmeets.Dtos;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Shackmeets.Controllers
{
  [Route("api")]
  public class UserApiController : Controller
  {
    private readonly ShackmeetsDbContext dbContext;
    private readonly AppSettings appSettings;
    private readonly ILogger logger;

    public UserApiController(ShackmeetsDbContext context, IOptions<AppSettings> appSettings, ILogger<UserApiController> logger)
    {
      this.dbContext = context;
      this.appSettings = appSettings.Value;
      this.logger = logger;
    }

    [HttpPost("[action]")]
    public IActionResult Login([FromBody] UserDto userDto)
    {
      try
      {
        // Change to DI service? Seems overkill
        var chatty = new ChattyWrapper();

        var isValid = chatty.VerifyCredentials(userDto.Username, userDto.Password);

        if (!isValid)
        {
          return BadRequest(new { result = "error", message = "Username or password is incorrect." });
        }

        var user = this.dbContext.Users.FirstOrDefault(u => u.Username == userDto.Username);

        if (user == null)
        {
          // New user, create a user record
          user = new User
          {
            Username = userDto.Username
          };

          this.dbContext.Users.Add(user);
          this.dbContext.SaveChanges();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(new Claim[]
            {
                  new Claim(ClaimTypes.Name, user.Username)
            }),
          Expires = DateTime.UtcNow.AddDays(30),
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        // return basic user info (without password) and token to store client side
        return Ok(new
        {
          result = "success",
          username = user.Username,
          token = tokenString
        });
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new { result = "error", message = "" });
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult UpdatePreferences([FromBody] UserDto userDto)
    {
      try
      {
        var user = this.dbContext.Users.FirstOrDefault(u => u.Username == userDto.Username);

        if (user == null)
        {
          return BadRequest(new { result = "User does not exist." });
        }

        // Update user
        user.LocationLatitude = userDto.LocationLatitude;
        user.LocationLongitude = userDto.LocationLongitude;
        user.MaxNotificationDistance = userDto.MaxNotificationDistance;
        user.NotificationOptionId = userDto.NotificationOptionId;
        user.NotifyByShackmessage = userDto.NotifyByShackmessage;
        user.NotifyByEmail = userDto.NotifyByEmail;
        user.NotificationEmail = userDto.NotificationEmail;

        this.dbContext.Users.Attach(user);
        this.dbContext.SaveChanges();

        return Ok(new { result = "success" });
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new { result = "error", message = "" });
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult LogOut([FromBody] string username)
    {
      try
      {
        return Ok(new { result = "success" });
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new { result = "error", message = "" });
      }
    }
  }
}