namespace HotelsManager.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int? HotelId { get; set; }
        public string? HotelName { get; set; }
        public int HotelStars { get; set; }
        public decimal? HotelPrice { get; set; }
        public string? HotelCurrency { get; set; }
        public Order() { }
        public Order(string username, int hotelId, string hotelName, int hotelStars, decimal? hotelPrice, string hotelCurrency)
        {
            UserName = username;
            HotelId = hotelId;
            HotelName = hotelName;
            HotelStars = hotelStars;
            HotelPrice = hotelPrice;
            HotelCurrency = hotelCurrency;
        }
    }
}

