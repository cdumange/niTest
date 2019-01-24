using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

using NHibernate;
using NHibernate.Cfg;
using niTest.Models;

namespace niTest
{
    public sealed class NHibernateHelper
    {
        private static ISessionFactory SessionFactory;

        private static void OpenSession()
        {
            Configuration configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Message).Assembly);
            
            
            SessionFactory = configuration.BuildSessionFactory();
        }

        public static ISession GetCurrentSession()
        {
            if (SessionFactory == null)
                NHibernateHelper.OpenSession();

            return SessionFactory.OpenSession();
        }

        public static void CloseSessionFactory()
        {
            if (SessionFactory != null)
                SessionFactory.Close();
        }
    }
}