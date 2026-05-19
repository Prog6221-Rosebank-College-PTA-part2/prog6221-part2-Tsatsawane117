 Cybersecurity Awareness Bot — Part 2 (WPF)

A WPF C# chatbot that educates users on cybersecurity through an interactive dark-themed GUI.

 Features
- 🎙️ Voice greeting on startup
- 🖼️ ASCII art logo header
- 👤 Personalised responses using your name
- 💬 20+ cybersecurity topics with varied responses
- 🧠 Memory — remembers your name and interests
- 😟 Sentiment detection — adapts tone to your mood
- 🔁 Follow-up flow — type "tell me more" to expand any topic
- ✅ Input validation and graceful error handling

 Topics Covered
Passwords, phishing, malware, ransomware, 2FA, encryption, VPNs, firewalls, social engineering, data breaches, identity theft, cloud security, privacy, safe browsing, and more.

 How to Run
1. Clone the repository
2. Open in Visual Studio 2022
3. Add your `greeting.wav` to the project root and set **Copy to Output Directory → Copy if newer**
4. Press **F5** to run

Requirements
- .NET 8.0 (Windows)
- Visual Studio 2022

## CI
This project uses GitHub Actions to automatically build and validate on every push.

![CI](https://github.com/YOUR-USERNAME/CybersecurityBotWPF/actions/workflows/dotnet.yml/badge.svg)
