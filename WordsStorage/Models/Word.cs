namespace WordsStorage.Models;

public class Word
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public string RussianVersion { get; set; }
    public string EnglishVersion { get; set; }
}
