using Inventory.Web.Models;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class EmailController : Controller
    {
        /// <summary>
        /// Sends purchase order email to the inventory team
        /// </summary>
        /// <param name="purchaseOrder">The purchase order object</param>
        public static void SendPurchaseOrder(PurchaseOrder purchaseOrder)
        {

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("inventory@bodhidevelopment.com", "AGA2cZX7&64s"),
                EnableSsl = true
            };

            string subject = "Receipt for " + purchaseOrder.Product.Name;
            string body = purchaseOrder.FullName + " purchased item " + purchaseOrder.Product.Name + " with ID " + purchaseOrder.ProductId;

#if !DEBUG
            // Only send out emails when running production
            client.Send("inventory@bodhidevelopment.com", "inventory@bodhidevelopment.com", subject, body);
#endif
        }
    }
}
