using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shackmeets
{
  public class AppSettings
  {
    public class ChattySettings
    {
      public string Username { get; set; }
      public string Password { get; set; }
    }

    public class GoogleMapsSettings
    {
      public string MapsApiKey { get; set; }
    }

    public string Secret { get; set; }
    public string ConnectionString { get; set; }

    public ChattySettings Chatty { get; set; }
    public GoogleMapsSettings GoogleMaps { get; set; }

    public AppSettings()
    {
      this.Chatty = new ChattySettings();
      this.GoogleMaps = new GoogleMapsSettings();
    }
  }
}
