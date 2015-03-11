using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoData
{
	public class Event : BaseMongoObject
	{
		public const string CollectionName = "events";

		public string Title { get; set; }

		public string Description { get; set; }

		public ObjectId GroupId { get; set; }

		public List<string> Users { get; set; }

		public override string GetCollectionName()
		{
			return Event.CollectionName;
		}
	}
}
