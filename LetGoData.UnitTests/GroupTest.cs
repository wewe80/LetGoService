using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LetGoData.UnitTests
{
	[TestClass]
	public class GroupTest
	{
		private static List<string> s_userIdList = new List<string>();
		private static List<string> s_groupIdList = new List<string>();

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

			{
				Group group = new Group();
				group.Id = Guid.NewGuid().ToString();
				group.Name = "Table Tennis";
				group.Users = new List<string> { s_userIdList[0] };

				DbManager.Instance.Insert(group);

				s_groupIdList.Add(group.Id);
			}

			{
				Group group = new Group();
				group.Id = Guid.NewGuid().ToString();
				group.Name = "Football";
				group.Users = new List<string> { s_userIdList[1] };

				DbManager.Instance.Insert(group);

				s_groupIdList.Add(group.Id);
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			foreach (string id in s_groupIdList)
			{
				DbManager.Instance.Delete(Group.CollectionName, id);
			}

			foreach (string id in s_userIdList)
			{
				DbManager.Instance.Delete(User.CollectionName, id);
			}
		}

		[TestMethod]
		public void TestCreateAndDeleteGroup()
		{
			string id = Guid.NewGuid().ToString();

			{
				Group group = new Group();
				group.Id = id;
				group.Name = "Temp";
				group.Users = s_userIdList;

				DbManager.Instance.Insert(group);
			}

			{
				Group group = DbManager.Instance.FindById<Group>(LetGoData.Group.CollectionName, id);
				Assert.IsTrue(group != null && group.Users.Count == 2 && group.Name == "Temp");

				//DbManager.Instance.Delete(group);
			}
		}

		[TestMethod]
		public void TestJoinGroup()
		{
			string groupId = s_groupIdList[0];
			string userId = s_userIdList[1];

			{
				Group group = DbManager.Instance.FindById<Group>(LetGoData.Group.CollectionName, groupId);
				Assert.IsTrue(group != null && !group.Users.Contains(userId));

				group.Users.Add(userId);
				DbManager.Instance.Update(group);

			}

			{
				Group group = DbManager.Instance.FindById<Group>(LetGoData.Group.CollectionName, groupId);
				Assert.IsTrue(group != null && group.Users.Contains(userId));

				// revert changes
				group.Users = new List<string> { s_userIdList[0] };
				DbManager.Instance.Update(group);
			}
		}
	}
}
