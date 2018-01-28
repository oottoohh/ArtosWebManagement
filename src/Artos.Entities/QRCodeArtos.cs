using System;
namespace Artos.Entities
{
    public class QRCodeArtos
    {
        public QRCodeArtos()
        {

        }
        //[JsonProperty("tid")]
        public long TransportId
        {
            get;
            set;
        }
        //[JsonProperty("rid")]
        public long RouteId
        {
            get;
            set;
        }
        //[JsonProperty("rot")]
        public string RouteName
        {
            get;
            set;
        }
        //[JsonProperty("mod")]
        public string Moda
        {
            get;
            set;
        }
        //[JsonProperty("prc")]
        public decimal Price
        {
            get;
            set;
        }
        //[JsonProperty("num")]
        public string PoliceNumber
        {
            get;
            set;
        }
        public QRCodeArtos(string input)
        {
            ParseQRCode(input);
        }

        public string GenerateQRCode(){
            return $"{TransportId}|{RouteId}|{RouteName}|{Moda}|{Price}|{PoliceNumber}";
        }
        public bool ParseQRCode(string input)
        {
            try
            {
                var strdata = input.Split('|');
                TransportId = long.Parse( strdata[0]);
                RouteId = long.Parse(strdata[1]);
                RouteName = strdata[2];
                Moda = strdata[3];
                Price = decimal.Parse(strdata[4]);
                PoliceNumber = strdata[5];

            }catch{
                return false;
            }
            return true;
        }
    }
}
