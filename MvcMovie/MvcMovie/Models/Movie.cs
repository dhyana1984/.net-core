using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class Movie
    {
        public int Id { get; set; }
        //从本质上来说，需要值类型（如 decimal、int、float、DateTime），但不需要[Required] 特性
        //Required 和 MinimumLength 特性表示属性必须有值；但用户可输入空格来满足此验证。
        [StringLength(60,MinimumLength =3)]
        [Required]
        public string Title { get; set; }

        // Display 特性指定要显示的字段名称的内容
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        //DisplayFormat 特性用于显式指定日期格式
        //ApplyFormatInEditMode 设置指定在文本框中显示值以进行编辑时也应用格式
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        //RegularExpression 特性用于限制可输入的字符
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$",ErrorMessage ="Please input in English")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        //Range 特性将值限制在指定范围内
        [Range(1,100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public string Rating { get; set; }
    }
}
