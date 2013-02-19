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
        static bool fullScreen;
        static int SCREEN_HEIGHT = 500; // (int) System.Windows.SystemParameters.PrimaryScreenHeight;
        static int SCREEN_WIDTH = 600; //(int) System.Windows.SystemParameters.PrimaryScreenWidth;
        static int blockY;
        static int blockX;

        static bool backgroundCached = false; 
        static Color[,] backgroundCache = new Color[SCREEN_WIDTH, SCREEN_HEIGHT];


       static bool[,] tetriBoard;
       static Color[,] currentColor; 

        public Graphics()
        {
            fullScreen = false;
            blockY = 20;
            blockX = 15;
            tetriBoard = new bool[blockX, blockY];
            currentColor = new Color[blockX, blockY]; 


            bool swit = false;
            for(int i = 0; i < blockX; i++)
            {
                if (blockY % 2 == 0)
                    swit = !swit;
                for(int y = 0; y < blockY; y++)
                {
                    // fixa så att det itne buggar med färgerna om det blir udda antal rader
                    if (swit)
                        currentColor[i, y] = Color.FromArgb(0, 0, 0);
                    else
                        currentColor[i, y] = Color.FromArgb(255, 255, 255);
                    swit = !swit;
                }
            }

            
            screen = Video.SetVideoMode(SCREEN_WIDTH, SCREEN_HEIGHT, 32, false, false, fullScreen, true);

            
            Draw();

            
            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);
            Events.Run();
        }

        public Graphics(int Xsize, int Ysize)
        {
            fullScreen = true;
            blockY = Ysize;
            blockX = Xsize;

            tetriBoard = new bool[blockX, blockY];
            currentColor = new Color[blockX, blockY]; 

            bool swit = false;
            for (int i = 0; i < blockX; i++)
            {
                if (blockY % 2 == 0)
                    swit = !swit;
                for (int y = 0; y < blockY; y++)
                {
                    // fixa så att det itne buggar med färgerna om det blir udda antal rader
                    if (swit)
                        currentColor[i, y] = Color.FromArgb(0, 0, 0);
                    else
                        currentColor[i, y] = Color.FromArgb(255, 255, 255);
                    swit = !swit;
                }
            }


            screen = Video.SetVideoMode(SCREEN_WIDTH, SCREEN_HEIGHT, 32, false, false, fullScreen, true);


            Draw();
            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);
            Events.Run();
        }

        private static void Draw()
        {
            DrawBackground();
            int blockSize = (int) Math.Min((SCREEN_WIDTH*0.7)/blockX, (SCREEN_HEIGHT*0.9)/blockY);

              int startX = (int)(SCREEN_WIDTH * 0.35) - (blockX * blockSize / 2);
              int startY = (int)(SCREEN_HEIGHT * 0.5) - (blockY * blockSize / 2);

            for(int x = 0 ; x < blockX ; x++)
            {
                for (int y = 0; y < blockY ; y++)
                {
                    //screen.Draw(new Point(startX + x * blockSize , startY + y * blockSize),currentColor[x,y]);
                    FillRect(startX + x * blockSize, startY + y * blockSize, blockSize, currentColor[x,y]);
                }
            }

            DrawTimer(10,26,45);
            DrawScore(0);
            screen.Update();
        }

        private static void FillRect(int posX, int posY, int size, Color col)
        {
            for(int x = posX; x<(posX+size);x++)
                for(int y = posY; y<(posY+size);y++)
                    screen.Draw(new Point(x, y), col);
        }
        private static void FillRect(int posX, int posY, int sizeX,int sizeY, Color col)
        {
            for (int x = posX; x < (posX + sizeX); x++)
                for (int y = posY; y < (posY + sizeY); y++)
                    screen.Draw(new Point(x, y), col);
        }

        private static void DrawTimer(int hour, int minute, int second)
        {
            if (hour > 99 || minute > 59 || second > 59 || hour < 0 || minute < 0 || second < 0)
                hour = minute = second = 0;

            int timerWidth = (int) (SCREEN_WIDTH * 0.2);
            int timerHeight = (int) (timerWidth/3);
            int timerX = (int)(SCREEN_WIDTH * 0.75);
            int timerY = (int)(SCREEN_HEIGHT * 0.2);

            FillRect(timerX, timerY, timerWidth, timerHeight, Color.FromArgb(255, 102, 0));
            int numberSize = (int)(timerWidth/5);

            DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 0 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(hour.ToString().Substring(0,1)), Color.FromArgb(255, 255, 255), numberSize);
            DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 1 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(hour.ToString().Substring(1,1)), Color.FromArgb(255, 255, 255), numberSize);

            DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 2 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(minute.ToString().Substring(0, 1)), Color.FromArgb(255, 255, 255), numberSize);
            DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 3 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(minute.ToString().Substring(1, 1)), Color.FromArgb(255, 255, 255), numberSize);

            DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 4 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(second.ToString().Substring(0, 1)), Color.FromArgb(255, 255, 255), numberSize);
            DrawNumber((int)(timerX + 0.1 * timerHeight + numberSize / 2 * 5 * 1.4), (int)(timerY + 0.1 * timerHeight), Convert.ToInt32(second.ToString().Substring(1, 1)), Color.FromArgb(255, 255, 255), numberSize);


        }

        private static void DrawScore(int score)
        {
            if(score < 0 || score > 99999999)
                score = 0;

            int scoreWidth = (int) (SCREEN_WIDTH * 0.2);
            int scoreHeight = (int) (scoreWidth/4.5);
            int scoreX = (int)(SCREEN_WIDTH * 0.75);
            int scoreY = (int)(SCREEN_HEIGHT * 0.4);

            FillRect(scoreX, scoreY, scoreWidth, scoreHeight, Color.FromArgb(255, 102, 0));
            String scorestr = score.ToString();

            int numberSize = (int)(scoreHeight * 0.8);

            for (int i = 0; i < 8; i++)
            {
                if (i < scorestr.Length)
                {
                    int tmp = Convert.ToInt32(scorestr.Substring(i,1));
                    DrawNumber((int)(scoreX + 0.1 * scoreHeight + numberSize / 2 * i * 1.4), (int)(scoreY + 0.1 * scoreHeight), tmp, Color.FromArgb(255, 255, 255), numberSize);
                }
                else
                {
                    DrawNumber((int)(scoreX + 0.1 * scoreHeight + numberSize / 2 * i * 1.4), (int)(scoreY + 0.1 * scoreHeight), 0, Color.FromArgb(255, 255, 255), numberSize);
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

        // **** Public methods ****
        public void setTime(String time)
        {

        }
        public void setTime(int hours, int minutes, int seconds)
        {

        }
        public void setScore(int score)
        {
            DrawScore(score);
        }
        public void setNext(short blockID)
        {

        }
        public void setBlock(short blockID, int Xpos, int Ypos)
        {

        }
        public void removeRow(int rownr)
        {

        }
        public void exit()
        {

        }
        public void setStressAnimation()
        {

        }
    }
}
