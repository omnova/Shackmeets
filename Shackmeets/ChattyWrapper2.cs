using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shackmeets
{
  /// <summary>
  /// Provides access to Shacknews functionality via the WinChatty v2 API.
  /// </summary>
  public class ChattyWrapper2
  {
    // Remember: this is only to be used for the Shackmeets account

    public async Task<bool> VerifyConnectionAsync()
    {
      const string url = @"https://winchatty.com/v2/checkConnection";

      using (var client = CreateHttpClient())
      {
        using (var content = new StringContent(""))
        {
          using (var result = await client.PostAsync(url, content))
          {
            var responseMessage = await result.Content.ReadAsStringAsync();

            return responseMessage == string.Empty;
          }
        }
      }
    }

    public async Task<bool> VerifyCredentialsAsync(string username, string password)
    {
      const string url = @"https://winchatty.com/v2/verifyCredentials";
      const string dataFormat = @"username={0}&password={1}";

      string data = string.Format(dataFormat, username, password);

      using (var client = CreateHttpClient())
      {
        using (var content = new StringContent(data))
        {
          using (var result = await client.PostAsync(url, content))
          {
            var responseMessage = await result.Content.ReadAsStringAsync();

            return responseMessage.Contains("\"isValid\":true");
          }
        }
      }
    }

    public async Task<bool> PostCommentAsync(string username, string password, int parentId, string text)
    {
      const string url = @"https://winchatty.com/v2/postComment";
      const string dataFormat = @"username={0}&password={1}&parentId={2}&text={3}";

      string data = string.Format(dataFormat, username, password, parentId, text);

      using (var client = CreateHttpClient())
      {
        using (var content = new StringContent(data))
        {
          using (var result = await client.PostAsync(url, content))
          {
            var responseMessage = await result.Content.ReadAsStringAsync();

            return responseMessage.Contains("\"isValid\":true");
          }
        }
      }
    }

    public async Task<bool> SendMessageAsync(string username, string password, string targetUsername, string subject, string text)
    {
      const string url = @"https://winchatty.com/v2/sendMessage";
      const string dataFormat = @"username={0}&password={1}&to={2}&subject={3}&body={4}";

      string data = string.Format(dataFormat, username, password, targetUsername, subject, text);

      using (var client = CreateHttpClient())
      {
        using (var content = new StringContent(data))
        {
          using (var result = await client.PostAsync(url, content))
          {
            var responseMessage = await result.Content.ReadAsStringAsync();

            return responseMessage.Contains("\"isValid\":true");
          }
        }
      }
    }

    private HttpClient CreateHttpClient()
    {
      var client = new HttpClient();

      client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
      client.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36");
      client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

      return client;
    }
  }
}
