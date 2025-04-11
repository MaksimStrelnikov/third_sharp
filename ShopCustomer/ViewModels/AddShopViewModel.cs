using System;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ShopCustomer.Models;
using ShopCustomer.Views;

namespace ShopCustomer.ViewModels;

public class AddShopViewModel : ViewModelBase
{
    private string _shopName;

    public string ShopName
    {
        get => _shopName;
        set
        {
            if (_shopName != value)
            {
                _shopName = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand AddShopCommand { get; }

    private readonly Window _mainWindow;
    private readonly Window _thisWindow;

    public AddShopViewModel(Window mainWindow, Window thisWindow)
    {
        _mainWindow = mainWindow;
        _thisWindow = thisWindow;
        AddShopCommand = new RelayCommand(_ => AddShop());
    }

    private void AddShop()
    {
        if (!string.IsNullOrWhiteSpace(ShopName))
        {
            var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
            if (mainWindowViewModel?.Shops.Any(shop =>
                    shop.Name.Equals(ShopName, StringComparison.OrdinalIgnoreCase)) == true)
            {
                var errorWindow = new ErrorWindow($"Магазин с названием {ShopName} уже существует.");
                errorWindow.ShowDialog(_mainWindow);
                return;
            }

            var shop = new Shop(ShopName);
            mainWindowViewModel?.Shops.Add(shop);
            shop.ProductOutOfStock += p =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    mainWindowViewModel.Log.Add(
                        $"[{DateTime.Now:T}] Товар {p.Name} в магазине {shop.Name} закончился.");
                });
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