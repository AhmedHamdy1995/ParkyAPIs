using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Models.DTO
{
    public class NationalParkDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
    }
}
