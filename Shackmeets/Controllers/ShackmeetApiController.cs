using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Shackmeets.Models;
using Shackmeets.Dtos;

namespace Shackmeets.Controllers
{
  [Route("api")]
  public class ShackmeetApiController : Controller
  {
    private ShackmeetsDbContext dbContext = null;

    public ShackmeetApiController(ShackmeetsDbContext context)
    {
      this.dbContext = context;
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
    public IActionResult CreateShackmeet([FromBody] MeetDto meetDto)
    {
      try
      {
        // Implement PRL

        // Create new meet
        var meet = new Meet
        {
          Name = meetDto.Name,
          Description = meetDto.Description,
          OrganizerUsername = meetDto.OrganizerUsername,
          EventDate = meetDto.EventDate,
          LocationName = meetDto.LocationName,
          LocationAddress = meetDto.LocationAddress,
          LocationState = meetDto.LocationState,
          LocationCountry = meetDto.LocationCountry,
          LocationLatitude = meetDto.LocationLatitude,
          LocationLongitude = meetDto.LocationLongitude,
          WillPostAnnouncement = meetDto.WillPostAnnouncement,
          IsDeleted = false
        };

        // Validate fields
        if (false)
        {
          // Return validation errors
        }

        this.dbContext.Meets.Add(meet);
        this.dbContext.SaveChanges();

        return Ok(new { result = "Yay sorta implemented" });
      }
      catch
      {
        // Log error

        return BadRequest(new { result = "error" });
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult UpdateShackmeet([FromBody] MeetDto meetDto)
    {
      try
      {
        var meet = this.dbContext.Meets.SingleOrDefault(m => m.MeetId == meetDto.MeetId);

        if (meet == null)
        {
          return BadRequest(new { result = "Shackmeet does not exist." });
        }
        
        // Update meet
        meet.Name = meetDto.Name;
        meet.Description = meetDto.Description;
        meet.OrganizerUsername = meetDto.OrganizerUsername;
        meet.EventDate = meetDto.EventDate;
        meet.LocationName = meetDto.LocationName;
        meet.LocationAddress = meetDto.LocationAddress;
        meet.LocationState = meetDto.LocationState;
        meet.LocationCountry = meetDto.LocationCountry;
        meet.LocationLatitude = meetDto.LocationLatitude;
        meet.LocationLongitude = meetDto.LocationLongitude;
        meet.WillPostAnnouncement = meetDto.WillPostAnnouncement;
        meet.IsDeleted = false;
        
        // Validate fields
        if (false)
        {
          // Return validation errors
        }

        this.dbContext.Meets.Attach(meet);
        this.dbContext.SaveChanges();

        return Ok(new { result = "Yay sorta implemented" });
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
  }
}
