using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;

using JackSharp;
using AudioTranscription.BusinessLogic;

namespace AudioTranscription;

class Program {
     [STAThread] public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            //.WithPlayground()
            .LoadConfig()
            .InitializeJack()
            .UsePlatformDetect()
            .WithInterFont()
            .UseReactiveUI()
            .LogToTrace();
}

public static class AppBuilderExtensions {
    public static AppBuilder LoadConfig(this AppBuilder builder) {
        Configuration.Load("config.json");
        return builder;
    }
    public static AppBuilder WithPlayground(this AppBuilder builder) {

        using var clientIn = new Processor("AudioTranscription", 2, 2, 0, 0, false);
        clientIn.Start();
        (var sI, var sO) = clientIn.GetSystemAudioPorts();
        (var aI, var aO) = clientIn.GetAppAudioPorts();
        foreach (var port in sI) {
            Console.WriteLine($"SystemInput: {port}");
        }
        foreach (var port in sO) {
            Console.WriteLine($"SystemOutput: {port}");
        }
        foreach (var port in aI) {
            Console.WriteLine($"AppInput: {port}");
        }
        foreach (var port in aO) {
            Console.WriteLine($"AppOutput: {port}");
        }

        return builder;
    }
    public static AppBuilder InitializeJack(this AppBuilder builder) {

        AudioTranscriptionJackConnector.Instance.Initialize();

        return builder;
    }
    
}
