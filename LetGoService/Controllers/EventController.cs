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
        public IEnumerable<Event> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/event/5
        public Event Get(string title)
        {
            List<Event> eventList = DbHelper.SearchEventByTitle(title);
            if (eventList.Count != 0) return null;
            string eventId = eventList[0].Id;
            return DbManager.Instance.FindById<Event>(LetGoData.Event.CollectionName, eventId);
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
