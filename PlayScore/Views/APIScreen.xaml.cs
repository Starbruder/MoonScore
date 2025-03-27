using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MoonScore.Views;

/// <summary>
/// Interaktionslogik für APIScreen.xaml
/// </summary>
public sealed partial class APIScreen : Window
{
    private readonly IServiceProvider _serviceProvider;

    public APIScreen(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
    }

    private void OpenMainWindow(object sender, RoutedEventArgs e)
    {
        _serviceProvider.GetRequiredService<MainWindow>().Show();
        this.Hide();
    }
}
