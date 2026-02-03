namespace SpanishWordLearner.Models;

public class SpanishWord
{
    public int Id { get; set; }
    public required string Spanish { get; set; }
    public required string English { get; set; }
    public required string PartOfSpeech { get; set; }
    public int FrequencyRank { get; set; }
    public string? ExampleSentence { get; set; }
    public string? ExampleTranslation { get; set; }
}
