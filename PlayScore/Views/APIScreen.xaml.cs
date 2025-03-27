using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace MoonScore.Views;

/// <summary>
/// Interaktionslogik für APIScreen.xaml
/// </summary>
public partial class APIScreen : Window
{
    private readonly IServiceProvider _serviceProvider;

    public APIScreen(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;
    }

    private void OpenMainWindow(object sender, RoutedEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        this.Hide();
    }
}
