using Microsoft.EntityFrameworkCore;
using WordsStorage.Database;
using WordsStorage.Models;

namespace WordsStorage.Services;

public class TopicRepository
{
    private StorageContext StorageContext;
    public TopicRepository(StorageContext storage)
    {
        StorageContext = storage;
    }

    public async Task<List<Topic>> GetAllTopics(CancellationToken ct)
    {
        return await StorageContext.Topics.AsNoTracking().ToListAsync(ct);
    }
    public async Task<Topic?> GetTopic(int id, CancellationToken ct)
    {
        return await StorageContext.Topics.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }
    public async Task<bool> AddTopic(Topic topic, CancellationToken ct)
    {
        var result = await StorageContext.Topics.FirstOrDefaultAsync(x => x.TopicName == topic.TopicName, ct);
        if (result == null)
        {
            topic.Words = new();
            await StorageContext.AddAsync(topic, ct);
            await StorageContext.SaveChangesAsync(ct);
            return true;
        }
        return false;
    }
    public async Task<bool> DeleteTopic(int id, CancellationToken ct)
    {
        var result = await StorageContext.Topics.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (result != null)
        {
            StorageContext.Topics.Remove(result);
            await StorageContext.SaveChangesAsync(ct);
            return true;
        }
        return false;
    }
}
