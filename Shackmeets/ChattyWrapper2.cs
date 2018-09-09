using System;
using System.Net;
using System.Net.Http;

namespace Shackmeets
{
  /// <summary>
  /// Provides access to Shacknews functionality via the WinChatty v2 API.
  /// </summary>
  public class ChattyWrapper2
  {
    // Remember: this is only to be used for the Shackmeets account

    public void VerifyConnectionAsync()
    {
      const string url = @"https://winchatty.com/v2/checkConnection";
    }

    public void VerifyCredentialsAsync(string username, string password)
    {
      const string url = @"https://winchatty.com/v2/verifyCredentials";
      const string dataFormat = @"username={0}&password={1}";
    }

    public void PostCommentAsync(string username, string password, int parentId, string text)
    {
      const string url = @"https://winchatty.com/v2/postComment";
      const string dataFormat = @"username={0}&password={1}&parentId={2}&text={3}";
    }

    public void SendMessageAsync(string username, string password, string targetUsername, string subject, string text)
    {
      const string url = @"https://winchatty.com/v2/sendMessage";
      const string dataFormat = @"username={0}&password={1}&to={2}&subject={3}&body={4}";
    }
  }
}
