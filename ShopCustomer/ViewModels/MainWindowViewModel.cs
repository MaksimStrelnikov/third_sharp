using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using ShopCustomer.Models;
using ShopCustomer.Views;

namespace ShopCustomer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<Shop> Shops { get; } = new();
    public ObservableCollection<Customer> Customers { get; } = new();
    public ObservableCollection<DeliveryService> Deliveries { get; } = new();
    public ObservableCollection<Product>? GlobalProducts { get; } = new();
    public ObservableCollection<string> Log { get; } = new();
    
    private readonly Window _mainWindow;
    
    public ICommand OpenAddShopWindowCommand { get; }
    public ICommand AddCustomerCommand { get; }
    public ICommand OpenAddDeliveryWindowCommand { get; }
    public ICommand OpenAddProductWindowCommand { get; }

    public MainWindowViewModel(Window mainWindow)
    {
        _mainWindow = mainWindow;
        
        OpenAddShopWindowCommand = new RelayCommand(_ => OpenAddShopWindow());
        AddCustomerCommand = new RelayCommand(_ => OpenAddCustomerWindow());
        OpenAddDeliveryWindowCommand = new RelayCommand(_ => OpenAddDeliveryWindow());
        OpenAddProductWindowCommand = new RelayCommand(_ => OpenAddProductWindow());
    }
    
    private async void OpenAddShopWindow()
    {
        var addShopWindow = new AddShopWindow();
        var addShopViewModel = new AddShopViewModel(_mainWindow, addShopWindow);
        addShopWindow.DataContext = addShopViewModel;

        await addShopWindow.ShowDialog(_mainWindow);
    }
    
    private async void OpenAddProductWindow()
    {
        var addProductWindow = new AddProductWindow();
        var addProductViewModel = new AddProductViewModel(_mainWindow, addProductWindow);
        addProductWindow.DataContext = addProductViewModel;

        await addProductWindow.ShowDialog(_mainWindow);
    }
    
    private async void OpenAddCustomerWindow()
    {
        var addCustomerWindow = new AddCustomerWindow();
        var addCustomerViewModel = new AddCustomerViewModel(_mainWindow, addCustomerWindow);
        addCustomerWindow.DataContext = addCustomerViewModel;

        await addCustomerWindow.ShowDialog(_mainWindow);
    }
    
    private async void OpenAddDeliveryWindow()
    {
        var addDeliveryWindow = new AddDeliveryServiceWindow();
        var addDeliveryViewModel = new AddDeliveryServiceViewModel(_mainWindow, addDeliveryWindow);
        addDeliveryWindow.DataContext = addDeliveryViewModel;

        await addDeliveryWindow.ShowDialog(_mainWindow);
    }

    /*private void AddShop()
    {
        var shop = new Shop();
        Shops.Add(shop);

        foreach (var customer in Customers)
            customer.StartShopping(shop);

        foreach (var delivery in Deliveries)
        {
            shop.ProductOutOfStock += p =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Log.Add($"[{DateTime.Now:T}] Товар закончился: {p.Name}");
                });
            };
        }

        foreach (var product in GlobalProducts)
            shop.AddProduct(product.Clone());
    }*/

    /*private void AddCustomer()
    {
        var customer = new Customer("Покупатель " + (Customers.Count + 1));
        Customers.Add(customer);

        foreach (var shop in Shops)
            customer.StartShopping(shop);

        customer.ProductBought += (name, p, s) =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Log.Add($"[{DateTime.Now:T}] {name} купил(а): {p.Name} в магазине {s.Name}");
            });
        };
    }*/

    /*private void AddDelivery()
    {
        var delivery = new DeliveryService();
        Deliveries.Add(delivery);

        delivery.ProductDelivered += (p, s, q) =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Log.Add($"[{DateTime.Now:T}] Доставлен товар {p.Name} в магазин {s.Name} в количестве {q} шт.");
            });
        };
    }*/

    /*private void AddProduct()
    {
        var product = new Product("Продукт " + (GlobalProducts.Count + 1), 10);
        GlobalProducts.Add(product);

        foreach (var shop in Shops)
            shop.AddProduct(product.Clone());
    }*/
}

