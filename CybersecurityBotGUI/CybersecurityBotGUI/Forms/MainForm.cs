using CybersecurityBotGUI.Helpers;
using CybersecurityBotGUI.Memory;
using CybersecurityBotGUI.Responses;
using CybersecurityBotGUI.Sentiment;

namespace CybersecurityBotGUI.Forms
{

    public class MainForm : Form
    {

        private RichTextBox _chatDisplay = null!;
        private TextBox _inputBox = null!;
        private Button _sendButton = null!;
        private Label _asciiLabel = null!;
        private Panel _headerPanel = null!;
        private Panel _inputPanel = null!;
        private Label _statusLabel = null!;
        private Button _clearButton = null!;


        private readonly ResponseEngine _responseEngine;
        private readonly UserMemory _userMemory;
        private readonly SentimentDetector _sentimentDetector;
        private readonly AudioHelper _audioHelper;


        private bool _nameCollected = false;
        private string _lastTopic = string.Empty;


        private static readonly Color BgDark = Color.FromArgb(13, 17, 23);
        private static readonly Color BgMid = Color.FromArgb(22, 27, 34);
        private static readonly Color BgPanel = Color.FromArgb(30, 37, 46);
        private static readonly Color AccentCyan = Color.FromArgb(0, 210, 211);
        private static readonly Color AccentGreen = Color.FromArgb(56, 211, 159);
        private static readonly Color AccentYellow = Color.FromArgb(255, 213, 79);
        private static readonly Color TextWhite = Color.FromArgb(230, 237, 243);
        private static readonly Color TextGray = Color.FromArgb(139, 148, 158);
        private static readonly Color BotMsgColor = Color.FromArgb(33, 43, 54);
        private static readonly Color UserMsgColor = Color.FromArgb(20, 40, 60);

        public MainForm()
        {
            _responseEngine = new ResponseEngine();
            _userMemory = new UserMemory();
            _sentimentDetector = new SentimentDetector();
            _audioHelper = new AudioHelper();

            InitialiseComponents();
            PlayVoiceGreetingAsync();
            ShowWelcomeMessage();
        }



        private void InitialiseComponents()
        {

            Text = "🔒 Cybersecurity Awareness Bot";
            Size = new Size(900, 700);
            MinimumSize = new Size(700, 550);
            BackColor = BgDark;
            ForeColor = TextWhite;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Consolas", 10f, FontStyle.Regular);


            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 160,
                BackColor = BgMid,
                Padding = new Padding(10, 8, 10, 8)
            };

            _asciiLabel = new Label
            {
                Text = GetAsciiLogo(),
                Font = new Font("Consolas", 7f, FontStyle.Bold),
                ForeColor = AccentCyan,
                BackColor = Color.Transparent,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };

            _headerPanel.Controls.Add(_asciiLabel);


