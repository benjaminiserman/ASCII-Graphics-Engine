using System;
using System.Threading;

public class MainClass 
{
  public static void Main(string[] args) 
  {
	// go ahead and screw with this project if you like
	// I don't know how to do input in java yet, but I'm still looking into that. This project can atleast do drawing.
	// try running it to see what it does
	// it's pong with no paddles!
	// a revolutionary game. the next big game. Basically watching the tv logo 
	// I'm planning on getting 2,000,000 sales an hour on release
	// yeah that's what I based it off of
	// try changing the arguments in the graphics constructor below!
	// wider screen, different character, etc.

	int width = 30, height = 20;
	
    Graphics g = new Graphics(width, height, '.', ConsoleColor.Black); // makes a new graphics screen with width 10, length 10, '.' as the empty character, and black as the default color. (IMPORTANT)

	char[,] ballCharacters = new char[,] // what characters are the ball out made of? These can be any unicode characters, but you want to choose characters of the same width as those of this font.
	{
		{ '/', '-', '\\'},
		{ '|', 'O', '|'},
		{ '\\', '-', '/'},
	};

	ConsoleColor[,] ballColors = new ConsoleColor[,] // what colors go with each of those characters?
	{
		{ ConsoleColor.Blue, ConsoleColor.Blue, ConsoleColor.Blue},
		{ ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Blue},
		{ ConsoleColor.Blue, ConsoleColor.Blue, ConsoleColor.Blue},
	};

	Graphics.Sprite ballSprite = new Graphics.Sprite(ballCharacters, ballColors); // create a new sprite with the given characters and colors

	Ball b = new Ball(3, 4, ballCharacters.GetLength(1), ballCharacters.GetLength(0), ballSprite); // these are just to make the ball work, not neccesarily important for your project
	b.SetBorder(width, height);

	char[,] screenCharacters = new char[height, width]; // create the background, if you want one. You can just steal this code for an OK background
	for (int i = 0; i < height; i++)
	{
		for (int j = 0; j < width; j++)
		{
			char c = ' ';
			if (i == 0 || i == height - 1) c = '=';
			else if (j == 0 || j == width - 1) c = '|';

			screenCharacters[i, j] = c;
		}
	}

	while (true) // game loop
	{
		g.Set(screenCharacters); // set the background, if you want one

		b.Move(); // move ball
		//g.SetChar(b.x, b.y, 'O', ConsoleColor.Blue); // this tells the graphics engine to draw an 'O' in slot (b.x, b.y) (IMPORTANT)
		g.SetMultiple(b.x, b.y, b.sprite); // this tells the graphics engine to draw b.sprite with its top-left corner in slot (b.x, b.y) (IMPORTANT)
		//g.SetText(b.x, b.y, "Supercalifragilisticexpialodocious");

		g.Draw(); // this draws the screen (IMPORTANT)
		
		// take manual control over drawing:
		/*Console.Clear();
		foreach((int x, int y, char character, ConsoleColor color) tuple in g.CustomDraw(true))
		{
			if (tuple.color == ConsoleColor.Yellow)
			{
				Console.BackgroundColor = ConsoleColor.Red;
			}
		}*/

		Thread.Sleep(200);
	}
  }
}