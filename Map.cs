using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

//using windowApp;

namespace Bomberman
{
	class Map
	{

		public int Width { get; private set; }
		public int Height { get; private set; }
		public int TileSize { get; private set; }
		public Tile.TileType[] Tiles { get; private set; }
		public PowerUp[] PowerUps; 
		public int Scroll { get; private set; }

		public Map(int tileSize)
		{
			TileSize = tileSize;

			this.Width = 49;
			this.Height = 17;
			Tiles = new Tile.TileType[Width * Height];
			PowerUps = new PowerUp[Width * Height];
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
					{
						Tiles[Utils.GetPos(x, y, Width)] = Tile.TileType.Wall;
					}
					else if (x % 2 == 0 && y % 2 == 0)
					{
						Tiles[Utils.GetPos(x, y, Width)] = Tile.TileType.Wall;
					}
					else if (Utils.Randomize(0, 100) < 25)
					{
						Tiles[Utils.GetPos(x, y, Width)] = Tile.TileType.DestrWall;
						if (Utils.Randomize(0, 100) < 5)
						{
							PowerUps[Utils.GetPos(x, y, this.Width)] = new PowerUp(PowerUp.PowerUpsList[Utils.Randomize(0, Enum.GetNames(typeof(PowerUp.PowerUps)).Length)]);
						}
					}
				}
			}

			this.Scroll = 0;
			Tiles[Utils.GetPos(1, 1, this.Width)] = Tile.TileType.None;
			Tiles[Utils.GetPos(1, 2, this.Width)] = Tile.TileType.None;
			Tiles[Utils.GetPos(2, 1, this.Width)] = Tile.TileType.None;

		}

		public void checkScroll(Player player)
		{
			int visibleTiles = Game.window.width / this.TileSize;
			if (player.X >= visibleTiles / 2 * this.TileSize && player.X <= (this.Width * this.TileSize) - (visibleTiles / 2 * this.TileSize))
			{
				this.Scroll = visibleTiles / 2 * this.TileSize - player.X;
			}
		}
	}
}
