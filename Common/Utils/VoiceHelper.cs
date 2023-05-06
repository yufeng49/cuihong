using System.Speech.Synthesis;

namespace Common.Utils
{
    public class VoiceHelper
    {
        private static SpeechSynthesizer ss;

        public static void Speak(string word)
        {
            ss = new SpeechSynthesizer();
            ss.Rate = 0;// -10 -- 10
            ss.Volume = 100;// 0 -- 100
            ss.SpeakAsync(word);
        }

    }
}
