using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class SendRequest
    {
        private static string ApiKey = "ApiKey"; // assign your apikey
        private static string ConsumerId = "ConsumerId"; // assign your consumerId
        private static string WM_CONSUMERCHANNELTYPE = "WM_CONSUMERCHANNELTYPE"; // assign WM_CONSUMERCHANNELTYPE
        private static string apiUrl = "https://marketplace.walmartapis.com/v3/";

        public static string GetOrders(string reqUrl)
        {
            apiUrl="orders?limit=25&&createdStartDate=2017-01-11&createdEndDate=2017-01-18";
            string method = "GET";
            string responseStr = "";
            HttpWebRequest req = CreateRequestObject(apiUrl, method);
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseStr = new StreamReader(responseStream).ReadToEnd();
            }
            else
                responseStr = "error";
            return responseStr;

        }
        
        public static string GetOrder(string reqUrl)
        {
            string method = "GET";
            string responseStr = "";
            HttpWebRequest req = CreateRequestObject(apiUrl, method);
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseStr = new StreamReader(responseStream).ReadToEnd();
            }
            else
                responseStr = "error";
            return responseStr;
        }

        public static string SendAcknowledge(string reqUrl)
        {

            string method = "POST";
            HttpWebRequest req = CreateRequestObject(reqUrl, method);
            req.MediaType = "application/xml";
            req.ContentLength = 0;
            
            HttpWebResponse response;
            response = (HttpWebResponse)req.GetResponse();
            string responseStr = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseStr = new StreamReader(responseStream).ReadToEnd();

            }
            else
            {
                responseStr = "error";
            }
            return responseStr;

        }

        public static string SendShipping(string reqUrl, string xml)
        { 

            string method = "POST";
            HttpWebRequest req = CreateRequestObject(reqUrl, method);
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            req.GetRequestStream().Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            string responseStr = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = response.GetResponseStream();
                responseStr = new StreamReader(responseStream).ReadToEnd();

            }
            else
                 responseStr="error";

            return responseStr;

        
        }

        private static HttpWebRequest CreateRequestObject(string requestUrl, string method)
        {
            Signature sig = new Signature(ConsumerId, ApiKey, requestUrl, method);
            string timeStamp = Signature.GetTimestampInJavaMillis();
            string sigStr = sig.GetSignature(timeStamp.ToString());
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUrl);
            req.ContentType = "application/xml";
            req.Accept = "application/xml";

            req.Method = method;
            req.Headers.Add("WM_SVC.NAME", "Walmart Marketplace");
            req.Headers.Add("WM_SEC.AUTH_SIGNATURE", sigStr);
            req.Headers.Add("WM_CONSUMER.ID", ConsumerId);
            req.Headers.Add("WM_SEC.TIMESTAMP", timeStamp.ToString());
            req.Headers.Add("WM_QOS.CORRELATION_ID", "123456abcdef");
            req.Headers.Add("WM_CONSUMER.CHANNEL.TYPE", WM_CONSUMERCHANNELTYPE);
            return req;


        }

     
        
    }
}
