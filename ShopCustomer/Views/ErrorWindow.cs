using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace ShopCustomer.Views;

public class ErrorWindow : Window
{
    public ErrorWindow(string message)
    {
        this.Width = 300;
        this.Height = 150;
        this.Title = "Ошибка";
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        var stackPanel = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var textBlock = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(10) };
        var button = new Button { Content = "OK", HorizontalAlignment = HorizontalAlignment.Center };
        button.Click += (sender, e) => this.Close();

        stackPanel.Children.Add(textBlock);
        stackPanel.Children.Add(button);

        this.Content = stackPanel;
    }
}