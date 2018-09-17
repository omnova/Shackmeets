using System;

namespace Shackmeets.Validators
{
  public class EntityValidator
  {
    public bool IsRequiredIntegerRange(string fieldName, int value, int? minValue, int? maxValue, out ValidationMessage message)
    {
      message = null;

      // Range
      if (minValue.HasValue && maxValue.HasValue && (value < minValue.Value || value > maxValue.Value))
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be an integer between {0} and {1}.", minValue, maxValue)
        };

        return false;
      }

      // Min value
      if (minValue.HasValue && value < minValue.Value)
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be an integer no less than {0}.", minValue)
        };

        return false;
      }

      // Max value
      if (maxValue.HasValue && value > maxValue.Value)
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be an integer no greater than {0}.", maxValue)
        };

        return false;
      }

      return true;
    }

    public bool IsRequiredDecimalRange(string fieldName, decimal? value, decimal? minValue, decimal? maxValue, out ValidationMessage message)
    {
      message = null;

      // Range
      if (minValue.HasValue && maxValue.HasValue && (value < minValue.Value || value > maxValue.Value))
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be a number between {0} and {1}.", minValue, maxValue)
        };

        return false;
      }

      // Min value
      if (minValue.HasValue && value < minValue.Value)
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be a number no less than {0}.", minValue)
        };

        return false;
      }

      // Max value
      if (maxValue.HasValue && value > maxValue.Value)
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be a number no greater than {0}.", maxValue)
        };

        return false;
      }

      return true;
    }

    public bool IsRequiredLength(string fieldName, string value, int? minLength, int? maxLength, out ValidationMessage message)
    {
      message = null;

      // Range
      if (minLength.HasValue && maxLength.HasValue && (value.Length < minLength.Value || value.Length > maxLength.Value))
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be between {0} and {1} characters long.", minLength, maxLength)
        };

        return false;
      }

      // Min length
      if (minLength.HasValue && value.Length < minLength.Value)
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be at least {0} characters long.", minLength)
        };

        return false;
      }

      // Max length
      if (maxLength.HasValue && value.Length > maxLength.Value)
      {
        message = new ValidationMessage
        {
          Field = fieldName,
          Message = string.Format("Value must be at most {0} characters long.", maxLength)
        };

        return false;
      }

      return true;
    }
  }
}
