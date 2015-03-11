using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LetGoData.UnitTests
{
	[TestClass]
	public class EventTest
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
		public void TestCreateAndDeleteEvent()
		{
			string id = Guid.NewGuid().ToString();

			{
				Event eve = new Event();
				eve.Id = id;
				eve.GroupId = s_groupIdList[0];
				eve.Title = "Temp Play table tennis in the first floor at 4:00 PM.";
				eve.Description = "Please come to join us!";

				DbManager.Instance.Insert(eve);
			}

			{
				Event eve = DbManager.Instance.FindById<Event>(LetGoData.Event.CollectionName, id);
				Assert.IsTrue(eve != null && eve.Title.Equals("Temp Play table tennis in the first floor at 4:00 PM."));

				DbManager.Instance.Delete(eve);
			}

			{
				Event eve = DbManager.Instance.FindById<Event>(LetGoData.Event.CollectionName, id);
				Assert.IsTrue(eve == null);
			}
		}

		[TestMethod]
		public void TestJoinEvent()
		{
			string eventId = s_eventIdList[0];
			string userId = s_userIdList[1];

			{
				Event eve = DbManager.Instance.FindById<Event>(LetGoData.Event.CollectionName, eventId);
				Assert.IsTrue(eve != null && eve.Users == null);

				eve.Users = new List<string> { userId };
				DbManager.Instance.Update(eve);

			}

			{
				Event eve = DbManager.Instance.FindById<Event>(LetGoData.Event.CollectionName, eventId);
				Assert.IsTrue(eve != null && eve.Users.Contains(userId));

				// revert changes
				eve.Users = null;
				DbManager.Instance.Update(eve);
			}
		}
	}
}
