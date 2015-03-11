using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LetGoData.UnitTests
{
	[TestClass]
	public class UserTest
	{
		[TestMethod]
		public void TestCreateUser()
		{
			string userId = Guid.NewGuid().ToString();

			{
				User user = new User();
				user.Id = userId;
				user.Name = "Wei Wei";
				user.Phone = "425-974-0443";

				DbManager.Instance.Insert(user);
			}

			{
				User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, userId);
				Assert.IsTrue(user != null && user.Name.Equals("Wei Wei"));

				DbManager.Instance.Delete(user);
			}
		}
	}
}
