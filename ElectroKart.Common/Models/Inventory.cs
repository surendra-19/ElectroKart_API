using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroKart.Common.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
