using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LetGoData;

namespace LetGoService.Controllers
{
    public class EventController : ApiController
    {
        // GET api/event
        //public IEnumerable<Event> Get()
        //{
        //    return null;
        //}

        // GET api/event/5
        public Event Get(string id)
        {
            return DbManager.Instance.FindById<Event>(LetGoData.Event.CollectionName, id);
        }

        // POST api/event
        public void Post([FromBody]Event value)
        {
            DbManager.Instance.Insert(value);
        }

        // PUT api/event/5
        public void Put([FromBody]Event value)
        {
            DbManager.Instance.Update(value);
        }

        // DELETE api/event/5
        public void Delete(string id)
        {
            DbManager.Instance.Delete(LetGoData.Event.CollectionName, id);
        }
    }
}
