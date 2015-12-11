using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Aiv.Draw;
using windowApp;

namespace Bomberman
{
	class Program
	{
		static void Main(string[] args)
		{
			Window window = new Window(1024, 576, "Bomberman", PixelFormat.RGB);
			window.SetIcon("bomb.ico", true);
			window.CursorVisible = false;

			Map map = new Map(32, AppDomain.CurrentDomain.BaseDirectory + "mappaBomberman2.csv");

			Player player = new Player(1, 1, map, AppDomain.CurrentDomain.BaseDirectory + "mappaBomberman.csv");
			//Player player2 = new Player(22, 15);

			while (window.opened)
			{
				Utils.Clear(window);
				
				if (window.GetKey(KeyCode.Left) && player.X > 0)
					player.Move(player.X - 1, player.Y, map);
				else if (window.GetKey(KeyCode.Down) && player.Y < map.Height)
					player.Move(player.X, player.Y + 1, map);
				else if (window.GetKey(KeyCode.Up) && player.Y > 0)
					player.Move(player.X, player.Y - 1, map);
				else if (window.GetKey(KeyCode.Right) && player.X < map.Width)
					player.Move(player.X + 1, player.Y, map);
				else
					player.CanMove();

				if (window.GetKey(KeyCode.Space))
				{
					player.PlaceBomb(window, map);
				}

				player.PrintPlayer(window, map);
				for (int i = 0; i < map.Tiles.Length; i++)
				{
					if (map.Tiles[i] == Tile.TileType.Wall)
					{
						Utils.DrawRectFilled(window, (i % map.Width) * map.TileSize, (int)(i / map.Width) * map.TileSize, map.TileSize, map.TileSize, 100, 51, 0);
						Utils.DrawRect(window, (i % map.Width) * map.TileSize, (int)(i / map.Width) * map.TileSize, map.TileSize, map.TileSize, 131, 71, 0);
					}
				}

				List<Bomb> toRemove = new List<Bomb>();
				foreach (Bomb bomb in player.Bombs)
				{
					if (bomb.Fuse(window, map, player))
						toRemove.Add(bomb);
				}
				foreach (var bombToRemove in toRemove)
				{
					player.Bombs.Remove(bombToRemove);
				}
				toRemove.Clear();

				//Console.SetCursorPosition(0, 0);
				//Console.WriteLine("FPS: {0}         ", 60f / window.deltaTime / 60f);
				window.Blit();
			}
		}
	}
}
