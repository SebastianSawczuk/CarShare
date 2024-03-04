namespace CarShare.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Adress? ClientAdress { get; set; }
        public int DebtCardNumber { get; set; }
    }
}
