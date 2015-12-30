using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Aiv.Draw.OpenGL;
//using Bomberman;

namespace Bomberman
{
	class Program
	{
		static void Main(string[] args)
		{
			Window window = new Window(1024, 576, "Bomberman", PixelFormat.RGB);
			Map map = new Map(32);
			Game.window = window;
			Game.map = map;
			window.SetIcon("bomb.ico", true);
			window.CursorVisible = false;

			KeyMap player1KeyMap = new KeyMap(KeyCode.Left, KeyCode.Right, KeyCode.Up, KeyCode.Down, KeyCode.Space);

			Player player = new Player(1, 1, map, player1KeyMap, "../../assets/player.png");
			//Player player2 = new Player(22, 15);

			while (window.opened)
			{
				Console.SetCursorPosition(0, 0);
				Console.WriteLine("FPS: {0}            ", 1 / window.deltaTime);

				Utils.Clear(window, 0, 128, 0);

				if (window.GetKey(player.KeyMap.Left) && player.X > 0)
					player.Move(Player.Direction.LEFT);
				else if (window.GetKey(player.KeyMap.Down) && player.Y < map.Height * map.TileSize)
					player.Move(Player.Direction.DOWN);
				else if (window.GetKey(player.KeyMap.Up) && player.Y > 0)
					player.Move(Player.Direction.UP);
				else if (window.GetKey(player.KeyMap.Right) && player.X < map.Width * map.TileSize)
					player.Move(Player.Direction.RIGHT);

				if (window.GetKey(player.KeyMap.PlaceBomb))
				{
					player.PlaceBomb(window, map);
				}

				player.PrintPlayer(window, map);
				for (int i = 0; i < map.Tiles.Length; i++)
				{
					if (map.Tiles[i] == Tile.TileType.Wall)
					{
						Utils.DrawSprite(window, Game.WallSprite, (i % map.Width) * map.TileSize + map.Scroll, (int)(i / map.Width) * map.TileSize, 0, 0, map.TileSize, map.TileSize);
					}
					else if (map.Tiles[i] == Tile.TileType.DestrWall)
					{
						Utils.DrawSprite(window, Game.DestrWallSprite, (i % map.Width) * map.TileSize + map.Scroll, (int)(i / map.Width) * map.TileSize, 0, 0, map.TileSize, map.TileSize);
					}
				}

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

				window.Blit();

				if (window.GetKey(KeyCode.Esc) && window.GetKey(KeyCode.Return))
					window.opened = false;
			}
		}
	}
}
