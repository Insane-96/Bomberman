using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw.OpenGL;

namespace Bomberman
{
	class Bomb
	{
		public int X;
		public int Y;
		public float TimeToExplode;
		ProgressBar TimerBar;

		/*public Bomb()
		{
			TimeToExplode = -1f;
			X = -1;
			Y = -1;
		}*/

		public Bomb(int x, int y, float timeToExplode)
		{
			TimeToExplode = timeToExplode;
			X = x;
			Y = y;
			TimerBar = new ProgressBar(0, (int)(TimeToExplode * 100), (int)(TimeToExplode * 100), X * Game.map.TileSize, Y * Game.map.TileSize, 32, 3, 200, 200, 0, 128, 0, 0, Game.window);
		}

		public bool Fuse(Player player)
		{
			Window window = Game.window;
			Map map = Game.map;
			if (this.TimeToExplode > 0)
			{
				this.TimeToExplode -= window.deltaTime;
				Utils.DrawSprite(window, Game.BombSprite, this.X * map.TileSize + map.Scroll, this.Y * map.TileSize, 0, 0, 32, 32);
				TimerBar.SetValue((int)(TimeToExplode * 100), this, window);
				return false;
			}
			else
			{
				Explode(window, map, player);
				return true;
			}
		}

		private void Explode(Window window, Map map, Player player)
		{
			player.BombsAvailable++;
			player.BombsPlaced--;
			map.Tiles[Utils.GetPos(this.X, this.Y, map.Width)] = Tile.TileType.None;
			DestroyTiles(window, map, player);
		}

		private void DestroyTiles(Window window, Map map, Player player)
		{
			for (int y = -1; y >= -player.BombRadius; y--)
			{
				Tile.TileType tile;
				if (this.Y + y > 0)
					tile = map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)];
				else
					break;

				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.None)
					Utils.DrawRectFilled(window, (this.X * map.TileSize) + map.Scroll, ((this.Y + y) * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Tile.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X && bomb.Y == this.Y + y)
							bomb.TimeToExplode = 0f;
					}

				if (map.PowerUps[Utils.GetPos(this.X, this.Y + y, map.Width)] != null && tile == Tile.TileType.None)
				{
					map.PowerUps[Utils.GetPos(this.X, this.Y + y, map.Width)] = null;
				}
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.Wall)
				{
					if (tile == Tile.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)] = Tile.TileType.None;
					}
					if (!player.SpinyBombs || tile == Tile.TileType.Wall)
						break;
				}
			}
			for (int y = 1; y <= player.BombRadius; y++)
			{
				Tile.TileType tile;
				if (this.Y + y < map.Height)
					tile = map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)];
				else
					break;
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.None)
					Utils.DrawRectFilled(window, (this.X * map.TileSize) + map.Scroll, ((this.Y + y) * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Tile.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X && bomb.Y == this.Y + y)
							bomb.TimeToExplode = 0f;

					}

				if (map.PowerUps[Utils.GetPos(this.X, this.Y + y, map.Width)] != null && tile == Tile.TileType.None)
				{
					map.PowerUps[Utils.GetPos(this.X, this.Y + y, map.Width)] = null;
				}
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.Wall)
				{
					if (tile == Tile.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)] = Tile.TileType.None;
					}
					if (!player.SpinyBombs || tile == Tile.TileType.Wall)
						break;
				}
			}
			for (int x = -1; x >= -player.BombRadius; x--)
			{
				Tile.TileType tile;
				if (this.X + x > 0)
					tile = map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)];
				else
					break;
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.None)
					Utils.DrawRectFilled(window, ((this.X + x) * map.TileSize) + map.Scroll, (this.Y * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Tile.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X + x && bomb.Y == this.Y)
							bomb.TimeToExplode = 0f;

					}

				if (map.PowerUps[Utils.GetPos(this.X + x, this.Y, map.Width)] != null && tile == Tile.TileType.None)
				{
					map.PowerUps[Utils.GetPos(this.X + x, this.Y, map.Width)] = null;
				}
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.Wall)
				{
					if (tile == Tile.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)] = Tile.TileType.None;
					}
					if (!player.SpinyBombs || tile == Tile.TileType.Wall)
						break;
				}
			}
			for (int x = 1; x <= player.BombRadius; x++)
			{
				Tile.TileType tile;
				if (this.X + x < map.Width)
					tile = map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)];
				else
					break;
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.None)
					Utils.DrawRectFilled(window, ((this.X + x) * map.TileSize) + map.Scroll, (this.Y * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Tile.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X + x && bomb.Y == this.Y)
							bomb.TimeToExplode = 0f;

					}

				if (map.PowerUps[Utils.GetPos(this.X + x, this.Y, map.Width)] != null && tile == Tile.TileType.None)
				{
					map.PowerUps[Utils.GetPos(this.X + x, this.Y, map.Width)] = null;
				}
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.Wall)
				{
					if (tile == Tile.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)] = Tile.TileType.None;
					}
					if (!player.SpinyBombs || tile == Tile.TileType.Wall)
						break;
				}
			}
			Utils.DrawRectFilled(window, this.X * map.TileSize + map.Scroll, this.Y * map.TileSize, 32, 32, 255, 128, 0);

		}
	}
}
