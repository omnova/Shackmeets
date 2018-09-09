using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shackmeets.Models
{
  public class Rsvp
  {
    public int RsvpId { get; set; }

    public int MeetId { get; set; }
    public string Username { get; set; }

    public int RsvpTypeId { get; set; }
    public int NumAttendees { get; set; }

    public Meet Meet { get; set; }
    public User User { get; set; }
  }
}
