namespace WordsStorage.Models;

public class Topic
{
    public int Id { get; set; }
    public string TopicName {  get; set; }
    public string TopicDescription { get; set; }
    public List<Word> Words { get; set; } = new List<Word>();

}
