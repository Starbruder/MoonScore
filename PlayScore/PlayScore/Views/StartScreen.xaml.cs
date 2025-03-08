using Microsoft.Extensions.DependencyInjection;
using PlayScore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlayScore;

/// <summary>
/// Interaktionslogik für StartScreen.xaml
/// </summary>
public partial class StartScreen : Window
{
    private readonly IServiceProvider _serviceProvider;

    public StartScreen(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;

        WindowState = WindowState.Maximized;
    }

    private void StartProgram_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
        Close();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}
