using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Core;


namespace Speechtrix
{
    class Graphics
    {
         private static Surface screen;
        static bool fullScreen = false;
        static int SCREEN_HEIGHT = (int) System.Windows.SystemParameters.PrimaryScreenHeight;
        static int SCREEN_WIDTH = (int) System.Windows.SystemParameters.PrimaryScreenWidth;
        static int blockY;
        static int blockX;
        static int blockSize;
        static bool debug = true;
        static Color gridColor1;
        static Color gridColor2;
        static int boardY;
        static int boardX;

        static bool backgroundCached = false; 
        static Color[,] backgroundCache = new Color[SCREEN_WIDTH, SCREEN_HEIGHT];


       static bool[,] tetriBoard;
       static Color[,] currentColor;
       static Color[,] defaultColor;

        public Graphics()
        {
            new Graphics(10, 20);
        }

        public Graphics(int Xsize, int Ysize)
        {
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


            screen = Video.SetVideoMode(SCREEN_WIDTH, SCREEN_HEIGHT, 32, false, false, fullScreen, true);
            Draw();


            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);
            Events.Run();
        }
        /**
         * The main Drawing function, draws the whole game 
         */
        private static void Draw()
        {
            DrawBackground();
            
            blockSize = (int) Math.Min((SCREEN_WIDTH*0.7)/blockX, (SCREEN_HEIGHT*0.9)/blockY);
            boardX = (int)(SCREEN_WIDTH * 0.35) - (blockX * blockSize / 2);
            boardY = (int)(SCREEN_HEIGHT * 0.5) - (blockY * blockSize / 2);

              // draws out the "game-field"
            for(int x = 0 ; x < blockX ; x++)
            {
                for (int y = 0; y < blockY ; y++)
                {
                    
                    FillRect(boardX + x * blockSize, boardY + y * blockSize, blockSize, currentColor[x,y]);
                }
            }

            if (debug)
            {
                // Draw an example timer
                DrawTimer(01, 06, 45);
                // draw an example score
                DrawScore(0);
                // draw an example block 
                DrawBlock(0, 0, 5, 5, Color.FromArgb(0, 122, 0));
                // draw and example "next block"
                DrawNextBlock(0, Color.FromArgb(0, 122, 0));
            }
            screen.Update();
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
        private static void DrawNextBlock(short blockID, Color col)
        {
            
            // Draw "next" GRID
            

            int startX = (int)(SCREEN_WIDTH * 0.75);
            int startY = (int)(SCREEN_HEIGHT * 0.55);
            bool swit = false;
            int nextBlockSize = (int)(SCREEN_WIDTH * 0.05);


            bool nextGrid = false;
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
         
		   bool [,] block = Blocks.getRotations(blockID, 0);
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    if(block[x,y])
                        FillRect(startX + x * nextBlockSize, startY + y * nextBlockSize, nextBlockSize, col);



        }

        private static void DrawBlock(short blockID, short rotation, int Xpos, int Ypos, Color col)
        {
            bool[,] block = getBlock(blockID, rotation);

            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    if (block[x, y])
                        FillRect(boardX + (Xpos + x) * blockSize,boardY +  (Ypos + y) * blockSize, blockSize, col);

        }


        private static void lockBlockInPosition(short blockID, short rotation, int Xpos, int Ypos, Color col)
        {
            bool[,] block = getBlock(blockID, rotation);
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (block[x, y])
                        currentColor[x,y] = col;
                }
            }

        }
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

        private static void DrawScore(int score)
        {
            Color background = Color.FromArgb(255, 102, 0);
            Color fontColor = Color.FromArgb(255, 255, 255);

            if(score < 0 || score > 99999999)
                score = 0;

            int scoreWidth = (int) (SCREEN_WIDTH * 0.2);
            int scoreHeight = (int) (scoreWidth/4.5);
            int scoreX = (int)(SCREEN_WIDTH * 0.75);
            int scoreY = (int)(SCREEN_HEIGHT * 0.4);

            FillRect(scoreX, scoreY, scoreWidth, scoreHeight, background);
            String scorestr = score.ToString();

            int numberSize = (int)(scoreHeight * 0.8);

            for (int i = 0; i < 8; i++)
            {
                if (i < scorestr.Length)
                {
                    int tmp = Convert.ToInt32(scorestr.Substring(i,1));
                    DrawNumber((int)(scoreX + 0.1 * scoreHeight + numberSize / 2 * i * 1.4), (int)(scoreY + 0.1 * scoreHeight), tmp, fontColor, numberSize);
                }
                else
                {
                    DrawNumber((int)(scoreX + 0.1 * scoreHeight + numberSize / 2 * i * 1.4), (int)(scoreY + 0.1 * scoreHeight), 0, fontColor, numberSize);
                }


            }
        }

        private static void DrawNumber(int posX, int posY, int nr, Color col, int size)
        {
            if (nr > 9 || nr < 0)
                return;
            

            // draws a number using 7 different lines as an alarm clock.
            bool[,] numbers = {
                                       { true, true, true, false, true, true, true }, // 0
                                       { false, false, true, false, false, true, false },// 1
                                       { true, false, true, true, true, false, true },// 2
                                       { true, false, true, false, false, true, true },// 3
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
	        }
            backgroundCached = true;
        }
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


        private static void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }
        private static bool[,] getBlock(short ID, short rotation)
        {
            Block b = new Block(0);
            b.setId(ID);
            bool[,] block = (b.getRot())[rotation];
            return block;
        }

        // **** Public methods ****
        public void setTime(String time)
        {
            
        }
        public void setTime(int hours, int minutes, int seconds)
        {
            DrawTimer(hours, minutes, seconds);
        }
        public void setScore(int score)
        {
            DrawScore(score);
        }
        public void setNext(short blockID, Color col)
        {
            DrawNextBlock(blockID, col);
        }
        public void setBlock(short blockID, short rotation, int Xpos, int Ypos, Color col)
        {
            Draw();
            DrawBlock(blockID, rotation, Xpos, Ypos, col);
        }
        public void lockBlock(short blockID, short rotation, int Xpos, int Ypos, Color col)
        {
            lockBlockInPosition(blockID, rotation, Xpos, Ypos, col);
            Draw();
        }
        public void removeRow(int rownr)
        {
            clearRow(rownr);
            Draw();
        }
        public void exit()
        {

        }
        public void setStressAnimation()
        {

        }
    }
}
