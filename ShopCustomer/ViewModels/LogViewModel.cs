using System;
using System.Collections.ObjectModel;

namespace ShopCustomer.ViewModels;

public class LogViewModel : ViewModelBase
{
    public ObservableCollection<string> Messages { get; } = new();

    public void AddMessage(string msg)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            Messages.Add($"[{DateTime.Now:T}] {msg}"));
    }
}