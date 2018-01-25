using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Artos.Management.Models;
using Newtonsoft.Json.Linq;
using IdentityModel.Client;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Artos.Management.Controllers
{
    public class EmoneyController : Controller
    {
        // GET: /<controller>/
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            //var content = await client.GetStringAsync("http://localhost:5001/api/EMoneys");

            //ViewBag.Json = JArray.Parse(content).ToString();

            ViewData["Token"] = accessToken;
            return View();
        }

        [Authorize]
        public IActionResult CreateEmoney()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}
