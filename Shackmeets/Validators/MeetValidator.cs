using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shackmeets.Models;

namespace Shackmeets.Validators
{
  public class MeetValidator
  {
    public ValidationResult Validate(Meet meet)
    {
      var result = new ValidationResult();

      //Name = meetDto.Name,



      //Description = meetDto.Description,
      //OrganizerUsername = meetDto.OrganizerUsername,
      //EventDate = meetDto.EventDate,
      //LocationName = meetDto.LocationName,
      //LocationAddress = meetDto.LocationAddress,
      //LocationState = meetDto.LocationState,
      //LocationCountry = meetDto.LocationCountry,
      //LocationLatitude = meetDto.LocationLatitude,
      //LocationLongitude = meetDto.LocationLongitude,
      //WillPostAnnouncement = meetDto.WillPostAnnouncement,
      //IsDeleted = false

      return result;
    }

    private bool IsRequiredLength(string entityName, string fieldName, string value, int? minLength, int? maxLength, ValidationResult result)
    {
      if (minLength.HasValue && maxLength.HasValue && (value.Length < minLength.Value || value.Length > maxLength.Value))
      {
        result.AddValidationError(entityName, fieldName, string.Format("Value must be between {0} and {1}", minLength, maxLength));

        return false;
      }

      if (minLength.HasValue && value.Length < minLength.Value)
      {
        result.AddValidationError(entityName, fieldName, string.Format("Value must be between {0} and {1}", minLength, maxLength));

        return false;
      }

      if (maxLength.HasValue && value.Length > maxLength.Value)
      {
        result.AddValidationError(entityName, fieldName, string.Format("Value must be between {0} and {1}", minLength, maxLength));

        return false;
      }

      return true;
    }
  }
}
