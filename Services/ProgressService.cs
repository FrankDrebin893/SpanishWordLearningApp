using Microsoft.JSInterop;
using SpanishWordLearner.Models;
using System.Text.Json;

namespace SpanishWordLearner.Services;

public class ProgressService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "spanish_word_progress";
    private UserProgress? _cachedProgress;

    public ProgressService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<UserProgress> GetProgressAsync()
    {
        if (_cachedProgress != null)
            return _cachedProgress;

        try
        {
            var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(json))
            {
                _cachedProgress = JsonSerializer.Deserialize<UserProgress>(json) ?? new UserProgress();
            }
            else
            {
                _cachedProgress = new UserProgress();
            }
        }
        catch
        {
            _cachedProgress = new UserProgress();
        }

        return _cachedProgress;
    }

    public async Task SaveProgressAsync(UserProgress progress)
    {
        _cachedProgress = progress;
        var json = JsonSerializer.Serialize(progress);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
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

    public async Task ResetProgressAsync()
    {
        _cachedProgress = new UserProgress();
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }
}
