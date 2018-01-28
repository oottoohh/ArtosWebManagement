using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Artos.Services.Transaction.Helpers;
using Artos.Entities;
using Newtonsoft.Json;

namespace Artos.Services.Transaction.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        private readonly ArtosDB _context;

        public TicketController(ArtosDB context)
        {
            _context = context;
        }
        /// <summary>
        /// create ticket
        /// </summary>
        /// <param name="value"></param>  
        [HttpPost("[action]")]
        public ActionResult CreateTicket([FromBody]TicketRequestCls value)
        {
            var hasil = new OutputData() { IsSucceed = true };
            //cek apakah ada tiket aktif ??
            var transactions = from a in _context.ArtosTransactions
                               where a.UserId == value.UserId && a.StartDate <= DateTime.Now && a.EndDate > DateTime.Now && a.IsActive
                               select a;
            if (transactions != null && transactions.Count() > 0)
            {
                hasil.IsSucceed = false;
                hasil.Data = transactions.SingleOrDefault();
                hasil.ErrorMessage = "There is active ticket";
                return Ok(hasil);
            }
            //create ticket pool
            var selticket = (from x in _context.TicketPools
                             where x.TicketNo == value.TicketNo
                             select x).SingleOrDefault();

            if (selticket != null)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "ticket is already created!";
                return Ok(hasil);
            }
            else
            {
                //save to ticket pool
                value.GeneratedDate = DateTime.Now;
                value.IsActive = true;
                _context.TicketPools.Add(value);
                _context.SaveChanges();
                //start 
                if (value.TransportId > 0)
                {
                    //get transport data
                    var seltransport = (from x in _context.Transportations
                                        where x.Id == value.TransportId
                                        select x).SingleOrDefault();
                    if (seltransport != null)
                    {
                        //insert header
                        var newtrans = new ArtosTransaction() { StartDate = value.GeneratedDate, EndDate = value.GeneratedDate + GlobalVars.TicketDuration, TicketNo = value.TicketNo, IsActive = true, TotalAmount = seltransport.Price > GlobalVars.MaxPrice ? GlobalVars.MaxPrice : seltransport.Price, TotalWithoutDisc = seltransport.Price > GlobalVars.MaxPrice ? GlobalVars.MaxPrice : seltransport.Price, UserId = value.UserId, UserName = value.UserName, TransactionTypeId = (int)TransactionsTypeIds.Transport };
                        _context.ArtosTransactions.Add(newtrans);
                        _context.SaveChanges();
                        //insert detail
                        var detailtrans = new ArtosTransactionDetail() { TicketNo = value.TicketNo, RouteId = value.RouteId, RouteName = value.RouteName, ScannerId = value.ScannerId, CheckInDate = DateTime.Now, Amount = seltransport.Price > GlobalVars.MaxPrice ? GlobalVars.MaxPrice : seltransport.Price, Moda = value.Moda, Latitude = value.Latitude, Longitude = value.Longitude, Location = value.Location, QRCode = value.QRCode };
                        _context.ArtosTransactionDetails.Add(detailtrans);
                    }
                }

            }
            hasil.ErrorMessage = "success";
            return Ok(hasil);
        }

        /// <summary>
        /// get active ticket for user
        /// </summary>
        /// <param name="UserId"></param>  
        [HttpGet("[action]")]
        public ActionResult GetActiveTicket(int UserId)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = (from x in _context.TicketPools
                           where x.Id == UserId && x.IsActive
                           select x).ToList();
            if (seldata != null && seldata.Count() > 0)
            {
                var ticketno = seldata[0].TicketNo;
                var header = _context.ArtosTransactions.SingleOrDefault(x => x.TicketNo == ticketno);
                var details = _context.ArtosTransactionDetails.Where(x => x.TicketNo == ticketno).ToList();
                var result = new TicketCls();
                result.TicketInfo = header;
                result.Routes = details;
                hasil.Data = result;
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"no active ticker for user id = {UserId}!";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// get list ticket for user
        /// </summary>
        /// <param name="UserId"></param>  
        [HttpGet("[action]")]
        public ActionResult GetTicketListForUser(int UserId)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var headers = _context.ArtosTransactions.Where(x => x.UserId == UserId).OrderBy(x=>x.StartDate).ToList();
            if (headers != null && headers.Count() > 0)
            {
                //get all details by ticket no
                var listTicketNo = (from x in headers
                                    select x.TicketNo).ToList();

                var details = (from y in _context.ArtosTransactionDetails
                              where listTicketNo.Contains(y.TicketNo)
                               select y).ToList();
                
                var result = new List<TicketCls>();
                foreach(var item in headers){
                    var newitem = new TicketCls();
                    newitem.TicketInfo = item;
                    newitem.Routes = details.Where(x => x.TicketNo == item.TicketNo).OrderBy(x => x.CheckInDate).ToList();
                    result.Add(newitem);
                }
                hasil.Data = result;
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"no ticket list for user id = {UserId}!";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// get list ticket by date for user
        /// </summary>
        /// <param name="UserId"></param>  
        /// <param name="SelectedDate"></param>  
        [HttpGet("[action]")]
        public ActionResult GetTicketListByDate(int UserId, DateTime SelectedDate)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var headers = _context.ArtosTransactions.Where(x => x.UserId == UserId && x.StartDate.Date == SelectedDate.Date && x.StartDate.Month == SelectedDate.Month && x.StartDate.Year == SelectedDate.Year ).OrderBy(x=>x.StartDate).ToList();
            if (headers != null && headers.Count() > 0)
            {
                //get all details by ticket no
                var listTicketNo = (from x in headers
                                    select x.TicketNo).ToList();

                var details = (from y in _context.ArtosTransactionDetails
                               where listTicketNo.Contains(y.TicketNo)
                               select y).ToList();

                var result = new List<TicketCls>();
                foreach (var item in headers)
                {
                    var newitem = new TicketCls();
                    newitem.TicketInfo = item;
                    newitem.Routes = details.Where(x => x.TicketNo == item.TicketNo).OrderBy(x => x.CheckInDate).ToList();
                    result.Add(newitem);
                }
                hasil.Data = result;
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"no ticket list for user id = {UserId}!";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// check if user can create ticket
        /// </summary>
        /// <param name="UserId"></param>  
        [HttpGet("[action]")]
        public ActionResult CanCreateTicket(int UserId)
        {
            var hasil = new OutputData() { IsSucceed = true, ErrorMessage="succeed" };
            var seldata = from x in _context.UserCards
                          where x.Id == UserId && x.IsDefault && !x.IsDeleted
                          select x;
            if (seldata != null)
            {
                int selcount = -1;
                int count = 0;
                foreach (var item in seldata)
                {
                    if (item.IsDefault)
                    {
                        selcount = count;
                        hasil.Data = item;
                        if(item.Balance < GlobalVars.MaxPrice){
                            hasil.IsSucceed = false;
                            hasil.ErrorMessage = $"not enough balance, min. balance: {GlobalVars.MaxPrice}";
                        }
                        break;
                    }
                    count++;
                }

            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"no default card for user id = {UserId}!";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// check qr code, is it valid
        /// </summary>
        /// <param name="QRCode"></param>  
        [HttpGet("[action]")]
        public ActionResult CheckQRCode(string QRCode)
        {
            var hasil = new OutputData() { IsSucceed = true, ErrorMessage = "succeed" };
            var qrcode = new QRCodeArtos(QRCode);
            //check transport
            var seltransport = _context.Transportations.SingleOrDefault(x => x.Id == qrcode.TransportId);
            if(seltransport==null){
                hasil.ErrorMessage += "transport id not found.";
                hasil.IsSucceed = false;
            }
            //check route
            var selroute  = _context.Transportations.SingleOrDefault(x => x.Id == qrcode.RouteId);
            if (selroute == null)
            {
                hasil.ErrorMessage += "route id not found.";
                hasil.IsSucceed = false;
            }else{
                if(qrcode.Price!=selroute.Price){
                    hasil.ErrorMessage += "price is not same with route data.";
                    hasil.IsSucceed = false;
                }
            }
            //police num
            if(string.IsNullOrEmpty(qrcode.PoliceNumber)){
                hasil.ErrorMessage += "police number is empty.";
                hasil.IsSucceed = false;
            }
            //moda
            if (string.IsNullOrEmpty(qrcode.Moda))
            {
                hasil.ErrorMessage += "moda is empty.";
                hasil.IsSucceed = false;
            }
            //route name
            if (string.IsNullOrEmpty(qrcode.RouteName))
            {
                hasil.ErrorMessage += "route name is empty.";
                hasil.IsSucceed = false;
            }
            hasil.Data = JsonConvert.SerializeObject(qrcode);
            return Ok(hasil);
        }
        /// <summary>
        /// create ticket no
        /// </summary>
        /// <param name="value"></param>  
        [HttpPost("[action]")]
        public ActionResult CreateTicketNo([FromBody]TicketGenerator value)
        {
            var hasil = new OutputData() { IsSucceed = true, ErrorMessage = "succeed" };

            hasil.Data = value.GetTicketNo();

            return Ok(hasil);
        }
    }

    public class TicketRequestCls:TicketPool
    {
       
        public double Latitude
        {
            get;
            set;
        }
        public double Longitude
        {
            get;
            set;
        }
        public string Location
        {
            get;
            set;
        }
        public string QRCode
        {
            get;
            set;
        }
        public string RouteName
        {
            get;
            set;
        }
        public long RouteId
        {
            get;
            set;
        }
        public long ScannerId
        {
            get;
            set;
        }
    }

    public class TicketCls 
    {
        public ArtosTransaction TicketInfo { set; get; }
        public List<ArtosTransactionDetail> Routes
        {
            get;
            set;
        }
    }
    
}