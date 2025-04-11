using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using ShopCustomer.Models;
using ShopCustomer.Views;

namespace ShopCustomer.ViewModels;

public class AddCustomerViewModel : ViewModelBase
{
    public string CustomerName { get; set; }
    
    public ObservableCollection<Shop> Shops { get; set; }
    
    private Shop _selectedShop;

    public Shop SelectedShop
    {
        get => _selectedShop;
        set
        {
            _selectedShop = value;
            _products.Clear();
            foreach (var product in value.Products.Keys)
            {
                _products.Add(product);
            }
            OnPropertyChanged(nameof(Products));
        }
    }

    public ObservableCollection<Product> Products
    {
        get => _products;
        set => _products = value;
    }

    private ObservableCollection<Product> _products = new ();
    public Product SelectedProduct { get; set; }

    public ICommand ConfirmCommand { get; }

    private readonly Window _mainWindow;
    private readonly Window _thisWindow;

    public AddCustomerViewModel(Window mainWindow, Window thisWindow)
    {
        _mainWindow = mainWindow;
        _thisWindow = thisWindow;
        Shops = (_mainWindow.DataContext as MainWindowViewModel).Shops;
        ConfirmCommand = new RelayCommand(_ => AddCustomer());
    }

    private void ToObservableCollection(IEnumerable<Product> products)
    {
        Products = new ObservableCollection<Product>(products);
    }
    
    private void AddCustomer()
    {
        if (!string.IsNullOrWhiteSpace(CustomerName))
        {
            var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
            if (mainWindowViewModel?.Customers.Any(customer => customer.Name.Equals(CustomerName, StringComparison.OrdinalIgnoreCase)) == true)
            {
                var errorWindow = new ErrorWindow($"Покупатель с именем {CustomerName} уже существует.");
                errorWindow.ShowDialog(_mainWindow);
                return;
            }

            if (SelectedShop == null || SelectedProduct == null)
            {
                var errorWindow = new ErrorWindow("Не выбран магазин и/или продукт для покупки");
                errorWindow.ShowDialog(_mainWindow);
                return;
            }

            var temp = new Customer(CustomerName, SelectedProduct, SelectedShop);
            temp.ProductBought += (name, p, s, q) =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                    mainWindowViewModel.Log.Add(
                        $"[{DateTime.Now:T}] {name} купил(а): {p.Name} в количестве {q} шт. в магазине {s.Name}"));
            };
            mainWindowViewModel.Customers.Add(temp);
            temp.StartShopping();
            
            _thisWindow.Close();
        }
        else
        {
            var errorWindow = new ErrorWindow("Имя не может быть пустым");
            errorWindow.ShowDialog(_mainWindow);
        }
    }
}