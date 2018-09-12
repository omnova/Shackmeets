using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Shackmeets.Models;

namespace Shackmeets.Controllers
{
  [Route("api")]
  public class ApiController : Controller
  {
    private ShackmeetsDbContext dbContext = null;

    public ApiController(ShackmeetsDbContext context)
    {
      this.dbContext = context;
    }

    [HttpPost("[action]")]
    public object Login(string username, string password)
    {
      try
      {
        var chatty = new ChattyWrapper();

        var isValid = chatty.VerifyCredentials(username, password);

        if (isValid)
        {
          var user = this.dbContext.Users.FirstOrDefault(u => u.Username == username);

          if (user == null)
          {
            // New user, create a user record
            user = new User
            {
              Username = username
            };

            this.dbContext.Users.Add(user);
          }
          else
          {
            // Existing user
            if (!string.IsNullOrEmpty(user.SessionKey))
            {
              // Return the existing token
              return new { result = "success", token = user.SessionKey };
            }

            this.dbContext.Users.Attach(user);
          }

          user.SessionKey = Guid.NewGuid().ToString();

          this.dbContext.SaveChanges();

          return new { result = "success", token = user.SessionKey };
        }
        else
        {
          return new { result = "failure" };
        }
      }
      catch
      {
        // Log error
      }

      return new { result = "error" };
    }

    [HttpPost("[action]")]
    [Authorize]
    public object LogOut(string username)
    {
      try
      {
        var user = this.dbContext.Users.FirstOrDefault(u => u.Username == username);

        if (user != null)
        {
          this.dbContext.Users.Attach(user);

          user.SessionKey = null;

          this.dbContext.SaveChanges();
        }

        return new { result = "success" };
      }
      catch
      {
        // Log error

        return new { result = "error" };
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
    public object CreateShackmeet([FromBody] Meet meet)
    {
      try
      {
        // Implement PRL
        // Validate fields
        // Verify token

        return new { result = "not implemented" };
      }
      catch
      {
        // Log error

        return new { result = "error" };
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public object Rsvp(int meetId, string username, int rsvpTypeId, int numAttendees)
    {
      try
      {
        // verify user token
        // verify user exists
        // verify meet exists

        var rsvp = this.dbContext.Rsvps.FirstOrDefault(r => r.MeetId == meetId && r.Username == username);

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
            MeetId = meetId,
            Username = username
          };

          this.dbContext.Add(rsvp);
        }

        rsvp.RsvpTypeId = rsvpTypeId;
        rsvp.NumAttendees = numAttendees;

        this.dbContext.SaveChanges();

        return new { result = "success" };
      }
      catch
      {
        // Log error

        return new { result = "error" };
      }
    }
    
    public void ResendNotification()
    {

    }
  }
}
