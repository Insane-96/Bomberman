using System;
using System.Runtime.CompilerServices;
using Aiv.Draw.OpenGL;

namespace Bomberman
{
	class Enemy
	{
		public enum Direction
		{
			UP,
			RIGHT,
			DOWN,
			LEFT
		}
		public int Health { get; private set; }
		public int MaxHealth { get; private set; }
		public int MovSpeed { get; private set; }
		public Direction DirectionMoving;
		public int X;
		public int Y;
		private Sprite sprite;

		public Enemy(int MaxHealth, int MovSpeed, int X, int Y, int Health = -1, Sprite sprite = null)
		{
			this.MaxHealth = MaxHealth;
			this.MovSpeed = MovSpeed;
			if (Health <= 0)
				this.Health = MaxHealth;
			else
				this.Health = Health;

			if (sprite == null)
				this.sprite = Game.EnemySprite;
			else
				this.sprite = sprite;

			this.X = X * Game.map.TileSize + Game.map.TileSize / 2;
			this.Y = Y * Game.map.TileSize + Game.map.TileSize / 2;

			

		}

		public void RandomDirection()
		{
			this.DirectionMoving = (Direction)Utils.Randomize(0, 4);
		}

		public void Move()
		{
			Window window = Game.window;
			Map map = Game.map;
			if (this.X % 32 >= 14 && this.X % 32 <= 18)
			{
				if (this.DirectionMoving == Direction.UP && map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.Y -= (int)(MovSpeed * window.deltaTime);
					this.X = this.X - this.X % 32 + 16;
				}
				else if (this.DirectionMoving == Direction.DOWN &&  map.Tiles[Utils.GetPos(this.X / map.TileSize, (this.Y + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.Y += (int)(MovSpeed * window.deltaTime);
					this.X = this.X - this.X % 32 + 16;
				}
			}
			if (this.Y % 32 >= 14 && this.Y % 32 <= 18)
			{
				if (this.DirectionMoving == Direction.RIGHT && map.Tiles[Utils.GetPos((this.X + (int)(MovSpeed * window.deltaTime) + 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.X += (int)(MovSpeed * window.deltaTime);
					this.Y = this.Y - this.Y % 32 + 16;
				}
				else if (this.DirectionMoving == Direction.LEFT && map.Tiles[Utils.GetPos((this.X - (int)(MovSpeed * window.deltaTime) - 16) / map.TileSize, this.Y / map.TileSize, map.Width)] == Map.TileType.None)
				{
					this.X -= (int)(MovSpeed * window.deltaTime);
					this.Y = this.Y - this.Y % 32 + 16;
				}
			}
		}
		//private void yMoveDiagonally()
		//{
		//	if (this.Y % 32 >= 5 && this.Y % 32 < 15)
		//		if (this.Y + (int)(MovSpeed * Game.window.deltaTime) % 32 >= 15)
		//			this.Y = this.Y - this.Y % 32 + 16;
		//		else
		//			this.Y += (int)(MovSpeed * Game.window.deltaTime);
		//	else if (this.Y % 32 <= 27 && this.Y % 32 > 17)
		//		if (this.Y - (int)(MovSpeed * Game.window.deltaTime) % 32 <= 17)
		//			this.Y = this.Y - this.Y % 32 + 16;
		//		else
		//			this.Y -= (int)(MovSpeed * Game.window.deltaTime);
		//}

		//private void xMoveDiagonally()
		//{
		//	if (this.X % 32 >= 5 && this.X % 32 < 15)
		//		if (this.X + (int)(MovSpeed * Game.window.deltaTime) % 32 >= 15)
		//			this.X = this.X - this.X % 32 + 16;
		//		else
		//			this.X += (int)(MovSpeed * Game.window.deltaTime);
		//	else if (this.X % 32 <= 27 && this.X % 32 > 17)
		//		if (this.X - (int)(MovSpeed * Game.window.deltaTime) % 32 <= 17)
		//			this.X = this.X - this.X % 32 + 16;
		//		else
		//			this.X -= (int)(MovSpeed * Game.window.deltaTime);
		//}

		public void Draw()
		{
			Utils.DrawSprite(Game.window, this.sprite, this.X + Game.map.Scroll - 16, this.Y - 16, 0, 0, 32, 32);
		}
	}
}
