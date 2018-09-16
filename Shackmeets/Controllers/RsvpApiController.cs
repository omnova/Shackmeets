using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Shackmeets.Models;
using Shackmeets.Dtos;
using Shackmeets.Validators;

namespace Shackmeets.Controllers
{
  [Route("api")]
  public class RsvpApiController : Controller
  {
    private readonly ShackmeetsDbContext dbContext;
    private readonly ILogger logger;

    public RsvpApiController(ShackmeetsDbContext context, ILogger<ShackmeetApiController> logger)
    {
      this.dbContext = context;
      this.logger = logger;
    }

    [HttpPost("[action]")]
    [Authorize]
    public IActionResult Rsvp([FromBody] RsvpDto rsvpDto)
    {
      try
      {
        this.logger.LogDebug("Rsvp");

        // Verify input exists
        if (rsvpDto == null)
        {
          return BadRequest(new { result = "error", message = "Input is not well formed." });
        }

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

        // Validate fields
        var validator = new RsvpValidator();
        var validationResult = validator.Validate(rsvp);

        if (!validationResult.IsValid)
        {
          return BadRequest(new { result = "error", messages = validationResult.Messages });
        }

        this.dbContext.SaveChanges();

        return Ok(new { result = "success" });
      }
      catch (Exception e)
      {
        // Log error
        this.logger.LogError("Message: {0}" + Environment.NewLine + "{1}", e.Message, e.StackTrace);

        return BadRequest(new { result = "error", message = "An error occurred. Please notify omnova if errors persist." });
      }
    }
  }
}
