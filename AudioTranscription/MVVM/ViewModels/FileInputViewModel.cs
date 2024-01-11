using ReactiveUI;

namespace AudioTranscription.MVVM.ViewModels;

public class FileInputViewModel : AudioSourceViewModelBase {
    private string filePath = "";

    public string FilePath {
        get => filePath;
        set => this.RaiseAndSetIfChanged(ref filePath, value);
    }
    public FileInputViewModel() {
        Connect = ReactiveCommand.Create(()=>{
            //TODO
        });
    }
}
