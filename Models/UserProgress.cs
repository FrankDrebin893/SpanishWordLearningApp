using LiteDB;

namespace SpanishWordLearner.Models;

public class UserProgress
{
    [BsonId]
    public int Id { get; set; } = 1; // Single user, always ID 1

    public Dictionary<int, WordProgress> WordStats { get; set; } = new();
    public List<QuizSession> QuizHistory { get; set; } = new();

    [BsonIgnore]
    public int TotalWordsLearned => WordStats.Count(w => w.Value.IsLearned);

    [BsonIgnore]
    public int TotalQuizzesTaken => QuizHistory.Count;

    [BsonIgnore]
    public double OverallAccuracy => QuizHistory.Count > 0
        ? QuizHistory.Average(q => q.Accuracy)
        : 0;
}

public class WordProgress
{
    public int WordId { get; set; }
    public int TimesCorrect { get; set; }
    public int TimesIncorrect { get; set; }
    public int TimesReviewed { get; set; }
    public DateTime LastReviewed { get; set; }
    public DateTime FirstSeen { get; set; }

    [BsonIgnore]
    public bool IsLearned => TimesCorrect >= 3 && Accuracy >= 0.7;

    [BsonIgnore]
    public double Accuracy => TimesReviewed > 0
        ? (double)TimesCorrect / TimesReviewed
        : 0;
}

public class QuizSession
{
    public DateTime Date { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }

    [BsonIgnore]
    public double Accuracy => TotalQuestions > 0
        ? (double)CorrectAnswers / TotalQuestions
        : 0;

    public List<int> WordIdsTested { get; set; } = new();
}
