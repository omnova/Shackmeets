using System;

namespace Shackmeets.Dtos
{
  public class MeetListingDto
  {
    public int MeetId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public string OrganizerUsername { get; set; }
    public DateTime EventDate { get; set; }

    public string LocationName { get; set; }
    public string LocationAddress { get; set; }
    public string LocationState { get; set; }
    public string LocationCountry { get; set; }
    public decimal LocationLatitude { get; set; }
    public decimal LocationLongitude { get; set; }

    public bool IsCancelled { get; set; }

    public int InterestedCount { get; set; }
    public int GoingCount { get; set; }
  }
}
