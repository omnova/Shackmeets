using Shackmeets.Models;

namespace Shackmeets.Validators
{
  public class MeetValidator
  {
    public ValidationResult Validate(Meet meet)
    {
      var validator = new EntityValidator();
      var result = new ValidationResult();

      ValidationMessage message = null;

      // Name
      if (!validator.IsRequiredLength("name", meet.Name, 1, 100, out message))
        result.Messages.Add(message);

      // Description
      if (!validator.IsRequiredLength("description", meet.Description, 1, null, out message))
        result.Messages.Add(message);

      // EventDate
      // TODO: eventually validate against current date, but this isn't super important

      // OrganizerUsername
      if (!validator.IsRequiredLength("organizerUsername", meet.OrganizerUsername, 1, null, out message))
        result.Messages.Add(message);

      // LocationName
      if (!validator.IsRequiredLength("locationName", meet.LocationName, 1, 50, out message))
        result.Messages.Add(message);

      // LocationAddress
      if (!validator.IsRequiredLength("locationAddress", meet.LocationAddress, 1, 50, out message))
        result.Messages.Add(message);

      // LocationLatitude
      if (!validator.IsRequiredDecimalRange("locationLatitude", meet.LocationLatitude, -90, 90, out message))
        result.Messages.Add(message);

      // LocationLongitude
      if (!validator.IsRequiredDecimalRange("locationLongitude", meet.LocationLongitude, -180, 180, out message))
        result.Messages.Add(message);

      return result;
    }
  }
}
