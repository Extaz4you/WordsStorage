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
        public async Task<ActionResult<List<Word>>> GetWords(int id, CancellationToken ct)
        {
            var words = await wordRepository.GetWordsFromTopic(id, ct);
            if (words is null)
            {
                logger.LogWarning("Тема {TopicId} не найдена", id);
                return NotFound();
            }

            logger.LogInformation("Получено {WordsCount} слов из темы {TopicId}", words.Count, id);
            return Ok(words);
        }

        [HttpGet("GetOneWordFromTopic")]
        public async Task<ActionResult<Word>> GetWord(string word, CancellationToken ct)
        {
            var result = await wordRepository.GetWordFromTopic(word, ct);
            if (result != null)
            {
                logger.LogInformation("Received te word: {Word} ", result.RussianVersion.ToUpper());
                return Ok(result);
            }
            else
            {
                logger.LogWarning("Didn't receive the word: {Word}", word.ToUpper());
                return NotFound();
            }
        }

        [HttpPost("AddWordToTopic")]
        public async Task<ActionResult<bool>> AddWord(Word word, CancellationToken ct)
        {
            if (await wordRepository.AddWordToTopic(word, ct))
            {
                logger.LogInformation("The word : {Word} was added ", word.RussianVersion.ToUpper());
                return Ok(true);
            }
            else
            {
                logger.LogWarning("The word : {Word} wasn't added ", word.RussianVersion.ToUpper());
                return Conflict();
            }
        }

        [HttpPost("AddWordsToTopic")]
        public async Task<ActionResult<bool>> AddWords(List<Word> words, int id, CancellationToken ct)
        {
            if (await wordRepository.AddWordsToTopic(words, id, ct))
            {
                logger.LogInformation("The words : {WordsCount} were added ", words.Count);
                return Ok(true);
            }
            else
            {
                logger.LogWarning("The word : {WordsCount} weren't added ", words.Count);
                return Conflict();
            }
        }

        [HttpDelete("DeleteWordFromTopic")]
        public async Task<ActionResult<bool>> DeleteWord(Word word, CancellationToken ct)
        {
            if (await wordRepository.DeleteWordFromTopic(word.TopicId, word.Id, ct))
            {
                logger.LogInformation("The word {Word} was deleted", word.RussianVersion.ToUpper());
                return Ok(true);
            }
            else
            {
                logger.LogWarning("The word {Word} wasn't deleted", word.RussianVersion.ToUpper());
                return NotFound();
            }
        }
    }
}
