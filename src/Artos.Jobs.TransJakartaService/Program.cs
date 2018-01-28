using System;
using System.IO;
using System.Net;
using System.Text;
using Artos.Entities;
using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Artos.Jobs.TransJakartaService
{
    class Program
    {
        public static IConfigurationRoot Configuration;
        public static MqttClient mqttClient
        {
            get;
            set;
        }
        public static string mqtt_topic
        {
            get;
            set;
        }
        public static string rediscon
        {
            get;
            set;
        }

        public static IRedisClient redisClient
        {
            get;
            set;
        }

        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (String.IsNullOrWhiteSpace(environment))
                throw new ArgumentNullException("Environment not found in ASPNETCORE_ENVIRONMENT");

            Console.WriteLine("Environment: {0}", environment);


            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: true);
            if (environment == "Development")
            {

                builder
                    .AddJsonFile(
                        Path.Combine(AppContext.BaseDirectory, string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar), $"appsettings.{environment}.json"),
                        optional: true
                    );
            }
            else
            {
                builder
                    .AddJsonFile($"appsettings.{environment}.json", optional: true);
            }
            //get config
            Configuration = builder.Build();

            rediscon = Configuration.GetConnectionString("RedisCon");
            var mqtt_host = Configuration.GetSection("server").GetSection("mqtt-host").Value;
            var mqtt_user = Configuration.GetSection("server").GetSection("mqtt-user").Value;
            var mqtt_pass = Configuration.GetSection("server").GetSection("mqtt-pass").Value;
             mqtt_topic = Configuration.GetSection("server").GetSection("mqtt-topic").Value;

            //init redis
            var redisManager = new PooledRedisClientManager(1, rediscon);
            redisClient = redisManager.GetClient();

            //init mqtt
            var client_name = Configuration.GetSection("server").GetSection("terminal-number").Value;
            // create client instance
            mqttClient = new MqttClient(mqtt_host);

            // register to message received
            mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            //string clientId = Guid.NewGuid().ToString();
            mqttClient.Connect(client_name);

            // subscribe to the topic "/home/temperature" with QoS 2
            mqttClient.Subscribe(new string[] { mqtt_topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
     
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            if(e.Topic == mqtt_topic){
                //save to redis
                var newTicket = redisClient.As<TicketPool>();
                redisClient.Store(newTicket);
            }
        }

        static void SendMessage(string message)
        {
            // publish a message on "/home/temperature" topic with QoS 2
            mqttClient.Publish(mqtt_topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);

        }

        static void ScanCard(string CardNumber){
            TicketPool newTicket = new TicketPool();
        }
    }
}
