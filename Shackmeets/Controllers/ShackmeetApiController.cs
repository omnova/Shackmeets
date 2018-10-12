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
using Shackmeets.Services;
using Shackmeets.Validators;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Shackmeets.Controllers
{
  [Route("api")]
  public class ShackmeetApiController : Controller
  {
    private readonly ShackmeetsDbContext dbContext;
    private readonly ILogger logger;
    private readonly AppSettings appSettings;
    private readonly IChattyService chattyService;
    private readonly IGoogleMapsService googleMapsService;

    public ShackmeetApiController(ShackmeetsDbContext context, ILogger<ShackmeetApiController> logger, IOptions<AppSettings> appSettings, IChattyService chattyService, IGoogleMapsService googleMapsService)
    {
      this.dbContext = context;
      this.logger = logger;
      this.appSettings = appSettings.Value;
      this.chattyService = chattyService;
      this.googleMapsService = googleMapsService;
    }

    [HttpGet("[action]")]
    public IActionResult GetShackmeets()
    {
      try
      {
        this.logger.LogDebug("GetShackmeets");

        // Could probably be more efficient
        var meets = this.dbContext
          .Meets
          .AsNoTracking()
          .Where(m => !m.IsCancelled && m.EventDate >= DateTime.Today)
          .Include(m => m.Rsvps)
          .ToList();

        // Had to split this because RSVPs weren't being included for some reason
        var meetListings = meets
          .Select(m => new MeetDto
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
            Rsvps = m.Rsvps.Select(r => new RsvpDto
            {
              Username = r.Username,
              MeetId = r.MeetId,
              RsvpType = r.RsvpType,
              NumAttendees = r.NumAttendees
            }).ToList()
          })
          .ToList();

        return Ok(meetListings);
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new CriticalErrorResponse());
      }
    }

    [HttpGet("[action]")]
    public IActionResult GetArchivedShackmeets()
    {
      try
      {
        this.logger.LogDebug("GetArchivedShackmeets");

        // Could probably be more efficient
        var meets = this.dbContext
          .Meets
          .AsNoTracking()
          .Where(m => m.IsCancelled || m.EventDate < DateTime.Today)
          .Include(m => m.Rsvps)
          .ToList();

        // Had to split this because RSVPs weren't being included for some reason
        var meetListings = meets
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

        return Ok(meetListings);
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new CriticalErrorResponse());
      }
    }

    [HttpGet("[action]")]
    public IActionResult GetShackmeet(int meetId)
    {
      try
      {
        this.logger.LogDebug("GetShackmeet");

        // Verify meet exists
        var meet = this.dbContext
          .Meets
          .AsNoTracking()
          .Include(m => m.Rsvps)
          .SingleOrDefault(m => m.MeetId == meetId);

        if (meet == null)
        {
          return BadRequest(new ErrorResponse("Shackmeet does not exist."));
        }

        // Load RSVPs
        var rsvpDtos = meet.Rsvps.Select(r => new RsvpDto
        {
          Username = r.Username,
          MeetId = r.MeetId,
          RsvpType = r.RsvpType,
          NumAttendees = r.NumAttendees
        }).ToList();

        // Map data to DTO
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

        return BadRequest(new CriticalErrorResponse());
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult CreateShackmeet([FromBody] MeetDto meetDto)
    {
      try
      {
        this.logger.LogDebug("CreateShackmeet");

        // Verify input exists
        if (meetDto == null)
        {
          return BadRequest(new BadInputResponse());
        }

        // PRL
        int numRecentMeets = this.dbContext.Meets.Count(m => m.OrganizerUsername == meetDto.OrganizerUsername && m.TimestampCreate >= DateTime.Now.AddMinutes(-5));

        if (numRecentMeets >= 2)
        {
          return BadRequest(new ErrorResponse("Slow your rolls."));
        }

        // Get additional address info (and verify address)
        var addressInfo = this.googleMapsService.GetAddressInfo(meetDto.LocationAddress);
      
        if (!addressInfo.IsValid)
        {
          return BadRequest(new ValidationErrorResponse("locationAddress", "Address does not correspond to a valid geocode location."));
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
        var validator = new MeetValidator();
        var validationResult = validator.Validate(meet);

        if (!validationResult.IsValid)
        {
          return BadRequest(new ValidationErrorResponse(validationResult.Messages));
        }

        this.dbContext.Meets.Add(meet);
        this.dbContext.SaveChanges();

        // Send new shackmeet notifications

        return Ok(new InsertSuccessResponse(meet.MeetId));
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);
        
        return BadRequest(new CriticalErrorResponse());
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult UpdateShackmeet([FromBody] MeetDto meetDto)
    {
      try
      {
        this.logger.LogDebug("UpdateShackmeet");

        // Verify input exists
        if (meetDto == null)
        {
          return BadRequest(new BadInputResponse());
        }

        // Load meet with related RSVPs and users (for use with notifications)
        var meet = this.dbContext
          .Meets
          .Where(m => m.MeetId == meetDto.MeetId)
          .Include(m => m.Rsvps)
          .ThenInclude(r => r.User)
          .SingleOrDefault(m => m.MeetId == meetDto.MeetId);
        
        // Verify meet exists
        if (meet == null)
        {
          return BadRequest(new ValidationErrorResponse("meetId", "Shackmeet does not exist."));
        }

        // Use the username of the authenticated user so they can only mess with their own shackmeets.
        var currentUsername = this.User.FindFirst(ClaimTypes.Name).Value;

        if (meet.OrganizerUsername != currentUsername)
        {
          return BadRequest(new ErrorResponse("Only the organizer can update a shackmeet."));
        }

        // Get additional address info (and verify address)
        var addressInfo = this.googleMapsService.GetAddressInfo(meetDto.LocationAddress);

        if (!addressInfo.IsValid)
        {
          return BadRequest(new ValidationErrorResponse("locationAddress", "Address does not correspond to a valid geocode location."));
        }

        this.dbContext.Meets.Attach(meet);

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
        var validator = new MeetValidator();
        var validationResult = validator.Validate(meet);

        if (!validationResult.IsValid)
        {
          return BadRequest(new ValidationErrorResponse(validationResult.Messages));
        }

        this.dbContext.SaveChanges();

        // Send update notifications

        return Ok(new SuccessResponse());
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new CriticalErrorResponse());
      }
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult CancelShackmeet([FromBody] MeetDto meetDto)
    {
      try
      {
        this.logger.LogDebug("CancelShackmeet");

        // Verify input exists
        if (meetDto == null)
        {
          return BadRequest(new BadInputResponse());
        }

        // Load meet with related RSVPs and users (for use with notifications)
        var meet = this.dbContext
          .Meets
          .Where(m => m.MeetId == meetDto.MeetId)
          .Include(m => m.Rsvps)
          .ThenInclude(r => r.User)
          .SingleOrDefault(m => m.MeetId == meetDto.MeetId);

        // Verify meet exists
        if (meet == null)
        {
          return BadRequest(new ValidationErrorResponse("meetId", "Shackmeet does not exist."));
        }

        // Use the username of the authenticated user so they can only mess with their own shackmeets.
        var currentUsername = this.User.FindFirst(ClaimTypes.Name).Value;

        if (meet.OrganizerUsername != currentUsername)
        {
          return BadRequest(new ErrorResponse("Only the organizer can cancel a shackmeet."));
        }

        this.dbContext.Meets.Attach(meet);

        // Update meet
        meet.IsCancelled = true;

        this.dbContext.SaveChanges();

        // Send cancellation notifications
        //var users = meet.Rsvps.Where()

        return Ok(new SuccessResponse());
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new CriticalErrorResponse());
      }
    }

    [HttpPost("[action]")]
    public IActionResult ResendShackmeetNotification([FromBody] MeetDto meetDto)
    {
      try
      {
        this.logger.LogDebug("ResendShackmeetNotification");

        // Verify input exists
        if (meetDto == null)
        {
          return BadRequest(new BadInputResponse());
        }

        var meet = this.dbContext.Meets.SingleOrDefault(m => m.MeetId == meetDto.MeetId);

        if (meet == null)
        {
          return BadRequest(new ValidationErrorResponse("meetId", "Shackmeet does not exist."));
        }

        // Use the username of the authenticated user so they can only mess with their own shackmeets.
        var currentUsername = this.User.FindFirst(ClaimTypes.Name).Value;

        if (meet.OrganizerUsername != currentUsername)
        {
          return BadRequest(new ErrorResponse("Only the organizer can resend notifications."));
        }

        if (meet.LastAnnouncementPostDate.HasValue && meet.LastAnnouncementPostDate.Value.AddHours(18) < DateTime.Now)
        {
          return BadRequest(new ErrorResponse("You may only resend notifications every 18 hours."));
        }

        this.dbContext.Meets.Attach(meet);

        // Update meet
        meet.IsCancelled = true;

        this.dbContext.SaveChanges();

        // Send cancellation notifications
        var usersToShackmessage = meet.Rsvps.Where(r => r.User.NotifyByShackmessage).ToList();
        var usersToEmail = meet.Rsvps.Where(r => r.User.NotifyByEmail).ToList();

        var notificationHelper = new NotificationHelper(this.appSettings, this.chattyService);

        //foreach (var user in usersToShackmessage)
        //{
        //  notificationHelper.SendShackMessage(user, )
        //}

        return Ok(new SuccessResponse());
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new CriticalErrorResponse());
      }
    }
  }
}
