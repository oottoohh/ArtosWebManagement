using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Artos.Services.Transaction.Helpers;
using Artos.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artos.Services.Transaction.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class EMoneyController : Controller
    {
        private readonly ArtosDB _context;

        public EMoneyController(ArtosDB context)
        {
            _context = context;
        }
        /// <summary>
        /// get list of card that belong to user
        /// </summary>
        /// <param name="UserId"></param>  
        [HttpGet("[action]")]
        public ActionResult GetCardListForUser(int UserId)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = from x in _context.UserCards
                          where x.Id == UserId && !x.IsDeleted 
                          select x;
            if (seldata != null)
            {
                hasil.Data = seldata.ToList();
                hasil.ErrorMessage = "success";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"card is not found for userid = {UserId}!";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// set default card for user
        /// </summary>
        /// <param name="UserId"></param>  
        /// <param name="CardId"></param>  
        [HttpGet("[action]")]
        public ActionResult SetDefaultCard(int UserId,int CardId)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = from x in _context.UserCards
                           where x.Id == UserId && !x.IsDeleted
                           select x;
            if (seldata != null)
            {
                int selcount = -1;
                int count = 0;
                foreach (var item in seldata)
                {
                    if (item.Id == CardId)
                    {
                        selcount = count;
                        item.IsDefault = true;
                    }
                    else
                    {
                        item.IsDefault = false;
                    }
                    count++;
                    _context.UserCards.Update(item);
                }
                if (selcount != -1)
                {
                    _context.SaveChanges();
                    hasil.ErrorMessage = "succeed";
                }
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"card is not found for userid = {UserId}!";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// get default card for user
        /// </summary>
        /// <param name="UserId"></param>  
        [HttpGet("[action]")]
        public ActionResult GetDefaultCard(int UserId)
        {
            var hasil = new OutputData() { IsSucceed = true };
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
                    }
                    count++;
                }
                if (selcount != -1)
                {
                    hasil.ErrorMessage = "succeed";
                }
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"default card is not found for userid = {UserId}!";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// add user cards
        /// </summary>
        /// <param name="userCards"></param>  
        [HttpPost("[action]")]
        public async Task<IActionResult> AddCards([FromBody] List<UserCard> userCards)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {
                foreach (var userCard in userCards)
                {
                    userCard.Balance = 0;
                    //validate card to bank api and get latest balance
                    //call validate service
                    //if valid then add to db
                    _context.UserCards.Add(userCard);
                }
                await _context.SaveChangesAsync();
                hasil.ErrorMessage = "Succeed";
            }
            catch(Exception ex)
            {
                    hasil.IsSucceed = false;
                    hasil.ErrorMessage = $"error on save card = {ex.Message}!";
                    return Ok(hasil);
            }
            return Ok(hasil);
        }
        /// <summary>
        /// Validate Card
        /// </summary>
        /// <param name="CardNumber"></param>  
        /// <param name="ProviderId"></param>  
        [HttpGet("[action]")]
        public ActionResult ValidateCard(string CardNumber, string ProviderId)
        {
            var hasil = new OutputData() { IsSucceed = true };
            if(true)//call bank api to validate
            {
                hasil.ErrorMessage = "ok valid";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"this card is not valid!";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// Remove Card
        /// </summary>
        /// <param name="CardId"></param>  
        [HttpDelete("[action]")]
        public async Task<IActionResult> RemoveCard([FromRoute] long CardId)
        {
            var hasil = new OutputData() { IsSucceed = true };
            try
            {

                var item = await _context.UserCards.SingleOrDefaultAsync(m => m.Id == CardId);
                if (item == null)
                {
                    hasil.IsSucceed = false;
                    hasil.ErrorMessage = $"card is not found!";
                }
                else
                {
                    _context.UserCards.Remove(item);
                    await _context.SaveChangesAsync();
                    hasil.ErrorMessage = "ok valid";
                }
            }
            catch (Exception ex)
            {

                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"error delete card : {ex.Message}!";
                return Ok(hasil);
            }

            return Ok(hasil);

        }
        /// <summary>
        /// get all emoney card and provider
        /// </summary>
        
        [HttpGet("[action]")]
        public ActionResult GetCardList()
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = _context.EMoneys;
            if (seldata != null) {
                hasil.Data = seldata.ToList();
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"card provider is not found !";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// get card transaction by userid and cardnumber
        /// </summary>
        ///  <param name="UserId"></param>  
        /// <param name="CardNumber"></param>  
        [HttpGet("[action]")]
        public ActionResult GetTransactionByCardNo(long UserId, string CardNumber)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = from x in _context.CardTransactions
                          where x.CardNumber == CardNumber && x.UserId == UserId
                          orderby x.TransactionDate descending
                          select x;
            if (seldata != null)
            {
                hasil.Data = seldata.ToList();
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"card transactions is not found !";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// get card transaction by date
        /// </summary>
        ///  <param name="UserId"></param>  
        /// <param name="CardNumber"></param>  
        ///  <param name="SelectedDate"></param>  
        [HttpGet("[action]")]
        public ActionResult GetCardTransactionByDate(long UserId, string CardNumber, DateTime SelectedDate)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = from x in _context.CardTransactions
                          where x.CardNumber == CardNumber && x.UserId == UserId && x.TransactionDate.Date == SelectedDate.Date && x.TransactionDate.Month == SelectedDate.Month && x.TransactionDate.Year == SelectedDate.Year
                          orderby x.TransactionDate descending
                          select x;
            if (seldata != null)
            {
                hasil.Data = seldata.ToList();
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"card transactions is not found !";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
        /// <summary>
        /// get transaction date list per user
        /// </summary>
        ///  <param name="UserId"></param>  
        
        [HttpGet("[action]")]
        public ActionResult GetTransactionDateList(long UserId)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seldata = from x in _context.CardTransactions
                          where x.UserId == UserId
                          orderby x.TransactionDate
                          select new { x.TransactionDate };
            if (seldata != null)
            {
                hasil.Data = seldata.Distinct().ToList();
                hasil.ErrorMessage = "succeed";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = $"card transactions is not found !";
                return Ok(hasil);
            }

            return Ok(hasil);
        }
    }
}