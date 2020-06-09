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

        public Guid PurchaseOffersId { get; set; }

        public string Rname { get; set; }

        public virtual ApplicationUser Seller { get; set; }

        public virtual ApplicationUser Buyer { get; set; }

        public DateTime? SubmittedTime { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
    
}