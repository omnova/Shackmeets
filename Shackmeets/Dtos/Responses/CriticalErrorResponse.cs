using Microsoft.AspNetCore.Mvc;

namespace Shackmeets.Dtos
{
  public class CriticalErrorResponse
  {
    public string Result { get; private set; }
    public string Message { get; private set; }

    public CriticalErrorResponse()
    {
      this.Result = "error";
      this.Message = "An error occurred. Please notify omnova if errors persist.";
    }

    public static BadRequestObjectResult CreateResult()
    {
      return new BadRequestObjectResult(new CriticalErrorResponse());
    }
  }
}
