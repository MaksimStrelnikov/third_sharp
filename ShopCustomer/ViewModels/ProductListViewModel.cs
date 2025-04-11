using System.Collections.ObjectModel;
using System.Linq;
using ShopCustomer.Models;

namespace ShopCustomer.ViewModels;

public class ProductListViewModel
{
    public ObservableCollection<ProductViewModel> Products { get; }

    public ProductListViewModel(Shop shop)
    {
        Products = new ObservableCollection<ProductViewModel>(
            shop.Products.Select(p => new ProductViewModel(p.Key, p.Value)));

        shop.ProductOutOfStock += p =>
        {
            var item = Products.FirstOrDefault(vm => vm.Product == p);
            item?.Update();
        };
    }
}