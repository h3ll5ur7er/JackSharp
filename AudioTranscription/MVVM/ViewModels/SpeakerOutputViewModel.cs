using AudioTranscription.BusinessLogic;
using ReactiveUI;

namespace AudioTranscription.MVVM.ViewModels;

public class SpeakerOutputViewModel : AudioDrainViewModelBase {
    private string port = "";

    public string Port {
        get => port;
        set => this.RaiseAndSetIfChanged(ref port, value);
    }
    

    public SpeakerOutputViewModel() {
        Connect = ReactiveCommand.Create(()=>{
            AudioTranscriptionJackConnector.Instance.ConnectOutput(Port);
        });
    }
}
