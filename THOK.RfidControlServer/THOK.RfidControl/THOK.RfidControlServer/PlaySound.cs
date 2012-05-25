using System;
using System.Collections.Generic;
using System.Text;
using SpeechLib;

namespace THOK.RfidControlServer
{
    public static class PlaySound
    {
        private static SpVoiceClass m_SpVoice = new SpVoiceClass();

        public static void Play(string fileName)
        {
            m_SpVoice.Speak("", SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);   
            fileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            if (!System.IO.File.Exists(fileName))
            {
                return;
            }
            SpFileStreamClass spFs = new SpFileStreamClass();
            spFs.Open(fileName, SpeechStreamFileMode.SSFMOpenForRead,true);
            ISpeechBaseStream Istream = spFs as ISpeechBaseStream;
            m_SpVoice.SpeakStream(Istream, SpeechLib.SpeechVoiceSpeakFlags.SVSFIsFilename);
        }

        public static void PlayAsync(string fileName)
        {
            fileName = AppDomain.CurrentDomain.BaseDirectory + fileName;
            if (!System.IO.File.Exists(fileName))
            {
                return;
            }
            SpFileStreamClass spFs = new SpFileStreamClass();
            spFs.Open(fileName, SpeechStreamFileMode.SSFMOpenForRead, true);
            ISpeechBaseStream Istream = spFs as ISpeechBaseStream;
            m_SpVoice.SpeakStream(Istream, SpeechLib.SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        public static void Say(string s)
        {
            m_SpVoice.Speak("", SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            m_SpVoice.Speak(string.Format("<lang langid='804'>{0}</lang>", s), SpeechVoiceSpeakFlags.SVSFDefault);
        }

        public static void SayAsync(string s)
        {
            m_SpVoice.Speak(string.Format("<lang langid='804'>{0}</lang>", s), SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        public static void SayStop()
        {
            m_SpVoice.Speak("", SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }
    }
}
