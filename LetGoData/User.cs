using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LetGoData
{
	[Serializable]
	public class User : BaseMongoObject
	{
		public const string CollectionName = "users";

		public string Name { get; set; }

		public string Phone { get; set; }

		public override string GetCollectionName()
		{
			return User.CollectionName;
		}
	}
}
