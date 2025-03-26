using MoonScore.DataConstants;
using System.Windows.Controls;

namespace MoonScore.Views;

/// <summary>
/// Interaktionslogik für APIScreen.xaml
/// </summary>
public partial class APIScreen : Page
{
    public APIScreen()
    {
        InitializeComponent();

        DateTextBox.Text = InitData.GetGamesInitDate();
    }
}
