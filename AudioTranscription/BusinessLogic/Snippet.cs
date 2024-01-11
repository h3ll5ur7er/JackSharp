

using System;
using System.Collections.Generic;

namespace AudioTranscription.BusinessLogic;

public class Snippet {
    public Guid Id { get; } = Guid.NewGuid();
    public List<Chunk> Chunks { get; } = new();
    public TimeSpan StartTime => Chunks[0].StartTime;
    public TimeSpan EndTime => Chunks[^1].EndTime;
    public Snippet(IEnumerable<Chunk> chunks) {
        Chunks.AddRange(chunks);
    }
}
