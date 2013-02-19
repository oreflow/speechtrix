using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Speechtrix
{
    class Speechtrix
    {
        const int maxSizeRow = 32;
        const int maxSizeCol = 64;
        
        bool[,] gamefield; //matris med positioner, där vi använder 16(?) som standardbredd (position (0,0) är högst upp i vänstra hörnet)
        float speed; //hastighet för fallande block i ms(?)
        int level; //kanske för reglera speed/hur mycket poäng man får/vilka block som kommer
        long score;
		int starttime; //set when starting a game
		int linesToNextLevel; //lines needed to be removed until reaching next level
		int startCol, startRow, endCol, endRow, breadth, width;

        public Speechtrix()
        {
            gamefield = new bool[maxSizeRow,maxSizeCol];
            speed = 500;
            level = 1;
            score = 0;
			linesToNextLevel = 30;
			//starttime = nu;
			startRow = 0; startCol = 0;
			breadth = 16; width = 40;
			endCol = breadth; endRow = width;
			Graphics g = new Graphics(breadth,width);
        }
        public static void Main(string[] args)
        {
			new Speechtrix();
        }

        void newLevel() //ändra level -> ändrar speed/poängsätt...
        {
            level++;
			linesToNextLevel = 30;
			if (speed > 100)
				setSpeed(speed-30);
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
			linesToNextLevel = 30;
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
				linesToNextLevel += 30;

			score += level*10*lines;
		}
        void checkFullLine() //kolla om det finns några rader i matrisen där alla är true, isf anropa deleteLine för varje rad
		{
			int deletedLines = 0;
			for (int i=startRow; i<endRow; i++)
			{
				int j = startCol;
				for (; j<maxSizeCol && !gamefield[i,j]; j++) ;
				if (j==maxSizeCol)
				{
					deletedLines++;
					deleteLine(i);
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