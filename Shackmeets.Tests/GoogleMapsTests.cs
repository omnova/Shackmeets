using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shackmeets.Services;

namespace Shackmeets.Tests
{
  [TestClass]
  public class GoogleMapsTests
  {
    private IGoogleMapsService googleMapsService = new GoogleMapsService();

    [TestMethod]
    public void TestGetAddressInfoWithAddress()
    {
      string address = "2601 Preston Rd, Frisco, TX 75034";

      var result = googleMapsService.GetAddressInfo(address);
    }

    [TestMethod]
    public void TestGetAddressInfoWithLatLong()
    {
      decimal latitude = 34.234m;
      decimal longitude = 25.234m;

      var result = googleMapsService.GetAddressInfo(latitude, longitude);
    }
  }
}
