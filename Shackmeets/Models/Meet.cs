using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shackmeets.Models
{
  public class Meet
  {
    public int MeetId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string OrganizerUsername { get; set; }
    public DateTime EventDate { get; set; }

    [JsonIgnore]
    public DateTime TimestampCreate { get; set; }

    [JsonIgnore]
    public DateTime? TimestampChange { get; set; }

    public string LocationName { get; set; }
    public string LocationAddress { get; set; }
    public string LocationState { get; set; }
    public string LocationCountry { get; set; }
    public decimal LocationLatitude { get; set; }
    public decimal LocationLongitude { get; set; }

    [JsonIgnore]
    public bool WillPostAnnouncement { get; set; }

    [JsonIgnore]
    public DateTime? LastAnnouncementPostDate { get; set; }

    [JsonIgnore]
    public bool IsDeleted { get; set; }

    [JsonIgnore]
    public User Organizer { get; set; }

    public List<Rsvp> Rsvps { get; set; }
  }
}
