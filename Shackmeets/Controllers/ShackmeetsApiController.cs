using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Shackmeets.Controllers
{
  [Route("api/[controller]")]
  public class ShackmeetsApiController : Controller
  {
    private static string[] Summaries = new[]
    {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet("[action]")]
    public IEnumerable<WeatherForecast> WeatherForecasts(int startDateIndex)
    {
      var rng = new Random();
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        DateFormatted = DateTime.Now.AddDays(index + startDateIndex).ToString("d"),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      });
    }

    public class WeatherForecast
    {
      public string DateFormatted { get; set; }
      public int TemperatureC { get; set; }
      public string Summary { get; set; }

      public int TemperatureF
      {
        get
        {
          return 32 + (int)(TemperatureC / 0.5556);
        }
      }
    }



    // placeholder stuff
    
    public void LogIn()
    {

    }
    
    public void GetFutureShackmeets()
    {
      
    }

    public void GetArchivedShackmeets()
    {

    }

    public void GetShackmeet(int meetId)
    {

    }

    public void Rsvp(int meetId, int userId)
    {

    }
    
    public void ResendNotification()
    {

    }
  }
}
