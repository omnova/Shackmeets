using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shackmeets.Models
{
  public class Notification
  {
    public int NotificationId { get; set; }

    public string TargetUsername { get; set; }
    public int NotificationReasonId { get; set; }

    public int? MeetId { get; set; }

    public string MessageSubject { get; set; }
    public string MessageBody { get; set; }

    public bool IsSent { get; set; }

    public DateTime TimestampCreate { get; set; }

    public Meet Meet { get; set; }
  }
}
