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
  public class NotificationApiController : Controller
  {
    private ShackmeetsDbContext dbContext = null;

    public NotificationApiController(ShackmeetsDbContext context)
    {
      this.dbContext = context;
    }

    [HttpPost("[action]")]
    public IActionResult ResendNotification([FromBody] ResendNotificationDto resendDto)
    {
      return Ok(new { message = "Not implemented" });
    }
  }
}
