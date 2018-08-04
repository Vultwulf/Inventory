using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory.Web.Models
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public virtual int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public virtual string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public virtual string Description { get; set; }

        [JsonProperty(PropertyName = "price")]
        public virtual decimal Price { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public virtual int Quantity { get; set; }
    }
}