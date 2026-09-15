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
    public async Task<ActionResult<IEnumerable<Topic>>> GetAllTopics()
    {
        var fulllist = await topicRepository.GetAllTopics();
        logger.LogInformation("Service received : {0} topics", fulllist.Count);
        return Ok(fulllist);
    }

    [HttpGet("Get")]
    public async Task<ActionResult> GetTopic(int id)
    {
        try
        {
            var topic = await topicRepository.GetTopic(id);
            if(topic != null)
            {
                logger.LogInformation("Topic was gotten {0} id", id);
                return Ok(topic);
            }
            else
            {
                logger.LogWarning("Topic wasn't gotten {0} id", id);
                return BadRequest();
            }
        }
        catch
        {
            logger.LogError("Something went wrong when service was trying to get a topic with {0} id", id);
            return BadRequest();
        }
    }

    [HttpPost("Add")]
    public async Task<ActionResult<bool>> AddTopic(Topic topic)
    {
        try
        {
            if(await topicRepository.AddTopic(topic))
            {
                logger.LogInformation("The topic was added with {0} id ", topic.Id);
                return Ok(true);
            }
            else
            {
                logger.LogWarning("The topic wasn't added with {0} id ", topic.Id);
                return BadRequest();
            }
        }
        catch 
        {
            logger.LogError("Something went wrong when service was trying to add a topic with {0} id", topic.Id);
            return BadRequest();
        }
    }

    [HttpDelete("Delete")]
    public async Task<ActionResult<bool>> DeleteTopic(int id)
    {
        try
        {
            if (await topicRepository.DeleteTopic(id))
            {
                logger.LogInformation("The topic was deleted with {0} id ", id);
                return Ok(true);
            }
            else
            {
                logger.LogWarning("The topic wasn't deleted with {0} id ", id);
                return NotFound();
            }
        }
        catch 
        {
            logger.LogError("Something went wrong when service was trying to delete a topic with {0} id", id);
            return BadRequest();
        }
    }
}
