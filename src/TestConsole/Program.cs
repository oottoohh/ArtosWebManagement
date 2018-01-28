using System;
using System.Net.Http;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
           TestToken();

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
        async static void TestToken(){
            // discover endpoints from metadata
            //var disco = await DiscoveryClient.GetAsync("http://36.67.176.219:5000");
            var discoclient = new DiscoveryClient("http://36.67.176.219:5000");
            discoclient.Policy.RequireHttps = false;
            var disco = await discoclient.GetAsync();
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "mobileapp", "mobile123");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("masterdataapi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            
            /*
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "webapp1", "web123");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("admin", "admin", "masterdataapi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
            */

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://36.67.176.219:5001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
