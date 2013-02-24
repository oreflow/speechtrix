using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;
using System.Diagnostics;


namespace Speechtrix
{
    public class Graphics
    {
        static bool debug = false;
        private static Surface screen;
        static bool fullScreen = true;
        static int SCREEN_HEIGHT = (int) System.Windows.SystemParameters.PrimaryScreenHeight;
        static int SCREEN_WIDTH = (int) System.Windows.SystemParameters.PrimaryScreenWidth;
        static int blockY;
        static int blockX;
        static int blockSize;
        
        static Color gridColor1;
        static Color gridColor2;
        static int boardY;
        static int boardX;

        static bool backgroundCached = false; 
        static Color[,] backgroundCache = new Color[SCREEN_WIDTH, SCREEN_HEIGHT];


        static bool[,] tetriBoard;
        static Color[,] currentColor;
        static Color[,] defaultColor;

	    static LogicBlock current = new LogicBlock();
		
        //static int currentBlockX = 0;
        //static int currentBlockY = 0;
        //current.getState()    static bool[,] currentBlock = new bool [4,4];
        static bool running;
        static Speechtrix callBack;

        static bool styleRectGenerated = false;
        static int styleRectGeneratedSize = 0;
        static double[,] styleRectMatrix;

        static bool styleNextRectGenerated = false;
        static int styleNextRectGeneratedSize = 0;
        static double[,] styleNextRectMatrix;


        public Graphics(Speechtrix _callBack)
        {
            new Graphics(10, 20, _callBack);
        }
        /*
         * Constructor taking the game-field size as arguments
         */
        public Graphics(int Xsize, int Ysize, Speechtrix _callBack )
        {
            callBack = _callBack;


            blockY = Ysize;
            blockX = Xsize;

            tetriBoard = new bool[blockX, blockY];
            currentColor = new Color[blockX, blockY];
            defaultColor = new Color[blockX, blockY];

            gridColor1 = Color.FromArgb(100,100,100);
            gridColor2 = Color.FromArgb(155, 155, 155);

            // adding default colors to currentColor and to defaultColor
            bool swit = false;
            for (int i = 0; i < blockX; i++)
            {
                if (blockY % 2 == 0)
                    swit = !swit;
                for (int y = 0; y < blockY; y++)
                {
                    if (swit)
                    {
                        currentColor[i, y] = gridColor1;
                        defaultColor[i, y] = gridColor1;
                    }
                    else
                    {
                        currentColor[i, y] = gridColor2;
                        defaultColor[i, y] = gridColor2;
                    }
                    swit = !swit;
                }
            }

            blockSize = (int)Math.Min((SCREEN_WIDTH * 0.7) / blockX, (SCREEN_HEIGHT * 0.9) / blockY);
            boardX = (int)(SCREEN_WIDTH * 0.35) - (blockX * blockSize / 2);
            boardY = (int)(SCREEN_HEIGHT * 0.5) - (blockY * blockSize / 2);

            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);
            Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(KeyboardEventHandler);

        }
        /*
         * Keyboard event handler for testing before we implement the voice controller
         */
        private static void KeyboardEventHandler(object sender, KeyboardEventArgs args)
        {

            switch (args.Key)
            {
                case Key.UpArrow:
                    callBack.keyUp();
                    break;

                case Key.DownArrow:
                    callBack.keyDown();
                    break;

                case Key.LeftArrow:
                    callBack.keyLeft();
                    break;

                case Key.RightArrow:
                    callBack.keyRight();
                    break;

                case Key.Escape:
                    running = false;
                    Events.QuitApplication();
                    break;
            }
        }


