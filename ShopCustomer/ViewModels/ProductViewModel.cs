using ShopCustomer.Models;

namespace ShopCustomer.ViewModels;

public class ProductViewModel(Product product, int quantity) : ViewModelBase
{
    public Product Product { get; } = product;

    public string Display => $"{Product.Name}: {quantity} шт.";

    public void Update()
    {
        OnPropertyChanged(nameof(Display));
    }
}