using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordsStorage.Models;
using WordsStorage.Services;

namespace WordsStorage.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {

        private WordRepository wordRepository;
        private ILogger<WordController> logger;
        public WordController(WordRepository repository, ILogger<WordController> log)
        {
            wordRepository = repository;
            logger = log;
        }


        [HttpGet("GetAllWordsFromTopic")]
        public async Task<ActionResult<List<Word>>> GetWords(int id)
        {
            try
            {
                var words = await wordRepository.GetWordsFromTopic(id);
                if (words.Any())
                {
                    logger.LogInformation("Received {0} words", words.Count);
                    return Ok(words);
                }
                else
                {
                    logger.LogInformation("Received {0} words", words.Count);
                    return NotFound();
                }
            }
            catch
            {
                logger.LogError("Something went wrong whe the service tried to get words from the topic with {0} id", id);
                return BadRequest();
            }
        }

        [HttpGet("GetOneWordFromTopic")]
        public async Task<ActionResult<Word>> GetWord(string word)
        {
            try
            {
                var result = await wordRepository.GetWordFromTopic(word);
                if (result != null)
                {
                    logger.LogInformation("Received te word: {0} ", result.RussianVersion.ToUpper());
                    return Ok(result);
                }
                else
                {
                    logger.LogInformation("Didn't receive the word: {0}", word.ToUpper());
                    return NotFound();
                }
            }
            catch
            {
                logger.LogError("Something went wrong whe the service tried to get word");
                return BadRequest();
            }
        }

        [HttpPost("AddWordToTopic")]
        public async Task<ActionResult<bool>> AddWord(Word word)
        {
            try
            {
                if(await wordRepository.AddWordToTopic(word))
                {
                    logger.LogInformation("The word : {0} was added ", word.RussianVersion.ToUpper());
                    return Ok(true);
                }
                else
                {
                    logger.LogInformation("The word : {0} wasn't added ", word.RussianVersion.ToUpper());
                    return BadRequest();
                }
            }
            catch
            {
                logger.LogError("Something went wrong whe the service tried to add word {0} to topic with id {1}",
                    word.RussianVersion.ToUpper(), word.TopicId);
                return BadRequest();
            }
        }

        [HttpPost("AddWordsToTopic")]
        public async Task<ActionResult<bool>> AddWords(List<Word> words, int id)
        {
            try
            {
                if (await wordRepository.AddWordsToTopic(words, id))
                {
                    logger.LogInformation("The words : {0} were added ", words.Count);
                    return Ok(true);
                }
                else
                {
                    logger.LogInformation("The word : {0} weren't added ", words.Count);
                    return BadRequest();
                }
            }
            catch
            {
                logger.LogError("Something went wrong whe the service tried to add words to topic with {0} id ", id);
                return BadRequest();
            }
        }

        [HttpDelete("DeleteWordFromTopic")]
        public async Task<ActionResult<bool>> DeleteWord(Word word)
        {
            try
            {
                if (await wordRepository.DeleteWordFromTopic(word))
                {
                    logger.LogInformation("The word {0} was deleted", word.RussianVersion.ToUpper());
                    return Ok(true);
                }
                else
                {
                    logger.LogInformation("The word {0} wasn't deleted", word.RussianVersion.ToUpper());
                    return BadRequest();
                }
            }
            catch
            {
                logger.LogError("Something went wrong whe the service tried to delete word {0} from the topic with {1} id",
                    word.RussianVersion.ToUpper(), word.TopicId);
                return BadRequest();
            }
        }
    }
}
