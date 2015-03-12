using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LetGoData
{
	public class DbHelper
	{
        public static List<Event> SearchEventByTitle(string title)
        {
            var query = Query<Event>.EQ(e => e.Title, title);
            return DbManager.Instance.Find<Event>(Event.CollectionName, query);
        }
        
        public static List<Group> SearchGroupByName(string name)
		{
			var query = Query<Group>.Matches(e => e.Name, string.Format("/^{0}/", name));
			return DbManager.Instance.Find<Group>(Group.CollectionName, query);
		}

		public static List<User> SearchUserByName(string name)
		{
			var query = Query<User>.Matches(e => e.Name, string.Format("/^{0}/", name));
			return DbManager.Instance.Find<User>(User.CollectionName, query);
		}

		public static List<User> SearchUserByPhone(string phone)
		{
			var query = Query<User>.EQ(e => e.Phone, phone);
			return DbManager.Instance.Find<User>(User.CollectionName, query);
		}

		public static void JoinGroup(string userId, string groupId)
		{
			Group group = DbManager.Instance.FindById<Group>(LetGoData.Group.CollectionName, groupId);
			if (group == null)
			{
				throw new Exception("group cannot be found.");
			}

			User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, userId);
			if (user == null)
			{
				throw new Exception("user cannot be found.");
			}

			if (group.Users != null && group.Users.Contains(userId))
			{
				throw new Exception("user has joined the group already.");
			}

			if (group.Users == null)
			{
				group.Users = new List<string> { userId };
			}
			else
			{
				group.Users.Add(userId);
			}

			// update the group
			DbManager.Instance.Update(group);
		}

		public static void LeaveGroup(string userId, string groupId)
		{
			Group group = DbManager.Instance.FindById<Group>(LetGoData.Group.CollectionName, groupId);
			if (group == null)
			{
				throw new Exception("group cannot be found.");
			}

			User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, userId);
			if (user == null)
			{
				throw new Exception("user cannot be found.");
			}

			if (group.Users == null || !group.Users.Contains(userId))
			{
				throw new Exception("user is not in the group.");
			}

			group.Users.Remove(userId);

			// update the group
			DbManager.Instance.Update(group);
		}

		public static void JoinEvent(string userId, string eventId)
		{
			Event eve = DbManager.Instance.FindById<Event>(LetGoData.Event.CollectionName, eventId);
			if (eve == null)
			{
				throw new Exception("event cannot be found.");
			}

			User user = DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, userId);
			if (user == null)
			{
				throw new Exception("user cannot be found.");
			}

			if (eve.Users != null && eve.Users.Contains(userId))
			{
				throw new Exception("user has joined the event already.");
			}

			if (eve.Users == null)
			{
				eve.Users = new List<string> { userId };
			}
			else
			{
				eve.Users.Add(userId);
			}
			
			// update the event
			DbManager.Instance.Update(eve);
		}
	}
}
