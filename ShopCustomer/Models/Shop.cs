using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopCustomer.Models;

public class Shop(string name)
{
    public Dictionary<Product, int> Products { get; } = new();
    public string Name { get; set; } = name;

    public event Action<Product>? ProductOutOfStock;

    public void AddProduct(Product p) => Products.Add(p, 0);

    private readonly Random _random = new();

    public Product? GetRandomProduct()
    {
        lock (Products)
        {
            var available = Products.Where((p, q) => q > 0).ToList();
            if (available.Count == 0)
                return null;

            var index = _random.Next(available.Count);
            return available[index].Key;
        }
    }

    public bool SellProduct(Product product, int count)
    {
        lock (Products)
        {
            if (Products[product] < count)
            {
                ProductOutOfStock?.Invoke(product);
            }

            return true;
        }
    }
    
    public bool SellsProduct(Product product)
    {
        lock (Products)
        {
            return Products.ContainsKey(product);
        }
    }

    public void DeliverProduct(Product product, int quantity)
    {
        lock (Products)
        {
            Products[product] += quantity;
        }
    }
}