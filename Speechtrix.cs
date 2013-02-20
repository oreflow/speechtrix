﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Speechtrix
{
    class Speechtrix
    {
        const int maxSizeRow = 32;
        const int maxSizeCol = 64;
		const int nextLevelLines = 20;
        
        bool[,] gamefield; //matris med positioner, där vi använder 16(?) som standardbredd (position (0,0) är högst upp i vänstra hörnet)
        float speed; //hastighet för fallande block i ms(?)
        int level; //kanske för reglera speed/hur mycket poäng man får/vilka block som kommer
        long score;
		int starttime; //set when starting a game
		int linesToNextLevel; //lines needed to be removed until reaching next level
		int startCol, startRow, endCol, endRow, height, width;
		Graphics g;

        public Speechtrix()
        {
            gamefield = new bool[maxSizeRow,maxSizeCol];
            speed = 500;
            level = 1;
            score = 0;
			linesToNextLevel = nextLevelLines;
			//starttime = nu;
			startRow = 0; startCol = 0;
			height = 20; width = 10;
			endCol = height; endRow = width;
			g = new Graphics(width,height);
        }

        public static void Main(string[] args)
        {
	//		Console.WriteLine("Would you like to play?");
	//		if (Console.Read() == "y")
		//	{
				Speechtrix sp = new Speechtrix();
				sp.newGame();
		//	}
        }

        void newLevel() //ändra level -> ändrar speed/poängsätt...
        {
            level++;
			linesToNextLevel = nextLevelLines;
			if (speed > 100)
				setSpeed(speed-nextLevelLines);
        }
		void setSpeed(float ms) //sätter fallhastighet
		{
			speed = ms;
		}
        void newGame() //nollställer tid, score, level, tömmer matris gamefield
		{
			for (int i=startRow; i<endRow; i++)
				for (int j=startCol; j<endCol; j++)
					gamefield[i,j] = false;
			speed = 500;
            level = 1;
            score = 0;
			linesToNextLevel = nextLevelLines;
			//starttime = nu;
		}
        void checkEnd() //när ett block inte får plats i gamefield
		{
			
		}
        void endGame() //när spelet tar slut
		{
			//show menu,highscore
		}
        void updateScore(int lines) //hur många lines som tagit bort
		{
			linesToNextLevel -= lines;
			if (linesToNextLevel <= 0)
				linesToNextLevel += nextLevelLines;

			score += level*10*lines;
	//		g.DrawScore(score);
		}
        void checkFullLine() //kolla om det finns några rader i matrisen där alla är true, isf anropa deleteLine för varje rad
		{
			int deletedLines = 0;
			for (int i=startRow; i<endRow; i++)
			{
				int j = startCol;
				for (; j<maxSizeCol && !gamefield[i,j]; j++) ; //as long as gamefield is all true on a row
				if (j==maxSizeCol) //if whole row true
				{
					deletedLines++;
					deleteLine(i);
					g.removeRow(i);
				}
			}
			updateScore(deletedLines);

		}
		void deleteLine(int lineNumber) //i matrisen, flytta alla bool-värden från rader ovanför lineNumber en rad nedåt, anropar updateScore
		{
			for (int i = lineNumber; i > startRow + 1; i--)
			{
				for (int j = startCol; j < endCol; j++)
				{
					gamefield[i, j] = gamefield[i - 1, j];
				}
			}
			//anropa deleteLine i graphics
		}
    }
}