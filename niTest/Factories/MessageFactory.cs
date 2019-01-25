using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using niTest.Models;
using niTest.Lib;

namespace niTest.Factories
{
    public class MessageFactory : IMessageFactory
    {
        bool IMessageFactory.Add(Message message)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                return ((int)session.Save(message)) > 0;
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
        /// <summary>
        /// Methode receiving a json filter and transforming it to a NHibernate filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<Message> IMessageFactory.Search(JObject filter)
        {
            return FilterBuilder<Message>.Search(filter);
        }

        IEnumerable<Message> IMessageFactory.GetAllMessage()
        {
            IEnumerable<Message> list = null;
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                list = session.Query<Message>();
            }

            return list;
        }

        void IMessageFactory.Update(Message message)
        {
            using (var session = NHibernateHelper.GetCurrentSession())
            {
                session.Update(message);
            }
        }
    }
}