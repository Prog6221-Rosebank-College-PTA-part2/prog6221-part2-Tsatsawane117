namespace CybersecurityBotWPF.Sentiment
{
    public enum SentimentType { Neutral, Worried, Frustrated, Curious, Happy, Confused }

    public class SentimentResult
    {
        public SentimentType Type { get; init; } = SentimentType.Neutral;
        public string Label { get; init; } = "Neutral";
    }


    public class SentimentDetector
    {

        private static readonly string[] WorriedWords =
        {
            "worried", "scared", "afraid", "nervous", "anxious", "fear",
            "terrified", "concerned", "unsafe", "vulnerable", "hacked",
            "danger", "at risk", "not safe", "help me", "panicking", "oh no"
        };

        private static readonly string[] FrustratedWords =
        {
            "frustrated", "annoyed", "angry", "upset", "stupid", "useless",
            "not working", "terrible", "hate", "ridiculous", "confusing",
            "complicated", "i give up", "impossible", "so hard", "ugh", "fed up"
        };

        private static readonly string[] CuriousWords =
        {
            "curious", "interested", "want to know", "wondering", "how does",
            "what is", "explain", "tell me about", "what are", "how do",
            "why is", "why does", "i want to learn", "i'm learning", "what happens"
        };

        private static readonly string[] HappyWords =
        {
            "great", "awesome", "amazing", "love it", "thank you", "thanks",
            "helpful", "brilliant", "excellent", "fantastic", "glad", "happy",
            "good", "nice", "perfect", "wonderful", "cheers", "appreciate"
        };

        private static readonly string[] ConfusedWords =
        {
            "confused", "don't understand", "not sure", "what do you mean",
            "unclear", "lost", "don't get it", "can you repeat", "huh",
            "i don't follow", "explain again", "too complicated", "what?"
        };

        public SentimentResult Detect(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new SentimentResult();

            string lower = input.ToLower();

            if (Matches(lower, WorriedWords))
                return new SentimentResult { Type = SentimentType.Worried, Label = "Worried" };
            if (Matches(lower, FrustratedWords))
                return new SentimentResult { Type = SentimentType.Frustrated, Label = "Frustrated" };
            if (Matches(lower, ConfusedWords))
                return new SentimentResult { Type = SentimentType.Confused, Label = "Confused" };
            if (Matches(lower, CuriousWords))
                return new SentimentResult { Type = SentimentType.Curious, Label = "Curious" };
            if (Matches(lower, HappyWords))
                return new SentimentResult { Type = SentimentType.Happy, Label = "Happy" };

            return new SentimentResult();
        }

        private static bool Matches(string input, string[] keywords)
            => keywords.Any(input.Contains);
    }
}