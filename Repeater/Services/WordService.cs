using Repeater.Models;
using System.Text;
using System.Text.Json;
using WordsStorage.Models;

namespace Repeater.Services;

public class WordService
{
    private HttpClient client;
    public WordService(HttpClient httpClient)
    {
        client = httpClient;
    }

    public async Task<WordDto?> GetWord(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
        var response = await client.GetAsync($"GetOneWordFromTopic?word={name}", cts.Token);
        if (!response.IsSuccessStatusCode) return null;
        var word = await response.Content.ReadFromJsonAsync<Word>(cts.Token);
        if (word != null) return new WordDto(word.RussianVersion, word.EnglishVersion);
        return null;
    }

    public async Task<List<WordDto>?> GetWords(int id)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
        var response = await client.GetAsync($"GetAllWordsFromTopic?id={id}", cts.Token);
        if (!response.IsSuccessStatusCode) return null;
        var words = await response.Content.ReadFromJsonAsync<List<Word>>(cts.Token);
        if(words == null) return null;
        return words.Select(x=>new WordDto(x.RussianVersion, x.EnglishVersion)).ToList();
    }

    public async Task<bool> AddWord(string word)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
        var newWord = JsonSerializer.Deserialize<Word>(word);
        if(newWord != null)
        {
            var rm = new HttpRequestMessage(HttpMethod.Post, "AddWordToTopic")
            {
                Content = new StringContent(JsonSerializer.Serialize(newWord), Encoding.UTF8, "application/json")
            };
            var result = await client.SendAsync(rm, cts.Token);

            return await result.Content.ReadFromJsonAsync<bool>(cts.Token);
        }
        return false;
    }

    public async Task<bool> AddWords(string words)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
        var newWords = JsonSerializer.Deserialize<List<Word>>(words);
        if (newWords != null)
        {
            var rm = new HttpRequestMessage(HttpMethod.Post, "AddWordsToTopic")
            {
                Content = new StringContent(JsonSerializer.Serialize(newWords), Encoding.UTF8, "application/json")
            };
            var result = await client.SendAsync(rm, cts.Token);

            return await result.Content.ReadFromJsonAsync<bool>(cts.Token);
        }
        return false;
    }
}
