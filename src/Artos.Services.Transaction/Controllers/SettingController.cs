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
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SettingController : Controller
    {
        private readonly ArtosDB _context;

        public SettingController(ArtosDB context)
        {
            _context = context;
        }
        /// <summary>
        /// get all static content (page, etc)
        /// </summary>
      

        [HttpGet("[action]")]
        public ActionResult GetStaticContent()
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = _context.StaticContents;
            if (seldata != null)
            {
                hasil.Data = seldata.ToList();
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"static content is not found !";
                return Ok(hasil);
            }

            return Ok(hasil);
        }

        /// <summary>
        /// get global config
        /// </summary>

        [HttpGet("[action]")]
        public ActionResult GetGlobalConfig()
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = _context.GlobalConfigs;
            if (seldata != null)
            {
                hasil.Data = seldata.ToList();
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"global config is not found !";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
    }
}
