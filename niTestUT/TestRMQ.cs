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
        RMQManager manager = null;
        string server = "192.168.1.111:5672", user = "guest", pwd = "guest";
        string queue = "test";

        [SetUp]
        public void init()
        {

            manager = new RMQManager(server, user, pwd);
            manager.SetQueue("test").Connect();
        }

        [Test]
        public void TestEnvoiSimple()
        {
            manager = new RMQManager(server, user, pwd);
            manager.SetQueue(queue).Connect();

            Assert.True(manager.SendMessage("test"));
        }
        [Test]
        public void TestListenSimple()
        {
            //TestContext.Out.WriteLine("test");
            manager.SetListener(queue, callBackTest);
        }

      

        public void callBackTest(string test)
        {
            RMQManager man = new RMQManager(server, user, pwd);
            
            man.SetQueue("retest").Connect().SendMessage("supertest");
            man.Close();
            Assert.NotNull(test);
        }

        [TearDown]
        public void Closure()
        {
            if(manager.IsOpened)
                manager.Close();
        }
    }
}
