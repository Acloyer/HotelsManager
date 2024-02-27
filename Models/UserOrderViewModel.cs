using HotelsManager.Models.Hotels;
using HotelsManager.Models.Orders;
using HotelsManager.Models.Users;
using HotelsManager.Models.Repository;

public class UserOrderViewModel
{
    public User user { get; set; }
    public IUserRepository ur {  get; set; }
    public IOrderRepository or {  get; set; }
}