using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw.OpenGL;

namespace Bomberman
{
	class ProgressBar
	{
		/// <summary>
		/// Minimum value of the Progress Bar
		/// </summary>
		public int Minimum;
		/// <summary>
		/// Maximum value of the Progress Bar
		/// </summary>
		public int Maximum;
		/// <summary>
		/// Progress Bar Width in px
		/// </summary>
		public int Width;
		/// <summary>
		/// Progress Bar Height in px
		/// </summary>
		public int Height;
		/// <summary>
		/// Foreground Red Color of the progress Bar
		/// </summary>
		public byte FColorR;
		/// <summary>
		/// Foreground Green Color of the progress Bar
		/// </summary>
		public byte FColorG;
		/// <summary>
		/// Foreground Blue Color of the progress Bar
		/// </summary>
		public byte FColorB;
		/// <summary>
		/// Background Red Color of the progress Bar
		/// </summary>
		public byte BColorR;
		/// <summary>
		/// Background Green Color of the progress Bar
		/// </summary>
		public byte BColorG;
		/// <summary>
		/// Background Blue Color of the progress Bar
		/// </summary>
		public byte BColorB;

		public int PosX;
		public int PosY;

		public int value;

		public void SetValue(int value, Window window)
		{
			this.value = value;
			Draw(window);
		}

		private void Draw(Window window)
		{
			for (int x = 0; x < Width; x++)
			{
				if (x < Width / (float)(Maximum - Minimum) * value)
					Utils.DrawVertLine(window, PosX * 32 + x, PosY * 32, Height, FColorR, FColorG, FColorB);
				else
					Utils.DrawVertLine(window, PosX * 32 + x, PosY * 32, Height, BColorR, BColorG, BColorB);
			}
		}

		public ProgressBar(int minimum, int maximum, int startingValue, int posX, int posY, int width, int height, byte fR, byte fG, byte fB, byte bR, byte bG, byte bB, Window window)
		{
			this.Minimum = minimum;
			this.Maximum = maximum;
			this.value = startingValue;
			this.PosX = posX;
			this.PosY = posY;
			this.Width = width;
			this.Height = height;
			this.FColorR = fR;
			this.FColorG = fG;
			this.FColorB = fB;
			this.BColorR = bR;
			this.BColorG = bG;
			this.BColorB = bB;
			Draw(window);
		}

	}
}
