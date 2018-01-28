using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artos.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Twilio.AspNet.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Artos.Services.Transaction.Controllers
{
    public class SmsController : TwilioController
    {
        private readonly IConfiguration Configuration;
        private ConnectionFactory factory { set; get; }
        private string QueTopic { set; get; }
        public SmsController(IConfiguration config)
        {
            Configuration = config;
            var rabbitcon = Configuration.GetValue<string>("server:rabbithost");
            QueTopic = Configuration.GetValue<string>("server:quetopic");
            factory = new ConnectionFactory() { HostName = rabbitcon };

        }
        // POST: Sms/Message
        [HttpPost]
        public ActionResult ReceiveMessage(string From, string Body)
        {
            var twiml = new Twilio.TwiML.MessagingResponse();
            var message = twiml.Message($"Hello {From}. You said {Body}");
            //send to que (rabbit)
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QueTopic,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var sms = new SMSMessage(){ From=From, Message=Body};
                string msg = JsonConvert.SerializeObject(sms);
                var body = Encoding.UTF8.GetBytes(msg);

                channel.BasicPublish(exchange: "",
                                     routingKey: QueTopic,
                                     basicProperties: null,
                                     body: body);
                //Console.WriteLine(" [x] Sent {0}", message);
            }
            return Ok(TwiML(message));


        }
    }
}
