using System;
using System.Collections.Generic;

namespace ShopCustomer.Models;

public class DeliveryService(string name) : IDeliverer
{
    public string Name { get; set; } = name;
    public readonly Dictionary<Shop, HashSet<Product>> Receivers = new();
    public event Action<Product, Shop, int> ProductDelivered;

    public void AddShop(Shop shop)
    {
        Receivers.Add(shop, new HashSet<Product>());
    }

    public void RemoveShop(Shop shop)
    {
        Receivers.Remove(shop);
    }

    public void AskToDeliver(Shop shop, Product product)
    {
        if (!Receivers.TryGetValue(shop, out var receiver)) throw new Exception($"Сервис {Name} не доставляет продукты в магазин {shop.Name}");
        receiver.Add(product);
    }
    
    public void AskToNotDeliver(Shop shop, Product product)
    {
        if (!Receivers.TryGetValue(shop, out var receiver)) throw new Exception($"Сервис {Name} не доставляет продукты в магазин {shop.Name}");
        receiver.Remove(product);
    }

    public bool DoesDeliverTo(Shop shop)
    {
        return Receivers.ContainsKey(shop);
    }
    
    public void Deliver(Product product, Shop shop, int quantity)
    {
        if (!Receivers.TryGetValue(shop, out var receiver))
        {
            throw new Exception($"Сервис {Name} не доставляет продукты в магазин {shop.Name}");
        }
        if (!receiver.Contains(product))
        {
            throw new Exception($"Сервис {Name} не доставляет продукт {product.Name} в магазин {shop.Name}");
        }
        ProductDelivered.Invoke(product, shop, quantity);
    }
}