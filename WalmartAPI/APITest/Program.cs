using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;

namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            // order api calls
            // Get all released orders
            string reqUrl ="orders?limit=25&&createdStartDate=2017-01-11&createdEndDate=2017-01-18";
            string responseStr = SendRequest.GetOrders(reqUrl);

            //Get an order
            string purchaseOrder = "2575193093776";
            reqUrl = "orders/" + purchaseOrder;
            responseStr = SendRequest.GetOrder(reqUrl);

            //Acknowledge orders 
            purchaseOrder = "2575193093776";
            reqUrl = "orders/" + purchaseOrder + "/acknowledge";
            responseStr = SendRequest.SendAcknowledge(reqUrl);

            //Shipping notifications/updates 
            purchaseOrder = "2575193093776";
            reqUrl = "orders/" + purchaseOrder + "/shipping";
            string xml = GetShippingXML();
            responseStr = SendRequest.SendShipping(reqUrl,xml);


        }

        private static string GetShippingXML()
        {

            StringBuilder sb = new StringBuilder();
            string url = "https://www.ups.com";
            sb.Append("<?xml version='1.0' encoding='UTF-8' standalone='yes'?>");
            sb.Append("<ns2:orderShipment xmlns:ns2='http://walmart.com/mp/v3/orders' xmlns:ns3='http://walmart.com/'>");
            sb.Append("<ns2:orderLines>");
            sb.Append("<ns2:orderLine>");
            sb.Append("<ns2:lineNumber>1</ns2:lineNumber>");
            sb.Append("<ns2:orderLineStatuses>");
            sb.Append("<ns2:orderLineStatus>");
            sb.Append("<ns2:status>Shipped</ns2:status>");
            sb.Append("<ns2:statusQuantity>");
            sb.Append("<ns2:unitOfMeasurement>Each</ns2:unitOfMeasurement>");
            sb.Append("<ns2:amount>2</ns2:amount>");
            sb.Append("</ns2:statusQuantity>");
            sb.Append("<ns2:trackingInfo>");
            sb.Append("<ns2:shipDateTime>2017-01-12T12:12:12.000Z</ns2:shipDateTime>");
            sb.Append("<ns2:carrierName>");
            sb.Append("<ns2:carrier>UPS</ns2:carrier>");
            sb.Append("</ns2:carrierName>");
            sb.Append("<ns2:methodCode>Standard</ns2:methodCode>");
            sb.Append("<ns2:trackingNumber>12345678899</ns2:trackingNumber>");
            sb.Append("<ns2:trackingURL><![CDATA[" + url + "]]></ns2:trackingURL>");
            sb.Append("</ns2:trackingInfo>");
            sb.Append("</ns2:orderLineStatus>");
            sb.Append("</ns2:orderLineStatuses>");
            sb.Append("</ns2:orderLine>");
            sb.Append("</ns2:orderLines>");
            sb.Append("</ns2:orderShipment>");

            return sb.ToString();
        }
    }
}
