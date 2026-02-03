using SpanishWordLearner.Models;
using System.Text.RegularExpressions;

namespace SpanishWordLearner.Services;

public class DataLoader
{
    public static List<SpanishWord> LoadFromFiles(string frequencyPath, string dictionaryPath)
    {
        var translations = LoadTranslations(dictionaryPath);
        var words = LoadFrequencyList(frequencyPath, translations);
        return words;
    }

    private static Dictionary<string, List<TranslationEntry>> LoadTranslations(string path)
    {
        var translations = new Dictionary<string, List<TranslationEntry>>(StringComparer.OrdinalIgnoreCase);

        if (!File.Exists(path))
            return translations;

        var content = File.ReadAllText(path);
        var entries = content.Split("_____", StringSplitOptions.RemoveEmptyEntries);

        foreach (var entry in entries)
        {
            var lines = entry.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) continue;

            var word = lines[0].Trim();
            if (string.IsNullOrEmpty(word) || word.StartsWith('-') || word.StartsWith('*'))
                continue;

            string? currentPos = null;
            var glosses = new List<string>();

            foreach (var line in lines.Skip(1))
            {
                var trimmed = line.TrimStart();
                if (trimmed.StartsWith("pos:"))
                {
                    // Save previous entry if we have one
                    if (currentPos != null && glosses.Count > 0)
                    {
                        if (!translations.TryGetValue(word, out var list))
                        {
                            list = new List<TranslationEntry>();
                            translations[word] = list;
                        }
                        list.Add(new TranslationEntry { Pos = currentPos, Glosses = new List<string>(glosses) });
                    }

                    currentPos = trimmed.Substring(4).Trim();
                    glosses.Clear();
                }
                else if (trimmed.StartsWith("gloss:"))
                {
                    var gloss = trimmed.Substring(6).Trim();
                    // Clean up the gloss - remove wiki markup
                    gloss = CleanGloss(gloss);
                    if (!string.IsNullOrEmpty(gloss))
                        glosses.Add(gloss);
                }
            }

            // Save last entry
            if (currentPos != null && glosses.Count > 0)
            {
                if (!translations.TryGetValue(word, out var list))
                {
                    list = new List<TranslationEntry>();
                    translations[word] = list;
                }
                list.Add(new TranslationEntry { Pos = currentPos, Glosses = new List<string>(glosses) });
            }
        }

        return translations;
    }

    private static string CleanGloss(string gloss)
    {
        // Remove common wiki-style formatting
        gloss = Regex.Replace(gloss, @"\[\[([^\]|]+)\|([^\]]+)\]\]", "$2");
        gloss = Regex.Replace(gloss, @"\[\[([^\]]+)\]\]", "$1");
        gloss = Regex.Replace(gloss, @"\{\{[^}]+\}\}", "");
        gloss = Regex.Replace(gloss, @"<[^>]+>", "");
        gloss = gloss.Trim();

        // Skip glosses that are just form references
        if (gloss.StartsWith("inflection of") ||
            gloss.StartsWith("form of") ||
            gloss.StartsWith("obsolete form") ||
            gloss.StartsWith("pronunciation spelling"))
            return "";

        return gloss;
    }

    private static List<SpanishWord> LoadFrequencyList(string path, Dictionary<string, List<TranslationEntry>> translations)
    {
        var words = new List<SpanishWord>();

        if (!File.Exists(path))
            return words;

        var lines = File.ReadAllLines(path);
        int rank = 0;
        int id = 0;

        foreach (var line in lines.Skip(1)) // Skip header
        {
            var parts = line.Split(',');
            if (parts.Length < 3) continue;

            var spanish = parts[1].Trim();
            var pos = parts[2].Trim();

            // Skip entries with flags indicating they're duplicates or have no usage
            if (parts.Length > 3 && (parts[3].Contains("DUPLICATE") || parts[3].Contains("NOUSAGE")))
                continue;

            rank++;

            // Try to get translation
            string english = "";
            if (translations.TryGetValue(spanish, out var entries))
            {
                // Try to find matching POS
                var matchingEntry = entries.FirstOrDefault(e =>
                    PosMatches(e.Pos, pos)) ?? entries.FirstOrDefault();

                if (matchingEntry != null && matchingEntry.Glosses.Count > 0)
                {
                    // Take first 2-3 glosses
                    english = string.Join("; ", matchingEntry.Glosses.Take(3));
                }
            }

            if (string.IsNullOrEmpty(english))
                continue; // Skip words without translations

            id++;
            words.Add(new SpanishWord
            {
                Id = id,
                Spanish = spanish,
                English = TruncateTranslation(english),
                PartOfSpeech = MapPartOfSpeech(pos),
                FrequencyRank = rank
            });

            // Limit to first 1000 words for performance
            if (id >= 1000)
                break;
        }

        return words;
    }

    private static bool PosMatches(string dictPos, string freqPos)
    {
        var normalizedDict = dictPos.ToLower();
        var normalizedFreq = freqPos.ToLower();

        return normalizedDict switch
        {
            "v" => normalizedFreq == "v",
            "noun" => normalizedFreq == "n" || normalizedFreq == "noun",
            "adj" => normalizedFreq == "adj" || normalizedFreq == "adjective",
            "adv" => normalizedFreq == "adv" || normalizedFreq == "adverb",
            "prep" => normalizedFreq == "prep" || normalizedFreq == "preposition",
            "conj" => normalizedFreq == "conj" || normalizedFreq == "conjunction",
            "pron" => normalizedFreq == "pron" || normalizedFreq == "pronoun",
            "det" => normalizedFreq == "determiner" || normalizedFreq == "det",
            _ => normalizedDict.StartsWith(normalizedFreq) || normalizedFreq.StartsWith(normalizedDict)
        };
    }

    private static string MapPartOfSpeech(string pos)
    {
        return pos.ToLower() switch
        {
            "v" => "verb",
            "n" => "noun",
            "adj" => "adjective",
            "adv" => "adverb",
            "prep" => "preposition",
            "conj" => "conjunction",
            "pron" => "pronoun",
            "art" => "article",
            "num" => "number",
            "determiner" => "determiner",
            "contraction" => "contraction",
            "none" => "particle",
            _ => pos
        };
    }

    private static string TruncateTranslation(string translation)
    {
        // Keep translations reasonable length for display
        if (translation.Length <= 80)
            return translation;

        var truncated = translation.Substring(0, 77);
        var lastSemi = truncated.LastIndexOf(';');
        if (lastSemi > 40)
            return truncated.Substring(0, lastSemi);

        return truncated + "...";
    }

    private class TranslationEntry
    {
        public string Pos { get; set; } = "";
        public List<string> Glosses { get; set; } = new();
    }
}