        public static void Run()
        {
            screen = Video.SetVideoMode(SCREEN_WIDTH, SCREEN_HEIGHT, 32, false, false, fullScreen, true);

            DrawBackground();
            Draw();

            running = true;
            Events.Run();
            while (true)
            {
            }
        }
        /**
         * Draws the game-field, NOTICE!, Don't use this function. use CheckScreen for redrawing the screen
         */
        private static void Draw()
        {
              // draws out the current "game-field", animated since it updates screen for each column
            for(int x = 0 ; x < blockX ; x++)
            {
                for (int y = 0; y < blockY ; y++)
                {
                    if(currentColor[x,y] != gridColor1 && currentColor[x,y] != gridColor2)
                        StyleRect(boardX + x * blockSize, boardY + y * blockSize, blockSize, currentColor[x, y]);
                    else
                        FillRect(boardX + x * blockSize, boardY + y * blockSize, blockSize, currentColor[x,y]);

                }
            }

            if (debug)
            {
                // Draw an example timer
                DrawTimer(01, 06, 45);
                // draw an example score
                DrawScore(250800);
                // draw an example block 
                DrawBlock(0, 0, 2, 2, Color.FromArgb(0, 122, 0));
                // draw and example "next block"
                DrawNextBlock(0, 0, Color.FromArgb(0, 122, 0));
            }
        }

        /*
         * Draw a square of size <size> at position posX, posY
         */
        private static void FillRect(int posX, int posY, int size, Color col)
        {
            for(int x = posX; x<(posX+size);x++)
                for(int y = posY; y<(posY+size);y++)
                    screen.Draw(new Point(x, y), col);
        }

        /*
         * Draw a stylish square of size <size> at position posX, posY
         */
        private static void StyleRect(int posX, int posY, int size, Color col)
        {
            if(!styleRectGenerated || styleRectGeneratedSize != size)
            {
                styleRectMatrix = new double [size,size];

            Point middle = new Point(size / 2, size / 2);
            double maxDistance = Math.Sqrt(Math.Pow(middle.X, 2.0) + Math.Pow(middle.Y, 2.0));
            double intensity = 0.3;
            Debug.Print("before calc");
            for (int x = 0; x < size; x++)
                for (int y = 0; y < + size; y++)
                {
                    double distance = Math.Sqrt(Math.Pow(x - middle.X, 2.0) + Math.Pow(y - middle.Y, 2.0));
                    //styleRectMatrix[x, y] = 1 + intensity * distance / maxDistance; // lighter edges
                    styleRectMatrix[x, y] = 1 - intensity * distance / maxDistance; // darker edges
                }
            Debug.Print("after calc");
            styleRectGenerated = true;
            styleRectGeneratedSize = size;
            }


            for (int x = posX; x < (posX + size); x++)
                for (int y = posY; y < (posY + size); y++)
                    screen.Draw(new Point(x, y), Color.FromArgb(
                                    Math.Max(0,Math.Min((int)(col.R * styleRectMatrix[x-posX, y-posY]), 255)),
                                    Math.Max(0,Math.Min((int)(col.G * styleRectMatrix[x-posX, y-posY]), 255)),
                                    Math.Max(0,Math.Min((int)(col.B * styleRectMatrix[x-posX, y-posY]), 255))));
            
            if (debug)
                Debug.Print("Drawn StyleRect");
        }

        /*
         * Draw a stylish square of size <size> at position posX, posY
         */
        private static void StyleNextRect(int posX, int posY, int size, Color col)
        {
            if (!styleNextRectGenerated || styleNextRectGeneratedSize != size)
            {
                styleNextRectMatrix = new double[size, size];

                Point middle = new Point(size / 2, size / 2);
                double maxDistance = Math.Sqrt(Math.Pow(middle.X, 2.0) + Math.Pow(middle.Y, 2.0));
                double intensity = 0.3;
                Debug.Print("before calc");
                for (int x = 0; x < size; x++)
                    for (int y = 0; y < +size; y++)
                    {
                        double distance = Math.Sqrt(Math.Pow(x - middle.X, 2.0) + Math.Pow(y - middle.Y, 2.0));
                        //styleNextRectMatrix[x, y] = 1 - intensity * distance / maxDistance; // lighter Edges
                        styleNextRectMatrix[x, y] = 1 - intensity * distance / maxDistance; // darker edges
                    }
                Debug.Print("after calc");
                styleNextRectGenerated = true;
                styleNextRectGeneratedSize = size;
            }


            for (int x = posX; x < (posX + size); x++)
                for (int y = posY; y < (posY + size); y++)
                    screen.Draw(new Point(x, y), Color.FromArgb(
                                    Math.Max(0, Math.Min((int)(col.R * styleNextRectMatrix[x - posX, y - posY]), 255)),
                                    Math.Max(0, Math.Min((int)(col.G * styleNextRectMatrix[x - posX, y - posY]), 255)),
                                    Math.Max(0, Math.Min((int)(col.B * styleNextRectMatrix[x - posX, y - posY]), 255))));

        }

