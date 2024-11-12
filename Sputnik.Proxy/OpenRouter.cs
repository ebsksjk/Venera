using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Sputnik.Proxy
{
    internal class OpenRouter
    {
        private static readonly HttpClient client = new HttpClient();

        public static async IAsyncEnumerable<string> Prompt(string prompt)
        {
            string systemPrompt = File.ReadAllText("Prompts\\Conversation.txt")
                .Replace(Environment.NewLine, " ")
                .Replace("\"", "\\\"");

            StringContent content = new StringContent(
                "{\"stream\": true, \"model\":\"mistralai/mixtral-8x7b-instruct\",\"messages\":[" +
                    "{\"role\":\"system\",\"content\":\"" + systemPrompt + "\"}," +
                    "{\"role\":\"user\",\"content\":\"" + prompt + "\"}]}",
                Encoding.UTF8,
                "application/json"
            );

            HttpRequestMessage request;

            try
            {
                request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://openrouter.ai/api/v1/chat/completions"),
                    Headers =
                {
                    { "Authorization", $"Bearer {Program.OR_API_KEY}" },
                    { "Accept", "text/event-stream" }
                },
                    Content = content
                };
            }
            catch (Exception)
            {
                yield break;
            }

            HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            // Read the response stream continuously for SSE
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();

                    // Process the line as an SSE event if it's not empty
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        if (line.StartsWith("data:"))
                        {
                            // Strip the "data:" prefix
                            string json = line.Substring(5).Trim();

                            JObject chatResponse = JObject.Parse("{}");
                            try
                            {
                                // Deserialize the JSON data to ChatResponse object
                                chatResponse = JObject.Parse(json);
                            }
                            catch (JsonReaderException)
                            {
                                yield break;
                            }

                            yield return (string)chatResponse["choices"][0]["delta"]["content"];
                        }
                    }
                }
            }
        }
    }
}
