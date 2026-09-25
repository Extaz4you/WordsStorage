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
    [HttpGet]
    public async Task<ActionResult<List<TopicDto>>> ListOfTopics()
    {
        return Ok(await service.GetAllTopics());
    }
}
