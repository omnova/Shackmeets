using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Shackmeets.Dtos
{
  public class ValidationErrorResponse
  {
    public string Result { get; private set; }
    public IEnumerable<object> Messages { get; set; }

    public ValidationErrorResponse(IEnumerable<object> messages)
    {
      this.Result = "error";
      this.Messages = messages;
    }

    public ValidationErrorResponse(string field, string messages)
    {
      this.Result = "error";
      this.Messages = new List<string>
      {
        string.Format("{0}: {1}", field, messages)
      };
    }

    public static BadRequestObjectResult CreateResult(IEnumerable<object> messages)
    {
      return new BadRequestObjectResult(new ValidationErrorResponse(messages));
    }
  }
}
