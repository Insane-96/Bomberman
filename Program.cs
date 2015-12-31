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

				Utils.Clear(window, 31, 139, 0);

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

				for (int i = 0; i < map.Tiles.Length; i++)
				{
					if (map.Tiles[i] == Tile.TileType.Wall)
					{
						Utils.DrawSprite(window, Game.WallSprite, (i % map.Width) * map.TileSize + map.Scroll, (int)(i / map.Width) * map.TileSize, 0, 0, map.TileSize, map.TileSize);
					}
					else if (map.Tiles[i] == Tile.TileType.DestrWall)
					{
						Utils.DrawSprite(window, Game.DestrWallSprite, (i % map.Width) * map.TileSize + map.Scroll, (int)(i / map.Width) * map.TileSize, 0, 0, map.TileSize, map.TileSize);
						if (map.PowerUps[i] != null)
							Utils.DrawFilledCircle(window, (i % map.Width) * map.TileSize + map.Scroll + 16, (int)(i / map.Width) * map.TileSize + 16, 4, 40, 40, 0);
					}
					else if (map.Tiles[i] == Tile.TileType.None && map.PowerUps[i] != null)
					{
						Utils.DrawSprite(window, map.PowerUps[i].sprite, (i % map.Width) * map.TileSize + map.Scroll, (int)(i / map.Width) * map.TileSize, 0, 0, 32, 32);
					}
				}
				player.PrintPlayer(window, map);

				Console.SetCursorPosition(0, 2);
				Console.WriteLine("Bombs: {0}", player.BombsAvailable+player.BombsPlaced);
				Console.WriteLine("Radius: {0}", player.BombRadius);
				Console.WriteLine("Spiny Bombs: {0}    ", player.SpinyBombs);
				Console.WriteLine("Movement Speed: {0}   ", player.MovSpeed);
				Console.WriteLine("Fuse time: {0}  ", player.BombFuseTime);
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
