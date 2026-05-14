 Cybersecurity Awareness Bot — Part 2 (GUI)

A WinForms C# chatbot that educates users on cybersecurity through an interactive, polished GUI.

Features

| Feature | Description |
|---|---|
| GUI Interface | WinForms dark-themed chat window with ASCII art header |
| Voice Greeting | Plays `greeting.wav` on startup via `System.Media` |
| Keyword Recognition | 20+ cybersecurity topics with targeted responses |
| Random Responses | Multiple response variants per topic for natural variety |
| Conversation Flow | Follow-up questions ("tell me more") expand on any topic |
| Memory & Recall | Remembers user name and favourite topic; references them later |
| Sentiment Detection | Detects worried, frustrated, curious, happy, confused moods and adapts tone |
| Error Handling | Graceful fallback for all unknown inputs |
| Code Structure | Modular: Forms / Helpers / Responses / Memory / Sentiment folders |

## Project Structure


CybersecurityBotGUI/
├── Program.cs
├── CybersecurityBotGUI.csproj
├── greeting.wav                  ← Add your own recording
├── Forms/
│   └── MainForm.cs               ← Main GUI window
├── Helpers/
│   └── AudioHelper.cs            ← WAV playback
├── Memory/
│   └── UserMemory.cs             ← Stores name, topic, message count
├── Responses/
│   └── ResponseEngine.cs         ← Keyword matching, random responses, follow-ups
└── Sentiment/
    └── SentimentDetector.cs      ← Mood detection and empathetic responses


Requirements

- .NET 8.0 (Windows)
- Visual Studio 2022

 Conversation Examples

Keyword recognition:
> User: "Tell me about phishing"
> Bot: *(randomly selects from 3 phishing responses)*

Follow-up flow:
> User: "Tell me more"
> Bot: *(expands on the last topic discussed)*

Sentiment detection:
> User: "I'm really worried about online scams"
> Bot: "I completely understand feeling worried — cybersecurity can feel overwhelming. Let me help put your mind at ease. ..."

Memory recall:
> Bot: *(after several messages)* "As someone interested in phishing, you might find this especially useful."

CI Workflow

GitHub Actions automatically builds the project on every push.

![CI]()
