using Microsoft.Extensions.DependencyInjection;
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

namespace MoonScore.Views
{
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
}
