using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw.OpenGL;

namespace Bomberman
{
	class Player
	{
		public enum Direction
		{
			UP,
			RIGHT,
			DOWN,
			LEFT
		}
		public int BombsPlaced;
		public int BombsAvailable;
		public int BombRadius;
		public float BombFuseTime;
		public bool SpinyBombs;
		public List<Bomb> Bombs;
		public int X { get; private set; }
		public int Y { get; private set; }
		public int MovSpeed { get; private set; }
		public KeyMap KeyMap;
		private Sprite sprite;

		public Player(int x, int y, Map map, KeyMap keyMap, string spritePath)
		{
			X = x * map.TileSize + map.TileSize / 2;
			Y = y * map.TileSize + map.TileSize / 2;
			BombsPlaced = 0;
			BombsAvailable = 3;
			BombRadius = 4;
			BombFuseTime = 4f;
			KeyMap = keyMap;
			SpinyBombs = false;
			MovSpeed = 100;
			sprite = new Sprite(spritePath);

			Bombs = new List<Bomb>();
		}

		public void PlaceBomb(Window window, Map map)
		{
			if (BombsAvailable > 0 && map.Tiles[Utils.GetPos(X / map.TileSize, Y / map.TileSize, map.Width)] == Tile.TileType.None)
			{
				Bombs.Add(new Bomb(X / map.TileSize, Y / map.TileSize, BombFuseTime));

				map.Tiles[Utils.GetPos(X / map.TileSize, Y / map.TileSize, map.Width)] = Tile.TileType.Bomb;

				BombsAvailable--;
				BombsPlaced++;
			}
		}

		public void Move(Direction direction)
		{
			Window window = Game.window;
			Map map = Game.map;
			if (this.X % 32 >= 15 && this.X % 32 <= 17)
			{
				if (direction == Direction.UP && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, map.Width)] == Tile.TileType.None)
				{
					this.Y -= (int)(MovSpeed * window.deltaTime);
				}
				else if (direction == Direction.DOWN && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, map.Width)] == Tile.TileType.None)
				{
					this.Y += (int)(MovSpeed * window.deltaTime);
				}
			}
			else if (this.X % 32 >= 5 && this.X % 32 <= 27)
			{
				if (direction == Direction.UP && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, map.Width)] == Tile.TileType.None)
				{
					this.Y -= (int)(MovSpeed * window.deltaTime);
					xMoveDiagonally();
				}
				else if (direction == Direction.DOWN && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, map.Width)] == Tile.TileType.None)
				{
					this.Y += (int)(MovSpeed * window.deltaTime);
					xMoveDiagonally();
				}
			}
			if (this.Y % 32 >= 15 && this.Y % 32 <= 17)
			{
				if (direction == Direction.RIGHT && map.Tiles[Utils.GetPos((this.X + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Tile.TileType.None)
				{
					this.X += (int)(MovSpeed * window.deltaTime);
				}
				else if (direction == Direction.LEFT && map.Tiles[Utils.GetPos((this.X - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Tile.TileType.None)
				{
					this.X -= (int)(MovSpeed * window.deltaTime);
				}
			}
			else if (this.Y % 32 >= 5 && this.Y % 32 <= 27)
			{
				if (direction == Direction.RIGHT && map.Tiles[Utils.GetPos((this.X + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Tile.TileType.None)
				{
					this.X += (int)(MovSpeed * window.deltaTime);
					yMoveDiagonally();
				}
				else if (direction == Direction.LEFT && map.Tiles[Utils.GetPos((this.X - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Tile.TileType.None)
				{
					this.X -= (int)(MovSpeed * window.deltaTime);
					yMoveDiagonally();
				}
			}
			Game.map.checkScroll(this);
			//Console.WriteLine(this.X + "     ");
			//Console.WriteLine(this.Y + "     ");
		}

		private void yMoveDiagonally()
		{
			if (this.Y % 32 >= 5 && this.Y % 32 < 15)
				this.Y += (int)(MovSpeed * Game.window.deltaTime);
			else if (this.Y % 32 <= 27 && this.Y % 32 > 17)
				this.Y -= (int)(MovSpeed * Game.window.deltaTime);
		}

		private void xMoveDiagonally()
		{
			if (this.X % 32 >= 5 && this.X % 32 < 15)
				this.X += (int)(MovSpeed * Game.window.deltaTime);
			else if (this.X % 32 <= 27 && this.X % 32 > 17)
				this.X -= (int)(MovSpeed * Game.window.deltaTime);
		}

		public void PrintPlayer(Window window, Map map)
		{
			Utils.DrawSprite(window, sprite, X - 16 + map.Scroll, Y - 16, 0, 0, 32, 32);
		}
	}
}
