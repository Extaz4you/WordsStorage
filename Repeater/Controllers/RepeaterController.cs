using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repeater.Models;
using Repeater.Services;
using WordsStorage.Models;

namespace Repeater.Controllers;

[Route("[controller]")]
[ApiController]
public class RepeaterController : ControllerBase
{
    private readonly TopicService service;
    public RepeaterController(TopicService topicService)
    {
        service = topicService;
    }

    [HttpGet("All_Topic")]
    public async Task<ActionResult<List<TopicDto>>> ListOfTopics()
    {
        var list = await service.GetAllTopics();
        if (list.Any())
        {
            //log
            return Ok(list);
        }
        else
        {
            //log
            return NoContent();
        } 
    }

    [HttpGet("One_Topic")]
    public async Task<ActionResult<TopicDto>> Topic(int id)
    {
        var topic = await service.GetTopic(id);
        if (topic != null)
        {
            //log
            return Ok(topic);
        }
        else
        {
            //log
            return NoContent();
        }
    }

    [HttpPost("Add_Topic")]
    public async Task<ActionResult<bool>> AddTopic(TopicDto dto)
    {
        var result = await service.AddTopic(dto);
        if (result)
        {
            //log
            return Ok(true);
        }
        else
        {
            //log
            return NoContent();
        }
    }

    [HttpDelete("Delete_Topic")]
    public async Task<ActionResult<bool>> DeleteTopic(int id)
    {
        var result = await service.DeleteTopic(id);
        if (result)
        {
            //log
            return Ok(result);
        }
        else
        {
            //log
            return BadRequest();
        }
    }
}
