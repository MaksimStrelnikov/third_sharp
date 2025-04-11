using System;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using ShopCustomer.Models;
using ShopCustomer.Views;

namespace ShopCustomer.ViewModels;

public class AddProductViewModel : ViewModelBase
{
    public string ProductName { get; set; }

    public ICommand ConfirmCommand { get; }

    private readonly Window _mainWindow;
    private readonly Window _thisWindow;

    public AddProductViewModel(Window mainWindow, Window thisWindow)
    {
        _mainWindow = mainWindow;
        _thisWindow = thisWindow;
        ConfirmCommand = new RelayCommand(_ => AddProduct());
    }
    
    private void AddProduct()
    {
        if (!string.IsNullOrWhiteSpace(ProductName))
        {
            var mainWindowViewModel = _mainWindow.DataContext as MainWindowViewModel;
            if (mainWindowViewModel.GlobalProducts.Any(product => product.Name.Equals(ProductName, StringComparison.OrdinalIgnoreCase)) == true)
            {
                var errorWindow = new ErrorWindow($"Продукт с названием {ProductName} уже существует.");
                errorWindow.ShowDialog(_mainWindow);
                return;
            }
            var product = new Product(ProductName);
            mainWindowViewModel.GlobalProducts.Add(product);
            
            _thisWindow.Close();
        }
        else
        {
            var errorWindow = new ErrorWindow("Имя не может быть пустым");
            errorWindow.ShowDialog(_mainWindow);
        }
    }
}