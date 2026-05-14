namespace CybersecurityBotGUI.Memory
{

    public class UserMemory
    {

        public string Name { get; private set; } = "there";
        public string? FavouriteTopic { get; private set; }

        public string? MentionedConcern { get; private set; }
        public int MessageCount { get; private set; } = 0;

        private readonly List<string> _topicHistory = new();

        public void SetName(string name)
        {
            Name = CapitaliseName(name.Trim());
        }

        public void SetFavouriteTopic(string topic)
        {
            FavouriteTopic = topic;
            if (!_topicHistory.Contains(topic))
                _topicHistory.Add(topic);
        }


        public void SetConcern(string concern)
        {
            MentionedConcern = concern;
        }

        public void IncrementMessageCount()
        {
            MessageCount++;
        }

        public string GetMemoryRecall()
        {
            if (FavouriteTopic != null && MessageCount > 3)
                return $"As someone interested in {FavouriteTopic}, you might find this especially useful.";
            return string.Empty;
        }

        public IReadOnlyList<string> GetTopicHistory() => _topicHistory.AsReadOnly();


        private static string CapitaliseName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return name;
            return char.ToUpper(name[0]) + name.Substring(1).ToLower();
        }
    }
}