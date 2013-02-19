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
        static int SCREEN_HEIGHT = (int) System.Windows.SystemParameters.PrimaryScreenHeight;
        static int SCREEN_WIDTH = (int) System.Windows.SystemParameters.PrimaryScreenWidth;
        static int blockY;
        static int blockX;

        static bool backgroundCached = false; 
        static Color[,] backgroundCache = new Color[SCREEN_WIDTH, SCREEN_HEIGHT];


       static bool[,] tetriBoard;
       static Color[,] currentColor; 

        public Graphics()
        {
            fullScreen = true;
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
            screen.Update();
        }

        private static void FillRect(int posX, int posY, int size, Color col)
        {
            for(int x = posX; x<(posX+size);x++)
                for(int y = posY; y<(posY+size);y++)
                    screen.Draw(new Point(x, y), col);
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
                if(i < size/2)

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

        }
        public void setNext(short blockID)
        {

        }
        public void setBlock(short blockID, int Xpos, int Ypos)
        {

        }
        public void removeRow()
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
