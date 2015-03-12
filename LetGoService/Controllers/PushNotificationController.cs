using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LetGoService.Handlers;

namespace LetGoService.Controllers
{
    public class PushNotificationController : ApiController
    {
        RequestManager rm = new RequestManager();
        
        public void PushNotification()
        {
            string uri = "https://notifymehack.azure-mobile.net/tables/ToDoItem";
            rm.SendPushRequest(uri);
        }
    }
}
