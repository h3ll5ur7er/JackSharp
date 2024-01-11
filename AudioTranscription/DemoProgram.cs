using System;
using Jack.NAudio;
using JackSharp;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
class DemoProgramm {
#pragma warning disable IDE0051 // Remove unused private members
    static void DemoMain(string[] _) {
        Console.WriteLine("Recording");

        using var clientIn = new Processor("AudioTranscription", 2, 0, 0, 0, true);
        using var clientOut = new Processor("AudioTranscription", 0, 2, 0, 0, true);
        using var audioIn = new AudioIn(clientIn);
        using var audioOut = new AudioOut(clientOut);

        var waveOutFile = new WaveFileWriter("test.wav", WaveFormat.CreateIeeeFloatWaveFormat(44100, 2));

        audioIn.DataAvailable += (s, e) => {
            waveOutFile.Write(e.Buffer, 0, e.BytesRecorded);
        };
        audioIn.RecordingStopped += (s, e) => {
            Console.WriteLine("Recording Stopped");
            waveOutFile.Flush();
            waveOutFile.Dispose();
        };
        audioIn.StartRecording();
        Console.WriteLine("Press any key to stop recording");
        Console.ReadKey();
        audioIn.StopRecording();

        ///////////////////////////////////////////////
        Console.WriteLine("Press any key to start playback");
        Console.ReadKey();

        Console.WriteLine("Playback");

        var wavInFile = new AudioFileReader("example.wav");
        Console.WriteLine($"Sample Rate: {wavInFile.WaveFormat.SampleRate}");
        Console.WriteLine($"Channels: {wavInFile.WaveFormat.Channels}");
        Console.WriteLine($"Bits per Sample: {wavInFile.WaveFormat.BitsPerSample}");
        Console.WriteLine($"Encoding: {wavInFile.WaveFormat.Encoding}");
        audioOut.Init(wavInFile);
        audioOut.Play();
        Console.WriteLine("Press any key to stop playback");
        Console.ReadKey();
        audioOut.Stop();
    }
#pragma warning restore IDE0051 // Remove unused private members
}
