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
    private IGoogleMapsService googleMapsService;

    public GoogleMapsTests()
    {
      // Restricted by IP address
      string apiKey = "AIzaSyASCzvAms7C6-dnRvwYhBpl4VwoiJlj8MI";

      this.googleMapsService = new GoogleMapsService(apiKey);
    }

    [TestMethod]
    public void TestGetAddressInfoWithAddress()
    {
      string address = "2601 Preston Rd, Frisco, TX 75034";

      var result = googleMapsService.GetAddressInfo(address);

      Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void TestGetAddressInfoWithCountry()
    {
      string address = "Norway";

      var result = googleMapsService.GetAddressInfo(address);

      Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void TestGetAddressInfoWithState()
    {
      string address = "Texas";

      var result = googleMapsService.GetAddressInfo(address);

      Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void TestGetAddressInfoWithLatLong()
    {
      decimal latitude = 33.0991992m;
      decimal longitude = -96.81132m;

      var result = googleMapsService.GetAddressInfo(latitude, longitude);

      Assert.IsTrue(result.IsValid);
    }
  }
}
