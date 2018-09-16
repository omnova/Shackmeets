using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Shackmeets.Services
{
  /// <summary>
  /// Service for querying the Google Maps API.
  /// </summary>
  public interface IGoogleMapsService
  {
    /// <summary>
    /// Gets information corresponding to an address.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    GoogleMapsAddressInfo GetAddressInfo(string address);

    /// <summary>
    /// Gets information corresponding to an address.
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <returns></returns>
    GoogleMapsAddressInfo GetAddressInfo(decimal latitude, decimal longitude);
  }

  public class GoogleMapsAddressInfo
  {
    public string FormattedAddress { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public bool IsValid { get; set; }
  }

  public class GoogleMapsService : IGoogleMapsService
  {
    private readonly AppSettings appSettings;

    public GoogleMapsService()
    {
    }

    public GoogleMapsService(IOptions<AppSettings> appSettings)
    {
      this.appSettings = appSettings.Value;
    }

    public GoogleMapsAddressInfo GetAddressInfo(string address)
    {      
      const string urlFormat = "https://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false";

      string url = string.Format(urlFormat, WebUtility.UrlEncode(address));

      return GetGoogleMapsAddressInfo(url);
    }

    public GoogleMapsAddressInfo GetAddressInfo(decimal latitude, decimal longitude)
    {
      const string urlFormat = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&sensor=false";

      string url = string.Format(urlFormat, WebUtility.UrlEncode(latitude.ToString()), WebUtility.UrlEncode(longitude.ToString()));

      return GetGoogleMapsAddressInfo(url);
    }

    // Could use a better name, or maybe just not bother
    private GoogleMapsAddressInfo GetGoogleMapsAddressInfo(string url)
    {
      using (var client = new WebClient())
      {
        string result = client.DownloadString(url);

        return ParseGoogleMapsAddressInfo(result);
      }
    }

    private GoogleMapsAddressInfo ParseGoogleMapsAddressInfo(string apiResult)
    {
      var jobject = JObject.Parse(apiResult);

      var addressInfo = new GoogleMapsAddressInfo(); // Call api

      if (jobject["results"] == null || !jobject["results"].Children().Any())
      {
        addressInfo.IsValid = false;
      }
      else
      {
        try
        {
          addressInfo.FormattedAddress = jobject["results"][0].Value<string>("formatted_address");
          addressInfo.Latitude = jobject["results"][0]["geometry"]["location"].Value<decimal>("lat");
          addressInfo.Longitude = jobject["results"][0]["geometry"]["location"].Value<decimal>("lng");
          addressInfo.State = jobject["results"][0]["address_components"].Children().FirstOrDefault(c => c["types"].Values<string>().Any(t => t == "administrative_area_level_1"))?.Value<string>("long_name");
          addressInfo.Country = jobject["results"][0]["address_components"].Children().FirstOrDefault(c => c["types"].Values<string>().Any(t => t == "country"))?.Value<string>("long_name");

          addressInfo.IsValid = true;
        }
        catch
        {
          addressInfo.IsValid = false;
        }
      }

      return addressInfo;
    }
      
    public void haversine(decimal v1, decimal v2, decimal v3, decimal v4)
    { 
      //$l1 = deg2rad($l1); 
      //$sinl1 = sin($l1); 
      //$l2 = deg2rad($l2); 
      //$o1 = deg2rad($o1); 
      //$o2 = deg2rad($o2);

      //return (7926 - 26 * $sinl1) *asin(min(1, 0.707106781186548 * sqrt((1 - (sin($l2) * $sinl1) - cos($l1) * cos($l2) * cos($o2 - $o1)))));
    }
  }
}
