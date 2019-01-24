using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json.Linq;

namespace niTest.Models
{
    public class Message
    {

        public int id { get; set; }
        public DateTime dateCreation { get; set; }
        public DateTime dateModif { get; set; }

        public JObject message { get; set; }

        private string messageStr {
            get
            {
                return this.message.ToString();
            }
            set
            {
                this.setMessage(value);
            }
        }

        public string origine { get; set; }
        public string typeMessage { get; set; }


        public Message setMessage(string message)
        {
            try
            {
                if(message.IndexOf(":") < 0)
                {
                    //nothing like a JSON, simple text
                    JObject value = new JObject();
                    value.Add("message", message);
                    this.message = value;
                }else
                {
                    JObject value = JObject.Parse(message);
                    this.message = value;
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            return this;
        }
        
    }
}