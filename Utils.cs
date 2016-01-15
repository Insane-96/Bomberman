using System;
using Aiv.Fast2D;

namespace Bomberman
{
	static class Utils
	{
		private static Random r = new Random();

		public static int Randomize(int n, int m)
		{
			return r.Next(n, m);
		}

		public static int GetPos(int x, int y, int width) => y * width + x;
	
	}
}