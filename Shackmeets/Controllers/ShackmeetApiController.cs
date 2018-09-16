using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Shackmeets.Models;
using Shackmeets.Dtos;
using Microsoft.Extensions.Logging;

namespace Shackmeets.Controllers
{
  [Route("api")]
  public class ShackmeetApiController : Controller
  {
    private readonly ShackmeetsDbContext dbContext;
    private readonly ILogger logger;

    public ShackmeetApiController(ShackmeetsDbContext context, ILogger<ShackmeetApiController> logger)
    {
      this.dbContext = context;
      this.logger = logger;
    }

    [HttpGet("[action]")]
    public IActionResult GetShackmeets()
    {
      try
      {
        var meets = this.dbContext
          .Meets
          .AsNoTracking()
          .Where(m => !m.IsCancelled && m.EventDate >= DateTime.Today)
          .Include(m => m.Rsvps)
          .Select(m => new MeetListingDto
          {
            MeetId = m.MeetId,
            Name = m.Name,
            Description = m.Description,
            OrganizerUsername = m.OrganizerUsername,
            EventDate = m.EventDate,
            LocationName = m.LocationName,
            LocationAddress = m.LocationAddress,
            LocationState = m.LocationState,
            LocationCountry = m.LocationCountry,
            LocationLatitude = m.LocationLatitude,
            LocationLongitude = m.LocationLongitude,
            IsCancelled = m.IsCancelled,
            GoingCount = m.GoingCount,
            InterestedCount = m.InterestedCount
          })
          .ToList();

        return Ok(meets);
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new { result = "error", message = "" });
      }
    }

