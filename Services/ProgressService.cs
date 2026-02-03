using LiteDB;
using SpanishWordLearner.Models;

namespace SpanishWordLearner.Services;

public class ProgressService : IDisposable
{
    private readonly LiteDatabase _db;
    private readonly ILiteCollection<UserProgress> _progressCollection;
    private UserProgress? _cachedProgress;

    public string DatabasePath { get; }

    public ProgressService()
    {
        DatabasePath = GetDatabasePath();

        // Ensure directory exists
        var directory = Path.GetDirectoryName(DatabasePath)!;
        Directory.CreateDirectory(directory);

        _db = new LiteDatabase(DatabasePath);
        _progressCollection = _db.GetCollection<UserProgress>("progress");
    }

    private static string GetDatabasePath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "SpanishWordLearner");
        return Path.Combine(appFolder, "progress.db");
    }

    public Task<UserProgress> GetProgressAsync()
    {
        if (_cachedProgress != null)
            return Task.FromResult(_cachedProgress);

        _cachedProgress = _progressCollection.FindById(1) ?? new UserProgress();
        return Task.FromResult(_cachedProgress);
    }

    public Task SaveProgressAsync(UserProgress progress)
    {
        _cachedProgress = progress;
        _progressCollection.Upsert(progress);
        return Task.CompletedTask;
    }

    public async Task RecordAnswerAsync(int wordId, bool correct)
    {
        var progress = await GetProgressAsync();

        if (!progress.WordStats.TryGetValue(wordId, out var wordProgress))
        {
            wordProgress = new WordProgress
            {
                WordId = wordId,
                FirstSeen = DateTime.UtcNow
            };
            progress.WordStats[wordId] = wordProgress;
        }

        wordProgress.TimesReviewed++;
        if (correct)
            wordProgress.TimesCorrect++;
        else
            wordProgress.TimesIncorrect++;
        wordProgress.LastReviewed = DateTime.UtcNow;

        await SaveProgressAsync(progress);
    }

    public async Task RecordQuizSessionAsync(QuizSession session)
    {
        var progress = await GetProgressAsync();
        progress.QuizHistory.Add(session);
        await SaveProgressAsync(progress);
    }

    public Task ResetProgressAsync()
    {
        _cachedProgress = new UserProgress();
        _progressCollection.Delete(1);
        _progressCollection.Insert(_cachedProgress);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
