using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace microservice.product.domain.Model
{
    public class OrderModel
    {
        [Key]
        public int OrdertId { get; set; }

        public int ProductId { get; set; }

        public string? OrderStatus { get; set; }
        //[JsonIgnore]
        //public ProductModel Product { get; set; }


    }
}
