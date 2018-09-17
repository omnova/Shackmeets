using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shackmeets.Services;

namespace Shackmeets.Tests
{
  [TestClass]
  public class NotificationTests
  {
    public NotificationTests()
    {

    }

    [TestMethod]
    public void TestEmail()
    {
      var helper = new NotificationHelper(null, null);

      helper.SendEmail("omnova@gmail.com", "testsubject", "budoty");

      Assert.IsTrue(true);
    }
    
    [TestMethod]
    public void TestEmail2()
    {
      var helper = new NotificationHelper(null, null);

      helper.SendEmail2();

      Assert.IsTrue(true);
    }
  }
}
