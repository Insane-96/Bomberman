using System;
using Aiv.Draw;

namespace windowApp
{
	static class Utils
	{
		private static Random r = new Random();

		public static int Randomize(int n, int m) => r.Next(n, m);
		public static int GetPos(int x, int y, int width) => (y * width) + x;

		public static void PutPixel(Window window, byte r, byte g, byte b, int x, int y)
		{
			if (x > window.width - 1 || x < 0 || y > window.height || y < 0) return;
			int pos = (y * 3 * window.width) + x * 3;

			window.bitmap[pos] = r;
			window.bitmap[pos + 1] = g;
			window.bitmap[pos + 2] = b;
		}

		public static void Clear(Window window, byte r = 0, byte g = 0, byte b = 0)
		{
			for (int y = 0; y < window.height; y++)
			{
				for (int x = 0; x < window.width; x++)
				{
					PutPixel(window, r, g, b, x, y);
				}
			}
		}

		public static void DrawHorizLine(Window window, int x, int y, int width, byte r, byte g, byte b)
		{
			for (int posX = 0; posX < width; posX++)
			{
				PutPixel(window, r, g, b, x + posX, y);
			}
		}

		public static void DrawVertLine(Window window, int x, int y, int height, byte r, byte g, byte b)
		{
			for (int posY = 0; posY < height; posY++)
			{
				PutPixel(window, r, g, b, x, y + posY);
			}
		}

		public static void DrawRect(Window window, int x, int y, int width, int height, byte r, byte g, byte b)
		{
			DrawHorizLine(window, x, y, width, r, g, b);
			DrawVertLine(window, x, y, height, r, g, b);
			DrawHorizLine(window, x, y + height, width, r, g, b);
			DrawVertLine(window, x + width, y, height, r, g, b);
		}

		public static void DrawRectFilled(Window window, int x, int y, int width, int height, byte r, byte g, byte b)
		{
			for (int posY = 0; posY < height; posY++)
				DrawHorizLine(window, x, y + posY, width, r, g, b);

		}
	}
}