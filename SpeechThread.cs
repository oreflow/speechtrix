using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Speech.Synthesis;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Speechtrix
{
	class SpeechThread
	{
        SpeechSynthesizer ss;
		Speechtrix callBack;
        bool inverted;
        SpeechRecognitionEngine spEngine;
		public SpeechThread(Speechtrix _callBack)
		{
			callBack = _callBack;
            ss = new SpeechSynthesizer();
            PromptBuilder p = new PromptBuilder();
            p.StartStyle(new PromptStyle(PromptEmphasis.None));
            
            p.AppendText("Initializing speech component.");
            p.EndStyle();

            ss.SpeakAsync(p);
            
			Debug.Print("Playing first synthesis");

			spEngine = new SpeechRecognitionEngine(
					new System.Globalization.CultureInfo("en-US"));

			spEngine.UnloadAllGrammars();

			Grammar tetrisg = tetrisSrgs();
			
			Grammar xmlgrammar = new Grammar("DynamicSRGSDocument.xml");

			tetrisg.Enabled = true;
			Console.Write(tetrisg.Loaded);

			spEngine.UnloadAllGrammars();

			// Load the grammar to the SpeechRecognitionEngine.
			spEngine.LoadGrammar(tetrisg);

			//Grammar g = new Grammar(buildGrammar());
			listGrammars(spEngine);
			//g.Name = "Tetris";
			Console.Write(tetrisg.Loaded);
			//	spEngine.LoadGrammar(g);

			listGrammars(spEngine);

			spEngine.SpeechRecognized +=
				new EventHandler<SpeechRecognizedEventArgs>(sp_speechRecognized);

			spEngine.SpeechRecognitionRejected +=
				new EventHandler<SpeechRecognitionRejectedEventArgs>(sp_speechRejected);

			spEngine.SetInputToDefaultAudioDevice();

			spEngine.RecognizeAsync(RecognizeMode.Multiple);
			//       spRecognizer.Enabled = true;
			//      spRecognizer.SpeechRecognized +=
			//          new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
			//spRecognizer.EmulateRecognize("Right");

		}

		void sp_speechRejected(Object sender, SpeechRecognitionRejectedEventArgs args)
		{
			Debug.Print("Speech not recognized: " + args.Result.Text);
			callBack.notUnderstand();
		}

		void sp_speechRecognized(Object sender, SpeechRecognizedEventArgs args)
		{
			Console.Write("Speech recognized: " + args.Result.Text);
            if (args.Result.Text.Contains("Right"))
            {
                if (args.Result.Text.Equals("Right"))
                {
                    if (!inverted)
                        callBack.keyRight();
                    else
                        callBack.keyLeft();
                    callBack.understand();
                }
                else if (getInt(args.Result.Text.Split().First()) != 0 && // the first word in the recognized sentence is a number 
                        (args.Result.Text.Split().Length == 2 ||            // and the length is either 2 or 4
                         (args.Result.Text.Split().Length == 4 &&
                          args.Result.Text.Split().ElementAt(1).Equals("to") &&          //where word 2 (index 1) == "to" and word 3 (index 2) is "the" if the length was 4
                          args.Result.Text.Split().ElementAt(2).Equals("the"))
                        )
                    )             
                {
                    int number = getInt(args.Result.Text.Split().First());
                    for (int i = 0; i < number; i++)
                    {
                        if (!inverted)
                            callBack.keyRight();
                        else
                            callBack.keyLeft();
                    }
                        callBack.understand();
                }

            }
            else if (args.Result.Text.Contains("Left"))
            {
                if (args.Result.Text.Equals("Left"))
                {
                    if (!inverted)
                        callBack.keyLeft();
                    else
                        callBack.keyRight();
                    callBack.understand();
                }
                else if (getInt(args.Result.Text.Split().First()) != 0 && // the first word in the recognized sentence is a number
                        (args.Result.Text.Split().Length == 2 ||            // the length is either 2 or 4
                         (args.Result.Text.Split().Length == 4 &&
                          args.Result.Text.Split().ElementAt(1).Equals("to") &&          //where word 2 (index 1) == "to" and word 3 (index 2) is "the" if the length was 4
                          args.Result.Text.Split().ElementAt(2).Equals("the"))
                        )
                    )
                {
                    int number = getInt(args.Result.Text.Split().First());
                    for (int i = 0; i < number; i++)
                    {
                        if (!inverted)
                            callBack.keyLeft();
                        else
                            callBack.keyRight();
                    }
                    callBack.understand();
                }

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
            else if (args.Result.Text.Equals("Quit"))
            {
                callBack.quit();
            }
            else if (args.Result.Text.Equals("Crazy Shit"))
            {
                if (inverted) inverted = false;
                else inverted = true;
            }
		}

        public static int getInt(String str)
        {
            if (str.Equals("One"))
                return 1;
            else if (str.Equals("Two"))
                return 2;
            else if (str.Equals("Three"))
                return 3;
            else if (str.Equals("Four"))
                return 4;
            else if (str.Equals("Five"))
                return 5;
            else if (str.Equals("Six"))
                return 6;
            else if (str.Equals("Seven"))
                return 7;
            else if (str.Equals("Eight"))
                return 8;
            else if (str.Equals("Nine"))
                return 9;
            return 0;
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
        public void speak(String str)
        {
            PromptBuilder p = new PromptBuilder();
            p.StartStyle(new PromptStyle(PromptEmphasis.Strong));

            p.AppendText(str);
            p.EndStyle();

            ss.SpeakAsync(p);
        }
		private GrammarBuilder buildGrammar()
		{
			// lol, om man inte vill göra en så här simpel grammatik så använder man typ Srgs för att skapa ett VXML-dokument
			// se: http://www.c-sharpcorner.com/UploadFile/mahesh/programming-speech-in-wpf-speech-recognition/

			GrammarBuilder gBuilder = new GrammarBuilder();
			gBuilder.Append(new Choices("One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", " "));
			gBuilder.Append(new Choices("to the", " "));
			gBuilder.Append(new Choices("Right", "Left", "Down", "Rotate", "Quit", "Crazy Shit"));
			return gBuilder;
		}

		public void Run()
		{
			while (true)
			{
				break;
			}

		}


		private Grammar tetrisSrgs()
		{
			SrgsDocument document = new SrgsDocument();

			SrgsRule rootRule = new SrgsRule("RootRule");
			rootRule.Scope = SrgsRuleScope.Public;
			
			SrgsOneOf oneOfNumbers = new SrgsOneOf();
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "One"));
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "Two"));
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "Three"));
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "Four"));
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "Five"));
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "Six"));
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "Seven"));
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "Eight"));
			oneOfNumbers.Items.Add(new SrgsItem(0, 1, "Nine"));

			document.Mode = SrgsGrammarMode.Voice;
			document.Rules.Add(rootRule);
			document.Root = rootRule;

			SrgsRule ruleNumbers = new SrgsRule("Numb", oneOfNumbers);
			SrgsItem toThe = new SrgsItem(0, 1, "to the");
			SrgsOneOf direction = new SrgsOneOf(new SrgsItem("Left"), new SrgsItem("Right"));
			SrgsRule ruleDirection = new SrgsRule("Direction", direction);
			SrgsItem move = new SrgsItem(oneOfNumbers, toThe, direction);
			SrgsRule ruleMove = new SrgsRule("Moves");
			ruleMove.Scope = SrgsRuleScope.Public;
			ruleMove.Elements.Add(move);
			document.Rules.Add(ruleMove);

			SrgsOneOf oneOfCommands = new SrgsOneOf("Down", "Rotate", "Quit", "Crazy shit");
			SrgsItem commands = new SrgsItem(oneOfCommands);
			SrgsRule ruleCommands = new SrgsRule("Commands");
			ruleCommands.Scope = SrgsRuleScope.Public;
			ruleCommands.Elements.Add(commands);
			document.Rules.Add(ruleCommands);

			rootRule.Add(new SrgsRuleRef(ruleMove));

			SrgsOneOf ro = new SrgsOneOf(move, commands);
			SrgsItem rootcommands = new SrgsItem(ro);
			SrgsRule roo = new SrgsRule("Rootcommands");
			roo.Scope = SrgsRuleScope.Public;
			roo.Elements.Add(rootcommands);
			document.Rules.Add(roo);

			// Set Document Root
			document.Root = roo;
			
			// Save Created SRGS Document to XML file
					XmlWriter writer = XmlWriter.Create("DynamicSRGSDocument.xml");
					document.WriteSrgs(writer);
					writer.Close();
			
			return new Grammar(document);
		}
	}
}