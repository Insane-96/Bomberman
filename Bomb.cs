using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw;
using windowApp;

namespace Bomberman
{
	class Bomb
	{
		public int X;
		public int Y;
		public float TimeToExplode;

		public Bomb()
		{
			TimeToExplode = -1f;
			X = -1;
			Y = -1;
		}

		public Bomb(int x, int y, float timeToExplode)
		{
			TimeToExplode = timeToExplode;
			X = x;
			Y = y;
		}

		public bool Fuse(Window window, Map map, Player player)
		{
			if (this.TimeToExplode > 0)
			{
				this.TimeToExplode -= window.deltaTime;
				Utils.DrawCircle(window, (this.X * map.TileSize) + 16, (this.Y * map.TileSize) + 16, 12, 128, 128, 0);
				//CheckForBombsNear(player, map);
				return false;
			}
			else
			{
				Explode(window, map, player);
				return true;
			}
		}

		/*private void CheckForBombsNear(Player player, Map map)
		{
			for (int x = this.X - 1; x <= this.X + 1; x++)
			{
				for (int y = this.Y; y <= this.Y + 1; y++)
				{
					if (map.Tiles[Utils.GetPos(x, y, map.Width)] == Tile.TileType.Bomb)

				}
			}
		}*/

		private void Explode(Window window, Map map, Player player)
		{
			player.BombsAvailable++;
			player.BombsPlaced--;
			//Utils.DrawRectFilled(window, (this.X * map.TileSize), (this.Y * map.TileSize) - (player.BombRadius * 32), 32, player.BombRadius * 32 * 2, 255, 128, 0);
			//Utils.DrawRectFilled(window, (this.X * map.TileSize) - (player.BombRadius * 32), (this.Y * map.TileSize), player.BombRadius * 32 * 2, 32, 255, 128, 0);
			map.Tiles[Utils.GetPos(this.X, this.Y, map.Width)] = Tile.TileType.None;
			DestroyTiles(window, map, player);
		}

		private void DestroyTiles(Window window, Map map, Player player)
		{
			for (int y = 0; y >= -player.BombRadius; y--)
			{
				Tile.TileType tile;
				if (this.Y + y > 0)
					tile = map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)];
				else
					break;

				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.None)
					Utils.DrawRectFilled(window, (this.X * map.TileSize), ((this.Y + y) * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Tile.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X && bomb.Y == this.Y + y)
							bomb.TimeToExplode = 0f;

					}
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.Wall)
				{
					if (tile == Tile.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)] = Tile.TileType.None;
					}
					break;
				}
			}
			for (int y = 0; y <= player.BombRadius; y++)
			{
				Tile.TileType tile;
				if (this.Y + y < map.Height)
					tile = map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)];
				else
					break;
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.None)
					Utils.DrawRectFilled(window, (this.X * map.TileSize), ((this.Y + y) * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Tile.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X && bomb.Y == this.Y + y)
							bomb.TimeToExplode = 0f;

					}
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.Wall)
				{
					if (tile == Tile.TileType.DestrWall)
						map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)] = Tile.TileType.None;
					break;
				}
			}
			for (int x = 0; x >= -player.BombRadius; x--)
			{
				Tile.TileType tile;
				if (this.X + x > 0)
					tile = map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)];
				else
					break;
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.None)
					Utils.DrawRectFilled(window, ((this.X + x) * map.TileSize), (this.Y * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Tile.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X + x && bomb.Y == this.Y)
							bomb.TimeToExplode = 0f;

					}
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.Wall)
				{
					if (tile == Tile.TileType.DestrWall)
						map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)] = Tile.TileType.None;
					break;
				}
			}
			for (int x = 0; x <= player.BombRadius; x++)
			{
				Tile.TileType tile;
				if (this.X + x < map.Width)
					tile = map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)];
				else
					break;
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.None)
					Utils.DrawRectFilled(window, ((this.X + x) * map.TileSize), (this.Y * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Tile.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X + x && bomb.Y == this.Y)
							bomb.TimeToExplode = 0f;

					}
				if (tile == Tile.TileType.DestrWall || tile == Tile.TileType.Wall)
				{
					if (tile == Tile.TileType.DestrWall)
						map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)] = Tile.TileType.None;
					break;
				}
			}
		}
	}
}
