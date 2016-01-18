using System;
using System.Collections.Generic;
using System.Drawing;
using Aiv.Fast2D;
using OpenTK;

namespace Bomberman
{
	class Text
	{
		static Sprite sprite;
		static Texture texture;
		static List<int> x;
		static List<int> y;
		static Color color;

		static Text()
		{
			sprite = new Sprite(100, 100);
			texture = new Texture("../../assets/text.png");
			x = new List<int>();
			y = new List<int>();

			for (int i = 0; i < 36; i++)
			{
				x.Add((i * 100) % 600);
				y.Add((i * 100) / 600);
			}
		}

		/// <summary>
		/// Prints a single letter
		/// </summary>
		/// <param name="letter">letter to be printed</param>
		/// <param name="posX">position x of the letter</param>
		/// <param name="posY">position y of the letter</param>
		/// <param name="width">letter width</param>
		/// <param name="height">letter height</param>
		public static void PrintLetter(char letter, int posX, int posY, float width, float height)
		{
			if (letter == 32 || letter == 44)
				return;
			if (letter <= 122 && letter >= 97)
				letter -= (char)32;
			if (letter >= 48 && letter <= 57)
				letter += (char)43;
			sprite.position = new Vector2(posX, posY);
			sprite.scale = new Vector2(width / sprite.Width, height / sprite.Height);
			sprite.DrawTexture(texture, x[letter - 65], y[letter - 65] * 100, 100, 100);
		}

		/// <summary>
		/// Prints a string of text
		/// WARNING: \n and similar are not supported
		/// </summary>
		/// <param name="text">text to be printed</param>
		/// <param name="x">x Position of the text</param>
		/// <param name="y">y Position of the text</param>
		/// <param name="width">single letter width</param>
		/// <param name="height">single letter height</param>
		public static void PrintText(string text, int x, int y, float width, float height)
		{
			for (int i = 0; i < text.Length; i++)
			{
				PrintLetter(text[i], x + i * (int)width, y, width, height);
			}
		}

		/// <summary>
		/// Changes text color
		/// </summary>
		/// <param name="color"></param>
		public static void ChangeColor(Color color)
		{
			byte[] bytes = BitConverter.GetBytes(color.ToArgb());
			ChangeColor(bytes[2], bytes[1], bytes[0], bytes[3]);
		}

		public static void ChangeColor(byte r, byte g, byte b, byte a = 255)
		{
			for (int i = 0; i < texture.Bitmap.Length; i += 4)
			{
				if (texture.Bitmap[i + 3] == 255)
				{
					texture.Bitmap[i] = r;
					texture.Bitmap[i + 1] = g;
					texture.Bitmap[i + 2] = b;
					texture.Bitmap[i + 3] = a;
				}
			}
			texture.Update();
		}
	}
}
