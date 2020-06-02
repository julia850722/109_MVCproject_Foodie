using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Foodie.Models
{
    public class PuchaseRequest
    {
        [Key]
        public Guid Id { get; set; }

        public virtual ApplicationUser Buyer { get; set; }

        [Required]
        [Display(Name ="欲申請代買餐廳")]
        public string RestaurantName { get; set; }
    }
    
}