using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Aiv.Draw.OpenGL;
//using Bomberman;

namespace Bomberman
{
	class Program
	{
		static void Main(string[] args)
		{
			Window window = new Window(976, 544, "Bomberman", PixelFormat.RGB);
			Map map = new Map(32);
			Game.window = window;
			Game.map = map;
			window.SetIcon("../../assets/bomb.ico", true);
			window.CursorVisible = false;

			KeyMap player1KeyMap = new KeyMap(KeyCode.Left, KeyCode.Right, KeyCode.Up, KeyCode.Down, KeyCode.Space);

			Player player = new Player(1, 1, map, player1KeyMap, "../../assets/player.png");
			//Player player2 = new Player(22, 15);

			MessageBox.Show("Use Arrows to move\nSpace to place bombs\nESC+Enter to exit\nPress Enter to start");

			while (window.opened)
			{
				Console.SetCursorPosition(0, 0);
				Console.WriteLine("FPS: {0}            ", 1 / window.deltaTime);

				//Clear window
				Utils.Clear(window, 31, 139, 0);

				player.checkMovement();

				map.Draw();
				player.Draw();

				player.PrintInfo();

				//Fuse bombs
				List<Bomb> toRemove = new List<Bomb>();
				foreach (Bomb bomb in player.Bombs)
				{
					if (bomb.Fuse(player))
						toRemove.Add(bomb);
				}
				foreach (var bombToRemove in toRemove)
				{
					player.Bombs.Remove(bombToRemove);
				}
				toRemove.Clear();

				//Draw
				window.Blit();

				//Esc + Return (Enter) closes the game
				if (window.GetKey(KeyCode.Esc) && window.GetKey(KeyCode.Return))
					window.opened = false;
			}
		}
	}
}
