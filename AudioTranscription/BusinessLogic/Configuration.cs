

using System;
using System.IO;
using System.Text.Json;

namespace AudioTranscription.BusinessLogic;

public class Configuration {
    private static Configuration? _instance;
    public static Configuration Instance => _instance ??= new();
    private Configuration() { }


    public int ChunkSizeSeconds { get; set; } = 3;
    public int SnippetChunkCount { get; set; } = 5;
    public int BytesPerSecond { get; set; } = 5;
    public int ChannelCount { get; set; } = 5;
    public int SamplingRate { get; set; } = 16000; // 16kHz
    public string BaseURL { get; set; } = "http://localhost:8000";


    public static void Load(string path) {
        return;
        try {
            _instance = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(path));
        } catch (Exception) {
            _instance = new();
        }
    }
    public static void Save(string path) {
        File.WriteAllText(path, JsonSerializer.Serialize(_instance));
    }
}