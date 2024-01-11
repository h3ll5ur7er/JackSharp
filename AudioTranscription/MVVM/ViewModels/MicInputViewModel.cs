using AudioTranscription.BusinessLogic;
using ReactiveUI;

namespace AudioTranscription.MVVM.ViewModels;

public class MicInputViewModel : AudioSourceViewModelBase {
    private string port = "";

    public string Port {
        get => port;
        set => this.RaiseAndSetIfChanged(ref port, value);
    }
    public MicInputViewModel() {
        Connect = ReactiveCommand.Create(()=>{
            AudioTranscriptionJackConnector.Instance.ConnectInput(Port);
        });
    }
}
