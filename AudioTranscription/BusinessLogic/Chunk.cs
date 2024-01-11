

using System;

namespace AudioTranscription.BusinessLogic;

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
