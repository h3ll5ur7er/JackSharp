

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AudioTranscription.BusinessLogic;

public class Api: ApiBase {
    private static Api? _instance;
    public static Api Instance => _instance ??= new();

    private Api() : base(Configuration.Instance.BaseURL) { }
    public async Task<TranscribeResponseDTO?> Transcribe(SnippetDTO snippet) {

        var query = new QueryParameters { { "id", snippet.SnippetId }, { "language", "auto" } };
        return await PostSnipppet("/transcribe", snippet.Data, "snippet.wav", query);
    }
    protected async Task<TResponse?> PostBatch<TResponse>(string relativePath, IEnumerable<byte[]> data, string fileName, QueryParameters? query = null) {
        using var stream = new MemoryStream();
        foreach (var entry in data) {
            stream.Write(entry);
        }
        stream.Position = 0;

        return await PostStream<TResponse?>(relativePath, stream, fileName, query);
    }
    protected async Task<TranscribeResponseDTO?> PostSnipppet(string relativePath, byte[] data, string fileName, QueryParameters? query = null) {
        using var stream = new MemoryStream(data);

        return await PostStream<TranscribeResponseDTO>(relativePath, stream, fileName, query);
    }
}

public class QueryParameters: Dictionary<string, object> {
    public string ToQueryString() {
        var query = string.Join("&", this.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        if (query.Length > 0) {
            query = "?" + query;
        }
        return query;
    }
}

public class TranscribeResponseDTO {
    [JsonPropertyName("text")]
    public string Text { get; set; } = "";
    [JsonPropertyName("language")]
    public string Language { get; set; } = "";
}

public class ApiBase(string baseURL) {
    public string BaseURL => baseURL;

    protected readonly HttpClient _www = new();

    protected async Task<TResponse?> Post<TRequest, TResponse>(string relativePath, TRequest request, QueryParameters? query = null) {
        var response = await _www.PostAsJsonAsync(BaseURL + relativePath + (query?.ToQueryString() ?? ""), request);
        if (response.StatusCode != HttpStatusCode.OK) {
            throw new Exception($"Api call failed: ({response.StatusCode}) {response.ReasonPhrase}");
        }
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
    protected async Task<TResponse?> PostFile<TResponse>(string relativePath, FileInfo file, QueryParameters? query = null) {
        using var stream = file.OpenRead();

        return await PostStream<TResponse>(relativePath, stream, file.Name, query);
    }
    protected async Task<TResponse?> PostStream<TResponse>(string relativePath, Stream stream, string fileName, QueryParameters? query = null) {
        var content = new MultipartFormDataContent { { new StreamContent(stream), "file", fileName } };
        var response = await _www.PostAsync(BaseURL + relativePath + (query?.ToQueryString() ?? ""), content);
        if (response.StatusCode != HttpStatusCode.OK) {
            throw new Exception($"Api call failed: ({response.StatusCode}) {response.ReasonPhrase}");
        }
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}

public class BatchDTO {
    public Guid BatchId { get; set; }
    public SnippetDTO[] Batches { get; set; } = [];
}
public class SnippetDTO {
    public Guid SnippetId { get; set; }
    public byte[] Data { get; set; } = [];
}

