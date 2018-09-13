using System;
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

namespace Shackmeets.Controllers
{
  [Route("api")]
  public class ApiController : Controller
  {
    private ShackmeetsDbContext dbContext = null;
    private AppSettings appSettings = null;

    public ApiController(ShackmeetsDbContext context, IOptions<AppSettings> appSettings)
    {
      this.dbContext = context;
      this.appSettings = appSettings.Value;
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
          return BadRequest(new { message = "Username or password is incorrect" });
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
          Username = user.Username,
          Token = tokenString
        });        
      }
      catch
      {
        // Log error

        return BadRequest(new { message = "An error occurred" });
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult LogOut(string username)
    {
      try
      {
        return Ok(new { result = "success" });
      }
      catch
      {
        // Log error

        return BadRequest(new { result = "error" });
      }      
    }

    [HttpGet("[action]")]
    public IEnumerable<Meet> GetShackmeets()
    {
      try
      {
        return this.dbContext
          .Meets
          .AsNoTracking()
          .Where(m => m.EventDate >= DateTime.Today)
          .Include(m => m.Rsvps)
          .ToList();
      }
      catch
      {
        // Log error

        return new List<Meet>();
      }
    }

    [HttpGet("[action]")]
    public IEnumerable<Meet> GetArchivedShackmeets()
    {
      try
      {
        return this.dbContext.Meets.AsNoTracking().Where(m => m.EventDate < DateTime.Today).ToList();
      }
      catch
      {
        // Log error

        return new List<Meet>();
      }      
    }

    [HttpGet("[action]")]
    public Meet GetShackmeet(int meetId)
    {
      try
      {
        return this.dbContext.Meets.AsNoTracking().SingleOrDefault(m => m.MeetId == meetId);
      }
      catch
      {
        // Log error

        return null;
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult CreateShackmeet([FromBody] MeetDto meet)
    {
      try
      {
        // Implement PRL
        // Validate fields

        return Ok(new { result = "Not implemented" });
      }
      catch
      {
        // Log error

        return BadRequest(new { result = "error" });
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult Rsvp([FromBody] RsvpDto rsvpDto)
    {
      try
      {
        // Verify user exists
        bool userExists = this.dbContext.Users.Any(u => u.Username == rsvpDto.Username);

        if (!userExists)
        {
          return BadRequest(new { result = "User does not exist." });
        }

        // Verify meet exists
        bool meetExists = this.dbContext.Meets.Any(m => m.MeetId == rsvpDto.MeetId);

        if (!meetExists)
        {
          return BadRequest(new { result = "Meet does not exist." });
        }

        var rsvp = this.dbContext.Rsvps.SingleOrDefault(r => r.MeetId == rsvpDto.MeetId && r.Username == rsvpDto.Username);

        if (rsvp != null)
        {
          // Existing RSVP
          this.dbContext.Rsvps.Attach(rsvp);
        }
        else
        {
          // New RSVP
          rsvp = new Rsvp
          {
            MeetId = rsvpDto.MeetId,
            Username = rsvpDto.Username
          };

          this.dbContext.Add(rsvp);
        }

        rsvp.RsvpTypeId = rsvpDto.RsvpTypeId;
        rsvp.NumAttendees = rsvpDto.NumAttendees;

        this.dbContext.SaveChanges();

        return Ok(new { result = "RSVP updated." });
      }
      catch
      {
        // Log error

        return BadRequest(new { result = "Error" });
      }
    }
    
    public void ResendNotification()
    {

    }
  }
}
