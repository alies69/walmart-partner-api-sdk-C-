using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    using System.Globalization;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;


    public class Signature
    {
        public string ConsumerId { get; set; }
        public string PrivateKey { get; set; }
        public string RequestUrl { get; set; }
        public string RequestMethod { get; set; }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public Signature()
        {
        }
        
        public Signature(string consumerId, string privateKey, string requestUrl, string requestMethod)
        {
            ConsumerId = consumerId;
            PrivateKey = privateKey;
            RequestUrl = requestUrl;
            RequestMethod = requestMethod;
        }

        

        public string GetSignature(string timeStamp)
        {
        
            var message = this.ConsumerId + "\n" + this.RequestUrl + "\n" +
                this.RequestMethod.ToUpper() + "\n" + timeStamp + "\n";

            RsaKeyParameters rsaKeyParameter;
            try
            {
                var keyBytes = Convert.FromBase64String(this.PrivateKey);
                var asymmetricKeyParameter = PrivateKeyFactory.CreateKey(keyBytes);
                rsaKeyParameter = (RsaKeyParameters)asymmetricKeyParameter;
            }
            catch (System.Exception)
            {
                throw new Exception("Unable to load private key");
            }

            var signer = SignerUtilities.GetSigner("SHA256withRSA");
            signer.Init(true, rsaKeyParameter);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
            var signed = signer.GenerateSignature();
            var hashed = Convert.ToBase64String(signed);
            return hashed;
        }


        public static string GetTimestampInJavaMillis()
        {

            var timestamp = Math.Round((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds * 1000);
            return timestamp.ToString();

        }
    }
}
