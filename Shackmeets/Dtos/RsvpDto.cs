using Shackmeets.Models;

namespace Shackmeets.Dtos
{
  public class RsvpDto
  {    
    public int MeetId { get; set; }
    public string Username { get; set; }
    public RsvpType RsvpType { get; set; }
    public int NumAttendees { get; set; }
  }
}
