using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Inventory.Web.Models;
using NHibernate;

namespace Inventory.Web.Controllers
{
    public class ProductController : ApiController
    {
        // GET: Product
        public IEnumerable<Product> Get()
        {
            IList<Product> product;

            using (ISession session = NHibernateSession.OpenSession())
            {
                product = session.Query<Product>().ToList();
            }

            return product;
        }

        // GET: Product/Details/5
        public Product Get(int id)
        {
            Product product = new Product();

            using (ISession session = NHibernateSession.OpenSession())
            {
                product = session.Query<Product>().Where(b => b.Id == id).FirstOrDefault();
            }

            return product;
        }

        // POST: Product/Create
        public void Post([FromBody]Product product)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(product);
                    transaction.Commit();
                }
            }
        }

        // PUT: Product/Edit/5
        public Product Put(int id, [FromBody]PurchaseOrder purchaseOrder)
        {
            Product product;

            // Full Name is required
            if(string.IsNullOrEmpty(purchaseOrder.FullName))
            {
                // Return error message
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("Full Name Required", id)),
                    ReasonPhrase = "FullNameRequired"
                };
                throw new HttpResponseException(resp);
            }

            using (ISession session = NHibernateSession.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    product = session.Query<Product>().Where(b => b.Id == id).FirstOrDefault();

                    if(product != null && product.Quantity > 0)
                    {
                        // If the product exists and is avaiable, validate the credit card
                        if(purchaseOrder.CreditCardNumber != null && 
                            PaymentController.IsValidCardNumber(purchaseOrder.CreditCardNumber))
                        {
                            // Attempt to charge payment using the gateway
                            if(PaymentController.ChargePayment(purchaseOrder.CreditCardNumber, product.Price))
                            {
                                // Attach the product to the purchase order
                                purchaseOrder.Product = product;

                                // Deduct one from the inventory
                                product.Quantity = product.Quantity - 1;

                                // Update the new product inventory
                                session.SaveOrUpdate(product);

                                // Send the purchase order for fulfillment
                                EmailController.SendPurchaseOrder(purchaseOrder);

                                // Commit the transaction
                                transaction.Commit();
                            }
                            else
                            {
                                // Payment gateway failed
                                transaction.Rollback();

                                // Return error message
                                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new StringContent(string.Format("Payment Gateway Failed", id)),
                                    ReasonPhrase = "PaymentGatewayFailed"
                                };
                                throw new HttpResponseException(resp);
                            }
                        } else
                        {
                            // Credit card invalid
                            transaction.Rollback();

                            // Return error message
                            var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new StringContent(string.Format("Invalid Credit Card Number", id)),
                                ReasonPhrase = "InvalidCreditCardNumber"
                            };
                            throw new HttpResponseException(resp);
                        }
                    }
                    else
                    {
                        // Quantity for product is unavailable
                        transaction.Rollback();

                        // Return error message
                        var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent(string.Format("No available product {0}", product.Name)),
                            ReasonPhrase = "NoInventory"
                        };
                        throw new HttpResponseException(resp);
                    }
                }
            }

            return product;
        }

        // DELETE: Product/Delete/5
        public void Delete(int id)
        {
            // TODO: Add delete logic here
            using (ISession session = NHibernateSession.OpenSession())
            {
                Product book = session.Get<Product>(id);

                using (ITransaction trans = session.BeginTransaction())
                {
                    session.Delete(book);
                    trans.Commit();
                }
            }
        }
    }
}
