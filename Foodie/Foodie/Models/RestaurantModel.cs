using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Foodie.Models
{
    public class RestaurantModel
    {
        [Key]
        public Guid Id { get; set; }

        public string RestaurantName { get; set; }

        public string Address { get; set; }
    }
}