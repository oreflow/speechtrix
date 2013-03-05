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

namespace Speechtrix
{
	class SpeechThread
	{
        SpeechSynthesizer ss;
		Speechtrix callBack;
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

			Grammar tetrisg = tetrisSrgs();

			tetrisg.Enabled = true;
			Console.Write(tetrisg.Loaded);
			// Load the grammar to the SpeechRecognitionEngine.
			spEngine.LoadGrammar(tetrisg);

			Grammar g = new Grammar(buildGrammar());
			listGrammars(spEngine);
			g.Name = "Tetris";
			Console.Write(tetrisg.Loaded);
			spEngine.UnloadAllGrammars();
			//	spEngine.LoadGrammar(g);
			Console.Write(spEngine.BabbleTimeout);

			listGrammars(spEngine);

			spEngine.SpeechRecognized +=
				new EventHandler<SpeechRecognizedEventArgs>(sp_speechRecognized);

			spEngine.SpeechRecognitionRejected +=
				new EventHandler<SpeechRecognitionRejectedEventArgs>(sp_speechRejected);

			spEngine.SetInputToDefaultAudioDevice();

			spEngine.RecognizeAsync();
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
			// Create SrgsDocument
			SrgsDocument document = new SrgsDocument();

			// Create Root Rule
			SrgsRule rootRule = new SrgsRule("Tetriscommands");
			rootRule.Scope = SrgsRuleScope.Public;

			rootRule.Elements.Add(new SrgsItem("Left Right Grammar "));

			SrgsItem elementItem = new SrgsItem();
			SrgsRule elementRule = new SrgsRule("ElementRule");
			elementRule.Elements.Add(elementItem);
			document.Rules.Add(elementRule);

			SrgsOneOf oneOfNumbers = new SrgsOneOf(
				new SrgsItem("One"),
				new SrgsItem("Two"),
				new SrgsItem("Three"),
				new SrgsItem("Four"),
				new SrgsItem("Five"),
				new SrgsItem("Six"),
				new SrgsItem("Seven"),
				new SrgsItem("Eight"),
				new SrgsItem("Nine")
			);
			SrgsRule ruleNumbers = new SrgsRule("Numb", oneOfNumbers);
			SrgsItem toThe = new SrgsItem(0, 1, "to the");
			SrgsOneOf direction = new SrgsOneOf(new SrgsItem("Left"), new SrgsItem("Right"));
			SrgsRule ruleDirection = new SrgsRule("Direction", direction);
			SrgsItem move = new SrgsItem(oneOfNumbers, toThe, direction);
			SrgsRule ruleMove = new SrgsRule("Moves", move);

			SrgsOneOf oneOfCommands = new SrgsOneOf("Down", "Rotate", "Quit", "Crazy shit");
			SrgsItem commands = new SrgsItem(oneOfCommands);
			SrgsRule ruleCommands = new SrgsRule("Commands", commands);

			// Add items to root Rule
			rootRule.Elements.Add(move);
			rootRule.Elements.Add(commands);

			// Add all Rules to Document
			document.Rules.Add(rootRule, ruleMove, ruleCommands);
			// Add some extra sperate Rules
			SrgsText textItem = new SrgsText("Start of the Document.");
			SrgsRule textRule = new SrgsRule("TextItem");
			textRule.Elements.Add(textItem);
			document.Rules.Add(textRule);

			SrgsItem stringItem = new SrgsItem("Item as String.");
			SrgsRule itemRule = new SrgsRule("ItemRule");
			itemRule.Elements.Add(stringItem);
			document.Rules.Add(itemRule);


			// Set Document Root
			document.Root = rootRule;

			// Save Created SRGS Document to XML file
			//		XmlWriter writer = XmlWriter.Create("DynamicSRGSDocument.xml");
			//		document.WriteSrgs(writer);
			//		writer.Close();

			return new Grammar(document, "Tetriscommands");
		}
		private void compileGrammar(SrgsDocument srgsDoc, string cfgPath)
		{
			FileStream fs = new FileStream(cfgPath, FileMode.Create);
			SrgsGrammarCompiler.Compile(srgsDoc, (Stream)fs);
			fs.Close();
		}
	}
}