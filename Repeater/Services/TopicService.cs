using Repeater.Models;
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
        if(list != null && list.Any())
        {
           return list.Select(c=>new TopicDto(c.TopicName, c.TopicDescription)).ToList();
        }
        return new List<TopicDto>();
    }
}
