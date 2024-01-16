using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AudioTranscription.MVVM.Views;
using AudioTranscription.MVVM.ViewModels;

namespace AudioTranscription;

public partial class App : Application {
    private static MainWindow? mainWindow;

    public static MainWindow? MainWindow => mainWindow;

    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            mainWindow = new MainWindow {
                DataContext = new MainWindowViewModel()
            };
            desktop.MainWindow = MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}