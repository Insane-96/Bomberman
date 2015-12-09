using System;
using System.Runtime.InteropServices;
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

			Map map = new Map(24, 17, 32, AppDomain.CurrentDomain.BaseDirectory + "mappaBomberman.csv");

			Player player = new Player(1, 1, map, AppDomain.CurrentDomain.BaseDirectory + "mappaBomberman.csv");
			//Player player2 = new Player(22, 15);

			while (window.opened)
			{
				Utils.Clear(window);

				if (window.GetKey(KeyCode.Left))
					player.Move(player.X - 1, player.Y, map);
				else if (window.GetKey(KeyCode.Down))
					player.Move(player.X, player.Y + 1, map);
				else if (window.GetKey(KeyCode.Up))
					player.Move(player.X, player.Y - 1, map);
				else if (window.GetKey(KeyCode.Right))
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

				foreach (Bomb bomb in player.Bombs)
				{
					if (bomb.TimeToExplode > 0)
					{
						bomb.TimeToExplode -= window.deltaTime;
						Utils.DrawRectFilled(window, (bomb.X * map.TileSize) + 8, (bomb.Y * map.TileSize) + 8, 16, 16, 128, 128, 0);
					}
					else if (bomb.TimeToExplode < 0 && bomb.TimeToExplode > -1)
					{
						Utils.DrawRectFilled(window, (bomb.X * map.TileSize)  -32, (bomb.Y * map.TileSize) - 32, 96, 96, 255, 128, 0);
						map.Tiles[Utils.GetPos(bomb.X, bomb.Y, map.Width)] = Tile.TileType.None;
						bomb.X = -1;
						bomb.Y = -1;
						bomb.TimeToExplode = -1f;
						player.BombsAvailable++;
						player.BombsPlaced--;
					}
				}

				//Console.SetCursorPosition(0, 0);
				//Console.WriteLine("FPS: {0}         ", 60f / window.deltaTime / 60f);
				window.Blit();
			}
		}
	}
}
