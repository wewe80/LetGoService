using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetGoData
{
	public class DbManager
	{
		private static DbManager s_instance = new DbManager();

		private MongoDatabase Database { get; set; }

		public static DbManager Instance { get { return s_instance; } }

		private DbManager()
		{
			MongoClient client = new MongoClient("mongodb://yoadmin:yo123456@ds033087.mongolab.com:33087/yodb");
			MongoServer server = client.GetServer();
			this.Database = server.GetDatabase("yodb");
		}

		public void Insert(BaseMongoObject obj)
		{
			MongoCollection collection = this.Database.GetCollection(obj.GetCollectionName());
			collection.Insert(obj.GetType(), obj);
		}

		public void Update(BaseMongoObject obj)
		{
			MongoCollection collection = this.Database.GetCollection(obj.GetCollectionName());
			collection.Save(obj.GetType(), obj);
		}

		public void Delete(BaseMongoObject obj)
		{
			var query = Query<BaseMongoObject>.EQ(e => e.Id, obj.Id);

			MongoCollection collection = this.Database.GetCollection(obj.GetCollectionName());
			collection.Remove(query);
		}

		public List<T> Find<T>(string collectionName, IMongoQuery query)
		{
			List<T> list = new List<T>();

			MongoCollection<T> collection = this.Database.GetCollection<T>(collectionName);
			MongoCursor<T> cursor = collection.Find(query);
			foreach (T item in cursor)
			{
				list.Add(item);
			}

			return list;
		}

		public T FindById<T>(string collectionName, string id) where T : BaseMongoObject
		{
			var query = Query<T>.EQ(e => e.Id, new ObjectId(id));
			MongoCollection<T> collection = this.Database.GetCollection<T>(collectionName);
			return collection.FindOne(query);
		}
	}
}
