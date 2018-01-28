using System;
namespace Artos.Services.Transaction.Helpers
{
    public class OTPLib
    {
        public OTPLib()
        {
        }

        public static string GenerateOTP(){
            const int maxcount = 4;
            Random rnd = new Random(Environment.TickCount); 
            var generatedOtp = "";
            for (int i = 0; i < maxcount;i++){
                generatedOtp += rnd.Next(1, 9).ToString();
            }
            return generatedOtp;
        }

        public static bool SendSMS(string PhoneNumber, string Message)
        {
            try
            {
                //send sms here
            }catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
