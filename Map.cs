using System;
using Aiv.Draw.OpenGL;

//using windowApp;

namespace Bomberman
{
	class Map
	{
		public enum TileType
		{
			None,
			Wall,
			DestrWall,
			Bomb,
		}
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int TileSize { get; private set; }
		public Map.TileType[] Tiles { get; private set; }
		public PowerUp[] PowerUps; 
		public int Scroll { get; private set; }

		public Map(int tileSize)
		{
			TileSize = tileSize;

			this.Width = 49;
			this.Height = 17;
			Tiles = new Map.TileType[Width * Height];
			PowerUps = new PowerUp[Width * Height];
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
					{
						Tiles[Utils.GetPos(x, y, Width)] = Map.TileType.Wall;
					}
					else if (x % 2 == 0 && y % 2 == 0)
					{
						Tiles[Utils.GetPos(x, y, Width)] = Map.TileType.Wall;
					}
					else if (Utils.Randomize(0, 100) < 25)
					{
						Tiles[Utils.GetPos(x, y, Width)] = Map.TileType.DestrWall;
						if (Utils.Randomize(0, 100) < 5)
						{
							PowerUps[Utils.GetPos(x, y, this.Width)] = new PowerUp(PowerUp.PowerUpsList[Utils.Randomize(0, Enum.GetNames(typeof(PowerUp.PowerUps)).Length)]);
						}
					}
				}
			}

			this.Scroll = 0;
			Tiles[Utils.GetPos(1, 1, this.Width)] = Map.TileType.None;
			Tiles[Utils.GetPos(1, 2, this.Width)] = Map.TileType.None;
			Tiles[Utils.GetPos(2, 1, this.Width)] = Map.TileType.None;
			PowerUps[Utils.GetPos(1, 1, this.Width)] = null;
			PowerUps[Utils.GetPos(1, 2, this.Width)] = null;
			PowerUps[Utils.GetPos(2, 1, this.Width)] = null;

		}

		public void CheckScroll(Player player)
		{
			int visibleTiles = Game.window.width / this.TileSize;
			if (player.X >= visibleTiles / 2 * this.TileSize && player.X <= (this.Width * this.TileSize) - (visibleTiles / 2 * this.TileSize))
			{
				this.Scroll = visibleTiles / 2 * this.TileSize - player.X;
			}
		}

		public void Draw()
		{
			Map map = Game.map;
			Window window = Game.window;
			for (int i = 0; i < map.Tiles.Length; i++)
			{
				if (map.Tiles[i] == Map.TileType.Wall)
				{
					Utils.DrawSprite(window, Game.WallSprite, (i % map.Width) * map.TileSize + map.Scroll, (int)(i / map.Width) * map.TileSize, 0, 0, map.TileSize, map.TileSize);
				}
				else if (map.Tiles[i] == Map.TileType.DestrWall)
				{
					Utils.DrawSprite(window, Game.DestrWallSprite, (i % map.Width) * map.TileSize + map.Scroll, (int)(i / map.Width) * map.TileSize, 0, 0, map.TileSize, map.TileSize);
					if (map.PowerUps[i] != null)
						Utils.DrawFilledCircle(window, (i % map.Width) * map.TileSize + map.Scroll + 16, (int)(i / map.Width) * map.TileSize + 16, 4, 40, 40, 0);
				}
				else if (map.Tiles[i] == Map.TileType.None && map.PowerUps[i] != null)
				{
					Utils.DrawSprite(window, map.PowerUps[i].sprite, (i % map.Width) * map.TileSize + map.Scroll, (int)(i / map.Width) * map.TileSize, 0, 0, 32, 32);
				}
			}
		}
	}
}
