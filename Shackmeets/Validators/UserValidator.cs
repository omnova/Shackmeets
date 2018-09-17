using Shackmeets.Models;

namespace Shackmeets.Validators
{
  public class UserValidator
  {
    public ValidationResult Validate(User user)
    {
      var validator = new EntityValidator();
      var result = new ValidationResult();

      ValidationMessage message = null;

      // LocationLatitude
      if (!validator.IsRequiredDecimalRange("locationLatitude", user.LocationLatitude, -90, 90, out message))
        result.Messages.Add(message);

      // LocationLongitude
      if (!validator.IsRequiredDecimalRange("locationLongitude", user.LocationLongitude, -180, 180, out message))
        result.Messages.Add(message);

      // MaxNotificationDistance
      if (!validator.IsRequiredIntegerRange("maxNotificationDistance", user.MaxNotificationDistance, null, 12450, out message))
        result.Messages.Add(message);

      // NotificationOption
      if (!validator.IsRequiredIntegerRange("notificationOption", (int)user.NotificationOption, 0, 2, out message))
        result.Messages.Add(message);

      // NotifyByEmail
      if (user.NotifyByEmail && !validator.IsRequiredLength("notificationEmail", user.NotificationEmail, 1, null, out message))
        result.Messages.Add(message);

      return result;
    }
  }
}
