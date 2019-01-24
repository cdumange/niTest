using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using niTest.Models;

namespace niTest.Factories
{
    public class MessageFactory : IMessageFactory
    {
        void IMessageFactory.Add(Message message)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                session.Save(message);
            }
        }

        Message IMessageFactory.GetById(int id)
        {
            try
            {
                using (var session = NHibernateHelper.GetCurrentSession())
                {
                    return session.Get<Message>(id);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        List<Message> IMessageFactory.Search(JObject filter)
        {
            throw new NotImplementedException();
        }

        List<Message> IMessageFactory.GetAllMessage()
        {
            List<Message> list = null;
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                list = session.Query<Message>().ToList();
            }

            return list;
        }

        void IMessageFactory.Update(Message message)
        {
            throw new NotImplementedException();
        }
    }
}