using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    [HttpGet("[action]")]
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

    [HttpGet("[action]")]
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
        return this.dbContext.Meets.AsNoTracking().Where(m => m.EventDate >= DateTime.Today).ToList();
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

    public void Rsvp(int meetId, int userId)
    {

    }
    
    public void ResendNotification()
    {

    }
  }
}
