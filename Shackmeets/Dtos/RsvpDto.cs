using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shackmeets.Dtos
{
  public class RsvpDto
  {    
    public int MeetId { get; set; }
    public string Username { get; set; }
    public int RsvpTypeId { get; set; }
    public int NumAttendees { get; set; }
  }
}
