using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Foodie.Models
{
    public class PurchaseOffer
    {

        [Key]
        public Guid Id { get; set; }

        [DisplayName("代買外送員")]
        public string SellerName { get; set; }
        public virtual ApplicationUser Seller { get; set; }
        [DisplayName("代買結束時間")]
        public DateTime? DeadLineTime { get; set; }
      
        [Required]
        [DisplayName("預計幾分鐘後結單")]
        public int OfferTime { get; set; }
        [DisplayName("餐廳地點")]
        public string OfferedRestaurant { get; set; }
    }
}