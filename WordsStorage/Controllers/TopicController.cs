using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordsStorage.Models;
using WordsStorage.Services;

namespace WordsStorage.Controllers;

[Route("[controller]")]
[ApiController]
public class TopicController : ControllerBase
{
    private TopicRepository topicRepository;
    private ILogger<TopicController> logger;
    public TopicController(TopicRepository repository, ILogger<TopicController> loggerService)
    {
        topicRepository = repository;
        logger = loggerService;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<Topic>>> GetAllTopics(CancellationToken ct)
    {
        var fulllist = await topicRepository.GetAllTopics(ct);
        logger.LogInformation("Service received : {TopicsCount} topics", fulllist.Count);
        return Ok(fulllist);
    }

    [HttpGet("Get")]
    public async Task<ActionResult> GetTopic(int id, CancellationToken ct)
    {
        var topic = await topicRepository.GetTopic(id, ct);
        if (topic != null)
        {
            logger.LogInformation("Topic was gotten {TopicId} id", id);
            return Ok(topic);
        }
        else
        {
            logger.LogWarning("Topic wasn't gotten {TopicId} id", id);
            return NotFound();
        }
    }

    [HttpPost("Add")]
    public async Task<ActionResult<bool>> AddTopic(Topic topic, CancellationToken ct)
    {
        if (await topicRepository.AddTopic(topic, ct))
        {
            logger.LogInformation("The topic was added with {TopicId} id ", topic.Id);
            return Ok(true);
        }
        else
        {
            logger.LogWarning("The topic wasn't added with {TopicId} id ", topic.Id);
            return Conflict();
        }
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult<bool>> DeleteTopic(int id, CancellationToken ct)
    {
        if (await topicRepository.DeleteTopic(id, ct))
        {
            logger.LogInformation("The topic was deleted with {TopicId} id ", id);
            return Ok(true);
        }
        else
        {
            logger.LogWarning("The topic wasn't deleted with {TopicId} id ", id);
            return NotFound();
        }
    }
}
