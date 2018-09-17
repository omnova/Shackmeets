using Microsoft.AspNetCore.Mvc;

namespace Shackmeets.Dtos
{
  public class BadInputResponse
  {
    public string Result { get; private set; }
    public string Message { get; private set; }

    public BadInputResponse()
    {
      this.Result = "error";
      this.Message = "Input is not well formed.";
    }

    public static BadRequestObjectResult CreateResult()
    {
      return new BadRequestObjectResult(new BadInputResponse());
    }
  }
}
