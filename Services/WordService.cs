using SpanishWordLearner.Models;

namespace SpanishWordLearner.Services;

public class WordService
{
    private readonly List<SpanishWord> _words;
    private readonly string _dataSource;

    public WordService(IWebHostEnvironment env)
    {
        // Try to load from spanish_data repository files
        var basePath = env.ContentRootPath;
        var frequencyPath = Path.Combine(basePath, "spanish_data", "frequency.csv");
        var dictionaryPath = Path.Combine(basePath, "spanish_data", "es-en.data");

        if (File.Exists(frequencyPath) && File.Exists(dictionaryPath))
        {
            _words = DataLoader.LoadFromFiles(frequencyPath, dictionaryPath);
            _dataSource = "spanish_data repository";
        }
        else
        {
            _words = GetFallbackWords();
            _dataSource = "embedded fallback data";
        }

        if (_words.Count == 0)
        {
            _words = GetFallbackWords();
            _dataSource = "embedded fallback data";
        }
    }

    public string DataSource => _dataSource;

    public List<SpanishWord> GetAllWords() => _words;

    public SpanishWord? GetWordById(int id) => _words.FirstOrDefault(w => w.Id == id);

    public List<SpanishWord> GetWordsByFrequency(int count, int skip = 0)
        => _words.OrderBy(w => w.FrequencyRank).Skip(skip).Take(count).ToList();

    public List<SpanishWord> GetRandomWords(int count, List<int>? excludeIds = null)
    {
        var available = excludeIds != null
            ? _words.Where(w => !excludeIds.Contains(w.Id)).ToList()
            : _words;
        return available.OrderBy(_ => Random.Shared.Next()).Take(count).ToList();
    }

    public int TotalWordCount => _words.Count;

