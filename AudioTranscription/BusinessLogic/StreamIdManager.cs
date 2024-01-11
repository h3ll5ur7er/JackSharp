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
