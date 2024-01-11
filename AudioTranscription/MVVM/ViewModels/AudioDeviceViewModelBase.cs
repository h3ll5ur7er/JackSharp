using JackSharp;

namespace AudioTranscription.MVVM.ViewModels;

public class AudioDeviceViewModelBase : ViewModelBase {
    public Processor? Processor { get; }
}
