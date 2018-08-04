using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory.Web.Models
{
    public class PurchaseOrder
    {
        /// <summary>
        /// Store for the ProductId property.
        /// </summary>
        [JsonProperty(PropertyName = "productId")]
        public virtual int ProductId { get; set; }

        /// <summary>
        /// Store for the FullName property.
        /// </summary>
        [JsonProperty(PropertyName = "fullName")]
        public virtual string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Store for the CreditCardNumber property.
        /// </summary>
        [JsonProperty(PropertyName = "creditCardNumber")]
        public virtual string CreditCardNumber { get; set; } = string.Empty;

        /// <summary>
        /// Store for the Product property.
        /// </summary>
        public Product Product { get; set; }
    }
}