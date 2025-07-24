using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroKart.Common.Models
{
    public class Brand
    {
        [Key]
        public int Brand_Id { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        public string Brand_Name { get; set; } = "";
    }
}
