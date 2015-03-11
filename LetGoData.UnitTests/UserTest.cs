using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LetGoData.UnitTests
{
	[TestClass]
	public class UserTest
	{
		[TestMethod]
		public void TestGetUserById()
		{
			User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, "54ff8e5bf6a5ec1b78ac1e73");
			Assert.IsTrue(user != null && user.Name.Equals("Wei Wei"));
		}
	}
}
