using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using CybersecurityBotWPF.Helpers;
using CybersecurityBotWPF.Memory;
using CybersecurityBotWPF.Responses;
using CybersecurityBotWPF.Sentiment;

namespace CybersecurityBotWPF.Views
{

    public partial class MainWindow : Window
    {
        // Services 
        private readonly ResponseEngine _responseEngine;
        private readonly UserMemory _userMemory;
        private readonly SentimentDetector _sentimentDetector;
        private readonly AudioHelper _audioHelper;

        // State 
        private bool _nameCollected = false;
        private string _lastTopic = string.Empty;

        // Brushes 
        private static readonly SolidColorBrush CyanBrush = new(Color.FromRgb(0, 210, 211));
        private static readonly SolidColorBrush YellowBrush = new(Color.FromRgb(255, 213, 79));
        private static readonly SolidColorBrush WhiteBrush = new(Color.FromRgb(230, 237, 243));
        private static readonly SolidColorBrush BotBg = new(Color.FromRgb(26, 36, 53));
        private static readonly SolidColorBrush UserBg = new(Color.FromRgb(15, 42, 61));

        public MainWindow()
        {
            InitializeComponent();
            _responseEngine = new ResponseEngine();
            _userMemory = new UserMemory();
            _sentimentDetector = new SentimentDetector();
            _audioHelper = new AudioHelper();
            Loaded += MainWindow_Loaded;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() => _audioHelper.PlayVoiceGreeting());
            ShowWelcomeMessage();
            InputBox.Focus();
        }

