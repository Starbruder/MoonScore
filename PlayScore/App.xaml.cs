using Microsoft.Extensions.DependencyInjection;
using MoonScore.Services;
using System.Configuration;
using System.Data.SQLite;
using System.Windows;

namespace MoonScore;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Set up the DI container
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        // Build the service provider
        ServiceProvider = serviceCollection.BuildServiceProvider();

        // Resolve and show MainWindow
        var startScreen = ServiceProvider.GetRequiredService<StartScreen>();
        startScreen.Show();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register dependencies
        string connectionString = ConfigurationManager.ConnectionStrings["SQLiteConnection"].ConnectionString;

        services.AddSingleton(new SQLiteConnection(connectionString));

        //Register Services
        services.AddSingleton<DatabaseManager>();
        services.AddSingleton<MoonphaseService>();
        services.AddSingleton<MoonphaseTranslationService>();
        services.AddSingleton<GameService>();

        // Register Windows/Screens
        services.AddSingleton<StartScreen>();
        services.AddSingleton<MainWindow>();
    }
}
