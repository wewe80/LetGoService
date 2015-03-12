using LetGoData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LetGoService.Controllers
{
    public class UserController : ApiController
    {
        // GET api/user
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/user/5
        public User Get(string id)
        {
			return DbManager.Instance.FindById<User>(LetGoData.User.CollectionName, id);
        }

        // POST api/user
        public void Post([FromBody]User value)
        {
            DbManager.Instance.Insert(value);
        }

        // PUT api/user/5
        public void Put(int id, [FromBody]User value)
        {
            DbManager.Instance.Update(value);
        }

        // DELETE api/user/5
        public void Delete(string id)
        {
            DbManager.Instance.Delete(LetGoData.User.CollectionName, id);
        }
    }
}
