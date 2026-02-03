namespace SpanishWordLearner.Models;

public class UserProgress
{
    public Dictionary<int, WordProgress> WordStats { get; set; } = new();
    public List<QuizSession> QuizHistory { get; set; } = new();
    public int TotalWordsLearned => WordStats.Count(w => w.Value.IsLearned);
    public int TotalQuizzesTaken => QuizHistory.Count;
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
    public bool IsLearned => TimesCorrect >= 3 && Accuracy >= 0.7;
    public double Accuracy => TimesReviewed > 0
        ? (double)TimesCorrect / TimesReviewed
        : 0;
}

public class QuizSession
{
    public DateTime Date { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double Accuracy => TotalQuestions > 0
        ? (double)CorrectAnswers / TotalQuestions
        : 0;
    public List<int> WordIdsTested { get; set; } = new();
}
