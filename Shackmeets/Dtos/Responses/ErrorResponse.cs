using Microsoft.AspNetCore.Mvc;

namespace Shackmeets.Dtos
{
  public class ErrorResponse
  {
    public string Result { get; private set; };
    public string Message { get; set; }

    public ErrorResponse(string message)
    {
      this.Result = "error";
      this.Message = message;
    }

    public static BadRequestObjectResult CreateResult(string message)
    {
      return new BadRequestObjectResult(new ErrorResponse(message));
    }
  }
}
