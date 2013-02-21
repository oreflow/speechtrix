using System;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Diagnostics;
using System.Drawing;


namespace Speechtrix
{
    public class Speechtrix
    {
        const int maxSizeRow = 32;
        const int maxSizeCol = 64;
		const int nextLevelLines = 20;
        Color[] cola = new Color[7]
            {Color.FromArgb(226, 0, 127), Color.FromArgb(255, 0, 0), Color.FromArgb(0, 255, 0), 
                Color.FromArgb(12, 0, 247), Color.FromArgb(255, 240, 0), Color.FromArgb(122, 78, 156),
                Color.FromArgb(45, 237, 120)};

        short[][,] sizes = new short[2][,]
        {
            new short[7,4]{{2,3,2,3},{1,4,1,4},{2,2,2,2},{2,3,2,3},{2,3,2,3},{2,3,2,3},{2,3,2,3}},
            new short[7,4]{{3,2,3,2},{4,1,4,1},{2,2,2,2},{3,2,3,2},{3,2,3,2},{3,2,3,2},{3,2,3,2}}
        };
        bool newBlock;
        bool[,] gamefield; //matris med positioner, där vi använder 16(?) som standardbredd (position (0,0) är högst upp i vänstra hörnet)
        int speed; //hastighet för fallande block i ms(?)
        int level; //kanske för reglera speed/hur mycket poäng man får/vilka block som kommer
        int score;
		int linesToNextLevel; //lines needed to be removed until reaching next level
		int startCol, startRow, endCol, endRow, height, width;
		Graphics g;
        Thread tt;
        Thread graphicsThread;
        LogicBlock current, next;
        Random rand;

        public Speechtrix()
        {
            rand = new Random();
            gamefield = new bool[maxSizeRow,maxSizeCol];
			linesToNextLevel = nextLevelLines;

			startRow = 0; startCol = 0;
			height = 20; width = 10;
			endCol = height; endRow = width;
            Debug.Print("creating speechtrix");
            g = new Graphics(width, height, this);
			graphicsThread = new Thread(new ThreadStart(Graphics.Run));
            graphicsThread.Start();

            Thread.Sleep(10000);

            Debug.Print("created graphics");
        }

        public static void Main(string[] args)
        {
	//		Console.WriteLine("Would you like to play?");
	//		if (Console.Read() == "y")
		//	{
				Speechtrix sp = new Speechtrix();
                
				sp.newGame();
                sp.play();

                
		//	}
        }
        void newGame() //nollställer tid, score, level, tömmer matris gamefield
        {
            for (int i = startRow; i < endRow; i++)
                for (int j = startCol; j < endCol; j++)
                    gamefield[i, j] = false;

            speed = 1500;
            level = 1;
            score = 0;
            linesToNextLevel = nextLevelLines;

            TimerThread timer = new TimerThread(ref g);
            tt = new Thread(new ThreadStart(timer.Run));
            tt.Start();
            updateScore(2523);
        }





        /*************************************************/
        void play()
        {
            current = new LogicBlock();
            next = new LogicBlock();
            
            next.nr = (short)rand.Next(0, 7);
            next.rot = (short)rand.Next(0, 4);
            next.blo = Blocks.getRotations(next.nr, next.rot);
            next.bxs = sizes[0][next.nr, next.rot];
            next.bys = sizes[1][next.nr, next.rot];
            next.y = 0;
            next.x = (short) ((width-next.bxs)/2);

            newBlock = true;

            for (short i=0; !checkEnd(); i++)
            {
                if (newBlock)
                {
                    newBlock = false;
                    
                    current = next;
                    
                    next.nr = (short)rand.Next(0, 7);
                    next.rot = (short)rand.Next(0, 4);
                    next.blo = Blocks.getRotations(next.nr, next.rot);
                    next.bxs = sizes[0][next.nr, next.rot];
                    next.bys = sizes[1][next.nr, next.rot];
                    next.y++;
                    next.x = (short)(width / 2 - next.bxs / 2);
                    next.color = cola[next.nr];
                    
                    g.setNext(next.nr, next.rot, next.color);
                    g.setBlock(current.nr,current.rot,current.x,current.y,current.color);
                    Debug.Print("Sizes: " + current.x + " " + current.y);
                }

                if (true) //ska vara checkRowBelow() här sen
                {
                    //g.lockBlock(current.nr, current.rot, current.x, current.y, current.color);
                    newBlock = true;
                }
                else
                {
                    current.y++; //falling
                    g.setBlock(current.nr, current.rot, current.x, current.y, current.color); //paint
                }

                Thread.Sleep(speed);
            }
            endGame();
        }
        /*************************************************/


        bool checkRowBelow()
        {
            for (int i=0; i<width; i++) ;
            return false;
        }

        void newLevel() //ändra level -> ändrar speed/poängsätt...
        {
            level++;
			linesToNextLevel = nextLevelLines;
			if (speed > 100)
				setSpeed(speed-nextLevelLines);
        }
		void setSpeed(int ms) //sätter fallhastighet
		{
			speed = ms;
		}

        bool checkEnd() //när ett block inte får plats i gamefield
		{
            bool end = false;



            return end;
		}
        void endGame() //när spelet tar slut
		{
            tt.Abort();
            checkHighscore();
            showMenu();
		}
        void checkHighscore()
        {

        }
        void showMenu()
        {

        }
        void updateScore(int lines) //hur många lines som tagit bort
		{
			linesToNextLevel -= lines;
			if (linesToNextLevel <= 0)
				linesToNextLevel += nextLevelLines;

			score += level*10*lines;
            Debug.Print("POints:  " + score);
            g.setScore(score);
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
            g.removeRow(lineNumber);
			//anropa deleteLine i graphics
		}
        public void keyUp()
        {

        }
        public void keyDown()
        {

        }
        public void keyLeft()
        {

        }
        public void keyRight()
        {

        }
    }
    class TimerThread
    {
        Graphics g;
        int hours;
        int minutes;
        int seconds;

        public TimerThread(ref Graphics _g)
        {

            Debug.Print("creating TimerThread");
            g = _g;
            hours = 0;
            minutes = 0;
            seconds = 0;
        }
        public void Run()
        {
            while (true)
            {
                Thread.Sleep(1000);
                seconds++;
                if (seconds == 60)
                {
                    minutes++;
                    seconds = 0;
                }
                if (minutes == 60)
                {
                    hours++;
                    minutes = 0;
                }
                g.setTime(hours, minutes, seconds);
            }
        }

    }

    class LogicBlock
    {
        public short nr, x, y, bxs, bys, rot;
        public Color color;
        public bool[,] blo = new bool[4,4];

        public LogicBlock()
        {
            blo = new bool[4,4];
        }      
    }
}