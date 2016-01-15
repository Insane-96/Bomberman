using System;
using System.Collections.Generic;
using System.Resources;
using System.Windows.Forms;
using Aiv.Fast2D;
//using Bomberman;

namespace Bomberman
{
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			MessageBox.Show("Use Arrows to move\nSpace to place bombs\nESC+Enter to exit\nPress Enter to start");

			while (Game.window.opened)
			{
				Game.Update();
			}
		}
	}
}
