using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shackmeets.Validators
{
  public class ValidationResult
  {
    public bool IsValid
    {
      get { return !this.ErrorMessages.Any(); }
    }
    
    public List<string> Messages { get; private set; }
    public List<string> ErrorMessages { get; private set; }
    public List<string> WarningMessages { get; private set; }

    public ValidationResult()
    {
      this.Messages = new List<string>();
      this.ErrorMessages = new List<string>();
      this.WarningMessages = new List<string>();
    }

    public void AddValidationWarning(string entityName, string fieldName, string message)
    {
      string formattedMessage = string.Format("{0}.{1}: {2}", entityName, fieldName, message);

      this.WarningMessages.Add(formattedMessage);
    }

    public void AddValidationError(string entityName, string fieldName, string message)
    {
      string formattedMessage = string.Format("{0}.{1}: {2}", entityName, fieldName, message);

      this.ErrorMessages.Add(formattedMessage);
    }
  }
}