        private void ShowWelcomeMessage()
        {
            AppendBotMessage(
                "Welcome to the Cybersecurity Awareness Bot!\n" +
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                "I am here to help you stay safe online.\n\n" +
                "Before we begin — what is your name?");
        }


        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ProcessInput();
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e) => ProcessInput();

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Children.Clear();
            HideSentimentBar();
        }


        private void ProcessInput()
        {
            string input = InputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            InputBox.Clear();
            AppendUserMessage(input);

            if (!_nameCollected)
            {
                _userMemory.SetName(input);
                _nameCollected = true;
                AppendBotMessage(
                    $"Nice to meet you, {_userMemory.Name}!\n\n" +
                    "I can help you with topics like:\n" +
                    "  [*] Passwords and password managers\n" +
                    "  [*] Phishing, smishing and vishing\n" +
                    "  [*] Malware and ransomware\n" +
                    "  [*] Encryption and 2FA\n" +
                    "  [*] Public Wi-Fi and VPNs\n" +
                    "  [*] Social engineering\n" +
                    "  [*] Cloud security and data breaches\n\n" +
                    "Type 'help' for a full list, or just ask away!");
                return;
            }

            if (IsFollowUpRequest(input))
            {
                HandleFollowUp();
                return;
            }

            SentimentResult sentiment = _sentimentDetector.Detect(input);
            ShowSentimentIndicator(sentiment);

            string sentimentPrefix = BuildSentimentPrefix(sentiment);
            string response = _responseEngine.GetResponse(input, _userMemory.Name, _userMemory);

            // Store detected topic for follow-up and memory
            string? detectedTopic = _responseEngine.GetLastDetectedTopic();
            if (detectedTopic != null)
            {
                _lastTopic = detectedTopic;
                _userMemory.SetFavouriteTopic(detectedTopic);
            }

            string fullReply = string.IsNullOrEmpty(sentimentPrefix)
                ? response
                : sentimentPrefix + "\n\n" + response;

            AppendBotMessage(fullReply);
        }

        private static bool IsFollowUpRequest(string input)
        {
            string lower = input.ToLower();
            return lower.Contains("tell me more") || lower.Contains("explain more") ||
                   lower.Contains("give me another") || lower.Contains("another tip") ||
                   lower.Contains("more info") || lower.Contains("more details") ||
                   lower.Contains("go on") || lower.Contains("continue") ||
                   lower.Contains("keep going") || lower.Contains("expand");
        }

        private void HandleFollowUp()
        {
            HideSentimentBar();
            if (string.IsNullOrEmpty(_lastTopic))
            {
                AppendBotMessage(
                    $"I am not sure which topic to expand on, {_userMemory.Name}. " +
                    "Could you mention the topic? " +
                    "For example: 'tell me more about phishing'.");
                return;
            }
            string moreInfo = _responseEngine.GetFollowUp(_lastTopic, _userMemory.Name);
            AppendBotMessage($"Here is more on {_lastTopic}, {_userMemory.Name}:\n\n{moreInfo}");
        }


        private string BuildSentimentPrefix(SentimentResult sentiment) =>
            sentiment.Type switch
            {
                SentimentType.Worried =>
                    $"I completely understand feeling worried, {_userMemory.Name} — " +
                    "cybersecurity can feel overwhelming. Let me help put your mind at ease.",
                SentimentType.Frustrated =>
                    $"I hear you, {_userMemory.Name} — this can be frustrating! " +
                    "Let us work through it together.",
                SentimentType.Curious =>
                    $"Great curiosity, {_userMemory.Name}! " +
                    "That is exactly the right mindset for staying safe online.",
                SentimentType.Happy =>
                    $"Love the positive energy, {_userMemory.Name}!",
                SentimentType.Confused =>
                    $"No worries at all, {_userMemory.Name} — " +
                    "I will explain this as clearly as possible.",
                _ => string.Empty
            };

        private void ShowSentimentIndicator(SentimentResult sentiment)
        {
            if (sentiment.Type == SentimentType.Neutral)
            {
                HideSentimentBar();
                return;
            }

            MoodLabel.Text = $"Mood: {sentiment.Label}";

            SentimentText.Text = sentiment.Type switch
            {
                SentimentType.Worried => "Mood detected: Worried — responding with extra support",
                SentimentType.Frustrated => "Mood detected: Frustrated — keeping it simple and clear",
                SentimentType.Curious => "Mood detected: Curious — giving a detailed answer",
                SentimentType.Happy => "Mood detected: Happy — keeping the good vibes!",
                SentimentType.Confused => "Mood detected: Confused — breaking it down step by step",
                _ => string.Empty
            };

            SentimentBar.Visibility = Visibility.Visible;
        }

        private void HideSentimentBar()
        {
            SentimentBar.Visibility = Visibility.Collapsed;
            MoodLabel.Text = string.Empty;
        }


        private void AppendUserMessage(string text)
        {
            ChatPanel.Children.Add(CreateBubble(
                label: $"[{DateTime.Now:HH:mm}]  {_userMemory.Name ?? "You"}",
                message: text,
                labelBrush: YellowBrush,
                bgBrush: UserBg,
                align: HorizontalAlignment.Right));
            ScrollToBottom();
        }

        private void AppendBotMessage(string text)
        {
            ChatPanel.Children.Add(CreateBubble(
                label: $"[{DateTime.Now:HH:mm}]  CyberBot",
                message: text,
                labelBrush: CyanBrush,
                bgBrush: BotBg,
                align: HorizontalAlignment.Left));
            ScrollToBottom();
        }

        private static Border CreateBubble(
            string label, string message,
            SolidColorBrush labelBrush, SolidColorBrush bgBrush,
            HorizontalAlignment align)
        {
            var labelBlock = new TextBlock
            {
                Text = label,
                Foreground = labelBrush,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 10,
                Margin = new Thickness(0, 0, 0, 5)
            };

            var msgBlock = new TextBlock
            {
                Text = message,
                Foreground = WhiteBrush,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20
            };

            var stack = new StackPanel();
            stack.Children.Add(labelBlock);
            stack.Children.Add(msgBlock);

            var bubble = new Border
            {
                Background = bgBrush,
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(16, 12, 16, 12),
                Margin = new Thickness(10, 5, 10, 5),
                MaxWidth = 680,
                HorizontalAlignment = align,
                Child = stack
            };

            // Fade-in animation
            bubble.Opacity = 0;
            var anim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(250)));
            bubble.BeginAnimation(OpacityProperty, anim);

            return bubble;
        }

       
        private void ScrollToBottom()
        {
            ChatScroller.UpdateLayout();
            ChatScroller.ScrollToBottom();
        }
    }
}