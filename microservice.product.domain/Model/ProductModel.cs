using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace microservice.product.domain.Model
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ProductCompany { get; set; }

        public decimal? ProductPrice { get; set; }

        public decimal? ProductQuantity { get; set; }

        public int ProductDiscount { get; set; }

        public int CustomerId { get; set; }

        //public List<OrderModel> Orders { get; set; }
    }
}
