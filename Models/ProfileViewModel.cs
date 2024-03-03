using HotelsManager.Models.Hotels;
using HotelsManager.Models.Orders;
using HotelsManager.Models.Users;
using HotelsManager.Models.Repository;

public class ProfileViewModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public int OrdersCount{ get; set; }
    public ProfileViewModel(string login, string pass, int oCount)
    {
        Login = login;
        Password = pass;
        OrdersCount = oCount;
    }
}
