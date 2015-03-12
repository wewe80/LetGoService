using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoData.UnitTests
{
	[TestClass]
	public class DbHelperTest
	{
		private static List<string> s_userIdList = new List<string>();
		private static List<string> s_groupIdList = new List<string>();
		private static List<string> s_eventIdList = new List<string>();

		[ClassInitialize]
		public static void ClassInit(TestContext context)
		{
			{
				User user = new User();
				user.Id = Guid.NewGuid().ToString();
				user.Name = Global.TEST_PREFIX + "test 001";
				user.Phone = Global.TEST_PREFIX + "910-987-0988";

				DbManager.Instance.Insert(user);

				s_userIdList.Add(user.Id);
			}

			{
				User user = new User();
				user.Id = Guid.NewGuid().ToString();
				user.Name = Global.TEST_PREFIX + "test 002";
				user.Phone = Global.TEST_PREFIX + "910-283-0988";

				DbManager.Instance.Insert(user);

				s_userIdList.Add(user.Id);
			}

			{
				Group group = new Group();
				group.Id = Guid.NewGuid().ToString();
				group.Name = Global.TEST_PREFIX + "Table Tennis";
				group.Users = new List<string> { s_userIdList[0] };

				DbManager.Instance.Insert(group);

				s_groupIdList.Add(group.Id);
			}

			{
				Event eve = new Event();
				eve.Id = Guid.NewGuid().ToString();
				eve.GroupId = s_groupIdList[0];
				eve.Title = "Play table tennis in the first floor at 4:00 PM.";
				eve.Description = "Please come to join us!";

				DbManager.Instance.Insert(eve);

				s_eventIdList.Add(eve.Id);
			}
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			foreach (string id in s_eventIdList)
			{
				DbManager.Instance.Delete(Event.CollectionName, id);
			}

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
		public void TestQuickJoinEvent()
		{
			string eventId = s_eventIdList[0];
			string userId = s_userIdList[1];

			DbHelper.JoinEvent(userId, eventId);

			{
				Event eve = DbManager.Instance.FindById<Event>(LetGoData.Event.CollectionName, eventId);
				Assert.IsTrue(eve != null && eve.Users.Contains(userId));

				// revert changes
				eve.Users = null;
				DbManager.Instance.Update(eve);
			}
		}

		[TestMethod]
		public void TestQuickJoinGroup()
		{
			string groupId = s_groupIdList[0];
			string userId = s_userIdList[1];

			DbHelper.JoinGroup(userId, groupId);

			{
				Group group = DbManager.Instance.FindById<Group>(LetGoData.Group.CollectionName, groupId);
				Assert.IsTrue(group != null && group.Users.Contains(userId));

				// revert changes
				group.Users = new List<string> { s_userIdList[0] };
				DbManager.Instance.Update(group);
			}
		}

		[TestMethod]
		public void TestQuickLeaveGroup()
		{
			string groupId = s_groupIdList[0];
			string userId = s_userIdList[0];

			{
				Group group = DbManager.Instance.FindById<Group>(LetGoData.Group.CollectionName, groupId);
				Assert.IsTrue(group != null && group.Users.Count == 1);
			}

			DbHelper.LeaveGroup(userId, groupId);

			{
				Group group = DbManager.Instance.FindById<Group>(LetGoData.Group.CollectionName, groupId);
				Assert.IsTrue(group != null && group.Users.Count == 0);
			}

			{
				// restsore data
				DbHelper.JoinGroup(userId, groupId);
			}
		}

		[TestMethod]
		public void TestSearchGroupByName()
		{
			List<Group> list = DbHelper.SearchGroupByName(Global.TEST_PREFIX + "Table Tennis");
			Assert.IsTrue(list.Count == 1);
		}

		[TestMethod]
		public void TestSearchGroupByPartialName()
		{
			List<Group> list = DbHelper.SearchGroupByName(Global.TEST_PREFIX);
			Assert.IsTrue(list.Count == 1);
		}

		[TestMethod]
		public void TestSearchUserByName()
		{
			List<User> list = DbHelper.SearchUserByName(Global.TEST_PREFIX + "test 002");
			Assert.IsTrue(list.Count == 1 && list[0].Phone.Equals(Global.TEST_PREFIX + "910-283-0988"));
		}

		[TestMethod]
		public void TestSearchUserByPartialName()
		{
			List<User> list = DbHelper.SearchUserByName(Global.TEST_PREFIX);
			Assert.IsTrue(list.Count == 2);
		}

		[TestMethod]
		public void TestSearchUserByPhone()
		{
			List<User> list = DbHelper.SearchUserByPhone(Global.TEST_PREFIX + "910-987-0988");
			Assert.IsTrue(list.Count == 1 && list[0].Name.Equals(Global.TEST_PREFIX + "test 001"));
		}
	}
}
