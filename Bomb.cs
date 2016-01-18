using System.Windows.Forms.VisualStyles;
using Aiv.Fast2D;
using Aiv.Vorbis;
using OpenTK;

namespace Bomberman
{
	class Bomb
	{
		public int X;
		public int Y;
		public float TimeToExplode;
		//private ProgressBar TimerBar;
		private Sprite sprite = new Sprite(32, 32);
		private AudioSource audioSource;
		private bool isJohn;
		public Texture johnTexture;
		public Sprite johnSprite;
		public float johnIndex;
		AudioClip audioClip;

		public Bomb(int x, int y, float timeToExplode, bool john = false)
		{
			TimeToExplode = timeToExplode;
			X = x;
			Y = y;
			//TimerBar = new ProgressBar(0, (int)(TimeToExplode * 100), (int)(TimeToExplode * 100), X * Game.map.TileSize, Y * Game.map.TileSize, 32, 3, 200, 200, 0, 128, 0, 0);
			audioSource = new AudioSource();
			audioClip = new AudioClip("../../assets/explosion" + Utils.Randomize(1, 3) + ".ogg");
			isJohn = john;
			johnTexture = new Texture("../../assets/cenaExpl.png");
			johnSprite = new Sprite(johnTexture.Width / 5 * 5, johnTexture.Height / 3 * 5);
			johnIndex = 0;
		}

		public bool Fuse(Player player)
		{
			Window window = Game.window;
			Map map = Game.map;
			if (this.TimeToExplode > 0)
			{
				this.TimeToExplode -= window.deltaTime;

				this.sprite.position.X = X * map.TileSize + map.Scroll;
				this.sprite.position.Y = Y * map.TileSize;
				this.sprite.DrawTexture(Game.BombTexture, ((int)(this.TimeToExplode * 2) % 2) * 32, 0, 32, 32);
				//TimerBar.SetValue((int)(TimeToExplode * 100));
				if (isJohn && TimeToExplode <= 2 && !audioSource.IsPlaying)
					audioSource.Play(new AudioClip("../../assets/john.ogg"));
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
			if (Game.stateJohn == 20)
			{
				Game.stateJohn = 21;
				player.BombRadius = 7;
				player.SpinyBombs = true;
				DestroyTiles(player);
				player.BombRadius = 1;
				player.SpinyBombs = false;
				johnSprite.position = new Vector2(X * map.TileSize - johnSprite.Width / 2 + 16, Y * map.TileSize - johnSprite.Height / 2 + 16);
			}
			else
			{
				DestroyTiles(player);
			}
			if (!audioSource.IsPlaying)
				audioSource.Play(audioClip);

			//Game.StartMusic();
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

				//if (tile == Map.TileType.DestrWall || tile == Map.TileType.None)
				//Utils.DrawRectFilled(window, (this.X * map.TileSize) + map.Scroll, ((this.Y + y) * map.TileSize), 32, 32, 255, 128, 0);
				if (tile == Map.TileType.Bomb)
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
				//if (tile == Map.TileType.DestrWall || tile == Map.TileType.None)
				//	Utils.DrawRectFilled(window, (this.X * map.TileSize) + map.Scroll, ((this.Y + y) * map.TileSize), 32, 32, 255, 128, 0);
				//else 
				if (tile == Map.TileType.Bomb)
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
				//if (tile == Map.TileType.DestrWall || tile == Map.TileType.None)
				//	Utils.DrawRectFilled(window, ((this.X + x) * map.TileSize) + map.Scroll, (this.Y * map.TileSize), 32, 32, 255, 128, 0);
				//else 
				if (tile == Map.TileType.Bomb)
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
				//if (tile == Map.TileType.DestrWall || tile == Map.TileType.None)
				//	Utils.DrawRectFilled(window, ((this.X + x) * map.TileSize) + map.Scroll, (this.Y * map.TileSize), 32, 32, 255, 128, 0);
				//else 
				if (tile == Map.TileType.Bomb)
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

			if (isJohn)
			{
				for (int x = -player.BombRadius; x <= player.BombRadius; x++)
				{
					for (int y = -player.BombRadius; y <= player.BombRadius; y++)
					{
						Map.TileType tile;
						if (this.X + x < map.Width && this.Y + y < map.Height && this.X + x > 0 && this.Y + y > 0)
							tile = map.Tiles[Utils.GetPos(this.X + x, this.Y + y, map.Width)];
						else
							continue;

						if (map.PowerUps[Utils.GetPos(this.X + x, this.Y + y, map.Width)] != null && tile == Map.TileType.None)
						{
							map.PowerUps[Utils.GetPos(this.X + x, this.Y + y, map.Width)] = null;
						}
						if (tile == Map.TileType.DestrWall || tile == Map.TileType.Wall)
						{
							if (tile == Map.TileType.DestrWall)
							{
								map.Tiles[Utils.GetPos(this.X + x, this.Y + y, map.Width)] = Map.TileType.None;
							}
						}
					}
				}
			}
			//Utils.DrawRectFilled(window, this.X * map.TileSize + map.Scroll, this.Y * map.TileSize, 32, 32, 255, 128, 0);

		}
	}
}
