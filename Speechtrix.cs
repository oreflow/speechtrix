using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Core;

namespace Speechtrix
{
    class Speechtrix
    {
        private static Surface screen;
        static double SCREEN_HEIGHT = 500;//System.Windows.SystemParameters.PrimaryScreenHeight;
        static double SCREEN_WIDTH = 700;//System.Windows.SystemParameters.PrimaryScreenWidth;
        static int blockY = 35;
        static int blockX = 20;


       static bool[,] tetriBoard = new bool[blockX, blockY];
       static Color[,] currentColor = new Color[blockX, blockY]; 

        public static void Main(string[] args)
        {
            bool swit = false;
            for(int x = 0; x<blockX;x++)
                for(int y = 0; y<blockY;y++)
                {
                    if(swit)
                    currentColor[x,y] = Color.FromArgb(0,0,0);
                else
                    currentColor[x,y] = Color.FromArgb(255,255,255);
                swit = !swit;
                }

           

            int SCREEN_HEIGHT = 480;// (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            int SCREEN_WIDTH = 640;// (int)System.Windows.SystemParameters.PrimaryScreenWidth;

            bool fullScreen = false;
            screen = Video.SetVideoMode(SCREEN_WIDTH, SCREEN_HEIGHT, 32, false, false, fullScreen, true);

            DrawBackground();
            Draw();
            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);
            Events.Tick += new EventHandler<TickEventArgs>(ApplicationTickEventHandler);
            Events.Run();
        }

        private static void Draw()
        {
            int blockSize = (int) Math.Min((SCREEN_WIDTH*0.7)/blockX, (SCREEN_HEIGHT*0.9)/blockY);

              int startX = (int)(SCREEN_WIDTH * 0.35) - (blockX * blockSize / 2);
              int startY = (int)(SCREEN_HEIGHT * 0.45) - (blockY * blockSize / 2);

            for(int x = 0 ; x < blockX ; x++)
            {
                for (int y = 0; y < blockY ; y++)
                {
                    screen.Draw(new Point(startX + x * blockSize , startY + y * blockSize),currentColor[x,y]);
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
        private static void DrawBackground()
        {
            Vector topLeft = new Vector(255,0,0); // red
	        Vector topRight = new Vector(0,0,255); // blue
	        Vector bottomLeft = new Vector(0,255,0); // green
	        Vector bottomRight = new Vector(255,255,0); // yellow
            Vector[] leftSide = new Vector[(int)SCREEN_HEIGHT];
	        Vector[] rightSide = new Vector[(int)SCREEN_HEIGHT];

	        Interpolate( topLeft, bottomLeft, ref leftSide );
	        Interpolate( topRight, bottomRight, ref rightSide );

	        Vector [] colors = new Vector[(int)SCREEN_WIDTH];

	        for( int y=0; y<SCREEN_HEIGHT; ++y )
	        {
		        Interpolate(leftSide[y],rightSide[y],ref colors);

		        for( int x=0; x<SCREEN_WIDTH; ++x )
		        {
                    screen.Draw(new Point(x, y), Color.FromArgb((int)colors[x].X,(int)colors[x].Y,(int)colors[x].Z));
		        }
	        }
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


        private static void ApplicationTickEventHandler(object sender, TickEventArgs args)
        {


        }

        private static void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }
    }
}
 
