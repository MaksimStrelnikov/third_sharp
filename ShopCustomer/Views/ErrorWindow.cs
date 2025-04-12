using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        TransparencyLevelHint = new List<WindowTransparencyLevel>
        {
            WindowTransparencyLevel.AcrylicBlur
        };
        Background = Brushes.Transparent;
        CanResize = false;

        var stackPanel = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 10
        };

        var textBlock = new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(5), FontSize = 16, FontWeight = FontWeight.Bold};
        var button = new Button { Content = "OK", HorizontalAlignment = HorizontalAlignment.Center, FontSize = 16, FontWeight = FontWeight.DemiBold };
        button.Click += (sender, e) => this.Close();

        stackPanel.Children.Add(textBlock);
        stackPanel.Children.Add(button);

        this.Content = stackPanel;
    }
}