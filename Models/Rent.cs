namespace CarShare.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Car Car { get; set; }
        public decimal RentPrice { get; set; }
        public DateTime RentStartDate { get; set; }
        public DateTime RentEndDate { get; set; }
    }
}
