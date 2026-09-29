using Repeater.Models;
using System.Text;
using System.Text.Json;
using WordsStorage.Models;

namespace Repeater.Services;

public class TopicService
{
    private readonly HttpClient client;
    public TopicService(HttpClient httpClient)
    {
        client = httpClient;
    }
    public async Task<List<TopicDto>> GetAllTopics()
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));

        var list = await client.GetFromJsonAsync<List<Topic>>("GetAll", cts.Token);
        if (list != null && list.Any())
        {
            return list.Select(c => new TopicDto(c.TopicName, c.TopicDescription)).ToList();
        }
        return new List<TopicDto>();
    }
    public async Task<TopicDto?> GetTopic(int id)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));

        var topic = await client.GetFromJsonAsync<Topic>($"Get?id={id}", cts.Token);
        if (topic != null)
        {
            return new TopicDto(topic.TopicName, topic.TopicDescription);
        }
        return null;
    }
    public async Task<bool> AddTopic(TopicDto topic)
    {
        var newTopic = new Topic()
        {
            TopicName = topic.TopicName,
            TopicDescription = topic.TopicDescription,
        };

        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
        var rm = new HttpRequestMessage(HttpMethod.Post, "Add") 
        { 
            Content = new StringContent(JsonSerializer.Serialize(newTopic), Encoding.UTF8, "application/json") 
        };
        var result = await client.SendAsync(rm, cts.Token);

        return await result.Content.ReadFromJsonAsync<bool>(cts.Token);
    }
    public async Task<bool> DeleteTopic(int id)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
        var rm = new HttpRequestMessage(HttpMethod.Delete, $"Delete?id={id}");
        var result = await client.SendAsync(rm, cts.Token);

        return await result.Content.ReadFromJsonAsync<bool>(cts.Token);
    }
}