    [HttpGet("[action]")]
    public IActionResult GetArchivedShackmeets()
    {
      try
      {
        var meets = this.dbContext
          .Meets
          .AsNoTracking()
          .Where(m => m.IsCancelled || m.EventDate < DateTime.Today)
          .Include(m => m.Rsvps)
          .Select(m => new MeetListingDto
          {
            MeetId = m.MeetId,
            Name = m.Name,
            Description = m.Description,
            OrganizerUsername = m.OrganizerUsername,
            EventDate = m.EventDate,
            LocationName = m.LocationName,
            LocationAddress = m.LocationAddress,
            LocationState = m.LocationState,
            LocationCountry = m.LocationCountry,
            LocationLatitude = m.LocationLatitude,
            LocationLongitude = m.LocationLongitude,
            IsCancelled = m.IsCancelled,
            GoingCount = m.GoingCount,
            InterestedCount = m.InterestedCount
          })
          .ToList();

        return Ok(meets);
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new { result = "error", message = "" });
      }
    }

    [HttpGet("[action]")]
    public IActionResult GetShackmeet(int meetId)
    {
      try
      {
        var meet = this.dbContext.Meets.AsNoTracking().SingleOrDefault(m => m.MeetId == meetId);

        if (meet == null)
        {
          return BadRequest(new { result = "error", message = "Shackmeet does not exist." });
        }

        var rsvpDtos = meet.Rsvps.Select(r => new RsvpDto
        {
          Username = r.Username,
          MeetId = r.MeetId,
          RsvpTypeId = r.RsvpTypeId,
          NumAttendees = r.NumAttendees
        }).ToList();

        var meetDto = new MeetDto
        {
          MeetId = meet.MeetId,
          OrganizerUsername = meet.OrganizerUsername,
          Name = meet.Name,
          Description = meet.Description,
          EventDate = meet.EventDate,
          LocationName = meet.LocationName,
          LocationAddress = meet.LocationAddress,
          LocationState = meet.LocationState,
          LocationCountry = meet.LocationCountry,
          LocationLatitude = meet.LocationLatitude,
          LocationLongitude = meet.LocationLongitude,
          Rsvps = rsvpDtos
        };

        return Ok(meetDto);
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
    public IActionResult CreateShackmeet([FromBody] MeetDto meetDto)
    {
      try
      {
        // PRL
        int numRecentMeets = this.dbContext.Meets.Count(m => m.OrganizerUsername == meetDto.OrganizerUsername && m.TimestampCreate >= DateTime.Now.AddMinutes(-5));

        if (numRecentMeets >= 2)
        {
          return BadRequest(new { result = "error", message = "Slow your rolls." });
        }

        // Get additional address info (and verify address)
        var mapsWrapper = new GoogleMapsWrapper();
        var addressInfo = mapsWrapper.GetAddressInfo(meetDto.LocationAddress);
      
        if (!addressInfo.IsValid)
        {
          return BadRequest(new { result = "error", message = "Invalid address." });
        }

        // Create new meet
        var meet = new Meet
        {
          Name = meetDto.Name,
          Description = meetDto.Description,
          OrganizerUsername = meetDto.OrganizerUsername,
          EventDate = meetDto.EventDate,
          LocationName = meetDto.LocationName,
          LocationAddress = meetDto.LocationAddress,
          LocationState = addressInfo.State,
          LocationCountry = addressInfo.Country,
          LocationLatitude = addressInfo.Latitude,
          LocationLongitude = addressInfo.Longitude,
          WillPostAnnouncement = meetDto.WillPostAnnouncement,
          IsCancelled = false
        };

        // Validate fields
        if (false)
        {
          // Return validation errors
        }

        this.dbContext.Meets.Add(meet);
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
    public IActionResult UpdateShackmeet([FromBody] MeetDto meetDto)
    {
      try
      {
        var meet = this.dbContext.Meets.SingleOrDefault(m => m.MeetId == meetDto.MeetId);

        if (meet == null)
        {
          return BadRequest(new { result = "error", message = "Shackmeet does not exist." });
        }

        // Get additional address info (and verify address)
        var mapsWrapper = new GoogleMapsWrapper();
        var addressInfo = mapsWrapper.GetAddressInfo(meetDto.LocationAddress);

        if (!addressInfo.IsValid)
        {
          return BadRequest(new { result = "error", message = "Invalid address." });
        }

        // Update meet
        meet.Name = meetDto.Name;
        meet.Description = meetDto.Description;
        meet.OrganizerUsername = meetDto.OrganizerUsername;
        meet.EventDate = meetDto.EventDate;
        meet.LocationName = meetDto.LocationName;
        meet.LocationAddress = meetDto.LocationAddress;
        meet.LocationState = addressInfo.State;
        meet.LocationCountry = addressInfo.Country;
        meet.LocationLatitude = addressInfo.Latitude;
        meet.LocationLongitude = addressInfo.Longitude;
        meet.WillPostAnnouncement = meetDto.WillPostAnnouncement;
        meet.IsCancelled = false;
        
        // Validate fields
        if (false)
        {
          // Return validation errors
        }

        this.dbContext.Meets.Attach(meet);
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
    public IActionResult CancelShackmeet([FromBody] MeetDto meetDto)
    {
      try
      {
        var meet = this.dbContext.Meets.SingleOrDefault(m => m.MeetId == meetDto.MeetId);

        if (meet == null)
        {
          return BadRequest(new { result = "error", message = "Shackmeet does not exist." });
        }

        // Update meet
        meet.IsCancelled = true;

        this.dbContext.Meets.Attach(meet);
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
    public IActionResult Rsvp([FromBody] RsvpDto rsvpDto)
    {
      try
      {
        // Verify user exists
        bool userExists = this.dbContext.Users.Any(u => u.Username == rsvpDto.Username);

        if (!userExists)
        {
          return BadRequest(new { result = "error", message = "User does not exist." });
        }

        // Verify meet exists
        bool meetExists = this.dbContext.Meets.Any(m => m.MeetId == rsvpDto.MeetId);

        if (!meetExists)
        {
          return BadRequest(new { result = "error", message = "Meet does not exist." });
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
