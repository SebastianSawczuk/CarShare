namespace CarShare.Models
{
    public class Car
    {
        
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Mileage { get; set; }
        public  double EngineCapacity { get; set; }
        public decimal BasicRentPrice { get; set; }
        public string PlatesNumber { get; set; }
        public bool IsAvailable { get; set; }
        public string ImgUrl { get; set; }
    }
}
