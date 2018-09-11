using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shackmeets.Tests
{
  [TestClass]
  public class ChattyTests
  {
    private string username = "";
    private string password = "";

    [TestMethod]
    public void TestConnection()
    {
      var chatty = new ChattyWrapper();

      bool result = chatty.VerifyConnection();
    }

    [TestMethod]
    public void TestAuthenticationSuccess()
    {
      var chatty = new ChattyWrapper();

      bool result = chatty.VerifyCredentials(username, password);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestAuthenticationFailure()
    {
      var chatty = new ChattyWrapper();

      // Fake a bad password
      bool result = chatty.VerifyCredentials(username, password + "da;lsjd;asf");

      Assert.IsFalse(result);
    }

    [TestMethod]
    public void TestPost()
    {
      var chatty = new ChattyWrapper();

      // Parent to
      // https://www.shacknews.com/chatty?id=34529311#item_34529311
      bool result = chatty.PostComment(username, password, 34529311, "TEST");

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestMessage()
    {
      var chatty = new ChattyWrapper();

      bool result = chatty.SendMessage(username, password, username, "subject", "body");

      Assert.IsTrue(result);
    }
  }
}
