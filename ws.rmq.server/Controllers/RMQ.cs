using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using ComModel;

namespace ws.rmq.server.Controllers
{
    public class RMQ : ApiController
    {
        [HttpPost]
        public IHttpActionResult SaveMessage(JObject message)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            
        }
    }
}