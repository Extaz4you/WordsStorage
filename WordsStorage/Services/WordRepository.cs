using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Word>?> GetWordsFromTopic(int id, CancellationToken ct)
    {
        var topic = await storageContext.Topics
            .AsNoTracking()
            .Include(x => x.Words)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return topic?.Words; 
    }

    public async Task<Word?> GetWordFromTopic(string wordNeeded, CancellationToken ct)
    {
        var word = await storageContext.Topics.AsNoTracking()
                                               .SelectMany(x=>x.Words)
                                               .FirstOrDefaultAsync(r=>r.EnglishVersion == wordNeeded || r.RussianVersion == wordNeeded, ct);
        if (word == null) return null;
        return word;
  
    }

    public async Task<bool> AddWordToTopic(Word word, CancellationToken ct)
    {
        var topic = await storageContext.Topics.FirstOrDefaultAsync(x=>x.Id == word.TopicId,ct);
        if(topic == null || topic.Words == null) return false;
        topic.Words.Add(word);
        await storageContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> AddWordsToTopic(List<Word> words, int topicId, CancellationToken ct)
    {
        var topic = await storageContext.Topics.FirstOrDefaultAsync(x => x.Id == topicId,ct);
        if (topic == null || topic.Words == null) return false;

        foreach (var word in words)
        {
            word.TopicId = topicId;
        }

        topic.Words.AddRange(words);
        await storageContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteWordFromTopic(int TopicId, int WordId, CancellationToken ct)
    {
        var word = storageContext.Words.FirstOrDefault(x => x.Id == WordId);
        if (word == null) return false;
        storageContext.Words.Remove(word);
        await storageContext.SaveChangesAsync(ct);
        return true;
    }
}
