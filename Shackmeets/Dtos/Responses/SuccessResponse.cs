using Microsoft.AspNetCore.Mvc;

namespace Shackmeets.Dtos
{
  public class SuccessResponse
  {
    public string Result { get; private set; }

    public SuccessResponse()
    {
      this.Result = "success";
    }

    public static OkObjectResult CreateResult()
    {
      return new OkObjectResult(new SuccessResponse());
    }
  }
}
