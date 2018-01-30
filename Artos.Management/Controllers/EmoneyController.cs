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
using Artos.Management.Models;
using IdentityModel.Client;
using Newtonsoft.Json;
using System.Text;

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
            var content = await client.GetStringAsync("http://localhost:5001/api/EMoneys");

            ViewBag.Json = JArray.Parse(content).ToString();

            //ViewData["Token"] = accessToken;
            return View();
        }

        [Authorize]
        public IActionResult CreateEmoney()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CardName,Provider,LogoUrl")] EmoneyValidModel dtForm)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                StringContent content = new StringContent(JsonConvert.SerializeObject(dtForm), Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.SetBearerToken(accessToken);
                var result = await client.PostAsync("http://localhost:5001/api/EMoneys", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);

            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([Bind("ID,CardName,Provider,LogoUrl")] EmoneyValidModel dtForm)
        {
            if (ModelState.IsValid)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                StringContent content = new StringContent(JsonConvert.SerializeObject(dtForm), Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.SetBearerToken(accessToken);
                var result = await client.PostAsync("http://localhost:5001/api/EMoneys", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine(resultContent);

            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var Detailcontent = await client.GetStringAsync("http://localhost:5001/api/EMoneys/" + id);
            if (Detailcontent == null)
            {
                return NotFound();
            }else
            {
                ViewBag.detail = JArray.Parse(Detailcontent).ToString();
            }

            return RedirectToAction(nameof(Index));
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
