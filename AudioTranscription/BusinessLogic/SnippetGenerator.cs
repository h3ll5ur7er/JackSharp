
using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AudioTranscription.BusinessLogic;

public class SnippetReadyEventArgs(TimeSpan startTime, TimeSpan endTime, byte[] data) {
    public Chunk Chunk => new(data, startTime, endTime);
}

public delegate void SnippetReadyEventHandler(object sender, SnippetReadyEventArgs e);


/// <summary>
/// Aggregates data from an audio stream and emits it to a subscriber when the data is ready.
/// </summary>
public class ChunkGenerator(int snippetLengthInMs, WaveFormat format) {
    private readonly MemoryStream _stream = new();
    private readonly int _maxSize = format.AverageBytesPerSecond * snippetLengthInMs / 1000;

    public event SnippetReadyEventHandler? ChunkReady;

    public void AddData(byte[] data) {
        _stream.Write(data);
        if (_stream.Length >= _maxSize) {
            ChunkReady?.Invoke(this, new(
                startTime: TimeSpan.FromSeconds(_stream.Position / (double)format.AverageBytesPerSecond), //TODO: this is not accurate
                endTime: TimeSpan.FromSeconds(_stream.Length / (double)format.AverageBytesPerSecond),     //TODO: this is not accurate
                data: _stream.ToArray()
            ));
            _stream.SetLength(0);
        }
    }

    public void AddData(byte[] data, int offset, int count) {
        _stream.Write(data, offset, count);
    }
}

public class SnippetGenerator {
    private readonly Queue<Chunk> _chunks = new();

    public event SnippetReadyEventHandler? SnippetReady;

    public SnippetGenerator(ChunkGenerator chunkGen) {
        chunkGen.ChunkReady += (sender, e) => {
            _chunks.Enqueue(e.Chunk);
            if (_chunks.Count >= 5) {
                EmitNext();
            }
        };
    }

    private void EmitNext() {
        var size = Configuration.Instance.ChunkSizeSeconds * Configuration.Instance.SnippetChunkCount * Configuration.Instance.BytesPerSecond;
        var snippet = new byte[size];
        var bounds = _chunks.Take(5).Select((chunk, i) => { Buffer.BlockCopy(chunk.Data, 0, snippet, i*snippet.Length, snippet.Length); return (chunk.StartTime, chunk.EndTime); }).ToList();
        _chunks.Dequeue();

      
        SnippetReady?.Invoke(this, new(
            startTime: bounds[0].StartTime,
            endTime: bounds[^1].EndTime,
            data: snippet
        ));
    }
}
