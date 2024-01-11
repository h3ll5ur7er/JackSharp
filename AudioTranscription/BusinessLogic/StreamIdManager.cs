

using System;
using System.Collections.Generic;

namespace AudioTranscription.BusinessLogic;

public class StreamIdManager {
    private static StreamIdManager? _instance;
    public static StreamIdManager Instance => _instance ??= new();
    // input stream has an id
    // each audio chunk (emitted by a stream) has an id
    // each audio snippet (composed of multiple chunks) has an id
    // each batch of snippets (composed of multiple snippets) has an id
    // each api response can be separated by batch id
    // each batched response can be separated, aligned and merged with the correct stream id

    // stream id -> batch id -> snippet id
    private readonly Dictionary<Guid, Guid> Streams = new();
    private readonly Dictionary<Guid, Guid> Batches = new();
    private readonly Dictionary<Guid, Guid> Snippets = new();

    

    private StreamIdManager() {

    }

}

public class AudioInputStream { }
public class Snippet {
    public Guid Id { get; } = Guid.NewGuid();
    public List<Chunk> Chunks { get; } = new();
    public TimeSpan StartTime => Chunks[0].StartTime;
    public TimeSpan EndTime => Chunks[^1].EndTime;
    public Snippet(IEnumerable<Chunk> chunks) {
        Chunks.AddRange(chunks);
    }
}
public class Chunk {
    public Guid Id { get; } = Guid.NewGuid();
    public byte[] Data { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }
    public Chunk(byte[] data, TimeSpan startTime, TimeSpan endTime) {
        Data = data;
        StartTime = startTime;
        EndTime = endTime;
    }
}
