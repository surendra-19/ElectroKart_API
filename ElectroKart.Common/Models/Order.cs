using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroKart.Common.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int Cust_Id { get; set; }

        [ForeignKey("Cust_Id")]
        public Customer? Customer { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string OrderStatus { get; set; } = "Pending";

        public DateTime? ShippedDate { get; set; }

        public DateTime? ReachedDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        public decimal TotalAmount { get; set; }
    }
}
