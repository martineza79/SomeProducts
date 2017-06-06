using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SomeProducts.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string UserId { get; set; }
    }
}