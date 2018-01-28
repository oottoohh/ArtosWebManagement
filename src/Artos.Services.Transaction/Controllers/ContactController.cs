using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artos.Entities;
using Artos.Services.Transaction.Helpers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Artos.Services.Transaction.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly ArtosDB _context;

        public ContactController(ArtosDB context)
        {
            _context = context;
        }
        /// <summary>
        /// send message from mobile app
        /// </summary>
        /// <param name="value"></param>  
        [HttpPost("[action]")]
        public ActionResult SendMessage([FromBody]Contact value)
        {
            var hasil = new OutputData() { IsSucceed = true };
            value.SubmitDate = DateTime.Now;


            //send the result
            if (string.IsNullOrEmpty(value.Message) || string.IsNullOrEmpty(value.Subject) || string.IsNullOrEmpty(value.Email))
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "subject, message and email is empty!";
                return Ok(hasil);
            }else{
                _context.Contacts.Add(value);
                _context.SaveChanges();
            }
            hasil.ErrorMessage = "success";
            return Ok(hasil);
        }

    }
}
