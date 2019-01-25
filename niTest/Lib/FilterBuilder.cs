using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Reflection;
using NHibernate;
using niTest.Models;
using NHibernate.Criterion;

namespace niTest.Lib
{
    public abstract class FilterBuilder<TEntity>
        where TEntity : class
    {
        public static IEnumerable<TEntity> Search(JObject filter) 
        {
            Type T = typeof(TEntity);
            var listProp = T.GetProperties();
            var listName = (from prop in listProp
                                 select prop.Name);

            List<String> p = new List<String>();
            //Checking if some properties of the filter aren't in the type
            foreach (var jsonVal in filter)
            {
                if (!listName.Contains(jsonVal.Key))
                {
                    p.Add(jsonVal.Key);
                }
            }
            
            //if there is any, throwing error
            if (p.Count > 0)
            {
                String retour = "Certaines paramètres du filtre d'existe pas dans l'object : ";
                p.ForEach(field => retour += Environment.NewLine + "- " + field);
                throw new ArgumentException(retour);
            }

            using (var session = NHibernateHelper.GetCurrentSession())
            {
                var crit = session.CreateCriteria<TEntity>();

                foreach(var value in filter)
                {
                    //récupération du PropertyInfo de l'object
                    PropertyInfo t = listProp.Where(x => x.Name.Equals(value.Key)).First();
                    
                    object o;
                    try
                    {
                        o = Activator.CreateInstance(t.PropertyType);
                    }
                    catch (MissingMethodException)
                    {
                        //most likely string. haven't found better
                        o = String.Empty;
                    }
                    //hate this
                    switch (o)
                    {
                        case int i:
                            crit.Add(Expression.Eq(value.Key, int.Parse(value.Value.ToString())));
                            break;
                        case long l:
                            crit.Add(Expression.Eq(value.Key, long.Parse(value.Value.ToString())));
                            break;
                        case float f:
                            crit.Add(Expression.Eq(value.Key, float.Parse(value.Value.ToString())));
                            break;
                        case double d:
                            crit.Add(Expression.Eq(value.Key, double.Parse(value.Value.ToString())));
                            break;
                        case bool b:
                            crit.Add(Expression.Eq(value.Key, bool.Parse(value.Value.ToString())));
                            break;
                        case DateTime v:
                            string tmp = value.Value.ToString();
                            //Trying to parse as DT
                            DateTime dt = DateTime.Parse(tmp);
                            crit.Add(Expression.Eq(value.Key,dt));
                            break;
                        default:
                            crit.Add(Expression.Like(value.Key, value.Value.ToString()));
                            break;
                    }
                     
                }
                var ret = crit.List<TEntity>();
                return ret;

            }
        }
    }
} 