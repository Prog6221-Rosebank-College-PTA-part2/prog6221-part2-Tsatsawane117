namespace CybersecurityBotWPF.Models
{
    public class ChatMessage
    {
        public string Sender { get; init; } = string.Empty;
        public string Text { get; init; } = string.Empty;
        public DateTime Timestamp { get; init; } = DateTime.Now;
        public bool IsBot { get; init; }
    }
}