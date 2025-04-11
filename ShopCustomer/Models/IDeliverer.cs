namespace ShopCustomer.Models;

public interface IDeliverer
{
    public void Deliver(Product product, Shop shop, int quantity);
}