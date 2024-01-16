using Jack.NAudio;
using JackSharp;
using NAudio.Wave;
using ReactiveUI;
using System.Reactive;
using AudioTranscription.BusinessLogic;
using System.Diagnostics;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AudioTranscription.MVVM.ViewModels;

public class FileInputViewModel : AudioSourceViewModelBase {
    private string filePath = "../example.wav";
    private readonly Processor jackFileProcessor;
    private AudioFileReader fileReader;
    private AudioOut player;
    public ReactiveCommand<Unit, Unit> Play {get;}
    public ReactiveCommand<Unit, Unit> Pause {get;}
    public ReactiveCommand<Unit, Unit> Stop {get;}
    public ReactiveCommand<Unit, Unit> Load {get;}
    public string FilePath {
        get => filePath;
        set {
            this.RaiseAndSetIfChanged(ref filePath, value);
            player?.Dispose(false);
            fileReader?.Close();
            fileReader = new AudioFileReader(filePath);

            Trace.WriteLine($"Sample Rate: {fileReader.WaveFormat.SampleRate}");
            Trace.WriteLine($"Channels: {fileReader.WaveFormat.Channels}");
            Trace.WriteLine($"Bits per Sample: {fileReader.WaveFormat.BitsPerSample}");
            Trace.WriteLine($"Encoding: {fileReader.WaveFormat.Encoding}");
            player!.Init(fileReader);
            player.Play();
            player.Pause();
        }
    }
    public FileInputViewModel() {
        jackFileProcessor = new(filePath.Split('/')[^1], 0, 2);
        player = new AudioOut(jackFileProcessor);
        FilePath = "../example.wav";

        Connect = ReactiveCommand.Create(()=>{
            (_, var outs) = jackFileProcessor.GetAppAudioPorts();
            AudioTranscriptionJackConnector.Instance.ConnectInput(outs[0]);
        });
        Play = ReactiveCommand.Create(()=> player!.Play(), Observable.Return(player.PlaybackState).Select(p => p != PlaybackState.Playing));
        Pause = ReactiveCommand.Create(()=> player!.Pause(), Observable.Return(player.PlaybackState).Select(p => p == PlaybackState.Playing));
        Stop = ReactiveCommand.Create(()=> player!.Stop(), Observable.Return(player.PlaybackState).Select(p => p != PlaybackState.Stopped));
        Load = ReactiveCommand.CreateFromTask(async ()=>{
            var options = new FilePickerOpenOptions{
                AllowMultiple = false,
                FileTypeFilter = [
                    new FilePickerFileType("*.wav"),
                    new FilePickerFileType("*.ogg"),
                    new FilePickerFileType("*.mp3"),
                ],
                SuggestedStartLocation = await App.MainWindow?.StorageProvider?.TryGetWellKnownFolderAsync(WellKnownFolder.Documents),
                Title="Select Audio File"
            };
            Task<IReadOnlyList<IStorageFile>> failed = Task.Factory.StartNew<IReadOnlyList<IStorageFile>>(()=>[]); 
            var pickedFile = await (App.MainWindow?.StorageProvider?.OpenFilePickerAsync(options) ?? failed);
            if (pickedFile.Count > 0)
                FilePath = pickedFile[0].TryGetLocalPath() ?? "";
        });
    }
}
