# Spanish Word Learner

A Blazor web application for learning the most common Spanish words through flashcards and quizzes.

## Features

### Learn Mode
- Flashcard-style learning with Spanish words shown first
- Click to reveal English translations
- Example sentences with translations for context
- Track whether you knew each word
- Progress through words ordered by frequency (most common first)

### Quiz Mode
- Multiple choice quizzes with 4 options
- Configurable quiz length (5, 10, 20, or 50 questions)
- Three quiz types:
  - Spanish to English
  - English to Spanish
  - Mixed (random direction)
- Immediate feedback on answers
- Review all answers at the end of each quiz

### Statistics
- Track total words learned and words seen
- View quiz history with scores and accuracy
- Per-word performance tracking
- Filter words by status: All, Learned, or Struggling
- A word is considered "learned" after 3+ correct answers with 70%+ accuracy
- Reset progress option

### Progress Persistence
- Progress stored locally using LiteDB (lightweight embedded database)
- Database location: `%APPDATA%\SpanishWordLearner\progress.db` (Windows)
- Progress persists across sessions and browser changes
- No account or login required

## Data Source

Word data is sourced from the [doozan/spanish_data](https://github.com/doozan/spanish_data) repository, which compiles:

- **Frequency data**: Most common Spanish lemmas from the [FrequencyWords](https://github.com/hermitdave/FrequencyWords) project (CC-BY-SA 3.0)
- **Translations**: Spanish to English definitions from [Wiktionary](https://en.wiktionary.org) (CC-BY-SA)

The app loads up to 1000 of the most frequent Spanish words with their translations.

If the data files are not present, the app falls back to an embedded list of 200 common words.

## Limitations

- **No audio pronunciation**: Words are text-only, no audio playback
- **No spaced repetition**: Words are presented in frequency order, not using SRS algorithms
- **Single user**: Progress is stored locally (not synced across devices)
- **Limited context**: Example sentences are only available for the fallback word set
- **English only**: Translations are Spanish to English only
- **Offline data**: Word list is static and loaded at startup (not updated dynamically)

## Requirements

- .NET 9.0 SDK or later

## Running the App

```bash
# Clone the repository with submodules
git clone --recurse-submodules <repository-url>
cd SpanishWordLearningApp

# Or if already cloned without submodules:
git submodule update --init --recursive

# Update spanish_data to latest:
git submodule update --remote spanish_data

# Run the application
dotnet run
```

Then open http://localhost:5062 in your browser.

## Project Structure

```
SpanishWordLearningApp/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   └── Pages/
│       ├── Home.razor          # Dashboard with progress overview
│       ├── Learn.razor         # Flashcard learning mode
│       ├── Quiz.razor          # Multiple choice quizzes
│       └── Statistics.razor    # Detailed progress statistics
├── Models/
│   ├── SpanishWord.cs          # Word data model
│   └── UserProgress.cs         # Progress tracking models
├── Services/
│   ├── DataLoader.cs           # Parses spanish_data files
│   ├── ProgressService.cs      # Manages user progress (LiteDB)
│   └── WordService.cs          # Provides word data to components
├── spanish_data/               # Git submodule from doozan/spanish_data
│   ├── frequency.csv           # Word frequency rankings
│   └── es-en.data              # Wiktionary translations
└── Program.cs                  # App configuration and DI setup
```

## License

This application code is provided as-is for educational purposes.

Data from spanish_data repository is used under the following licenses:
- frequency.csv: CC-BY-SA 3.0 (FrequencyWords)
- es-en.data: CC-BY-SA (Wiktionary)
