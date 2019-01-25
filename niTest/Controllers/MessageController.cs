using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using niTest.Models;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using niTest.Factories;
using Newtonsoft.Json.Linq;

namespace niTest.Controllers
{


    [Route("api/message")]
    public class MessageController : ApiController
    {
        private IMessageFactory fact = null;

        
        public MessageController()
        {
            fact = new MessageFactory();
        }
        [HttpGet]
        public HttpResponseMessage getAllMessage()
        {
            IEnumerable<Message> list = fact.GetAllMessage();

            
            var res = Request.CreateResponse();
            res.Content = new StringContent(JsonConvert.SerializeObject(list), Encoding.UTF8, "application/json");
            return res;
        }

        
        [HttpGet]
        [Route("api/message/{id:int}")]
        public HttpResponseMessage getMessageById(int id)
        {
            var res = Request.CreateResponse();
            res.Content = new StringContent(JsonConvert.SerializeObject(fact.GetById(id)), Encoding.UTF8, "application/json");
            return res;
        }

        [HttpPost]
        public IHttpActionResult updateMessage([FromBody] string body)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/message/search")]
        public IHttpActionResult SearchMessages([FromBody] JObject filter)
        {
            try
            {
                //JObject json = JObject.Parse(filter);
                var ret = fact.Search(filter);
                return Ok(JsonConvert.SerializeObject(ret));
            }
            catch (JsonReaderException)
            {
                return BadRequest("Filtre ko");
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}