using Avalonia.Collections;
using JackSharp;
using ReactiveUI;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using AudioTranscription.BusinessLogic;
using System.Collections.Generic;

namespace AudioTranscription.MVVM.ViewModels;

public class DesignMainWindowViewModel : ReactiveObject {

    public AvaloniaList<AudioSourceViewModelBase> AudioSources => [
        new MicInputViewModel { Port = "system:capture_1" },
        new MicInputViewModel { Port = "system:capture_2" },
        new FileInputViewModel { FilePath = "test.wav" }
    ];

    public AvaloniaList<AudioDrainViewModelBase> AudioOutputs => [
        new SpeakerOutputViewModel { Port = "system:playback_1" },
        new SpeakerOutputViewModel { Port = "system:playback_2" },
        new FileOutputViewModel { FilePath = "test.wav" }
    ];
}

public class MainWindowViewModel : ReactiveObject {
    // Output chunk size in seconds
    private int chunkSizeSeconds = 10;
    public int ChunkSizeSeconds { get => chunkSizeSeconds; set => this.RaiseAndSetIfChanged(ref chunkSizeSeconds, value); }

    // List of audio sources
    private AvaloniaList<AudioSourceViewModelBase> audioSources = new();
    public AvaloniaList<AudioSourceViewModelBase> AudioSources { get => audioSources; set => this.RaiseAndSetIfChanged(ref audioSources, value); }

    // List of audio outputs
    private AvaloniaList<AudioDrainViewModelBase> audioOutputs = new();
    public AvaloniaList<AudioDrainViewModelBase> AudioOutputs { get => audioOutputs; set => this.RaiseAndSetIfChanged(ref audioOutputs, value); }

    public ReactiveCommand<string, Unit> AddFileInput { get; }


    public MainWindowViewModel() {
        AddFileInput = ReactiveCommand.Create<string>(path => AudioSources.Add(new FileInputViewModel { FilePath = path }));

        Trace.WriteLine("Initializing Jack");
        (var systemInputs, var systemOutputs) = AudioTranscriptionJackConnector.Instance.SystemAudioPorts();
        AudioSources = new AvaloniaList<AudioSourceViewModelBase>(systemOutputs.Select(port => new MicInputViewModel { Port = port })) {
            new FileInputViewModel()
        };
        AudioOutputs = new AvaloniaList<AudioDrainViewModelBase>(systemInputs.Select(port => new SpeakerOutputViewModel { Port = port })) {
            new FileOutputViewModel(),
            new NoOutputViewModel()
        };
        Trace.WriteLine("Jack Initialized");
    }

}
