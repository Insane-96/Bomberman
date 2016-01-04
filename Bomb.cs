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
			TimerBar = new ProgressBar(0, (int)(TimeToExplode * 100), (int)(TimeToExplode * 100), X * Game.map.TileSize, Y * Game.map.TileSize, 32, 3, 200, 200, 0, 128, 0, 0);
		}

		public bool Fuse(Player player)
		{
			Window window = Game.window;
			Map map = Game.map;
			if (this.TimeToExplode > 0)
			{
				this.TimeToExplode -= window.deltaTime;
				Utils.DrawSprite(window, Game.BombSprite, this.X * map.TileSize + map.Scroll, this.Y * map.TileSize, 0, 0, 32, 32);
				TimerBar.SetValue((int)(TimeToExplode * 100));
				return false;
			}
			else
			{
				Explode(player);
				return true;
			}
		}

		private void Explode(Player player)
		{
			Map map = Game.map;
			player.BombsAvailable++;
			player.BombsPlaced--;
			map.Tiles[Utils.GetPos(this.X, this.Y, map.Width)] = Map.TileType.None;
			DestroyTiles(player);
		}

		private void DestroyTiles(Player player)
		{
			Map map = Game.map;
			Window window = Game.window;
			for (int y = -1; y >= -player.BombRadius; y--)
			{
				Map.TileType tile;
				if (this.Y + y > 0)
					tile = map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)];
				else
					break;

				if (tile == Map.TileType.DestrWall || tile == Map.TileType.None)
					Utils.DrawRectFilled(window, (this.X * map.TileSize) + map.Scroll, ((this.Y + y) * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Map.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X && bomb.Y == this.Y + y)
							bomb.TimeToExplode = 0f;
					}

				if (map.PowerUps[Utils.GetPos(this.X, this.Y + y, map.Width)] != null && tile == Map.TileType.None)
				{
					map.PowerUps[Utils.GetPos(this.X, this.Y + y, map.Width)] = null;
				}
				if (tile == Map.TileType.DestrWall || tile == Map.TileType.Wall)
				{
					if (tile == Map.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)] = Map.TileType.None;
					}
					if (!player.SpinyBombs || tile == Map.TileType.Wall)
						break;
				}
			}
			for (int y = 1; y <= player.BombRadius; y++)
			{
				Map.TileType tile;
				if (this.Y + y < map.Height)
					tile = map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)];
				else
					break;
				if (tile == Map.TileType.DestrWall || tile == Map.TileType.None)
					Utils.DrawRectFilled(window, (this.X * map.TileSize) + map.Scroll, ((this.Y + y) * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Map.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X && bomb.Y == this.Y + y)
							bomb.TimeToExplode = 0f;

					}

				if (map.PowerUps[Utils.GetPos(this.X, this.Y + y, map.Width)] != null && tile == Map.TileType.None)
				{
					map.PowerUps[Utils.GetPos(this.X, this.Y + y, map.Width)] = null;
				}
				if (tile == Map.TileType.DestrWall || tile == Map.TileType.Wall)
				{
					if (tile == Map.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X, this.Y + y, map.Width)] = Map.TileType.None;
					}
					if (!player.SpinyBombs || tile == Map.TileType.Wall)
						break;
				}
			}
			for (int x = -1; x >= -player.BombRadius; x--)
			{
				Map.TileType tile;
				if (this.X + x > 0)
					tile = map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)];
				else
					break;
				if (tile == Map.TileType.DestrWall || tile == Map.TileType.None)
					Utils.DrawRectFilled(window, ((this.X + x) * map.TileSize) + map.Scroll, (this.Y * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Map.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X + x && bomb.Y == this.Y)
							bomb.TimeToExplode = 0f;

					}

				if (map.PowerUps[Utils.GetPos(this.X + x, this.Y, map.Width)] != null && tile == Map.TileType.None)
				{
					map.PowerUps[Utils.GetPos(this.X + x, this.Y, map.Width)] = null;
				}
				if (tile == Map.TileType.DestrWall || tile == Map.TileType.Wall)
				{
					if (tile == Map.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)] = Map.TileType.None;
					}
					if (!player.SpinyBombs || tile == Map.TileType.Wall)
						break;
				}
			}
			for (int x = 1; x <= player.BombRadius; x++)
			{
				Map.TileType tile;
				if (this.X + x < map.Width)
					tile = map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)];
				else
					break;
				if (tile == Map.TileType.DestrWall || tile == Map.TileType.None)
					Utils.DrawRectFilled(window, ((this.X + x) * map.TileSize) + map.Scroll, (this.Y * map.TileSize), 32, 32, 255, 128, 0);
				else if (tile == Map.TileType.Bomb)
					foreach (var bomb in player.Bombs)
					{
						if (bomb.X == this.X + x && bomb.Y == this.Y)
							bomb.TimeToExplode = 0f;

					}

				if (map.PowerUps[Utils.GetPos(this.X + x, this.Y, map.Width)] != null && tile == Map.TileType.None)
				{
					map.PowerUps[Utils.GetPos(this.X + x, this.Y, map.Width)] = null;
				}
				if (tile == Map.TileType.DestrWall || tile == Map.TileType.Wall)
				{
					if (tile == Map.TileType.DestrWall)
					{
						map.Tiles[Utils.GetPos(this.X + x, this.Y, map.Width)] = Map.TileType.None;
					}
					if (!player.SpinyBombs || tile == Map.TileType.Wall)
						break;
				}
			}
			Utils.DrawRectFilled(window, this.X * map.TileSize + map.Scroll, this.Y * map.TileSize, 32, 32, 255, 128, 0);

		}
	}
}
