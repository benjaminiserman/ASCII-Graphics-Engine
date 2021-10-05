using System; // copy this entire file to use
using System.Collections.Generic;

public class Graphics
{
	private int width, height; // width and height of display window
	private char empty; // which character represents empty space? defaults to ' ', but '.' looks good too/
	private ConsoleColor defaultColor; // default color to use if color is not specified. Defaults to white.

	private char[] display; // the array of characters. do not touch
	private ConsoleColor[][] colors; // the color of each character

	public Graphics(int width, int height, char empty = ' ', ConsoleColor defaultColor = ConsoleColor.White)
	{
		this.width = width;
		this.height = height;
		this.empty = empty;
		this.defaultColor = defaultColor;

		display = new char[height * width * 2 + height];
		colors = new ConsoleColor[height][];

		for (int y = 0; y < height; y++)
		{
			colors[y] = new ConsoleColor[width];
		}

		Clear();
	}

	public void Draw(bool clear = true) // draws the screen to the console
	{
		if (clear) Console.Clear();

		Console.ForegroundColor = colors[0][0];
		int startIndex = 0;
		string displayString = new string(display);

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if (Console.ForegroundColor != colors[y][x])
				{
					int index = y * width * 2 + (x * 2);
					Console.Write(displayString.Substring(startIndex, index - startIndex));
					startIndex = index;
					Console.ForegroundColor = colors[y][x];
				}
			}
		}

		Console.Write(displayString.Substring(startIndex, height * width * 2 - startIndex));

		Clear();
	}

	public IEnumerable<(int x, int y, char character, ConsoleColor color)> CustomDraw(bool draw)
	{
		Console.ForegroundColor = colors[0][0];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				int index = y * width * 2 + (x * 2);

				Console.ResetColor();
				Console.ForegroundColor = colors[y][x];

				yield return (x, y, display[index], colors[y][x]);

				if (draw)
				{
					Console.Write(display[index]);
					Console.ResetColor();
					Console.Write(display[index + 1]);
				}
			}

			Console.Write('\n');
		}

		Clear();
	}

	private void Clear()
	{	
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				display[y * width * 2 + (x * 2)] = empty;
				display[y * width * 2 + (x * 2) + 1] = ' ';
				colors[y][x] = defaultColor;
			}

			display[y * width * 2 + (width * 2) - 1] = '\n';
		}		
	}

	public void SetChar(int x, int y, char c, ConsoleColor color = ConsoleColor.White) // change a single character & color on the screen
	{
		if (x < 0 || x >= width || y < 0 || y >= height) return;

		display[y * width * 2 + (x * 2)] = c;
		colors[y][x] = color;
	}

	public void SetMultiple(int x, int y, char[,] charArray, ConsoleColor[,] colorArray) // change multiple characters / colors on the screen. must be rectangular
	{
		for (int i = 0; i < charArray.Length; i++)
		{
			for (int j = 0; j < charArray.GetLength(i); j++)
			{
				SetChar(x + i, y + j, charArray[j, i], colorArray[j, i]);
			}
		}
	}

	public void SetMultiple(int x, int y, char[,] charArray, ConsoleColor color = ConsoleColor.White)
	{
		for (int i = 0; i < charArray.Length; i++)
		{
			for (int j = 0; j < charArray.GetLength(i); j++)
			{
				SetChar(x + i, y + j, charArray[j, i], color);
			}
		}
	}

	public void SetMultiple(int x, int y, string s, ConsoleColor[] colorArray)
	{
		int w = 0, v = 0;
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] == '\n')
			{
				w = 0;
				v++;
			}
			else 
			{
				SetChar(x + w, y + v, s[i], colorArray[i - v]);
				w++;
			}
		}
	}

	public void SetMultiple(int x, int y, string s, ConsoleColor color = ConsoleColor.White)
	{
		int w = 0, v = 0;
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] == '\n')
			{
				w = 0;
				v++;
			}
			else 
			{
				SetChar(x + w, y + v, s[i], color);
				w++;
			}
		}
	}

	public void SetMultiple(int x, int y, Sprite sprite) => SetMultiple(x, y, sprite.display, sprite.colorArray);

	public void Set(char[] display, ConsoleColor[][] colors)
	{
		this.display = display;
		this.colors = colors;
	}

	public void Set(char[,] displayArray, ConsoleColor[,] colorArray = null)
	{
		bool isColor = colorArray != null;
		int width = displayArray.GetLength(1);
		int height = displayArray.GetLength(0);

		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				display[i * width * 2 + (j * 2)] = displayArray[i, j];
				
				if (isColor) colors[i][j] = colorArray[i, j];
				else colors[i][j] = defaultColor;
			}

			display[i * width * 2 + (width * 2) - 1] = '\n';
		}
	}

	public void SetText(int x, int y, string text, ConsoleColor color = ConsoleColor.White)
	{
		if (x < 0 || x >= width || y < 0 || y >= height) return;

		int start = y * width * 2 + (x * 2);
		int newLineCount = 0;
		int cx = x, cy = y;
		for (int i = 0; i < text.Length; i++)
		{
			if (i + start < display.Length) 
			{
				if (display[i + start] == '\n') 
				{
					newLineCount++;
					cy++;
					cx = 0;
				}
				
				display[i + start + newLineCount] = text[i];
				colors[cy][cx] = color;
				cx += i % 2;
			}
		}
	}

	public struct Sprite // stores characters and colors, can be drawn with SetMultiple(int, int, Sprite)
	{
		public string display;
		public ConsoleColor[] colorArray;

		public Sprite(string display, ConsoleColor[] colorArray)
		{
			if (display.Length != colorArray.Length) throw new Exception("display and colorArray must have same length!");

			this.display = display;
			this.colorArray = colorArray;
		}

		public Sprite(string display, ConsoleColor color = ConsoleColor.White)
		{
			this.display = display;
			colorArray = new ConsoleColor[display.Length];
			for (int i = 0; i < colorArray.Length; i++)
			{
				colorArray[i] = color;
			}
		}

		public Sprite(char[,] charArray, ConsoleColor[,] colorArray)
		{
			if (charArray.Length != colorArray.Length) throw new Exception("charArray and colorArray must have same length!");

			int height = charArray.GetLength(0);
			int width = charArray.GetLength(1);

			char[] flatArray = new char[height * width + height];
			this.colorArray = new ConsoleColor[height * width];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					flatArray[i * (width + 1) + j] = charArray[i, j];
					this.colorArray[i * width + j] = colorArray[i, j];
				}

				flatArray[i * (width + 1) + width] = '\n';
			}

			display = new string(flatArray);
		}

		public Sprite(char[,] charArray, ConsoleColor color = ConsoleColor.White)
		{
			int height = charArray.GetLength(0);
			int width = charArray.GetLength(1);

			char[] flatArray = new char[height * width + height];
			colorArray = new ConsoleColor[height * width];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					flatArray[i * (width + 1) + j] = charArray[i, j];
					colorArray[i * width + j] = color;
				}

				flatArray[i * (width + 1) + width] = '\n';
			}

			display = new string(flatArray);
		}
	}
}