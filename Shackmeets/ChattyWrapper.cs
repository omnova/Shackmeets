using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// TODO: Make async.  I'm lazy.

namespace Shackmeets
{
  /// <summary>
  /// Provides access to Shacknews functionality using the WinChatty v2 API.
  /// </summary>
  public class ChattyWrapper
  {
    // Remember: this is only to be used for the Shackmeets account
    
    /// <summary>
    /// Verifies that the connection to the Winchatty v2 API is valid.
    /// </summary>
    /// <returns>True if valid.</returns>
    public bool VerifyConnection()
    {
      const string url = @"https://winchatty.com/v2/checkConnection";

      using (var client = new ChattyWebClient())
      {
        string result = client.DownloadString(url);

        return result == "";
      }
    }

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

    /// <summary>
    /// Verifies that credentials are valid.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>True if valid.</returns>
    public bool VerifyCredentials(string username, string password)
    {
      const string url = @"https://winchatty.com/v2/verifyCredentials";
      const string dataFormat = @"username={0}&password={1}";

      using (var client = new ChattyWebClient())
      {
        string data = string.Format(dataFormat, username, password);
        string result = client.UploadString(url, data);

        return result.Contains("\"isValid\":true");
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

    /// <summary>
    /// Posts a Shacknews comment.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="parentId"></param>
    /// <param name="text"></param>
    /// <returns>True if successful.</returns>
    public bool PostComment(string username, string password, int parentId, string text)
    {
      const string url = @"https://winchatty.com/v2/postComment";
      const string dataFormat = @"username={0}&password={1}&parentId={2}&text={3}";

      using (var client = new ChattyWebClient())
      {
        string data = string.Format(dataFormat, username, password, parentId, text);
        string result = client.UploadString(url, data);

        const string success = "{\"result\":\"success\"}";

        return result == success;
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

    /// <summary>
    /// Sends a shackmessage.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="targetUsername"></param>
    /// <param name="subject"></param>
    /// <param name="text"></param>
    /// <returns>True if successful.</returns>
    public bool SendMessage(string username, string password, string targetUsername, string subject, string text)
    {
      const string url = @"https://winchatty.com/v2/sendMessage";
      const string dataFormat = @"username={0}&password={1}&to={2}&subject={3}&body={4}";

      using (var client = new ChattyWebClient())
      {
        string data = string.Format(dataFormat, username, password, targetUsername, subject, text);
        string result = client.UploadString(url, data);

        const string success = "{\"result\":\"success\"}";

        return result == success;
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
    
    // Required to enable automatic GZip decompression
    // https://stackoverflow.com/questions/2973208/automatically-decompress-gzip-response-via-webclient-downloaddata/4914874#4914874
    private class ChattyWebClient : WebClient
    {
      protected override WebRequest GetWebRequest(Uri address)
      {
        var request = base.GetWebRequest(address) as HttpWebRequest;

        // The below seem to be required for the WinChatty calls to work.
        request.ContentType = "application/x-www-form-urlencoded";
        request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

        return request;
      }
    }
  }
}
