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
        Speechtrix callBack;
        public SpeechThread(Speechtrix _callBack)
        {
            callBack = _callBack;
            SpeechSynthesizer ss = new SpeechSynthesizer();
            ss.SpeakAsync("Initializing speech component.");
            Debug.Print("Playing first synthesis");

            
            SpeechRecognizer spRecognizer = new SpeechRecognizer();
            spRecognizer.Enabled = true;
            Grammar g = new Grammar(buildGrammar());
            spRecognizer.LoadGrammar(g);

            spRecognizer.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);

        }

        void SpeechRecognized( Object sender, SpeechRecognizedEventArgs args)
        {
            Debug.Print("Speech recognized: " + args.Result.Text);

            if(args.Result.Text.Equals("Right"))
            {
                callBack.keyRight();
            }
            else if (args.Result.Text.Equals("Left"))
            {
                callBack.keyLeft();
            }
            else if (args.Result.Text.Equals("Down"))
            {
                callBack.keyDown();
            }
            else if (args.Result.Text.Equals("Rotate"))
            {
                callBack.keyUp();
            }
        }

        private GrammarBuilder buildGrammar()
        {
            // lol, om man inte vill göra en så här simpel grammatik så använder man typ Srgs för att skapa ett VXML-dokument
            // se: http://www.c-sharpcorner.com/UploadFile/mahesh/programming-speech-in-wpf-speech-recognition/

            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(new Choices("Right", "Left", "Down", "Rotate"));
            return gBuilder;
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
