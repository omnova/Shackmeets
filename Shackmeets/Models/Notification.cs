using System;

namespace Shackmeets.Models
{
  public enum NotificationType
  {
    Shackmessage = 1,
    Email = 2
  }

  public enum NotificationReason
  {
    ShackmeetAnnouncement = 1,
    ShackmeetReminder = 2,
    ShackmeetUpdate = 3,
    ShackmeetCancellation = 4
  }

  public class Notification
  {
    public int NotificationId { get; set; }

    public NotificationType NotificationType { get; set; }
    public NotificationReason NotificationReason { get; set; }
    
    public int? MeetId { get; set; }

    public string TargetUsername { get; set; }
    public string MessageSubject { get; set; }
    public string MessageBody { get; set; }

    public bool IsSent { get; set; }

    public DateTime TimestampCreate { get; set; }

    public User TargetUser { get; set; }
    public Meet Meet { get; set; }
  }
}
