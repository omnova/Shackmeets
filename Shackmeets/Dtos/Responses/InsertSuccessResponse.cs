using Microsoft.AspNetCore.Mvc;

namespace Shackmeets.Dtos
{
  public class InsertSuccessResponse
  {
    public string Result { get; private set; }
    public int NewId { get; private set; }

    public InsertSuccessResponse(int newId)
    {
      this.Result = "success";
      this.NewId = newId;
    }

    public static OkObjectResult CreateResult(int newId)
    {
      return new OkObjectResult(new InsertSuccessResponse(newId));
    }
  }
}
