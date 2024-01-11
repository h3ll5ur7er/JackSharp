using ReactiveUI;

namespace AudioTranscription.MVVM.ViewModels;

public class FileOutputViewModel : AudioDrainViewModelBase {
    private string filePath = "";

    public string FilePath {
        get => filePath;
        set => this.RaiseAndSetIfChanged(ref filePath, value);
    }
}
