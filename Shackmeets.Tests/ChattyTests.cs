using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shackmeets.Tests
{
  [TestClass]
  public class ChattyTests
  {
    private readonly string username = "";
    private readonly string password = "";

    private ChattyWrapper chatty = new ChattyWrapper();

    [TestMethod]
    public void TestConnection()
    {
      bool result = chatty.VerifyConnection();
    }

    [TestMethod]
    public void TestAuthenticationSuccess()
    {
      bool result = chatty.VerifyCredentials(username, password);

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestAuthenticationFailure()
    {
      // Fake a bad password
      bool result = chatty.VerifyCredentials(username, password + "da;lsjd;asf");

      Assert.IsFalse(result);
    }

    //[TestMethod]
    //public void TestAuthenticationAsyncSuccess()
    //{
    //  TestAuthenticationAsyncSuccessInner();
    //}

    //public async void TestAuthenticationAsyncSuccessInner()
    //{
    //  var result = chatty.VerifyCredentialsAsync(username, password);

    //  var response = await result;

    //  Assert.IsTrue(response);
    //}

    //[TestMethod]
    //public void TestAuthenticationAsyncFailure()
    //{
    //  // Fake a bad password
    //  bool result = chatty.VerifyCredentials(username, password + "da;lsjd;asf");

    //  Assert.IsFalse(result);
    //}

    [TestMethod]
    public void TestPost()
    {
      // Parent to
      // https://www.shacknews.com/chatty?id=34529311#item_34529311
      bool result = chatty.PostComment(username, password, 34529311, "TEST");

      Assert.IsTrue(result);
    }

    [TestMethod]
    public void TestMessage()
    {
      bool result = chatty.SendMessage(username, password, username, "subject", "body");

      Assert.IsTrue(result);
    }
  }
}
