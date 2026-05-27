namespace CybersecurityBotGUI.Sentiment
{
    public enum SentimentType
    {
        Neutral,
        Worried,
        Frustrated,
        Curious,
        Happy,
        Confused
    }

    public class SentimentResult
    {
        public SentimentType Type { get; init; } = SentimentType.Neutral;
        public string Label { get; init; } = "Neutral";
    }


    public class SentimentDetector
    {
     

        private static readonly string[] WorriedKeywords =
        {
            "worried", "scared", "afraid", "nervous", "anxious", "fear", "terrified",
            "concerned", "unsafe", "vulnerable", "hack", "hacked", "danger", "at risk",
            "not safe", "help me", "oh no", "panicking"
        };

        private static readonly string[] FrustratedKeywords =
        {
            "frustrated", "annoyed", "angry", "upset", "this is stupid", "useless",
            "not working", "terrible", "hate", "ridiculous", "confusing", "complicated",
            "i give up", "impossible", "so hard", "ugh", "argh", "fed up"
        };

        private static readonly string[] CuriousKeywords =
        {
            "curious", "interested", "want to know", "wondering", "how does", "what is",
            "explain", "tell me about", "what are", "how do", "why is", "why does",
            "what happens", "can you explain", "i want to learn", "i'm learning"
        };

        private static readonly string[] HappyKeywords =
        {
            "great", "awesome", "amazing", "love it", "thank you", "thanks", "helpful",
            "brilliant", "excellent", "fantastic", "glad", "happy", "good", "nice",
            "perfect", "wonderful", "cheers", "appreciate"
        };

        private static readonly string[] ConfusedKeywords =
        {
            "confused", "don't understand", "not sure", "what do you mean", "unclear",
            "lost", "don't get it", "can you repeat", "say that again", "huh", "what?",
            "i don't follow", "explain again", "too complicated"
        };


        public SentimentResult Detect(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new SentimentResult { Type = SentimentType.Neutral, Label = "Neutral" };

            string lower = input.ToLower();

            // Priority order: Worried > Frustrated > Confused > Curious > Happy > Neutral
            if (MatchesAny(lower, WorriedKeywords))
                return new SentimentResult { Type = SentimentType.Worried, Label = "Worried" };

            if (MatchesAny(lower, FrustratedKeywords))
                return new SentimentResult { Type = SentimentType.Frustrated, Label = "Frustrated" };

            if (MatchesAny(lower, ConfusedKeywords))
                return new SentimentResult { Type = SentimentType.Confused, Label = "Confused" };

            if (MatchesAny(lower, CuriousKeywords))
                return new SentimentResult { Type = SentimentType.Curious, Label = "Curious" };

            if (MatchesAny(lower, HappyKeywords))
                return new SentimentResult { Type = SentimentType.Happy, Label = "Happy" };

            return new SentimentResult { Type = SentimentType.Neutral, Label = "Neutral" };
        }


        private static bool MatchesAny(string input, string[] keywords)
            => keywords.Any(k => input.Contains(k));
    }
}