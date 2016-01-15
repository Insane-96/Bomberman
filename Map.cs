using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;
using Aiv.Fast2D;

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
		private Sprite[] Sprites;
		public Map.TileType[] Tiles { get; private set; }
		public PowerUp[] PowerUps;
		public Sprite[] PowerUpSprites;
		public int Scroll { get; private set; }

		public Map(int tileSize)
		{
			try
			{
				TileSize = tileSize;

				this.Width = 49;
				this.Height = 17;
				Sprites = new Sprite[Width * Height];
				Tiles = new Map.TileType[Width * Height];
				PowerUps = new PowerUp[Width * Height];
				PowerUpSprites = new Sprite[Width * Height];
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
								PowerUps[Utils.GetPos(x, y, this.Width)] =
									new PowerUp(PowerUp.PowerUpsList[Utils.Randomize(0, Enum.GetNames(typeof(PowerUp.PowerUps)).Length)]);
								PowerUpSprites[Utils.GetPos(x, y, this.Width)] = new Sprite(32, 32);
							}
						}
						Sprites[Utils.GetPos(x, y, this.Width)] = new Sprite(32, 32);
						Sprites[Utils.GetPos(x, y, this.Width)].position.X = x * 32;
						Sprites[Utils.GetPos(x, y, this.Width)].position.Y = y * 32;
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
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		public void CheckScroll(Player player)
		{
			int visibleTiles = Game.window.Width / this.TileSize;
			if (player.X >= visibleTiles / 2 * this.TileSize && player.X <= (this.Width * this.TileSize) - (visibleTiles / 2 * this.TileSize))
			{
				this.Scroll = visibleTiles / 2 * this.TileSize - player.X;
			}
			for (int i = 0; i < this.Tiles.Length; i++)
			{
				this.Sprites[i].position.X = (i % Width) * TileSize + Scroll;
				//Console.SetCursorPosition(0, 6);
				//Console.WriteLine("Scroll {this.Scroll}");
			}
		}

		public void Draw()
		{
			Map map = Game.map;

			for (int i = 0; i < map.Tiles.Length; i++)
			{
				if (map.Tiles[i] == TileType.Wall)
				{
					Sprites[i].DrawTexture(Game.WallTexture);
				}
				else if (map.Tiles[i] == TileType.DestrWall)
				{
					Sprites[i].DrawTexture(Game.DestrWallTexture);
				}
				else if (map.Tiles[i] == TileType.None && map.PowerUps[i] != null)
				{
					PowerUpSprites[i].position.X = (i % Width) * TileSize + Scroll;
					PowerUpSprites[i].position.Y = (i / Width) * TileSize;
					PowerUpSprites[i].DrawTexture(Game.PowerUpTexture);
				}
			}
		}
	}
}
