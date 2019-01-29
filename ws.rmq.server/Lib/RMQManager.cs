using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ws.rmq.server.Lib
{
    /// <summary>
    /// Class managing all processes linked to the usage of RMQ
    /// </summary>
    public class RMQManager
    {
        public string server { get; set; }
        public string user { get; set; }
        public string pwd { get; set; }

        public string queue { get; private set; }
        public string routing { get; private set; } = "routing";

        public string exchangeName { get; set; } = "Envoi vers RMQ";

        public bool IsOpened { get
            {
                return conn != null && conn.IsOpen;
            }
        }

        public List<EventingBasicConsumer> consumers { get; } = new List<EventingBasicConsumer>();

        private ConnectionFactory fact = null;
        private IConnection conn = null;
        private string connString = "amqp://{user}:{pwd}@{server}";

        public RMQManager(string server, string user, string pwd)
        {
            this.server = server;
            this.user = user;
            this.pwd = pwd;
        }
        /// <summary>
        /// Manage the connection to the RMQ server
        /// </summary>
        /// <param name="forceRenew">Force the renewal of the connection</param>
        /// <returns></returns>
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

        public RMQManager SetRouting(string routing)
        {
            this.routing = routing;
            return this;
        }

        /// <summary>
        /// Verifying if connection is instanced & opened
        /// </summary>
        /// <returns></returns>
        public bool CheckConnection()
        {
            if (conn == null || !conn.IsOpen)
            {
                throw new Exception("RMQ connection not opened");
            }
            return true;
        }
        /// <summary>
        /// Sending a message to the configured queue
        /// </summary>
        /// <param name="message">the string message</param>
        /// <param name="queue">optional parameter. Allows to send a message to another queue then default</param>
        /// <returns>was the message sent</returns>
        public bool SendMessage(string message, string queue= null)
        {
            queue = queue != null ? queue : this.queue;
            CheckConnection();
            try
            {
                IModel model = this.conn.CreateModel();
                model.ExchangeDeclare(this.exchangeName, ExchangeType.Direct);
                model.QueueDeclare(queue, false, false, false);
                model.QueueBind(queue, this.exchangeName, this.routing);
                model.BasicPublish(this.exchangeName, this.routing, null, Encoding.UTF8.GetBytes(message));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
           
        }

        public RMQManager SetListener(string queue, Action<string> callback)
        {
            CheckConnection();
            IConnection conn2 = fact.CreateConnection();
            using (IModel model = conn2.CreateModel())
            {
                model.ExchangeDeclare(this.exchangeName, ExchangeType.Direct);
                model.QueueDeclare(queue, false, false, false);
                model.QueueBind(queue, this.exchangeName, this.routing);
                EventingBasicConsumer consumer = new EventingBasicConsumer(model);
                //Defining the callback
                consumer.Received += (mod, data) =>
                {
                    
                    Console.Out.WriteLine("test");
                    string body = Encoding.UTF8.GetString(data.Body);
                    callback(body);

                };
                //Launching the listenning
                model.BasicConsume(queue, true, consumer);

                consumers.Add(consumer);
            }
            
            return this;
        }

        public RMQManager Close()
        {
            //Fermeture de tous les consumers
            if (consumers != null && consumers.Count > 0)
            {
                try
                {
                    consumers.ForEach(consumer => {
                        consumer.Model.BasicCancel(consumer.ConsumerTag);
                    });
                }
                catch (Exception)
                {
                    //nothing to do
                }
                
            }
            //fermeture de la connection
            if (conn != null)
            {
                try
                {
                    conn.Close();
                }
                catch (Exception)
                {
                    //nothing to do
                }
                
            }

            return this;
        }

    }
}