            _statusLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Color.FromArgb(18, 22, 28),
                ForeColor = AccentGreen,
                Font = new Font("Consolas", 8.5f),
                Text = "  🟢  Bot online  |  Type your message and press Enter or Send",
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0)
            };


            _chatDisplay = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = BgDark,
                ForeColor = TextWhite,
                Font = new Font("Consolas", 10.5f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Padding = new Padding(12),
                WordWrap = true
            };


            _inputPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = BgPanel,
                Padding = new Padding(10, 8, 10, 8)
            };

            _inputBox = new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(22, 27, 34),
                ForeColor = TextWhite,
                Font = new Font("Consolas", 11f),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Ask me about cybersecurity...",
            };
            _inputBox.KeyDown += InputBox_KeyDown;

            _sendButton = new Button
            {
                Text = "Send  ➤",
                Dock = DockStyle.Right,
                Width = 110,
                BackColor = AccentCyan,
                ForeColor = BgDark,
                Font = new Font("Consolas", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _sendButton.FlatAppearance.BorderSize = 0;
            _sendButton.Click += SendButton_Click;

            _clearButton = new Button
            {
                Text = "Clear",
                Dock = DockStyle.Right,
                Width = 70,
                BackColor = BgMid,
                ForeColor = TextGray,
                Font = new Font("Consolas", 9f),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 6, 0)
            };
            _clearButton.FlatAppearance.BorderSize = 0;
            _clearButton.Click += (s, e) => _chatDisplay.Clear();

            _inputPanel.Controls.Add(_inputBox);
            _inputPanel.Controls.Add(_sendButton);
            _inputPanel.Controls.Add(_clearButton);

            // ── Assemble layout ───────────────────────────────────────────────────
            Controls.Add(_chatDisplay);
            Controls.Add(_statusLabel);
            Controls.Add(_inputPanel);
            Controls.Add(_headerPanel);

            _inputBox.Select();
        }



        private static string GetAsciiLogo()
        {
            return
                "  ██████╗██╗   ██╗██████╗ ███████╗██████╗      ██████╗  ██████╗ ████████╗\r\n" +
                " ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗     ██╔══██╗██╔═══██╗╚══██╔══╝\r\n" +
                " ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝     ██████╔╝██║   ██║   ██║   \r\n" +
                " ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗     ██╔══██╗██║   ██║   ██║   \r\n" +
                " ╚██████╗   ██║   ██████╔╝███████╗██║  ██║ ███╗██████╔╝╚██████╔╝   ██║   \r\n" +
                "  ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝ ╚══╝╚═════╝  ╚═════╝    ╚═╝  \r\n" +
                "              🔒  Cybersecurity Awareness Bot  |  Keeping You Safe Online 🛡️";
        }



        private void PlayVoiceGreetingAsync()
        {
            Task.Run(() => _audioHelper.PlayVoiceGreeting());
        }

        private void ShowWelcomeMessage()
        {
            AppendBotMessage(
                "Welcome to the Cybersecurity Awareness Bot!\n" +
                "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                "I'm here to help you stay safe online.\n\n" +
                "Before we begin — what's your name?");
        }



        private void InputBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ProcessInput();
            }
        }

        private void SendButton_Click(object? sender, EventArgs e)
        {
            ProcessInput();
        }



        private void ProcessInput()
        {
            string input = _inputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            _inputBox.Clear();
            AppendUserMessage(input);

            // Name collection on first message
            if (!_nameCollected)
            {
                _userMemory.SetName(input);
                _nameCollected = true;
                AppendBotMessage(
                    $"Nice to meet you, {_userMemory.Name}! 😊\n\n" +
                    "I can help you with topics like:\n" +
                    "  🔑  Password safety & managers\n" +
                    "  🎣  Phishing & scams\n" +
                    "  🛡️  Malware & ransomware\n" +
                    "  🔐  Encryption & 2FA\n" +
                    "  📶  Public Wi-Fi & VPNs\n" +
                    "  🕵️  Social engineering\n" +
                    "  ...and much more!\n\n" +
                    "Type 'help' for a full topic list, or just ask away!");
                return;
            }


            if (IsFollowUpRequest(input))
            {
                HandleFollowUp();
                return;
            }


            SentimentResult sentiment = _sentimentDetector.Detect(input);
            string sentimentPrefix = BuildSentimentPrefix(sentiment);


            string response = _responseEngine.GetResponse(input, _userMemory.Name, _userMemory);


            string? detectedTopic = _responseEngine.GetLastDetectedTopic();
            if (detectedTopic != null)
            {
                _lastTopic = detectedTopic;
                _userMemory.SetFavouriteTopic(detectedTopic);
            }

            // Compose final reply
            string fullReply = string.IsNullOrEmpty(sentimentPrefix)
                ? response
                : sentimentPrefix + "\n\n" + response;

            AppendBotMessage(fullReply);
        }



        private bool IsFollowUpRequest(string input)
        {
            string lower = input.ToLower();
            return lower.Contains("tell me more") || lower.Contains("explain more") ||
                   lower.Contains("give me another") || lower.Contains("another tip") ||
                   lower.Contains("more info") || lower.Contains("more details") ||
                   lower.Contains("go on") || lower.Contains("continue");
        }

        private void HandleFollowUp()
        {
            if (string.IsNullOrEmpty(_lastTopic))
            {
                AppendBotMessage($"I'm not sure which topic to expand on, {_userMemory.Name}. " +
                                 "Could you mention the topic again? (e.g. 'tell me more about phishing')");
                return;
            }

            string moreInfo = _responseEngine.GetFollowUp(_lastTopic, _userMemory.Name);
            AppendBotMessage($"Here's more on {_lastTopic}, {_userMemory.Name}:\n\n{moreInfo}");
        }



        private string BuildSentimentPrefix(SentimentResult sentiment)
        {
            return sentiment.Type switch
            {
                SentimentType.Worried =>
                    $"I completely understand feeling worried, {_userMemory.Name} — " +
                    "cybersecurity can feel overwhelming. Let me help put your mind at ease. 💙",
                SentimentType.Frustrated =>
                    $"I hear you, {_userMemory.Name} — this stuff can be frustrating! " +
                    "Let's work through it together. 💪",
                SentimentType.Curious =>
                    $"Great curiosity, {_userMemory.Name}! That's exactly the right mindset " +
                    "for staying safe online. 🌟",
                SentimentType.Happy =>
                    $"Love the energy, {_userMemory.Name}! Let's keep it up. 😄",
                SentimentType.Confused =>
                    $"No worries at all, {_userMemory.Name} — these topics can be confusing. " +
                    "I'll explain it as clearly as possible. 🙂",
                _ => string.Empty
            };
        }



        private void AppendUserMessage(string text)
        {
            _chatDisplay.SelectionStart = _chatDisplay.TextLength;
            _chatDisplay.SelectionLength = 0;

            _chatDisplay.SelectionColor = AccentYellow;
            _chatDisplay.AppendText($"\n  [{DateTime.Now:HH:mm}]  {_userMemory.Name ?? "You"}\n");

            // Message bubble background simulation
            _chatDisplay.SelectionColor = TextWhite;
            _chatDisplay.SelectionBackColor = UserMsgColor;
            _chatDisplay.AppendText($"  {text}\n");
            _chatDisplay.SelectionBackColor = BgDark;

            _chatDisplay.AppendText("\n");
            ScrollToBottom();
        }

        private void AppendBotMessage(string text)
        {
            _chatDisplay.SelectionStart = _chatDisplay.TextLength;
            _chatDisplay.SelectionLength = 0;

            // Bot label
            _chatDisplay.SelectionColor = AccentCyan;
            _chatDisplay.AppendText($"\n  [{DateTime.Now:HH:mm}]  🤖 CyberBot\n");

            // Message
            _chatDisplay.SelectionColor = TextWhite;
            _chatDisplay.SelectionBackColor = BotMsgColor;

            foreach (string line in text.Split('\n'))
                _chatDisplay.AppendText($"  {line}\n");

            _chatDisplay.SelectionBackColor = BgDark;
            _chatDisplay.AppendText("\n");
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            _chatDisplay.SelectionStart = _chatDisplay.TextLength;
            _chatDisplay.ScrollToCaret();
        }
    }
}