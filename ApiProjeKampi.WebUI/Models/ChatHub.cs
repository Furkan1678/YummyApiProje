using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiProjeKampi.WebUI.Models
{
    public class ChatHub : Hub
    {
        private const string apikey = "AIzaSyDj5xTAh3R-Mxs6LpCml6bR0gzKtMXcl5k"; // API anahtarını buraya ekleyin
        private const string modelGmn = "gemini-2.0-flash";
        public readonly IHttpClientFactory _httpClientFactory;

        public ChatHub(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private static readonly Dictionary<string, List<object>> _history = new();

        public override Task OnConnectedAsync()
        {
            _history[Context.ConnectionId] = new List<object>();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _history.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userMessage)
        {
            await Clients.Caller.SendAsync("ReceiveUserEcho", userMessage);

            var history = _history[Context.ConnectionId];
            history.Add(new { role = "user", parts = new[] { new { text = userMessage } } });

            await streamGeminiAI(history, Context.ConnectionAborted);
        }

        public async Task streamGeminiAI(List<object> history, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            var endpointUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{modelGmn}:streamGenerateContent?key={apikey}";
            var payload = new
            {
                contents = history,
                generationConfig = new { temperature = 0.2 },
            };

            var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            using var req = new HttpRequestMessage(HttpMethod.Post, endpointUrl);
            req.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            resp.EnsureSuccessStatusCode();

            using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            var sb = new StringBuilder();

            try
            {
                while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // "data:" ile başlamayan satırları atla
                    if (!line.StartsWith("data:")) continue;

                    var data = line.Substring("data:".Length).Trim();

                    // Stream bitti sinyali
                    if (data == "[DONE]") break;

                    try
                    {
                        var chunk = JsonSerializer.Deserialize<GeminiStreamChunk>(
                            data,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                        );

                        var delta = chunk?.Candidates?.FirstOrDefault()
                                                ?.Content?.Parts?.FirstOrDefault()?.Text;

                        if (!string.IsNullOrEmpty(delta))
                        {
                            sb.Append(delta);
                            await Clients.Caller.SendAsync("ReceiveToken", delta, cancellationToken);
                        }
                    }
                    catch (JsonException jex)
                    {
                        await Clients.Caller.SendAsync("ReceiveToken", $"JSON ayrıştırma hatası: {jex.Message}", cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveToken", $"Bir hata oluştu: {ex.Message}", cancellationToken);
            }

            var full = sb.ToString();
            if (!string.IsNullOrEmpty(full))
            {
                history.Add(new { role = "model", parts = new[] { new { text = full } } });
            }

            await Clients.Caller.SendAsync("CompleteMessage", full, cancellationToken);
        }

    }

    public class GeminiStreamChunk
    {
        [JsonPropertyName("candidates")]
        public List<Candidate>? Candidates { get; set; }
    }

    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content? Content { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part>? Parts { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }
    }

    public class Part
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}