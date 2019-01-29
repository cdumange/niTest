using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using System.Text;

namespace ws.rmq.server.Lib
{
    public class RMQManager
    {
        public string server { get; set; }
        public string user { get; set; }
        public string pwd { get; set; }

        public string queue { get; private set; }
        public string routing { get; private set; } = "routing";

        public string exchangeName { get; set; } = "Envoi vers RMQ";

        private ConnectionFactory fact = null;
        private IConnection conn = null;
        private string connString = "amqp://{user}:{pwd}@{server}";

        public RMQManager(string server, string user, string pwd)
        {
            this.server = server;
            this.user = user;
            this.pwd = pwd;
        }

        public RMQManager Connect(bool forceRenew = false)
        {
            if (conn == null || forceRenew)
            {
                //Fermeture de la connection si déjà ouverte
                if (conn != null)
                {
                    try
                    {
                        conn.Close();
                    }
                    catch (Exception)
                    {
                        //nothing to do;
                    }
                    
                }
                //Création de la factory
                fact = new ConnectionFactory();
                fact.Uri = new Uri(connString
                    .Replace("{server}", this.server)
                    .Replace("{user}", this.user)
                    .Replace("{pwd}", this.pwd));

                //Récupération de la connection
                conn = fact.CreateConnection();
            }
            return this;
        }

        public RMQManager SetQueue(string queue)
        {
            this.queue = queue;
            return this;
        }

        public bool SendMessage(string message)
        {
            try
            {
                IModel model = this.conn.CreateModel();
                model.ExchangeDeclare(this.exchangeName, ExchangeType.Direct);
                model.QueueDeclare(this.queue, false, false, false);
                model.QueueBind(this.queue, this.exchangeName, this.routing);
                model.BasicPublish(this.exchangeName, this.routing, null, Encoding.UTF8.GetBytes(message));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }
    }
}