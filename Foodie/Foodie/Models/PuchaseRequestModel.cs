using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("餐廳名稱")]
        public string Rname { get; set; }


        [DisplayName("外送員")]
        public virtual ApplicationUser Seller { get; set; }


        [DisplayName("買方")]
        public virtual ApplicationUser Buyer { get; set; }

        [DisplayName("送出時間")]
        public DateTime? SubmittedTime { get; set; }

        [DisplayName("餐點內容")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
    
}