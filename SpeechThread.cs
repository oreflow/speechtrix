using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Diagnostics;

namespace Speechtrix
{
    class SpeechThread
    {

        public SpeechThread(Speechtrix callBack)
        {
            SpeechSynthesizer ss = new SpeechSynthesizer();
            ss.SpeakAsync("Initializing speech component.");
            Debug.Print("Playing first synthesis");
        }

        public void Run()
        {
            while (true)
            {
                break;
            }

        }
    }
}
