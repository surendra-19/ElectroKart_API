using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroKart_API.Models.DTOS
{
    public class Customer
    {
        [Key]
        public int Cust_Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; } = "";

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; } = "";

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = "";

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Phone { get; set; } = "";

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string Address { get; set; } = "";
    }
}
