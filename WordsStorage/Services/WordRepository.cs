using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;
using WordsStorage.Database;
using WordsStorage.Models;

namespace WordsStorage.Services;

public class WordRepository
{
    private StorageContext storageContext;
    public WordRepository(StorageContext storage)
    {
        storageContext = storage;
    }

    public async Task<List<Word>> GetWordsFromTopic(int id)
    {
        var topic = await storageContext.Topics
            .Include(x => x.Words)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (topic == null) return new List<Word>();
        return topic.Words ?? new List<Word>();
    }

    public async Task<Word> GetWordFromTopic(string word)
    {
        var topic = await storageContext.Topics.SelectMany(x=>x.Words)
                                               .FirstOrDefaultAsync(r=>r.EnglishVersion == word || r.RussianVersion == word);
        if (topic == null) return new();
        return topic;
  
    }

    public async Task<bool> AddWordToTopic(Word word)
    {
        var topic = await storageContext.Topics.FirstOrDefaultAsync(x=>x.Id == word.TopicId);
        if(topic == null || topic.Words == null) return false;
        topic.Words.Add(word);
        await storageContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddWordsToTopic(List<Word> words, int topicId)
    {
        var topic = await storageContext.Topics.FirstOrDefaultAsync(x => x.Id == topicId);
        if (topic == null || topic.Words == null) return false;

        foreach (var word in words)
        {
            word.TopicId = topicId;
        }

        topic.Words.AddRange(words);
        await storageContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteWordFromTopic(Word word)
    {
        var topic = await storageContext.Topics.FirstOrDefaultAsync(x => x.Id == word.TopicId);
        if (topic == null) return false;
        topic.Words.Remove(word);
        await storageContext.SaveChangesAsync();
        return true;
    }
}
