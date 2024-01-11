

using JackSharp;
using JackSharp.Processing;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AudioTranscription.BusinessLogic;

public class AudioTranscriptionJackConnector {
    private static AudioTranscriptionJackConnector? _instance;
    public static AudioTranscriptionJackConnector Instance => _instance ??= new();

    private readonly Processor _processor;
    private string? _currentInput;
    private string? _currentOutput;
    private List<string>? _appInputs;
    private List<string>? _appOutputs;
    private ChunkGenerator chunkGenerator;
    private SnippetGenerator snippetGenerator;
    public int BufferSize {get; private set;}
    private AudioTranscriptionJackConnector() {
        _processor = new Processor("AudioTranscriptionEngine", 1, 1);
        chunkGenerator = new(3000, WaveFormat.CreateIeeeFloatWaveFormat(Configuration.Instance.SamplingRate, Configuration.Instance.ChannelCount));
        snippetGenerator = new(chunkGenerator);
    }

    private void AudioProcessing(ProcessBuffer buffer) {
        BufferSize = buffer.AudioIn[0].BufferSize;
        chunkGenerator.AddData(buffer.AudioIn[0].Audio.Take(buffer.AudioIn[0].BufferSize).SelectMany(f => BitConverter.GetBytes(f)).ToArray());
    }

    public void Initialize() {
        _processor.Start();
        snippetGenerator.SnippetReady += SnippetHandler;
        _processor.ProcessFunc += AudioProcessing;
        (_appInputs, _appOutputs) = _processor.GetAppAudioPorts();
    }

    private async void SnippetHandler(object? sender, SnippetReadyEventArgs e) {
        var response = await Api.Instance.Transcribe(new SnippetDTO{Data = e.Data});
        Trace.WriteLine($"response {response?.Language}: {response?.Text}");
    }

    public (List<string> systemInputs, List<string> systemOutputs) SystemAudioPorts() {
        return _processor.GetSystemAudioPorts();
    }

    public void DisconnectAll() {
        if(_currentInput != null)
            Disconnect(_currentInput, _appInputs[0]);
        if(_currentOutput != null)
            Disconnect(_appOutputs[0], _currentOutput);
    }

    public void Disconnect(string source, string destination) {
        _processor.Disconnect(source, destination);
        if (source == _currentInput) _currentInput = null;
        if (destination == _currentOutput) _currentOutput = null;
    }

    public void ConnectInput(string port) {
        if(_currentInput != null)
            Disconnect(_currentInput, _appInputs[0]);
        _processor.ConnectPort(port, _appInputs[0]);
    }

    public void ConnectOutput(string port) {

        if(_currentOutput != null)
            Disconnect(_appOutputs[0], _currentOutput);
        _processor.ConnectPort(_appOutputs[0], port);
    }


}