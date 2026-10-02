using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repeater.Models;
using Repeater.Services;
using System.Net.WebSockets;
using WordsStorage.Models;

namespace Repeater.Controllers;

[Route("[controller]")]
[ApiController]
public class RepeaterController : ControllerBase
{
    private readonly TopicService topic_service;
    private readonly WordService word_service;
    public RepeaterController(TopicService topicService, WordService wordService)
    {
        topic_service = topicService;
        word_service = wordService;
    }

    [HttpGet("All_Topic")]
    public async Task<ActionResult<List<TopicDto>>> ListOfTopics()
    {
        var list = await topic_service.GetAllTopics();
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
        var topic = await topic_service.GetTopic(id);
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
        var result = await topic_service.AddTopic(dto);
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
        var result = await topic_service.DeleteTopic(id);
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

    [HttpGet("One_word")]
    public async Task<ActionResult<WordDto>> GetWord(string name)
    {
        var word = await word_service.GetWord(name);
        if(word != null)
        {
            //log
            return Ok(word);
        }
        else
        {
            //log
            return NotFound();
        }
    }

    [HttpGet("All_words")]
    public async Task<ActionResult<WordDto>> GetWords(int id)
    {
        var word = await word_service.GetWords(id);
        if (word != null)
        {
            //log
            return Ok(word);
        }
        else
        {
            //log
            return NotFound();
        }
    }
}
