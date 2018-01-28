using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Artos.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Artos.Jobs.FeaturedPhone
{
    class Program
    {
        public static IConfigurationRoot Configuration;

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
                    .AddJsonFile($"appsettings.{environment}.json", optional: false);
            }

            Configuration = builder.Build();
            var hostname = Configuration.GetSection("server").GetSection("rabbithost").Value;
            var quetopic = Configuration.GetSection("server").GetSection("quetopic").Value;

            //Configuration.GetSection("Logging"))
            var factory = new ConnectionFactory() { HostName = hostname };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: quetopic,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    RouteMessage(message);//.Start();
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: quetopic,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

        }

        static void RouteMessage(string Message){
            var SMS = JsonConvert.DeserializeObject<SMSMessage>(Message);
            if(!string.IsNullOrEmpty(SMS.Message)){
                switch(SMS.Message.Split(' ')[0].ToLower()){
                    case "beli":
                        break;
                }
            }
        }
    }
}
