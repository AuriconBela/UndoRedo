using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UndoRedo.Repository;
using UndoRedo.ViewModel;

namespace UndoRedo;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;
    public App()
    {
        ServiceCollection services = new();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }
    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IDataRepository, DataRepository>();
        services.AddSingleton<IMainViewModel, MainViewModel>(sp =>
        {
            var repo = sp.GetRequiredService<IDataRepository>();
            return new MainViewModel(repo);
        });
        services.AddSingleton(sp =>
        {
            var vm = sp.GetRequiredService<IMainViewModel>();
            return new MainWindow(vm);
        });
    }
    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = _serviceProvider.GetService<MainWindow>();
        mainWindow?.Show();
    }
}
