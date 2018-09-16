using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shackmeets
{
  public class GoogleMapsAddressInfo
  {
    public string Address { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool IsValid { get; set; }
  }

  public class GoogleMapsWrapper
  {
    public GoogleMapsAddressInfo GetAddressInfo(string address)
    {
      var addressInfo = new GoogleMapsAddressInfo(); // Call api

      addressInfo.IsValid = true;

      return addressInfo;
    }
  }
}
