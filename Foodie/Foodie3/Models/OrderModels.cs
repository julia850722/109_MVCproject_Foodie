using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Foodie3.Models
{
    public class OrderModels
    {
        [Key]
        public Guid Id { get; set; }
        [Display(Name = "餐點")]
        [Required(ErrorMessage = "餐點 必須輸入!!!")]
        public string Meal { get; set; }
        [Display(Name = "套餐")]
        public string ABC { get; set; }
        [Display(Name = "數量")]
        [Required(ErrorMessage = "數量 必須輸入!!!")]
        public int Number { get; set; }
        [Display(Name = "價錢")]
        public int Price { get; set; }
        [Display(Name = "備註")]
        [DataType(DataType.MultilineText)]
        public string Tips { get; set; }
        [Display(Name = "建立人員")]
        public virtual ApplicationUser User { get; set; }
        [Display(Name = "建立時間")]
        public DateTime? Created { get; set; }
        [Display(Name = "結單")]
        public bool EndFlag { get; set; }
    }
}