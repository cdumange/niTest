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


namespace niTest.Controllers
{

    
    public class MessageController : ApiController
    {
        private IMessageFactory fact = null;

        public MessageController()
        {
            fact = new MessageFactory();
        }
        [HttpGet]
        [Route("api/message/")]
        public HttpResponseMessage getAllMessage()
        {
            List<Message> list = fact.GetAllMessage();

            
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
    }
}