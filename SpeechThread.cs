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
		Speechtrix callBack;
		Boolean inverted = false;
		public SpeechThread(Speechtrix _callBack)
		{
			callBack = _callBack;
			SpeechSynthesizer ss = new SpeechSynthesizer();
			ss.SpeakAsync("Initializing speech component.");
			Debug.Print("Playing first synthesis");

			SpeechRecognitionEngine spEngine =
				new SpeechRecognitionEngine(
					new System.Globalization.CultureInfo("en-US"));

			spEngine.UnloadAllGrammars();

			Grammar tetrisg = tetrisSrgs();
			
			Grammar xmlgrammar = new Grammar(@"C:\Users\Jesper\Dropbox\Visual 2012 workspace\Projects\Speechtrix\speechtrix\bin\Debug\DynamicSRGSDocument.xml");

			tetrisg.Enabled = true;
			Console.Write(tetrisg.Loaded);

			spEngine.UnloadAllGrammars();

			// Load the grammar to the SpeechRecognitionEngine.
			spEngine.LoadGrammar(tetrisg);

			Grammar g = new Grammar(buildGrammar());
			listGrammars(spEngine);
			g.Name = "Tetris";
			Console.Write(tetrisg.Loaded);
			//	spEngine.LoadGrammar(g);

			listGrammars(spEngine);

			spEngine.SpeechRecognized +=
				new EventHandler<SpeechRecognizedEventArgs>(sp_speechRecognized);

			spEngine.SpeechRecognitionRejected +=
				new EventHandler<SpeechRecognitionRejectedEventArgs>(sp_speechRejected);

			spEngine.SetInputToDefaultAudioDevice();

			spEngine.RecognizeAsync(RecognizeMode.Multiple);
		}

		void sp_speechRejected(Object sender, SpeechRecognitionRejectedEventArgs args)
		{
			Debug.Print("Speech not recognized: " + args.Result.Text);
			callBack.notUnderstand();
		}

		void sp_speechRecognized(Object sender, SpeechRecognizedEventArgs args)
		{
			Debug.Print("Speech recognized: " + args.Result.Text);
			
			if (args.Result.Text.Equals("Right"))
			{
				if (!inverted)
					callBack.keyRight();
				else
					callBack.keyLeft();
				callBack.understand();
			}
			else if (args.Result.Text.Equals("Left"))
			{
				if (!inverted)
					callBack.keyLeft();
				else
					callBack.keyRight();
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
			oneOfNumbers.Items.Add(new SrgsItem("One"));
			oneOfNumbers.Items.Add(new SrgsItem("Two"));
			oneOfNumbers.Items.Add(new SrgsItem("Three"));
			oneOfNumbers.Items.Add(new SrgsItem("Four"));
			oneOfNumbers.Items.Add(new SrgsItem("Five"));
			oneOfNumbers.Items.Add(new SrgsItem("Six"));
			oneOfNumbers.Items.Add(new SrgsItem("Seven"));
			oneOfNumbers.Items.Add(new SrgsItem("Eight"));
			oneOfNumbers.Items.Add(new SrgsItem("Nine"));

			document.Mode = SrgsGrammarMode.Voice;
			document.Rules.Add(rootRule);
			document.Root = rootRule;

			SrgsRule ruleNumbers = new SrgsRule("Numb", oneOfNumbers);
			SrgsItem toThe = new SrgsItem(0, 1, "to the");
			SrgsOneOf direction = new SrgsOneOf(new SrgsItem("Left"), new SrgsItem("Right"));
			SrgsRule ruleDirection = new SrgsRule("Direction", direction);
			SrgsItem howmuch = new SrgsItem(0,1,oneOfNumbers, toThe);
			SrgsItem move = new SrgsItem(howmuch, direction);
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