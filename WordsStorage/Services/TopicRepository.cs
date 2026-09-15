using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
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

    public async Task<List<Topic>> GetAllTopics()
    {
        return await StorageContext.Topics.ToListAsync();
    }
    public async Task<Topic> GetTopic(int id)
    {
        return await StorageContext.Topics.FirstOrDefaultAsync(x => x.Id == id) ?? new Topic();
    }
    public async Task<bool> AddTopic(Topic topic)
    {
        try
        {
            var result = await StorageContext.Topics.FirstOrDefaultAsync(x => x.TopicName == topic.TopicName);
            if(result == null)
            {
                topic.Words = new();
                await StorageContext.AddAsync(topic);
                await StorageContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> DeleteTopic(int id)
    {
        try
        {
            var result = await StorageContext.Topics.FirstOrDefaultAsync(x=>x.Id == id);
            if(result != null)
            {
                StorageContext.Topics.Remove(result);
                await StorageContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
