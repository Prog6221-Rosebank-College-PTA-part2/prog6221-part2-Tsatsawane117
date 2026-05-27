namespace CybersecurityBotGUI.Helpers
{

    public class AudioHelper
    {
        private const string VoiceGreetingPath = "C:\\Users\\nekho\\source\\repos\\CybersecurityBotGUI\\CybersecurityBotGUI\\greeting.wav";

        public void PlayVoiceGreeting()
        {
            try
            {
                if (!File.Exists(VoiceGreetingPath)) return;

                if (OperatingSystem.IsWindows())
                    PlayOnWindows(VoiceGreetingPath);
            }
            catch
            {
         
            }
        }

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        private static void PlayOnWindows(string path)
        {
            using var player = new System.Media.SoundPlayer(path);
            player.PlaySync();
        }
    }
}
