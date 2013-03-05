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

            
     //       SpeechRecognizer spRecognizer = new SpeechRecognizer();
			SpeechRecognitionEngine spEngine =
				new SpeechRecognitionEngine(
					new System.Globalization.CultureInfo("en-US"));

				Grammar g = new Grammar(buildGrammar());
				listGrammars(spEngine);
				g.Name = "Tetris";

				spEngine.UnloadAllGrammars();
				spEngine.LoadGrammar(g);

				listGrammars(spEngine);

				spEngine.SpeechRecognized +=
					new EventHandler<SpeechRecognizedEventArgs>(spEngine_SpeechRecognized);

				spEngine.SetInputToDefaultAudioDevice();

				spEngine.RecognizeAsync(RecognizeMode.Multiple);
				//       spRecognizer.Enabled = true;
				//      spRecognizer.SpeechRecognized +=
				//          new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
				//spRecognizer.EmulateRecognize("Right");

        }


        void spEngine_SpeechRecognized( Object sender, SpeechRecognizedEventArgs args)
        {
            Debug.Print("Speech recognized: " + args.Result.Text);

            if(args.Result.Text.Equals("Right"))
            {
                callBack.keyRight();
				callBack.understand();
            }
            else if (args.Result.Text.Equals("Left"))
            {
                callBack.keyLeft();
				callBack.understand();
            }
            else if (args.Result.Text.Equals("Down"))
            {
                callBack.keyDown();
				callBack.understand();
            }
			else if (args.Result.Text.Equals("Rotate"))
			{
				callBack.keyUp();
				callBack.understand();
			}
			else
			{
			//	callBack.notUnderstand();
			}

        }
		private static void listGrammars(SpeechRecognitionEngine recognizer)
		{
			// Make a copy of the recognizer's grammar collection.
			List<Grammar> loadedGrammars = new List<Grammar>(recognizer.Grammars);

			if (loadedGrammars.Count > 0)
			{
				Console.WriteLine("Loaded grammars:");
				foreach (Grammar g in recognizer.Grammars)
				{
					Console.WriteLine(" - {0}", g.Name);
				}
			}
			else
			{
				Console.WriteLine("No grammars loaded.");
			}
			Console.WriteLine();
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