        /*
         * Draw a rectangle of size <sizeX, sizeY> at position posX, posY
         */
        private static void FillRect(int posX, int posY, int sizeX,int sizeY, Color col)
        {
            for (int x = posX; x < (posX + sizeX); x++)
                for (int y = posY; y < (posY + sizeY); y++)
                    screen.Draw(new Point(x, y), col);
        }
        /*
         * Draws out the given block as "next-block"
         */
        private static void DrawNextBlock(short blockID, short rotation, Color col)
        {
            // Draw "next" GRID
            int startX = (int)(SCREEN_WIDTH * 0.75);
            int startY = (int)(SCREEN_HEIGHT * 0.55);
            bool swit = false;
            int nextBlockSize = (int)(SCREEN_WIDTH * 0.05);

            bool nextGrid = true;
            if (nextGrid)
            {
                for (int x = 0; x < 4; x++)
                {
                    swit = !swit;
                    for (int y = 0; y < 4; y++)
                    {
                        if (swit)
                            FillRect(startX + x * nextBlockSize, startY + y * nextBlockSize, nextBlockSize, gridColor1);
                        else
                            FillRect(startX + x * nextBlockSize, startY + y * nextBlockSize, nextBlockSize, gridColor2);
                        swit = !swit;
                    }
                }
            }

            // Draw the block on the GRID
		   bool [,] block = Blocks.getRotations(blockID, rotation);
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    if(block[x,y])
                        StyleNextRect(startX + x * nextBlockSize, startY + y * nextBlockSize, nextBlockSize, col);


            screen.Update();
        }
        /*
         * Draws out the given block as "next-block"
         */
        private static void DrawNextBlock(LogicBlock lb)
        {
            // Draw "next" GRID
            int startX = (int)(SCREEN_WIDTH * 0.75);
            int startY = (int)(SCREEN_HEIGHT * 0.55);
            bool swit = false;
            int nextBlockSize = (int)(SCREEN_WIDTH * 0.05);

            bool nextGrid = true;
            if (nextGrid)
            {
                for (int x = 0; x < 4; x++)
                {
                    swit = !swit;
                    for (int y = 0; y < 4; y++)
                    {
                        if (swit)
                            FillRect(startX + x * nextBlockSize, startY + y * nextBlockSize, nextBlockSize, gridColor1);
                        else
                            FillRect(startX + x * nextBlockSize, startY + y * nextBlockSize, nextBlockSize, gridColor2);
                        swit = !swit;
                    }
                }
            }

            // Draw the block on the GRID
            for (int x = 0; x < lb.bxs; x++)
                for (int y = 0; y < lb.bys; y++)
                    if (lb.getState()[x, y])
                        StyleNextRect(startX + x * nextBlockSize, startY + y * nextBlockSize, nextBlockSize, lb.color);

            screen.Update();
        }
        /*
         * Draws a given block on the game board, at position Xpos, Ypos, Color col
         */
        private static void DrawBlock(short blockID, short rotation, int Xpos, int Ypos, Color col)
        {       
            bool[,] block = getBlock(blockID, rotation);

            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                {
                    if (current.getState()[x, y])
                        FillRect(boardX + (current.x + x) * blockSize, boardY + (current.y + y) * blockSize, blockSize, defaultColor[current.x + x, current.y + y]);
                }

            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                {
                    if (block[x, y] && ((Xpos + x) > boardX || (Ypos + y) > boardY))
                    {
                        Events.QuitApplication();
                        throw new FormatException();
                    }
                    if (block[x, y])
                        StyleRect(boardX + (Xpos + x) * blockSize, boardY + (Ypos + y) * blockSize, blockSize, col);
                }

            current.x = (short) Xpos;
            current.y = (short) Ypos;

        }
		/*
         * Draws a given block on the game board, at position Xpos, Ypos, Color col
         */
		private static void DrawBlock(LogicBlock lb)
		{
			for (int x = 0; x < lb.bxs; x++)
				for (int y = 0; y < lb.bys; y++)
				{
                    if((lb.x + x) < blockX && (lb.y + y) < blockY)
                    {
					    if (lb.getState()[x, y] && ((lb.x + x) > boardX || (lb.y + y) > boardY))
					    {
						    Events.QuitApplication();
						    throw new FormatException();
					    }
					    if (lb.getState()[x, y])
						    StyleRect(boardX + (lb.x + x) * blockSize, boardY + (lb.y + y) * blockSize, blockSize, lb.color);
                    }
				}

			current.x = lb.x;
			current.y = lb.y;
		}
        /**
         * Function that redraws stuff that are wrong in the board
         * wrong = differs from what is said in currentColor[x,y]
         */
        private static void CheckScreen()
        {
            for (int x = 0; x < blockX; x++)
            {
                for (int y = 0; y < blockY; y++)
                {
                    Color tmp = screen.GetPixel(new Point(boardX + x * blockSize, boardY + y * blockSize));

                    if (tmp != currentColor[x, y])
                    {
                        if (currentColor[x, y] != gridColor1 && currentColor[x, y] != gridColor2)
                            StyleRect(boardX + x * blockSize, boardY + y * blockSize, blockSize, currentColor[x, y]);
                        else
                            FillRect(boardX + x * blockSize, boardY + y * blockSize, blockSize, currentColor[x, y]);
                    }
                }
            }

        }
        /*
         * Locks a block to its position so that it will be drawn out as a landed unmovable block
         */
        private static void lockBlockInPosition(short blockID, short rotation, int Xpos, int Ypos, Color col, short[][,] sizes)
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (current.getState()[x, y])
                    {
                        currentColor[x+Xpos, y+Ypos] = col;
                        current.getState()[x+Xpos, y+Ypos] = false;
                    }
                }
            }
            current.x = 0;
            current.y = 0;
        }
        /*
         * Locks a block to its position so that it will be drawn out as a landed unmovable block
         */
        private static void lockBlockInPosition(LogicBlock logblock, LogicBlock next) //låser bara x0,y0
        {
            try
            {
                Debug.Print("bxs: " + logblock.bxs + ", bxy: " + logblock.bys);
                for (int x=0; x < logblock.bxs; x++)
                {
                    for (int y=0; y < logblock.bys; y++)
                    {
                        Debug.Print(x + ", " + y + ": " + logblock.getState()[x, y]);
                        if (logblock.getState()[x, y])
                        {
                            currentColor[x + logblock.x, y + logblock.y] = logblock.color;
                        }
                    }
                }
            }    catch(IndexOutOfRangeException e) //should not happen now
            {
                Debug.Print("CATCH!: "+(logblock.x) + "  " + (logblock.y));
            } 

		//	current.x = 0;
		//	current.y = 0;

            DrawBlock(current);
            copyNextToCurrent(next);
        }
        

        /*
         * Draws out the timer box with given time
         */ 
        private static void DrawTimer(int hour, int minute, int second)
        {
            Color background = Color.FromArgb(255, 102, 0);
            Color fontColor = Color.FromArgb(255, 255, 255);

            if (hour > 99 || minute > 59 || second > 59 || hour < 0 || minute < 0 || second < 0)
                hour = minute = second = 0;

            int timerWidth = (int) (SCREEN_WIDTH * 0.2);
            int timerHeight = (int) (timerWidth/3);
            int timerX = (int)(SCREEN_WIDTH * 0.75);
            int timerY = (int)(SCREEN_HEIGHT * 0.2);

            FillRect(timerX, timerY, timerWidth, timerHeight, background);
            int numberSize = (int)(timerWidth/5);



            if (hour.ToString().Length > 1)
            {
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 0 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(hour.ToString().Substring(0, 1)), fontColor, numberSize);
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 1 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(hour.ToString().Substring(1, 1)), fontColor, numberSize);
            }
            else
            {
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 0 * 1.4), (int)(timerY + 0.1 * timerHeight), 0, fontColor, numberSize);
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 1 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(hour.ToString().Substring(0, 1)), fontColor, numberSize);
            }
            FillRect((int)(timerX + numberSize / 2 * 0 * 1.4 + numberSize * 1.5), (int)(timerY + 0.3 * timerHeight), (int)(0.05 * timerHeight), fontColor);
            FillRect((int)(timerX + numberSize / 2 * 0 * 1.4 + numberSize * 1.5), (int)(timerY + 0.5 * timerHeight), (int)(0.05 * timerHeight), fontColor);

            if (minute.ToString().Length > 1)
            {
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 0 * 1.4 + numberSize * 1.5), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(minute.ToString().Substring(0, 1)), fontColor, numberSize);
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 1 * 1.4 + numberSize * 1.5), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(minute.ToString().Substring(1, 1)), fontColor, numberSize);
            }
            else
            {
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 0 * 1.4 + numberSize * 1.5), (int)(timerY + 0.1 * timerHeight), 0, fontColor, numberSize);
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 1 * 1.4 + numberSize * 1.5), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(minute.ToString().Substring(0, 1)), fontColor, numberSize);
            }
            FillRect((int)(timerX + numberSize / 2 * 0 * 1.4 + numberSize * 3), (int)(timerY + 0.3 * timerHeight), (int)(0.05 * timerHeight), fontColor);
            FillRect((int)(timerX + numberSize / 2 * 0 * 1.4 + numberSize * 3), (int)(timerY + 0.5 * timerHeight), (int)(0.05 * timerHeight), fontColor);

            if (second.ToString().Length > 1)
            {
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 0 * 1.4 + numberSize * 3), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(second.ToString().Substring(0, 1)), fontColor, numberSize);
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 1 * 1.4 + numberSize * 3), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(second.ToString().Substring(1, 1)), fontColor, numberSize);
            }
            else
            {
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 0 * 1.4 + numberSize * 3), (int)(timerY + 0.1 * timerHeight), 0, fontColor, numberSize);
                DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 1 * 1.4 + numberSize * 3), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(second.ToString().Substring(0, 1)), fontColor, numberSize);
            }
        }
        /*
         * Draws out the score box with given score up to 8 digits
         */ 
        private static void DrawScore(int score)
        {
            Color background = Color.FromArgb(255, 102, 0);
            Color fontColor = Color.FromArgb(255, 255, 255);

            if(score < 0 || score > 99999999)
                score = 1337;

            int scoreWidth = (int) (SCREEN_WIDTH * 0.2);
            int scoreHeight = (int) (scoreWidth/4.5);
            int scoreX = (int)(SCREEN_WIDTH * 0.75);
            int scoreY = (int)(SCREEN_HEIGHT * 0.4);

            FillRect(scoreX, scoreY, scoreWidth, scoreHeight, background);
            String scorestr = score.ToString();


            int numberSize = (int)(scoreHeight * 0.8);
            
            for (int i = 0; i < 8; i++)
            {
                if ( i < scorestr.Length)
                {
                    int tmp = Convert.ToInt32(scorestr.Substring(i,1));
                    DrawNumber((int)(scoreX + 0.1 * scoreHeight + numberSize / 2 * i * 1.4), (int)(scoreY + 0.1 * scoreHeight), tmp, fontColor, numberSize);
                }
                else
                {
                    //DrawNumber((int)(scoreX + 0.1 * scoreHeight + numberSize / 2 * i * 1.4), (int)(scoreY + 0.1 * scoreHeight), 0, fontColor, numberSize);
                }
            }
            screen.Update();
        }
        /*
         * Draws a number using 7 lines as in a standard alarm clock
         * at position posX, posY, with height = size and width = height/2
         * argument nr defines which number 0 - 9 is being drawn
         */ 
        private static void DrawNumber(int posX, int posY, int nr, Color col, int size)
        {
            if (nr > 9 || nr < 0)
                return;
            

            // draws a number using 7 different lines as an alarm clock.
            bool[,] numbers = {
                                       { true, true, true, false, true, true, true }, // 0
                                       { false, false, true, false, false, true, false },// 1
                                       { true, false, true, true, true, false, true },// 2
                                       { true, false, true, true, false, true, true },// 3
                                       { false, true, true, true, false, true, false },// 4
                                       { true, true, false, true, false, true, true },// 5
                                       { true, true, false, true, true, true, true },// 6
                                       { true, false, true, false, false, true, false },// 7
                                       { true, true, true, true, true, true, true },// 8
                                       { true, true, true, true, false, true, true }};// 9
            
            short[] width = new short [size/2];
            short tmpWidth = 0;
            for (int i = 0; i < size / 2; i++)
            {
                if (i < size / 2 * 0.1)
                    width[i] = tmpWidth;
                else if (i < size / 2 * 0.2)
                {
                    tmpWidth += 1;
                    width[i] = tmpWidth;
                }
                else if (i < size / 2 * 0.8)
                {
                    width[i] = tmpWidth;
                }
                else if (i < size / 2 * 0.9)
                {
                    tmpWidth -= 1;
                    width[i] = tmpWidth;
                }
                else if (i < size / 2)
                {
                    width[i] = tmpWidth;
                }
                
            }

            // draw out stuff
            if (numbers[nr, 0] == true)
            {
                for(int x = 0; x < size/2 ; x++)
                    for(int y = 0 ; y < width[x] ; y++)
                        screen.Draw(new Point(posX + x, posY + y), col);
            
            }
            if (numbers[nr, 1] == true)
            {
                for (int y = 0; y < size / 2; y++)
                    for (int x = 0; x < width[y]; x++)
                        screen.Draw(new Point(posX + x, posY + y), col);

            }
            if (numbers[nr, 2] == true)
            {
                for (int y = 0; y < size / 2; y++)
                    for (int x = 0; x < width[y]; x++)
                        screen.Draw(new Point(size/2 + posX - x, posY + y), col);

            }
            if (numbers[nr, 3] == true)
            {
                for (int x = 0; x < size / 2; x++)
                    for (int y = 0; y < width[x]; y++)
                        screen.Draw(new Point(posX + x, size/2 + posY + y), col);

            }
            if (numbers[nr, 4] == true)
            {
                for (int y = 0; y < size / 2; y++)
                    for (int x = 0; x < width[y]; x++)
                        screen.Draw(new Point(posX + x,size/2 + posY + y), col);

            }
            if (numbers[nr, 5] == true)
            {
                for (int y = 0; y < size / 2; y++)
                    for (int x = 0; x < width[y]; x++)
                        screen.Draw(new Point(size/2 + posX - x, size/2 + posY + y), col);

            }
            if (numbers[nr, 6] == true)
            {
                for (int x = 0; x < size / 2; x++)
                    for (int y = 0; y < width[x]; y++)
                        screen.Draw(new Point(posX + x, size + posY - y), col);

            }

        }
        /*
         * Draws the background color shading, also caches the background in case it will be redrawn
         */
        private static void DrawBackground()
        {
            if(backgroundCached)
                for (int x = 0; x < SCREEN_WIDTH; x++)
                    for (int y = 0; y < SCREEN_HEIGHT; y++)
                        screen.Draw(new Point(x, y), backgroundCache[x,y]);

            Vector topLeft = new Vector(255,0,0); // red
	        Vector topRight = new Vector(0,0,255); // blue
	        Vector bottomLeft = new Vector(0,255,0); // green
	        Vector bottomRight = new Vector(255,255,0); // yellow
            Vector[] leftSide = new Vector[SCREEN_HEIGHT];
	        Vector[] rightSide = new Vector[SCREEN_HEIGHT];

	        Interpolate( topLeft, bottomLeft, ref leftSide );
	        Interpolate( topRight, bottomRight, ref rightSide );

	        Vector [] colors = new Vector[SCREEN_WIDTH];

	        for( int y=0; y<SCREEN_HEIGHT; ++y )
	        {
		        Interpolate(leftSide[y],rightSide[y],ref colors);

		        for( int x=0; x<SCREEN_WIDTH; ++x )
		        {
                    screen.Draw(new Point(x, y), Color.FromArgb((int)colors[x].X,(int)colors[x].Y,(int)colors[x].Z));
                    backgroundCache[x, y] = Color.FromArgb((int)colors[x].X, (int)colors[x].Y, (int)colors[x].Z);
		        }
                if(y % 5 == 0)
                    screen.Update(); // comment this row to remove background animation
	        }
            backgroundCached = true;
        }
        /*
         * interpolates between two Vectors, returns the interpolation in the array <result>
         */
        private static void Interpolate(Vector a, Vector b, ref Vector [] result)
        {
            	int size = result.Length;
	            Vector step = Vector.Divide((b-a), (float)Math.Max(size-1,1));
	            Vector current = a;
	            for( int i=0; i<size; ++i )
	            {
		            result[i] = current;
		            current += step;
	            }
        }
        /*
         * Removes one of the "locked" rows from the board since the row has been filled. 
         */
        private static void clearRow(int rownr)
        {
            for (int x = 0; x < blockX; x++)
            {
                for (int y = rownr; y > 0; y--)
                {
                    if(currentColor[x,y-1] == gridColor1 || currentColor[x,y-1] == gridColor2)
                    {
                        currentColor[x, y] = defaultColor[x, y];
                        break;
                    }
                    else
                        currentColor[x, y] = currentColor[x, y - 1];
                }
         }
            CheckScreen();
            
        }


        private static void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            running = false;
            Events.QuitApplication();
        }
        private static bool[,] getBlock(short ID, short rotation)
        {
            bool[,] block = Blocks.getRotations(ID, rotation);
            return block;
        }

        // **** Public methods ****
        public void setTime(int hours, int minutes, int seconds)
        {
            if (!running)
                return;
            //Debug.Print("reaching function");
            DrawTimer(hours, minutes, seconds);
            screen.Update();
        }
        public void setScore(int score)
        {
            if (!running)
                return;
            DrawScore(score);
            screen.Update();
        }
        public void setNext(short blockID, short rotation, Color col)
        {
            if (!running)
                return;
            DrawNextBlock(blockID, rotation, col);
            screen.Update();
        }
        public void setNext(LogicBlock lb)
        {
            if (!running)
                return;
            DrawNextBlock(lb);
            screen.Update();
        }
        public void setBlock(short blockID, short rotation, int Xpos, int Ypos, Color col)
        {
            if (!running)
                return;
            DrawBlock(blockID, rotation, Xpos, Ypos, col);
            screen.Update();
        }
		public void setBlock(LogicBlock logblock)
		{
			if (!running || !logblock.movable)
				return;
            CheckScreen();
            DrawBlock(logblock);
			screen.Update();
		}
        public void lockBlock(short blockID, short rotation, int Xpos, int Ypos, Color col, short[][,] sizes)
        {
            if (!running)
                return;
            lockBlockInPosition(blockID, rotation, Xpos, Ypos, col, sizes);
            Draw();
        }
        public void lockBlock(LogicBlock logblock, LogicBlock next)
        {
            if (!running)
                return;
            lockBlockInPosition(logblock, next);
            Draw();
        }
        public void removeRow(int rownr)
        {
            if (!running)
                return;
            clearRow(rownr);
            Draw();
        }
        public void exit()
        {
            running = false;
            Events.QuitApplication();
        }
        public void setStressAnimation()
        {

        }
        static void copyNextToCurrent(LogicBlock next)
        {
            current.nr = next.nr;
            current.state = next.state;
            current.bxs = next.bxs;
            current.bys = next.bys;
            current.x = next.x;
            current.y = next.y;
            current.color = next.color;
        }
    }
}
