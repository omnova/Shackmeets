using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shackmeets.Models
{
  public class User
  {
    //public int UserId { get; set; }

    public string Username { get; set; }
    public string SessionKey { get; set; }

    public decimal LocationLatitude { get; set; }
    public decimal LocationLongitude { get; set; }
    public int MaxNotificationDistance { get; set; }

    public int NotificationOptionId { get; set; }
    public bool NotifyByShackmessage { get; set; }
    public bool NotifyByEmail { get; set; }
    public string NotificationEmail { get; set; }

    public bool IsBanned { get; set; }

    public List<Meet> Meets { get; set; }
    public List<Rsvp> Rsvps { get; set; }
  }
}
