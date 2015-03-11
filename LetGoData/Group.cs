using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoData
{
	public class Group : BaseMongoObject
	{
		public const string CollectionName = "groups";

		public string Name { get; set; }

		public List<string> Users { get; set; }

		public override string GetCollectionName()
		{
			return Group.CollectionName;
		}
	}
}
