using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoData
{
	[Serializable]
	public abstract class BaseMongoObject
	{
		public ObjectId Id { get; set; }

		public abstract string GetCollectionName();
	}
}
