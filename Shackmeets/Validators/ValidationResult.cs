using System;
using System.Collections.Generic;
using System.Linq;

namespace Shackmeets.Validators
{
  public class ValidationResult
  {
    public bool IsValid
    {
      get { return !this.Messages.Any(); }
    }
    
    public List<ValidationMessage> Messages { get; private set; }

    public ValidationResult()
    {
      this.Messages = new List<ValidationMessage>();
    }
  }
}
