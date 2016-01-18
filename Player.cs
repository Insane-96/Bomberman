using System;
using System.Collections.Generic;
using Aiv.Fast2D;
using Aiv.Vorbis;

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
		public int MovSpeed;
		private KeyMap KeyMap;
		private Sprite sprite;
		private int animSpeed;
		private float animOffest;
		private bool isMoving = false;
		private AudioSource audioSource;
		private AudioClip audioClip;

		public Player(int x, int y, KeyMap keyMap)
		{
			X = x * Game.map.TileSize + Game.map.TileSize / 2;
			Y = y * Game.map.TileSize + Game.map.TileSize / 2;
			BombsPlaced = 0;
			BombsAvailable = 1;
			BombRadius = 1;
			BombFuseTime = 4f;
			KeyMap = keyMap;
			SpinyBombs = false;
			MovSpeed = 100;
			sprite = new Sprite(32, 32);

			audioSource = new AudioSource();
			audioClip = new AudioClip("../../assets/move.ogg");

			isMoving = false;
			animSpeed = 8;

			Bombs = new List<Bomb>();
		}

		public void PlaceBomb()
		{
			Map map = Game.map;
			if (BombsAvailable > 0 && map.Tiles[Utils.GetPos(X / map.TileSize, Y / map.TileSize, map.Width)] == Map.TileType.None)
			{
				Bombs.Add(new Bomb(X / map.TileSize, Y / map.TileSize, BombFuseTime, Game.stateJohn == 20 ? true : false));

				map.Tiles[Utils.GetPos(X / map.TileSize, Y / map.TileSize, map.Width)] = Map.TileType.Bomb;

				BombsAvailable--;
				BombsPlaced++;
			}
		}

		private void Move(Direction direction)
		{
			//if (!audioSource.IsPlaying)
			//	audioSource.Play(audioClip);
			this.isMoving = true;
			Window window = Game.window;
			Map map = Game.map;
			
			if (this.X % 32 >= 15 && this.X % 32 <= 17)
			{
				if (direction == Direction.UP && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.Y -= (int)(MovSpeed * window.deltaTime);
					this.X = this.X - this.X % 32 + 16;
				}
				else if (direction == Direction.DOWN && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.Y += (int)(MovSpeed * window.deltaTime);
					this.X = this.X - this.X % 32 + 16;
				}
			}
			else if (this.X % 32 >= 5 && this.X % 32 <= 27)
			{
				if (direction == Direction.UP && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.Y -= (int)(MovSpeed * window.deltaTime);
					xMoveDiagonally();
				}
				else if (direction == Direction.DOWN && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.Y += (int)(MovSpeed * window.deltaTime);
					xMoveDiagonally();
				}
			}
			if (this.Y % 32 >= 15 && this.Y % 32 <= 17)
			{
				if (direction == Direction.RIGHT && map.Tiles[Utils.GetPos((this.X + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.X += (int)(MovSpeed * window.deltaTime);
					this.Y = this.Y - this.Y % 32 + 16;
				}
				else if (direction == Direction.LEFT && map.Tiles[Utils.GetPos((this.X - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.X -= (int)(MovSpeed * window.deltaTime);
					this.Y = this.Y - this.Y % 32 + 16;
				}
			}
			else if (this.Y % 32 >= 5 && this.Y % 32 <= 27)
			{
				if (direction == Direction.RIGHT && map.Tiles[Utils.GetPos((this.X + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.X += (int)(MovSpeed * window.deltaTime);
					yMoveDiagonally();
				}
				else if (direction == Direction.LEFT && map.Tiles[Utils.GetPos((this.X - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.X -= (int)(MovSpeed * window.deltaTime);
					yMoveDiagonally();
				}
			}
			if (map.PowerUps[Utils.GetPos(this.X / map.TileSize, this.Y / map.TileSize, map.Width)] != null)
			{
				map.PowerUps[Utils.GetPos(this.X / map.TileSize, this.Y / map.TileSize, map.Width)].PickUp(this);
				map.PowerUps[Utils.GetPos(this.X / map.TileSize, this.Y / map.TileSize, map.Width)] = null;
			}

			Game.map.CheckScroll(this);
		}

		#region movimento

		private void yMoveDiagonally()
		{
			if (this.Y % 32 >= 5 && this.Y % 32 < 15)
				if (this.Y + (int)(MovSpeed * Game.window.deltaTime) % 32 >= 15)
					this.Y = this.Y - this.Y % 32 + 16;
				else
					this.Y += (int)(MovSpeed * Game.window.deltaTime);
			else if (this.Y % 32 <= 27 && this.Y % 32 > 17)
				if (this.Y - (int)(MovSpeed * Game.window.deltaTime) % 32 <= 17)
					this.Y = this.Y - this.Y % 32 + 16;
				else
					this.Y -= (int)(MovSpeed * Game.window.deltaTime);
		}

		private void xMoveDiagonally()
		{
			if (this.X % 32 >= 5 && this.X % 32 < 15)
				if (this.X + (int)(MovSpeed * Game.window.deltaTime) % 32 >= 15)
					this.X = this.X - this.X % 32 + 16;
				else
					this.X += (int)(MovSpeed * Game.window.deltaTime);
			else if (this.X % 32 <= 27 && this.X % 32 > 17)
				if (this.X - (int)(MovSpeed * Game.window.deltaTime) % 32 <= 17)
					this.X = this.X - this.X % 32 + 16;
				else
					this.X -= (int)(MovSpeed * Game.window.deltaTime);
		}

#endregion

		public void Draw()
		{
			if (this.isMoving)
			{
				animOffest += animSpeed * Game.window.deltaTime;
				if (animOffest > 3)
					animOffest = 0;
			}
			this.sprite.position.X = this.X - 16 + Game.map.Scroll;
			this.sprite.position.Y = this.Y - 16;
			this.sprite.DrawTexture(Game.PlayerTexture, (int)animOffest * 32, 0, 32, 32);
		}

		public void CheckMovement()
		{
			Window window = Game.window;
			Map map = Game.map;
			if (window.GetKey(this.KeyMap.Left) && this.X > 0)
				this.Move(Player.Direction.LEFT);
			else if (window.GetKey(this.KeyMap.Down) && this.Y < map.Height * map.TileSize)
				this.Move(Player.Direction.DOWN);
			else if (window.GetKey(this.KeyMap.Up) && this.Y > 0)
				this.Move(Player.Direction.UP);
			else if (window.GetKey(this.KeyMap.Right) && this.X < map.Width*map.TileSize)
				this.Move(Player.Direction.RIGHT);
			else
				this.isMoving = false;
			if (window.GetKey(this.KeyMap.PlaceBomb))
				this.PlaceBomb();
		}

		public void PrintInfo()
		{
			Console.SetCursorPosition(0, 2);
			Console.WriteLine("Bombs: {0}", this.BombsAvailable + this.BombsPlaced);
			Console.WriteLine("Radius: {0}", this.BombRadius);
			Console.WriteLine("Spiny Bombs: {0}    ", this.SpinyBombs);
			Console.WriteLine("Movement Speed: {0}   ", this.MovSpeed);
			Console.WriteLine("Fuse time: {0}  ", this.BombFuseTime);
		}
	}
}
