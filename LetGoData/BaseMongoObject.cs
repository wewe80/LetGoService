using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoData
{
	public abstract class BaseMongoObject
	{
		public string Id { get; set; }

		public abstract string GetCollectionName();
	}
}
