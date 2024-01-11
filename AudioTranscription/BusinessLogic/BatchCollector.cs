

using System;
using System.Collections.Generic;

namespace AudioTranscription.BusinessLogic;

public class BatchCollector(int maxBatchSize) {
    private readonly Queue<byte[]> batches = new();

    public void AddBatch(byte[] batch) {
        batches.Enqueue(batch);
    }

    public byte[][] GetBatch() {
        var entries = Math.Min(maxBatchSize, batches.Count);
        var batch = new byte[entries][];
        for (var i = 0; i < entries; i++) {
            batch[i] = batches.Dequeue();
        }
        return batch;
    }
    public byte[]? GetSnippet() {
        if (batches.Count == 0) return null;
        return batches.Dequeue();
    }

}