    private static List<SpanishWord> GetFallbackWords()
    {
        // Most frequent Spanish words based on frequency data
        return new List<SpanishWord>
        {
            new() { Id = 1, Spanish = "de", English = "of, from", PartOfSpeech = "prep", FrequencyRank = 1, ExampleSentence = "Soy de Mexico.", ExampleTranslation = "I am from Mexico." },
            new() { Id = 2, Spanish = "que", English = "that, which", PartOfSpeech = "conj", FrequencyRank = 2, ExampleSentence = "Creo que si.", ExampleTranslation = "I think so." },
            new() { Id = 3, Spanish = "no", English = "no, not", PartOfSpeech = "adv", FrequencyRank = 3, ExampleSentence = "No entiendo.", ExampleTranslation = "I don't understand." },
            new() { Id = 4, Spanish = "a", English = "to, at", PartOfSpeech = "prep", FrequencyRank = 4, ExampleSentence = "Voy a casa.", ExampleTranslation = "I'm going home." },
            new() { Id = 5, Spanish = "la", English = "the (feminine)", PartOfSpeech = "art", FrequencyRank = 5, ExampleSentence = "La casa es grande.", ExampleTranslation = "The house is big." },
            new() { Id = 6, Spanish = "el", English = "the (masculine)", PartOfSpeech = "art", FrequencyRank = 6, ExampleSentence = "El libro es nuevo.", ExampleTranslation = "The book is new." },
            new() { Id = 7, Spanish = "es", English = "is (ser)", PartOfSpeech = "verb", FrequencyRank = 7, ExampleSentence = "Es importante.", ExampleTranslation = "It is important." },
            new() { Id = 8, Spanish = "y", English = "and", PartOfSpeech = "conj", FrequencyRank = 8, ExampleSentence = "Pan y agua.", ExampleTranslation = "Bread and water." },
            new() { Id = 9, Spanish = "en", English = "in, on", PartOfSpeech = "prep", FrequencyRank = 9, ExampleSentence = "Estoy en casa.", ExampleTranslation = "I'm at home." },
            new() { Id = 10, Spanish = "lo", English = "it, him", PartOfSpeech = "pron", FrequencyRank = 10, ExampleSentence = "Lo se.", ExampleTranslation = "I know it." },
            new() { Id = 11, Spanish = "un", English = "a, an (masculine)", PartOfSpeech = "art", FrequencyRank = 11, ExampleSentence = "Un momento.", ExampleTranslation = "One moment." },
            new() { Id = 12, Spanish = "por", English = "by, for, through", PartOfSpeech = "prep", FrequencyRank = 12, ExampleSentence = "Gracias por todo.", ExampleTranslation = "Thanks for everything." },
            new() { Id = 13, Spanish = "con", English = "with", PartOfSpeech = "prep", FrequencyRank = 13, ExampleSentence = "Cafe con leche.", ExampleTranslation = "Coffee with milk." },
            new() { Id = 14, Spanish = "me", English = "me, myself", PartOfSpeech = "pron", FrequencyRank = 14, ExampleSentence = "Me gusta.", ExampleTranslation = "I like it." },
            new() { Id = 15, Spanish = "si", English = "if, yes", PartOfSpeech = "conj", FrequencyRank = 15, ExampleSentence = "Si quieres.", ExampleTranslation = "If you want." },
            new() { Id = 16, Spanish = "una", English = "a, an (feminine)", PartOfSpeech = "art", FrequencyRank = 16, ExampleSentence = "Una pregunta.", ExampleTranslation = "A question." },
            new() { Id = 17, Spanish = "los", English = "the (masc. plural)", PartOfSpeech = "art", FrequencyRank = 17, ExampleSentence = "Los ninos juegan.", ExampleTranslation = "The children play." },
            new() { Id = 18, Spanish = "se", English = "oneself, itself", PartOfSpeech = "pron", FrequencyRank = 18, ExampleSentence = "Se llama Juan.", ExampleTranslation = "His name is Juan." },
            new() { Id = 19, Spanish = "para", English = "for, to, in order to", PartOfSpeech = "prep", FrequencyRank = 19, ExampleSentence = "Es para ti.", ExampleTranslation = "It's for you." },
            new() { Id = 20, Spanish = "como", English = "like, as, how", PartOfSpeech = "adv", FrequencyRank = 20, ExampleSentence = "Como estas?", ExampleTranslation = "How are you?" },
            new() { Id = 21, Spanish = "pero", English = "but", PartOfSpeech = "conj", FrequencyRank = 21, ExampleSentence = "Pequeno pero fuerte.", ExampleTranslation = "Small but strong." },
            new() { Id = 22, Spanish = "su", English = "his, her, your, their", PartOfSpeech = "adj", FrequencyRank = 22, ExampleSentence = "Su casa es bonita.", ExampleTranslation = "His/Her house is pretty." },
            new() { Id = 23, Spanish = "mas", English = "more, plus", PartOfSpeech = "adv", FrequencyRank = 23, ExampleSentence = "Quiero mas.", ExampleTranslation = "I want more." },
            new() { Id = 24, Spanish = "ser", English = "to be (permanent)", PartOfSpeech = "verb", FrequencyRank = 24, ExampleSentence = "Quiero ser doctor.", ExampleTranslation = "I want to be a doctor." },
            new() { Id = 25, Spanish = "ya", English = "already, now", PartOfSpeech = "adv", FrequencyRank = 25, ExampleSentence = "Ya voy!", ExampleTranslation = "I'm coming!" },
            new() { Id = 26, Spanish = "le", English = "to him/her/you", PartOfSpeech = "pron", FrequencyRank = 26, ExampleSentence = "Le dije la verdad.", ExampleTranslation = "I told him the truth." },
            new() { Id = 27, Spanish = "esto", English = "this", PartOfSpeech = "pron", FrequencyRank = 27, ExampleSentence = "Que es esto?", ExampleTranslation = "What is this?" },
            new() { Id = 28, Spanish = "todo", English = "all, everything", PartOfSpeech = "adj", FrequencyRank = 28, ExampleSentence = "Todo esta bien.", ExampleTranslation = "Everything is fine." },
            new() { Id = 29, Spanish = "esta", English = "this, is (estar)", PartOfSpeech = "adj", FrequencyRank = 29, ExampleSentence = "Esta aqui.", ExampleTranslation = "It's here." },
            new() { Id = 30, Spanish = "bien", English = "well, good", PartOfSpeech = "adv", FrequencyRank = 30, ExampleSentence = "Muy bien.", ExampleTranslation = "Very well." },
            new() { Id = 31, Spanish = "tener", English = "to have", PartOfSpeech = "verb", FrequencyRank = 31, ExampleSentence = "Tengo hambre.", ExampleTranslation = "I'm hungry." },
            new() { Id = 32, Spanish = "hay", English = "there is, there are", PartOfSpeech = "verb", FrequencyRank = 32, ExampleSentence = "Hay mucha gente.", ExampleTranslation = "There are many people." },
            new() { Id = 33, Spanish = "hacer", English = "to do, to make", PartOfSpeech = "verb", FrequencyRank = 33, ExampleSentence = "Que vas a hacer?", ExampleTranslation = "What are you going to do?" },
            new() { Id = 34, Spanish = "te", English = "you, yourself", PartOfSpeech = "pron", FrequencyRank = 34, ExampleSentence = "Te quiero.", ExampleTranslation = "I love you." },
            new() { Id = 35, Spanish = "eso", English = "that", PartOfSpeech = "pron", FrequencyRank = 35, ExampleSentence = "Eso es verdad.", ExampleTranslation = "That is true." },
            new() { Id = 36, Spanish = "muy", English = "very", PartOfSpeech = "adv", FrequencyRank = 36, ExampleSentence = "Muy bueno.", ExampleTranslation = "Very good." },
            new() { Id = 37, Spanish = "yo", English = "I", PartOfSpeech = "pron", FrequencyRank = 37, ExampleSentence = "Yo tambien.", ExampleTranslation = "Me too." },
            new() { Id = 38, Spanish = "mi", English = "my", PartOfSpeech = "adj", FrequencyRank = 38, ExampleSentence = "Mi familia.", ExampleTranslation = "My family." },
            new() { Id = 39, Spanish = "tu", English = "you, your", PartOfSpeech = "pron", FrequencyRank = 39, ExampleSentence = "Tu nombre.", ExampleTranslation = "Your name." },
            new() { Id = 40, Spanish = "cosa", English = "thing", PartOfSpeech = "noun", FrequencyRank = 40, ExampleSentence = "Es otra cosa.", ExampleTranslation = "It's another thing." },
            new() { Id = 41, Spanish = "al", English = "to the", PartOfSpeech = "prep", FrequencyRank = 41, ExampleSentence = "Voy al parque.", ExampleTranslation = "I'm going to the park." },
            new() { Id = 42, Spanish = "aqui", English = "here", PartOfSpeech = "adv", FrequencyRank = 42, ExampleSentence = "Ven aqui.", ExampleTranslation = "Come here." },
            new() { Id = 43, Spanish = "del", English = "of the, from the", PartOfSpeech = "prep", FrequencyRank = 43, ExampleSentence = "El libro del profesor.", ExampleTranslation = "The teacher's book." },
            new() { Id = 44, Spanish = "poder", English = "to be able to, can", PartOfSpeech = "verb", FrequencyRank = 44, ExampleSentence = "Puedo ayudar.", ExampleTranslation = "I can help." },
            new() { Id = 45, Spanish = "decir", English = "to say, to tell", PartOfSpeech = "verb", FrequencyRank = 45, ExampleSentence = "Que quieres decir?", ExampleTranslation = "What do you mean?" },
            new() { Id = 46, Spanish = "ir", English = "to go", PartOfSpeech = "verb", FrequencyRank = 46, ExampleSentence = "Vamos a ir.", ExampleTranslation = "We're going to go." },
            new() { Id = 47, Spanish = "ese", English = "that (masc.)", PartOfSpeech = "adj", FrequencyRank = 47, ExampleSentence = "Ese hombre.", ExampleTranslation = "That man." },
            new() { Id = 48, Spanish = "ver", English = "to see", PartOfSpeech = "verb", FrequencyRank = 48, ExampleSentence = "Quiero ver.", ExampleTranslation = "I want to see." },
            new() { Id = 49, Spanish = "ahora", English = "now", PartOfSpeech = "adv", FrequencyRank = 49, ExampleSentence = "Ahora mismo.", ExampleTranslation = "Right now." },
            new() { Id = 50, Spanish = "tiempo", English = "time, weather", PartOfSpeech = "noun", FrequencyRank = 50, ExampleSentence = "No tengo tiempo.", ExampleTranslation = "I don't have time." },
            new() { Id = 51, Spanish = "saber", English = "to know", PartOfSpeech = "verb", FrequencyRank = 51, ExampleSentence = "No se nada.", ExampleTranslation = "I don't know anything." },
            new() { Id = 52, Spanish = "cuando", English = "when", PartOfSpeech = "adv", FrequencyRank = 52, ExampleSentence = "Cuando llegaste?", ExampleTranslation = "When did you arrive?" },
            new() { Id = 53, Spanish = "nada", English = "nothing", PartOfSpeech = "pron", FrequencyRank = 53, ExampleSentence = "No pasa nada.", ExampleTranslation = "It's nothing." },
            new() { Id = 54, Spanish = "solo", English = "only, alone", PartOfSpeech = "adv", FrequencyRank = 54, ExampleSentence = "Solo uno.", ExampleTranslation = "Only one." },
            new() { Id = 55, Spanish = "vida", English = "life", PartOfSpeech = "noun", FrequencyRank = 55, ExampleSentence = "La vida es bella.", ExampleTranslation = "Life is beautiful." },
            new() { Id = 56, Spanish = "hombre", English = "man", PartOfSpeech = "noun", FrequencyRank = 56, ExampleSentence = "El hombre camina.", ExampleTranslation = "The man walks." },
            new() { Id = 57, Spanish = "dos", English = "two", PartOfSpeech = "num", FrequencyRank = 57, ExampleSentence = "Son las dos.", ExampleTranslation = "It's two o'clock." },
            new() { Id = 58, Spanish = "querer", English = "to want, to love", PartOfSpeech = "verb", FrequencyRank = 58, ExampleSentence = "Quiero comer.", ExampleTranslation = "I want to eat." },
            new() { Id = 59, Spanish = "ese", English = "that one", PartOfSpeech = "pron", FrequencyRank = 59, ExampleSentence = "Dame ese.", ExampleTranslation = "Give me that one." },
            new() { Id = 60, Spanish = "grande", English = "big, large", PartOfSpeech = "adj", FrequencyRank = 60, ExampleSentence = "Una casa grande.", ExampleTranslation = "A big house." },
            new() { Id = 61, Spanish = "donde", English = "where", PartOfSpeech = "adv", FrequencyRank = 61, ExampleSentence = "Donde estas?", ExampleTranslation = "Where are you?" },
            new() { Id = 62, Spanish = "algo", English = "something", PartOfSpeech = "pron", FrequencyRank = 62, ExampleSentence = "Quieres algo?", ExampleTranslation = "Do you want something?" },
            new() { Id = 63, Spanish = "mujer", English = "woman, wife", PartOfSpeech = "noun", FrequencyRank = 63, ExampleSentence = "La mujer sonrie.", ExampleTranslation = "The woman smiles." },
            new() { Id = 64, Spanish = "casa", English = "house, home", PartOfSpeech = "noun", FrequencyRank = 64, ExampleSentence = "Mi casa.", ExampleTranslation = "My house." },
            new() { Id = 65, Spanish = "mundo", English = "world", PartOfSpeech = "noun", FrequencyRank = 65, ExampleSentence = "Todo el mundo.", ExampleTranslation = "Everyone / The whole world." },
            new() { Id = 66, Spanish = "dia", English = "day", PartOfSpeech = "noun", FrequencyRank = 66, ExampleSentence = "Buenos dias.", ExampleTranslation = "Good morning." },
            new() { Id = 67, Spanish = "mismo", English = "same, self", PartOfSpeech = "adj", FrequencyRank = 67, ExampleSentence = "Lo mismo.", ExampleTranslation = "The same." },
            new() { Id = 68, Spanish = "despues", English = "after, later", PartOfSpeech = "adv", FrequencyRank = 68, ExampleSentence = "Hasta despues.", ExampleTranslation = "See you later." },
            new() { Id = 69, Spanish = "otro", English = "other, another", PartOfSpeech = "adj", FrequencyRank = 69, ExampleSentence = "Otro dia.", ExampleTranslation = "Another day." },
            new() { Id = 70, Spanish = "poco", English = "little, few", PartOfSpeech = "adj", FrequencyRank = 70, ExampleSentence = "Un poco.", ExampleTranslation = "A little." },
            new() { Id = 71, Spanish = "dar", English = "to give", PartOfSpeech = "verb", FrequencyRank = 71, ExampleSentence = "Dame el libro.", ExampleTranslation = "Give me the book." },
            new() { Id = 72, Spanish = "bueno", English = "good", PartOfSpeech = "adj", FrequencyRank = 72, ExampleSentence = "Es bueno.", ExampleTranslation = "It's good." },
            new() { Id = 73, Spanish = "vez", English = "time, occasion", PartOfSpeech = "noun", FrequencyRank = 73, ExampleSentence = "Una vez mas.", ExampleTranslation = "One more time." },
            new() { Id = 74, Spanish = "pasar", English = "to pass, to happen", PartOfSpeech = "verb", FrequencyRank = 74, ExampleSentence = "Que paso?", ExampleTranslation = "What happened?" },
            new() { Id = 75, Spanish = "creer", English = "to believe, to think", PartOfSpeech = "verb", FrequencyRank = 75, ExampleSentence = "Creo que si.", ExampleTranslation = "I think so." },
            new() { Id = 76, Spanish = "haber", English = "to have (auxiliary)", PartOfSpeech = "verb", FrequencyRank = 76, ExampleSentence = "He comido.", ExampleTranslation = "I have eaten." },
            new() { Id = 77, Spanish = "estar", English = "to be (temporary)", PartOfSpeech = "verb", FrequencyRank = 77, ExampleSentence = "Estoy feliz.", ExampleTranslation = "I am happy." },
            new() { Id = 78, Spanish = "llegar", English = "to arrive", PartOfSpeech = "verb", FrequencyRank = 78, ExampleSentence = "Ya llegue.", ExampleTranslation = "I arrived." },
            new() { Id = 79, Spanish = "nuevo", English = "new", PartOfSpeech = "adj", FrequencyRank = 79, ExampleSentence = "Algo nuevo.", ExampleTranslation = "Something new." },
            new() { Id = 80, Spanish = "parte", English = "part", PartOfSpeech = "noun", FrequencyRank = 80, ExampleSentence = "La mejor parte.", ExampleTranslation = "The best part." },
            new() { Id = 81, Spanish = "nosotros", English = "we, us", PartOfSpeech = "pron", FrequencyRank = 81, ExampleSentence = "Nosotros vamos.", ExampleTranslation = "We are going." },
            new() { Id = 82, Spanish = "encontrar", English = "to find", PartOfSpeech = "verb", FrequencyRank = 82, ExampleSentence = "No puedo encontrarlo.", ExampleTranslation = "I can't find it." },
            new() { Id = 83, Spanish = "pensar", English = "to think", PartOfSpeech = "verb", FrequencyRank = 83, ExampleSentence = "Pienso mucho.", ExampleTranslation = "I think a lot." },
            new() { Id = 84, Spanish = "hablar", English = "to speak, to talk", PartOfSpeech = "verb", FrequencyRank = 84, ExampleSentence = "Hablas espanol?", ExampleTranslation = "Do you speak Spanish?" },
            new() { Id = 85, Spanish = "llevar", English = "to carry, to wear", PartOfSpeech = "verb", FrequencyRank = 85, ExampleSentence = "Lleva una camisa azul.", ExampleTranslation = "He's wearing a blue shirt." },
            new() { Id = 86, Spanish = "dejar", English = "to leave, to let", PartOfSpeech = "verb", FrequencyRank = 86, ExampleSentence = "Dejame pensar.", ExampleTranslation = "Let me think." },
            new() { Id = 87, Spanish = "seguir", English = "to follow, to continue", PartOfSpeech = "verb", FrequencyRank = 87, ExampleSentence = "Sigue adelante.", ExampleTranslation = "Keep going." },
            new() { Id = 88, Spanish = "nunca", English = "never", PartOfSpeech = "adv", FrequencyRank = 88, ExampleSentence = "Nunca mas.", ExampleTranslation = "Never again." },
            new() { Id = 89, Spanish = "siempre", English = "always", PartOfSpeech = "adv", FrequencyRank = 89, ExampleSentence = "Para siempre.", ExampleTranslation = "Forever." },
            new() { Id = 90, Spanish = "parecer", English = "to seem, to appear", PartOfSpeech = "verb", FrequencyRank = 90, ExampleSentence = "Parece facil.", ExampleTranslation = "It seems easy." },
            new() { Id = 91, Spanish = "conocer", English = "to know (people/places)", PartOfSpeech = "verb", FrequencyRank = 91, ExampleSentence = "Te conozco.", ExampleTranslation = "I know you." },
            new() { Id = 92, Spanish = "hora", English = "hour, time", PartOfSpeech = "noun", FrequencyRank = 92, ExampleSentence = "Que hora es?", ExampleTranslation = "What time is it?" },
            new() { Id = 93, Spanish = "primero", English = "first", PartOfSpeech = "adj", FrequencyRank = 93, ExampleSentence = "La primera vez.", ExampleTranslation = "The first time." },
            new() { Id = 94, Spanish = "mano", English = "hand", PartOfSpeech = "noun", FrequencyRank = 94, ExampleSentence = "Dame la mano.", ExampleTranslation = "Give me your hand." },
            new() { Id = 95, Spanish = "mejor", English = "better, best", PartOfSpeech = "adj", FrequencyRank = 95, ExampleSentence = "Es el mejor.", ExampleTranslation = "It's the best." },
            new() { Id = 96, Spanish = "tres", English = "three", PartOfSpeech = "num", FrequencyRank = 96, ExampleSentence = "Tres amigos.", ExampleTranslation = "Three friends." },
            new() { Id = 97, Spanish = "sentir", English = "to feel", PartOfSpeech = "verb", FrequencyRank = 97, ExampleSentence = "Lo siento.", ExampleTranslation = "I'm sorry." },
            new() { Id = 98, Spanish = "padre", English = "father", PartOfSpeech = "noun", FrequencyRank = 98, ExampleSentence = "Mi padre trabaja.", ExampleTranslation = "My father works." },
            new() { Id = 99, Spanish = "madre", English = "mother", PartOfSpeech = "noun", FrequencyRank = 99, ExampleSentence = "Mi madre cocina.", ExampleTranslation = "My mother cooks." },
            new() { Id = 100, Spanish = "noche", English = "night", PartOfSpeech = "noun", FrequencyRank = 100, ExampleSentence = "Buenas noches.", ExampleTranslation = "Good night." },
            new() { Id = 101, Spanish = "hijo", English = "son, child", PartOfSpeech = "noun", FrequencyRank = 101, ExampleSentence = "Mi hijo estudia.", ExampleTranslation = "My son studies." },
            new() { Id = 102, Spanish = "cuenta", English = "account, bill", PartOfSpeech = "noun", FrequencyRank = 102, ExampleSentence = "La cuenta, por favor.", ExampleTranslation = "The bill, please." },
            new() { Id = 103, Spanish = "trabajo", English = "work, job", PartOfSpeech = "noun", FrequencyRank = 103, ExampleSentence = "Tengo mucho trabajo.", ExampleTranslation = "I have a lot of work." },
            new() { Id = 104, Spanish = "punto", English = "point, dot", PartOfSpeech = "noun", FrequencyRank = 104, ExampleSentence = "Buen punto.", ExampleTranslation = "Good point." },
            new() { Id = 105, Spanish = "ojo", English = "eye", PartOfSpeech = "noun", FrequencyRank = 105, ExampleSentence = "Tiene ojos azules.", ExampleTranslation = "He/She has blue eyes." },
            new() { Id = 106, Spanish = "agua", English = "water", PartOfSpeech = "noun", FrequencyRank = 106, ExampleSentence = "Quiero agua.", ExampleTranslation = "I want water." },
            new() { Id = 107, Spanish = "amigo", English = "friend", PartOfSpeech = "noun", FrequencyRank = 107, ExampleSentence = "Es mi amigo.", ExampleTranslation = "He's my friend." },
            new() { Id = 108, Spanish = "menos", English = "less, minus", PartOfSpeech = "adv", FrequencyRank = 108, ExampleSentence = "Mas o menos.", ExampleTranslation = "More or less." },
            new() { Id = 109, Spanish = "senor", English = "sir, mister", PartOfSpeech = "noun", FrequencyRank = 109, ExampleSentence = "Senor Garcia.", ExampleTranslation = "Mr. Garcia." },
            new() { Id = 110, Spanish = "esperar", English = "to wait, to hope", PartOfSpeech = "verb", FrequencyRank = 110, ExampleSentence = "Espera un momento.", ExampleTranslation = "Wait a moment." },
            new() { Id = 111, Spanish = "poner", English = "to put, to place", PartOfSpeech = "verb", FrequencyRank = 111, ExampleSentence = "Ponlo aqui.", ExampleTranslation = "Put it here." },
            new() { Id = 112, Spanish = "tomar", English = "to take, to drink", PartOfSpeech = "verb", FrequencyRank = 112, ExampleSentence = "Voy a tomar cafe.", ExampleTranslation = "I'm going to have coffee." },
            new() { Id = 113, Spanish = "lado", English = "side", PartOfSpeech = "noun", FrequencyRank = 113, ExampleSentence = "Al otro lado.", ExampleTranslation = "On the other side." },
            new() { Id = 114, Spanish = "volver", English = "to return", PartOfSpeech = "verb", FrequencyRank = 114, ExampleSentence = "Vuelvo pronto.", ExampleTranslation = "I'll be back soon." },
            new() { Id = 115, Spanish = "quedar", English = "to stay, to remain", PartOfSpeech = "verb", FrequencyRank = 115, ExampleSentence = "Me quedo aqui.", ExampleTranslation = "I'm staying here." },
            new() { Id = 116, Spanish = "mucho", English = "much, a lot", PartOfSpeech = "adv", FrequencyRank = 116, ExampleSentence = "Gracias, mucho.", ExampleTranslation = "Thank you very much." },
            new() { Id = 117, Spanish = "nombre", English = "name", PartOfSpeech = "noun", FrequencyRank = 117, ExampleSentence = "Cual es tu nombre?", ExampleTranslation = "What is your name?" },
            new() { Id = 118, Spanish = "ciudad", English = "city", PartOfSpeech = "noun", FrequencyRank = 118, ExampleSentence = "Una ciudad grande.", ExampleTranslation = "A big city." },
            new() { Id = 119, Spanish = "caso", English = "case", PartOfSpeech = "noun", FrequencyRank = 119, ExampleSentence = "En ese caso.", ExampleTranslation = "In that case." },
            new() { Id = 120, Spanish = "mientras", English = "while", PartOfSpeech = "conj", FrequencyRank = 120, ExampleSentence = "Mientras tanto.", ExampleTranslation = "Meanwhile." },
            new() { Id = 121, Spanish = "comer", English = "to eat", PartOfSpeech = "verb", FrequencyRank = 121, ExampleSentence = "Vamos a comer.", ExampleTranslation = "Let's eat." },
            new() { Id = 122, Spanish = "escribir", English = "to write", PartOfSpeech = "verb", FrequencyRank = 122, ExampleSentence = "Voy a escribir.", ExampleTranslation = "I'm going to write." },
            new() { Id = 123, Spanish = "leer", English = "to read", PartOfSpeech = "verb", FrequencyRank = 123, ExampleSentence = "Me gusta leer.", ExampleTranslation = "I like to read." },
            new() { Id = 124, Spanish = "libro", English = "book", PartOfSpeech = "noun", FrequencyRank = 124, ExampleSentence = "Un buen libro.", ExampleTranslation = "A good book." },
            new() { Id = 125, Spanish = "dinero", English = "money", PartOfSpeech = "noun", FrequencyRank = 125, ExampleSentence = "No tengo dinero.", ExampleTranslation = "I don't have money." },
            new() { Id = 126, Spanish = "entrar", English = "to enter", PartOfSpeech = "verb", FrequencyRank = 126, ExampleSentence = "Puedo entrar?", ExampleTranslation = "May I come in?" },
            new() { Id = 127, Spanish = "salir", English = "to leave, to go out", PartOfSpeech = "verb", FrequencyRank = 127, ExampleSentence = "Voy a salir.", ExampleTranslation = "I'm going out." },
            new() { Id = 128, Spanish = "ano", English = "year", PartOfSpeech = "noun", FrequencyRank = 128, ExampleSentence = "El proximo ano.", ExampleTranslation = "Next year." },
            new() { Id = 129, Spanish = "llamar", English = "to call", PartOfSpeech = "verb", FrequencyRank = 129, ExampleSentence = "Te voy a llamar.", ExampleTranslation = "I'm going to call you." },
            new() { Id = 130, Spanish = "momento", English = "moment", PartOfSpeech = "noun", FrequencyRank = 130, ExampleSentence = "Un momento.", ExampleTranslation = "One moment." },
            new() { Id = 131, Spanish = "forma", English = "form, way", PartOfSpeech = "noun", FrequencyRank = 131, ExampleSentence = "De alguna forma.", ExampleTranslation = "Somehow." },
            new() { Id = 132, Spanish = "manera", English = "way, manner", PartOfSpeech = "noun", FrequencyRank = 132, ExampleSentence = "De esta manera.", ExampleTranslation = "This way." },
            new() { Id = 133, Spanish = "puerta", English = "door", PartOfSpeech = "noun", FrequencyRank = 133, ExampleSentence = "Abre la puerta.", ExampleTranslation = "Open the door." },
            new() { Id = 134, Spanish = "palabra", English = "word", PartOfSpeech = "noun", FrequencyRank = 134, ExampleSentence = "Una palabra.", ExampleTranslation = "A word." },
            new() { Id = 135, Spanish = "gente", English = "people", PartOfSpeech = "noun", FrequencyRank = 135, ExampleSentence = "Mucha gente.", ExampleTranslation = "Many people." },
            new() { Id = 136, Spanish = "lugar", English = "place", PartOfSpeech = "noun", FrequencyRank = 136, ExampleSentence = "Un buen lugar.", ExampleTranslation = "A good place." },
            new() { Id = 137, Spanish = "pequeno", English = "small, little", PartOfSpeech = "adj", FrequencyRank = 137, ExampleSentence = "Es muy pequeno.", ExampleTranslation = "It's very small." },
            new() { Id = 138, Spanish = "venir", English = "to come", PartOfSpeech = "verb", FrequencyRank = 138, ExampleSentence = "Ven aqui.", ExampleTranslation = "Come here." },
            new() { Id = 139, Spanish = "largo", English = "long", PartOfSpeech = "adj", FrequencyRank = 139, ExampleSentence = "Es muy largo.", ExampleTranslation = "It's very long." },
            new() { Id = 140, Spanish = "pregunta", English = "question", PartOfSpeech = "noun", FrequencyRank = 140, ExampleSentence = "Tengo una pregunta.", ExampleTranslation = "I have a question." },
            new() { Id = 141, Spanish = "cabeza", English = "head", PartOfSpeech = "noun", FrequencyRank = 141, ExampleSentence = "Me duele la cabeza.", ExampleTranslation = "My head hurts." },
            new() { Id = 142, Spanish = "vivir", English = "to live", PartOfSpeech = "verb", FrequencyRank = 142, ExampleSentence = "Donde vives?", ExampleTranslation = "Where do you live?" },
            new() { Id = 143, Spanish = "cuerpo", English = "body", PartOfSpeech = "noun", FrequencyRank = 143, ExampleSentence = "Todo el cuerpo.", ExampleTranslation = "The whole body." },
            new() { Id = 144, Spanish = "pie", English = "foot", PartOfSpeech = "noun", FrequencyRank = 144, ExampleSentence = "A pie.", ExampleTranslation = "On foot." },
            new() { Id = 145, Spanish = "cierto", English = "certain, true", PartOfSpeech = "adj", FrequencyRank = 145, ExampleSentence = "Es cierto.", ExampleTranslation = "It's true." },
            new() { Id = 146, Spanish = "lejos", English = "far", PartOfSpeech = "adv", FrequencyRank = 146, ExampleSentence = "Esta lejos.", ExampleTranslation = "It's far." },
            new() { Id = 147, Spanish = "cerca", English = "near, close", PartOfSpeech = "adv", FrequencyRank = 147, ExampleSentence = "Esta cerca.", ExampleTranslation = "It's close." },
            new() { Id = 148, Spanish = "importante", English = "important", PartOfSpeech = "adj", FrequencyRank = 148, ExampleSentence = "Es importante.", ExampleTranslation = "It's important." },
            new() { Id = 149, Spanish = "ultimo", English = "last, final", PartOfSpeech = "adj", FrequencyRank = 149, ExampleSentence = "El ultimo dia.", ExampleTranslation = "The last day." },
            new() { Id = 150, Spanish = "cambiar", English = "to change", PartOfSpeech = "verb", FrequencyRank = 150, ExampleSentence = "Voy a cambiar.", ExampleTranslation = "I'm going to change." },
            new() { Id = 151, Spanish = "dormir", English = "to sleep", PartOfSpeech = "verb", FrequencyRank = 151, ExampleSentence = "Voy a dormir.", ExampleTranslation = "I'm going to sleep." },
            new() { Id = 152, Spanish = "morir", English = "to die", PartOfSpeech = "verb", FrequencyRank = 152, ExampleSentence = "No quiero morir.", ExampleTranslation = "I don't want to die." },
            new() { Id = 153, Spanish = "negro", English = "black", PartOfSpeech = "adj", FrequencyRank = 153, ExampleSentence = "El gato negro.", ExampleTranslation = "The black cat." },
            new() { Id = 154, Spanish = "blanco", English = "white", PartOfSpeech = "adj", FrequencyRank = 154, ExampleSentence = "Es blanco.", ExampleTranslation = "It's white." },
            new() { Id = 155, Spanish = "rojo", English = "red", PartOfSpeech = "adj", FrequencyRank = 155, ExampleSentence = "Es rojo.", ExampleTranslation = "It's red." },
            new() { Id = 156, Spanish = "verde", English = "green", PartOfSpeech = "adj", FrequencyRank = 156, ExampleSentence = "Es verde.", ExampleTranslation = "It's green." },
            new() { Id = 157, Spanish = "azul", English = "blue", PartOfSpeech = "adj", FrequencyRank = 157, ExampleSentence = "El cielo azul.", ExampleTranslation = "The blue sky." },
            new() { Id = 158, Spanish = "joven", English = "young", PartOfSpeech = "adj", FrequencyRank = 158, ExampleSentence = "Es muy joven.", ExampleTranslation = "He/She is very young." },
            new() { Id = 159, Spanish = "viejo", English = "old", PartOfSpeech = "adj", FrequencyRank = 159, ExampleSentence = "Es muy viejo.", ExampleTranslation = "It's very old." },
            new() { Id = 160, Spanish = "facil", English = "easy", PartOfSpeech = "adj", FrequencyRank = 160, ExampleSentence = "Es facil.", ExampleTranslation = "It's easy." },
            new() { Id = 161, Spanish = "dificil", English = "difficult", PartOfSpeech = "adj", FrequencyRank = 161, ExampleSentence = "Es dificil.", ExampleTranslation = "It's difficult." },
            new() { Id = 162, Spanish = "alto", English = "tall, high", PartOfSpeech = "adj", FrequencyRank = 162, ExampleSentence = "Es muy alto.", ExampleTranslation = "He/She is very tall." },
            new() { Id = 163, Spanish = "bajo", English = "short, low", PartOfSpeech = "adj", FrequencyRank = 163, ExampleSentence = "Habla bajo.", ExampleTranslation = "Speak quietly." },
            new() { Id = 164, Spanish = "rapido", English = "fast, quick", PartOfSpeech = "adj", FrequencyRank = 164, ExampleSentence = "Es muy rapido.", ExampleTranslation = "It's very fast." },
            new() { Id = 165, Spanish = "lento", English = "slow", PartOfSpeech = "adj", FrequencyRank = 165, ExampleSentence = "Mas lento, por favor.", ExampleTranslation = "Slower, please." },
            new() { Id = 166, Spanish = "frio", English = "cold", PartOfSpeech = "adj", FrequencyRank = 166, ExampleSentence = "Hace frio.", ExampleTranslation = "It's cold." },
            new() { Id = 167, Spanish = "caliente", English = "hot", PartOfSpeech = "adj", FrequencyRank = 167, ExampleSentence = "Esta caliente.", ExampleTranslation = "It's hot." },
            new() { Id = 168, Spanish = "feliz", English = "happy", PartOfSpeech = "adj", FrequencyRank = 168, ExampleSentence = "Estoy feliz.", ExampleTranslation = "I'm happy." },
            new() { Id = 169, Spanish = "triste", English = "sad", PartOfSpeech = "adj", FrequencyRank = 169, ExampleSentence = "Estoy triste.", ExampleTranslation = "I'm sad." },
            new() { Id = 170, Spanish = "cansado", English = "tired", PartOfSpeech = "adj", FrequencyRank = 170, ExampleSentence = "Estoy cansado.", ExampleTranslation = "I'm tired." },
            new() { Id = 171, Spanish = "enfermo", English = "sick", PartOfSpeech = "adj", FrequencyRank = 171, ExampleSentence = "Estoy enfermo.", ExampleTranslation = "I'm sick." },
            new() { Id = 172, Spanish = "listo", English = "ready, smart", PartOfSpeech = "adj", FrequencyRank = 172, ExampleSentence = "Estoy listo.", ExampleTranslation = "I'm ready." },
            new() { Id = 173, Spanish = "rico", English = "rich, delicious", PartOfSpeech = "adj", FrequencyRank = 173, ExampleSentence = "Esta muy rico.", ExampleTranslation = "It's very delicious." },
            new() { Id = 174, Spanish = "pobre", English = "poor", PartOfSpeech = "adj", FrequencyRank = 174, ExampleSentence = "Es muy pobre.", ExampleTranslation = "He/She is very poor." },
            new() { Id = 175, Spanish = "bonito", English = "pretty, nice", PartOfSpeech = "adj", FrequencyRank = 175, ExampleSentence = "Es muy bonito.", ExampleTranslation = "It's very pretty." },
            new() { Id = 176, Spanish = "feo", English = "ugly", PartOfSpeech = "adj", FrequencyRank = 176, ExampleSentence = "Es muy feo.", ExampleTranslation = "It's very ugly." },
            new() { Id = 177, Spanish = "carro", English = "car", PartOfSpeech = "noun", FrequencyRank = 177, ExampleSentence = "Mi carro es nuevo.", ExampleTranslation = "My car is new." },
            new() { Id = 178, Spanish = "calle", English = "street", PartOfSpeech = "noun", FrequencyRank = 178, ExampleSentence = "En la calle.", ExampleTranslation = "On the street." },
            new() { Id = 179, Spanish = "mesa", English = "table", PartOfSpeech = "noun", FrequencyRank = 179, ExampleSentence = "En la mesa.", ExampleTranslation = "On the table." },
            new() { Id = 180, Spanish = "silla", English = "chair", PartOfSpeech = "noun", FrequencyRank = 180, ExampleSentence = "Sientate en la silla.", ExampleTranslation = "Sit on the chair." },
            new() { Id = 181, Spanish = "perro", English = "dog", PartOfSpeech = "noun", FrequencyRank = 181, ExampleSentence = "Tengo un perro.", ExampleTranslation = "I have a dog." },
            new() { Id = 182, Spanish = "gato", English = "cat", PartOfSpeech = "noun", FrequencyRank = 182, ExampleSentence = "El gato duerme.", ExampleTranslation = "The cat sleeps." },
            new() { Id = 183, Spanish = "comida", English = "food", PartOfSpeech = "noun", FrequencyRank = 183, ExampleSentence = "La comida esta lista.", ExampleTranslation = "The food is ready." },
            new() { Id = 184, Spanish = "pan", English = "bread", PartOfSpeech = "noun", FrequencyRank = 184, ExampleSentence = "Quiero pan.", ExampleTranslation = "I want bread." },
            new() { Id = 185, Spanish = "leche", English = "milk", PartOfSpeech = "noun", FrequencyRank = 185, ExampleSentence = "Un vaso de leche.", ExampleTranslation = "A glass of milk." },
            new() { Id = 186, Spanish = "cafe", English = "coffee", PartOfSpeech = "noun", FrequencyRank = 186, ExampleSentence = "Quiero cafe.", ExampleTranslation = "I want coffee." },
            new() { Id = 187, Spanish = "cerveza", English = "beer", PartOfSpeech = "noun", FrequencyRank = 187, ExampleSentence = "Una cerveza, por favor.", ExampleTranslation = "A beer, please." },
            new() { Id = 188, Spanish = "vino", English = "wine", PartOfSpeech = "noun", FrequencyRank = 188, ExampleSentence = "Un vaso de vino.", ExampleTranslation = "A glass of wine." },
            new() { Id = 189, Spanish = "camino", English = "road, path", PartOfSpeech = "noun", FrequencyRank = 189, ExampleSentence = "Por este camino.", ExampleTranslation = "This way." },
            new() { Id = 190, Spanish = "luz", English = "light", PartOfSpeech = "noun", FrequencyRank = 190, ExampleSentence = "Enciende la luz.", ExampleTranslation = "Turn on the light." },
            new() { Id = 191, Spanish = "sol", English = "sun", PartOfSpeech = "noun", FrequencyRank = 191, ExampleSentence = "Hace sol.", ExampleTranslation = "It's sunny." },
            new() { Id = 192, Spanish = "luna", English = "moon", PartOfSpeech = "noun", FrequencyRank = 192, ExampleSentence = "La luna llena.", ExampleTranslation = "The full moon." },
            new() { Id = 193, Spanish = "tierra", English = "earth, land", PartOfSpeech = "noun", FrequencyRank = 193, ExampleSentence = "La tierra es redonda.", ExampleTranslation = "The earth is round." },
            new() { Id = 194, Spanish = "cielo", English = "sky, heaven", PartOfSpeech = "noun", FrequencyRank = 194, ExampleSentence = "El cielo esta azul.", ExampleTranslation = "The sky is blue." },
            new() { Id = 195, Spanish = "mar", English = "sea", PartOfSpeech = "noun", FrequencyRank = 195, ExampleSentence = "El mar esta tranquilo.", ExampleTranslation = "The sea is calm." },
            new() { Id = 196, Spanish = "playa", English = "beach", PartOfSpeech = "noun", FrequencyRank = 196, ExampleSentence = "Vamos a la playa.", ExampleTranslation = "Let's go to the beach." },
            new() { Id = 197, Spanish = "montano", English = "mountain", PartOfSpeech = "noun", FrequencyRank = 197, ExampleSentence = "Una montana alta.", ExampleTranslation = "A tall mountain." },
            new() { Id = 198, Spanish = "arbol", English = "tree", PartOfSpeech = "noun", FrequencyRank = 198, ExampleSentence = "Debajo del arbol.", ExampleTranslation = "Under the tree." },
            new() { Id = 199, Spanish = "flor", English = "flower", PartOfSpeech = "noun", FrequencyRank = 199, ExampleSentence = "Una flor bonita.", ExampleTranslation = "A pretty flower." },
            new() { Id = 200, Spanish = "corazon", English = "heart", PartOfSpeech = "noun", FrequencyRank = 200, ExampleSentence = "Con todo mi corazon.", ExampleTranslation = "With all my heart." }
        };
    }
}
