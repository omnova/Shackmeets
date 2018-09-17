namespace Shackmeets.Models
{
  public enum RsvpType
  {
    NotGoing = 0,
    Interested = 1,
    Going = 2
  }

  public class Rsvp
  {
    public int RsvpId { get; set; }

    public int MeetId { get; set; }
    public string Username { get; set; }

    public RsvpType RsvpType { get; set; }
    public int NumAttendees { get; set; }
    
    public Meet Meet { get; set; }
    public User User { get; set; }
  }
}
