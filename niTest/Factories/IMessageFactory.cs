using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComModel;
using Newtonsoft.Json.Linq;

namespace niTest.Factories
{
    interface IMessageFactory
    {
        bool Add(Message message);
        void Update(Message message);
        Message GetById(int id);
        IEnumerable<Message> GetAllMessage();
        IEnumerable<Message> Search(JObject filter);
    }
}
