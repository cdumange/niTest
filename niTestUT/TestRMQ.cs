using System;
using System.Collections.Generic;
using System.Text;
using ws.rmq.server.Lib;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace niTestUT
{
    [TestFixture]
    class TestRMQ
    {
        string server = "192.168.1.111:5672", user = "guest", pwd = "guest";
        [Test]
        public void TestEnvoiSimple()
        {
            RMQManager manager = new RMQManager(server, user, pwd);
            manager.SetQueue("test").Connect();

            Assert.True(manager.SendMessage("test"));
        }
    }
}
