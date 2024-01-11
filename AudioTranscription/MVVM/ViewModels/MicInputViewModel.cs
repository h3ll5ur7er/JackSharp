using ReactiveUI;

namespace AudioTranscription.MVVM.ViewModels;

public class MicInputViewModel : AudioSourceViewModelBase {
    private string port = "";

    public string Port {
        get => port;
        set => this.RaiseAndSetIfChanged(ref port, value);
    }
}
