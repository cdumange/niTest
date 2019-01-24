using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using niTest.Models;
using Newtonsoft.Json.Linq;

namespace niTest.Factories
{
    interface IMessageFactory
    {
        void Add(Message message);
        void Update(Message message);
        Message GetById(int id);
        List<Message> GetAllMessage();
        List<Message> Search(JObject filter);
    }
}
