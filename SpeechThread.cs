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
            
            Grammar g = new Grammar(buildGrammar());
            spRecognizer.UnloadAllGrammars();
            spRecognizer.LoadGrammar(g);
            

            spRecognizer.Enabled = true;

            spRecognizer.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
            //spRecognizer.EmulateRecognize("Right");
            

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
            

            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(new Choices("Right", "Left", "Down", "Rotate"));
            return gBuilder;
        }

        private GrammarBuilder buildGrammar2()
        {
            /*
             * Speech Recognition Grammar Specification (SRGS) is a W3C recommendation
             * to build grammar that is used in speech enabled applications.
             * More details about SRGS can be found at http://www.w3.org/TR/speech-grammar/.
             */

            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(new Choices("Right", "Left", "Down", "Rotate"));
            return gBuilder;
        }

    }
}
