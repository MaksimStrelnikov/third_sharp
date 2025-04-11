using System;
using System.Threading;

namespace ShopCustomer.Models;

public class Customer(string name, Product product, Shop shop)
{
    private static readonly Random Random = new();
    
    public Product Product { get; set; } = product;
    public Shop Shop { get; set; } = shop;

    public string Name { get; } = name;

    public event Action<string, Product, Shop, int>? ProductBought;

    public void StartShopping()
    {
        new Thread(() =>
        {
            while (true)
            {
                Thread.Sleep(Random.Next(1000, 5000));
                var temp = Random.Next(1, 5);
                if (Product != null && Shop.SellProduct(Product, temp))
                {
                    ProductBought?.Invoke(Name, Product, Shop, temp);
                }
            }
        })
        {
            IsBackground = true
        }.Start();
    }
}