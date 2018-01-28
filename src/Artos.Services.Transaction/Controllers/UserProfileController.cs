using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Artos.Entities;
using Artos.Services.Transaction.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Artos.Services.Transaction.Controllers
{
    //[Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserProfileController : Controller
    {
        private readonly ArtosDB _context;

        public UserProfileController(ArtosDB context)
        {
            _context = context;
        }
        /// <summary>
        /// Register user from mobile app
        /// </summary>
        /// <param name="value"></param>  
        [HttpPost("[action]")]
        public ActionResult Register([FromBody]RegisterCls value)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seluser = (from x in _context.UserProfiles
                           where x.UserName.ToLower() == value.Name.ToLower() && x.Phone == value.Phone
                           select x).SingleOrDefault();

             //send the result
            if (seluser != null)
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "user is already registered!";
                return Ok(hasil);
            }
            else
            {
                var newUser = new UserProfile();
                newUser.Phone = value.Phone;
                newUser.UserName = value.Name;
                _context.UserProfiles.Add(newUser);
                _context.SaveChanges();
                
                //generate otp and save to database
                var otp = new OTP() { OTPType = OTPTypes.Register, CreatedDate = DateTime.Now, Phone = value.Phone, ExpiryDate = DateTime.Now.AddMinutes(5), Attempt = 0, IsActive = true, IsValidated = false, Number = OTPLib.GenerateOTP() };
                hasil.Data = new { userid = newUser.Id, username = newUser.UserName, otp = otp.Number, otptype = otp.OTPType };
                _context.OTPs.Add(otp);
                _context.SaveChanges();
                //send otp to user via web service - twilio
                OTPLib.SendSMS(otp.Phone, $"input this number for {otp.OTPType.ToString()} on Artos App : {otp.Number}");

            }
            hasil.ErrorMessage = "success";
            return Ok(hasil);
        }
        /// <summary>
        /// re-send otp number and invalidate previous number
        /// </summary>
        /// <param name="value"></param>  
        [HttpPost("[action]")]
        public ActionResult ReSendOTP([FromBody]ReSendOTPCls value)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var selotp = (from x in _context.OTPs
                          where x.Number == value.Number && x.Phone == value.Phone && x.IsActive
                          select x).SingleOrDefault();
            if (selotp != null)
            {
                selotp.IsActive = false;
                selotp.IsValidated = false;
                _context.OTPs.Update(selotp);
                _context.SaveChanges();
                //generate otp and save to database
                var otp = new OTP() { OTPType = selotp.OTPType, CreatedDate = DateTime.Now, Phone = value.Phone, ExpiryDate = DateTime.Now.AddMinutes(5), Attempt = 0, IsActive = true, IsValidated = false, Number = OTPLib.GenerateOTP() };
                hasil.Data = new { otp = otp.Number, otptype = otp.OTPType };
                _context.OTPs.Add(otp);
                _context.SaveChanges();
                //send otp to user via web service - twilio
                OTPLib.SendSMS(otp.Phone, $"input this number for {otp.OTPType.ToString()} on Artos App : {otp.Number}");

            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "otp is not found!";
                return Ok(hasil);
            }
            hasil.ErrorMessage = "success";
            return Ok(hasil);
        }
        /// <summary>
        /// change status otp to validated and set to non active
        /// </summary>
        /// <param name="value"></param>  
        [HttpPost("[action]")]
        public ActionResult ValidateOTP([FromBody]ReSendOTPCls value)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var selotps = (from x in _context.OTPs
                           where x.Number == value.Number && x.Phone == value.Phone
                           select x);
            if (selotps != null && selotps.Count() > 0)
            {
                foreach (var selotp in selotps)
                {

                    selotp.IsActive = false;
                    selotp.IsValidated = true;
                    _context.OTPs.Update(selotp);

                }
                _context.SaveChanges();
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "otp is not found!";
                return Ok(hasil);
            }
            hasil.ErrorMessage = "success";
            return Ok(hasil);
        }
        /// <summary>
        /// Login user from mobile app
        /// </summary>
        /// <param name="login"></param>  
        [HttpPost("[action]")]
        public ActionResult Login([FromBody]LoginCls login)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seluser = (from x in _context.UserProfiles
                           where x.Phone == login.Phone
                           select x).SingleOrDefault();
            //generate otp and save to database
            var otp = new OTP() { OTPType = OTPTypes.Login, CreatedDate = DateTime.Now, Phone = login.Phone, ExpiryDate = DateTime.Now.AddMinutes(5), Attempt = 0, IsActive = true, IsValidated = false, Number = OTPLib.GenerateOTP() };
            hasil.Data = new { userid=seluser.Id, username = seluser.UserName, otp = otp.Number, otptype = otp.OTPType };
            _context.OTPs.Add(otp);
            _context.SaveChanges();
            //send otp via twilio
            OTPLib.SendSMS(otp.Phone, $"input this number for {otp.OTPType.ToString()} on Artos App : {otp.Number}");

            //send result
            if (seluser != null)
            {
                return Ok(hasil);
            }
            hasil.IsSucceed = false;
            hasil.ErrorMessage = "user not found!";
            return Ok(hasil);
        }
        /// <summary>
        /// reset user password
        /// </summary>
        /// <param name="value"></param>  
        [HttpPost("[action]")]
        public ActionResult ResetUserPassword([FromBody]ResetPassCls value)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seluser = (from x in _context.UserProfiles
                           where x.UserName == value.UserName && x.Email == value.Email
                           select x).SingleOrDefault();
            if (seluser != null)
            {
                seluser.Password = Artos.Tools.PasswordHelper.CreatePassword(10); //Guid.NewGuid().ToString().Replace("-", "").Substring(1, 10);
                _context.UserProfiles.Update(seluser);
                _context.SaveChanges();

                //send otp to user via web service - twilio
                OTPLib.SendSMS(seluser.Phone, $"your password for Artos has been reset. check your email ({seluser.Email}).");
                if (!string.IsNullOrEmpty(seluser.Email))
                    EmailLib.SendEmail("Artos password has been reset", $"your new password is {seluser.Password}", seluser.Email);
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "username is not found!";
                return Ok(hasil);
            }
            hasil.ErrorMessage = "success";
            return Ok(hasil);
        }
        /// <summary>
        /// change user password
        /// </summary>
        /// <param name="value"></param>  
        [HttpPost("[action]")]
        public ActionResult ChangeUserPassword([FromBody]ChangePasswordCls value)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seluser = (from x in _context.UserProfiles
                           where x.UserName == value.UserName && x.Password == value.OldPassword
                           select x).SingleOrDefault();
            if (seluser != null)
            {
                seluser.Password = value.NewPassword;
                _context.UserProfiles.Update(seluser);
                _context.SaveChanges();

                //send otp to user via web service - twilio
                OTPLib.SendSMS(seluser.Phone, $"your password for Artos has been changed. ");
                if (!string.IsNullOrEmpty(seluser.Email))
                    EmailLib.SendEmail("Artos password has been changed.", $"your password for Artos has been changed.", seluser.Email);
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "username is not found or password not match!";
                return Ok(hasil);
            }
            hasil.ErrorMessage = "success";
            return Ok(hasil);
        }

        /// <summary>
        /// get user profile by user id
        /// </summary>
        /// <param name="UserId"></param>  
        [HttpGet("[action]")]
        public ActionResult GetUserProfile(int UserId)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seluser = (from x in _context.UserProfiles
                           where x.Id == UserId
                           select x).SingleOrDefault();
            if (seluser != null)
            {
                hasil.Data = new {seluser.Id, seluser.Email, seluser.CardID, seluser.Address, seluser.FullName, seluser.Phone, seluser.PicUrl, seluser.PIN };
                hasil.ErrorMessage = "success";
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "username is not found or password not match!";
                return Ok(hasil);
            }
          
            return Ok(hasil);
        }
        /// <summary>
        /// save user profile
        /// </summary>
        /// <param name="value"></param>  
        /// <param name="UserId"></param> 
        [HttpPut("[action]")]
        public ActionResult SaveUserProfile([FromRoute] long UserId, [FromBody]SaveUserProfileCls value)
        {
            var hasil = new OutputData() { IsSucceed = true };
            var seluser = (from x in _context.UserProfiles
                           where x.Id ==UserId
                           select x).SingleOrDefault();
            if (seluser != null)
            {
                seluser.FullName = value.FullName;
                seluser.Address = value.Address;
                seluser.CardID = value.CardID;
                seluser.Email = value.Email;
                seluser.Phone = value.Phone;
                seluser.PicUrl = value.PicUrl;
                seluser.PIN = value.PIN;
                
                _context.UserProfiles.Update(seluser);
                _context.SaveChanges();

                if (!string.IsNullOrEmpty(seluser.Email))
                    EmailLib.SendEmail("Artos profile has been changed.", $"your profile for Artos has been changed.", seluser.Email);
            }
            else
            {
                hasil.IsSucceed = false;
                hasil.ErrorMessage = "userid is not found !";
                return Ok(hasil);
            }
            hasil.ErrorMessage = "success";
            return Ok(hasil);
        }
    }
    public class SaveUserProfileCls
    {
        public string Email { get; set; }
        public string CardID { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PIN { get; set; }
        public string PicUrl { get; set; }
        public string FullName { get; set; }

    }
    public class ChangePasswordCls
    {
        public string UserName
        {
            get;
            set;
        }
        public string NewPassword
        {
            get;
            set;
        }
        public string OldPassword
        {
            get;
            set;
        }
    }
    public class ResetPassCls
    {
        public string UserName
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
    }
    public class RegisterCls
    {
        public string Name
        {
            get;
            set;
        }
        public string Phone
        {
            get;
            set;
        }
    }

    public class LoginCls{
        public string Phone
        {
            get;
            set;
        }
    }

    public class ReSendOTPCls
    {
        public string Phone
        {
            get;
            set;
        }
        public string Number
        {
            get;
            set;
        }
    }
}
