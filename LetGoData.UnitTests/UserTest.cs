using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LetGoData.UnitTests
{
	[TestClass]
	public class UserTest
	{
		private static List<string> s_userIdList = new List<string>();

		[ClassInitialize]
		public static void ClassInit(TestContext context)
		{
			{
				User user = new User();
				user.Id = Guid.NewGuid().ToString();
				user.Name = "test 001";
				user.Phone = "910-987-0988";

				DbManager.Instance.Insert(user);

				s_userIdList.Add(user.Id);
			}

			{
				User user = new User();
				user.Id = Guid.NewGuid().ToString();
				user.Name = "test 002";
				user.Phone = "910-283-0988";

				DbManager.Instance.Insert(user);

				s_userIdList.Add(user.Id);
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			foreach (string id in s_userIdList)
			{
				DbManager.Instance.Delete(User.CollectionName, id);
			}
		}

		[TestMethod]
		public void TestCreateAndDeleteUser()
		{
			string userId = Guid.NewGuid().ToString();

			{
				User user = new User();
				user.Id = userId;
				user.Name = "test 100";
				user.Phone = "425-974-1234";

				DbManager.Instance.Insert(user);
			}

			{
				User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, userId);
				Assert.IsTrue(user != null && user.Name.Equals("test 100"));

				DbManager.Instance.Delete(user);
			}

			{
				User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, userId);
				Assert.IsTrue(user == null);
			}
		}

		[TestMethod]
		public void TestUpdateUser()
		{
			string userId = s_userIdList[0];

			{
				User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, userId);
				user.Name = "name modified";
				DbManager.Instance.Update(user);
			}

			{
				User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, userId);
				Assert.IsTrue(user != null && user.Name.Equals("name modified"));

				// revert changes
				user.Name = "test 001";
				DbManager.Instance.Update(user);
			}
		}
	}
}
