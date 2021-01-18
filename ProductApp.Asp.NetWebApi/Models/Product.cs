using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductApp.Asp.NetWebApi.Models
{
    public class Product
    {

        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength =2)]
        public string Name { get; set; }

        [Required]
        [Range(1, 999999)]
        public decimal Price { get; set; }

        [Required]
        [Range(2018, 2021)]
        public int ProductionYear { get; set; }

    }
}