using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroKart.Common.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string ProductName { get; set; } = "";

        [Column(TypeName = "NVARCHAR(250)")]
        public string ProductDescription { get; set; } = "";

        [Required]
        [Column(TypeName = "DECIMAL(15,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string Brand { get; set; } = "";

        [Column("Category_Id")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string ImageUrl { get; set; } = "";
    }
}
