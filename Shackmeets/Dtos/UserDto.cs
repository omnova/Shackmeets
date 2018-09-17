using Shackmeets.Models;

namespace Shackmeets.Dtos
{
  public class UserDto
  {
    public string Username { get; set; }
    public string Password { get; set; }

    // User preferences - may need to move to a different DTO
    public decimal LocationLatitude { get; set; }
    public decimal LocationLongitude { get; set; }
    public int MaxNotificationDistance { get; set; }

    public NotificationOption NotificationOption { get; set; }
    public bool NotifyByShackmessage { get; set; }
    public bool NotifyByEmail { get; set; }
    public string NotificationEmail { get; set; }
  }
}
