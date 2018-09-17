using Shackmeets.Models;

namespace Shackmeets.Validators
{
  public class RsvpValidator
  {
    public ValidationResult Validate(Rsvp rsvp)
    {
      var validator = new EntityValidator();
      var result = new ValidationResult();

      ValidationMessage message = null;

      // RsvpTypeId
      if (!validator.IsRequiredIntegerRange("rsvpTypeId", (int)rsvp.RsvpType, 0, 2, out message))
        result.Messages.Add(message);

      // NumAttendees
      if (!validator.IsRequiredIntegerRange("locationLatitude", rsvp.NumAttendees, 1, 10, out message))
        result.Messages.Add(message);

      return result;
    }
  }
}
