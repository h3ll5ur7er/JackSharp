using JackSharp;
using ReactiveUI;
using System.Reactive;

namespace AudioTranscription.MVVM.ViewModels;

public class AudioDeviceViewModelBase : ViewModelBase {
    public ReactiveCommand<Unit, Unit> Connect {get; protected set;}
}
