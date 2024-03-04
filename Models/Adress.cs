
using System.ComponentModel.DataAnnotations;

namespace CarShare.Models
{
    public class Adress
    {
        public int Id { get; set; }
        [MaxLength(6)]
        public string ZipNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
    }
}
