using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using ShopCustomer.Models;
using ShopCustomer.Views;

namespace ShopCustomer.ViewModels;

public class AddDeliveryServiceViewModel : ViewModelBase
{
    public string DeliveryServiceName { get; set; }
    public ObservableCollection<Product>? Products { get; }
    public Product? SelectedProduct { get; set; }
    public ObservableCollection<Shop>? Shops { get; }
    public Shop? SelectedShop { get; set; }

    public ICommand ConfirmCommand { get; }

    private readonly Window _mainWindow;
    private readonly Window _thisWindow;

    public AddDeliveryServiceViewModel(Window mainWindow, Window thisWindow)
    {
        _mainWindow = mainWindow;
        _thisWindow = thisWindow;
        Products = (_mainWindow.DataContext as MainWindowViewModel)?.GlobalProducts;
        Shops = (_mainWindow.DataContext as MainWindowViewModel)?.Shops;
        ConfirmCommand = new RelayCommand(_ => AddDeliveryService());
    }

    private void AddDeliveryService()
    {
        if (!string.IsNullOrWhiteSpace(DeliveryServiceName))
        {
            var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
            if (SelectedShop == null || SelectedProduct == null)
            {
                var errorWindow = new ErrorWindow("Не выбран магазин и/или продукт для доставки");
                errorWindow.ShowDialog(_mainWindow);
                return;
            }

            if (!SelectedShop.SellsProduct(SelectedProduct))
            {
                SelectedShop.AddProduct(SelectedProduct);
            }

            DeliveryService temp;

            if (mainWindowViewModel?.Deliveries.Any(deliverer =>
                    deliverer.Name.Equals(DeliveryServiceName, StringComparison.OrdinalIgnoreCase)) == true)
            {
                temp = mainWindowViewModel.Deliveries.First(
                    deliverer => deliverer.Name.Equals(DeliveryServiceName, StringComparison.OrdinalIgnoreCase)
                );
                if (!temp.Receivers.ContainsKey(SelectedShop))
                {
                    temp.AddShop(SelectedShop);
                    SelectedShop.ProductOutOfStock += product =>
                    {
                        temp.Deliver(product, SelectedShop, Random.Shared.Next(10, 40));
                        Dispatcher.UIThread.InvokeAsync(() =>
                            mainWindowViewModel.Log.Add(
                                $"[{DateTime.Now:T}] {DeliveryServiceName} доставил в магазин {SelectedShop.Name} {30} шт. товара {product.Name}."));
                    };
                }

                temp.AskToDeliver(SelectedShop, SelectedProduct);
            }
            else
            {
                temp = new DeliveryService(DeliveryServiceName);

                temp.AddShop(SelectedShop);
                SelectedShop.ProductOutOfStock += product =>
                {
                    temp.Deliver(product, SelectedShop, Random.Shared.Next(10, 40));
                    Dispatcher.UIThread.InvokeAsync(() =>
                        mainWindowViewModel.Log.Add(
                            $"[{DateTime.Now:T}] {DeliveryServiceName} доставил в магазин {SelectedShop.Name} {30} шт. товара {product.Name}."));
                };
                temp.AskToDeliver(SelectedShop, SelectedProduct);
                mainWindowViewModel?.Deliveries.Add(temp);
            }

            temp.ProductDelivered += (product, shop, quantity) =>
            {
                shop.DeliverProduct(product, quantity);
                mainWindowViewModel.Log.Add(
                    $"[{DateTime.Now:T}] Доставлен товар {product.Name} в магазин {shop.Name} в количестве {quantity} шт.");
            };

            _thisWindow.Close();
        }
        else
        {
            var errorWindow = new ErrorWindow("Имя не может быть пустым");
            errorWindow.ShowDialog(_mainWindow);
        }
    }
}