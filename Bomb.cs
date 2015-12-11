using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
				//Utils.DrawRectFilled(window, (this.X * map.TileSize) + 8, (this.Y * map.TileSize) + 8, 16, 16, 128, 128, 0);
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
			Utils.DrawRectFilled(window, (this.X * map.TileSize) - 32, (this.Y * map.TileSize) - 32, 96, 96, 255, 128, 0);
			map.Tiles[Utils.GetPos(this.X, this.Y, map.Width)] = Tile.TileType.None;
			DestroyTiles(map);
		}

		private void DestroyTiles(Map map)
		{
			for (int x = this.X - 1; x <= this.X + 1; x++)
			{
				for (int y = this.Y - 1; y <= this.Y + 1; y++)
				{
					if (map.Tiles[Utils.GetPos(x, y, map.Width)] == Tile.TileType.Wall)
						map.Tiles[Utils.GetPos(x, y, map.Width)] = Tile.TileType.None;
				}
			}
		}
	}
}
