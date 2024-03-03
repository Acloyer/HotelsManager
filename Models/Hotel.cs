namespace HotelsManager.Models.Hotels
{
    public class Hotel
    {
        public int Id { get; set; }
    
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public int Stars { get; set; }
        public Hotel() { }
        public Hotel(int id, string name, decimal price, string currency, int stars)
        {
            Id = id;
            Name = name;
            Price = price;
            Currency = currency;
            Stars = stars;
        }
    }
}

