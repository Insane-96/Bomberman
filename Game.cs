using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Aiv.Draw.OpenGL;

namespace Bomberman
{
	class Game
	{
		public static Window window;
		public static Map map;
		public static Sprite BombSprite = new Sprite("../../assets/bomb.png");
		public static Sprite WallSprite = new Sprite("../../assets/wall.png");
		public static Sprite DestrWallSprite = new Sprite("../../assets/destrWall.png");
		public static Sprite EnemySprite = new Sprite("../../assets/enemy.png");
		public static Player player;

		public static Enemy[] EnemiesList;

		static Game()
		{
			init();
		}

		private static void init()
		{
			//init window
			window = new Window(976, 544, "Bomberman", PixelFormat.RGB);
			window.SetIcon("../../assets/bomb.ico", true);
			window.CursorVisible = false;

			//init map
			map = new Map(32);

			//init player's key map and player
			KeyMap player1KeyMap = new KeyMap(KeyCode.Left, KeyCode.Right, KeyCode.Up, KeyCode.Down, KeyCode.Space);
			player = new Player(1, 1, player1KeyMap, "../../assets/player.png");

			//init enemies
			EnemiesList = new Enemy[5];
			for (int i = 0; i < EnemiesList.Length; i++)
			{
				int rX, rY;
				do
				{
					rX = Utils.Randomize(7, map.Width);
					rY = Utils.Randomize(7, map.Height);
				} while (map.Tiles[Utils.GetPos(rX, rY, map.Width)] != Map.TileType.None);
				EnemiesList[i] = new Enemy(1, Utils.Randomize(70, 81), rX, rY);
			}
		}

		public static void Update()
		{
			Console.SetCursorPosition(0, 0);
			Console.WriteLine("FPS: {0}            ", 1 / Game.window.deltaTime);

			//Clear window
			Utils.Clear(window, 31, 139, 0);

			player.checkMovement();

			EnemiesAI();

			map.Draw();
			player.Draw();
			DrawEnemies();

			player.PrintInfo();

			//Fuse bombs
			List<Bomb> toRemove = new List<Bomb>();
			foreach (Bomb bomb in player.Bombs)
			{
				if (bomb.Fuse(player))
					toRemove.Add(bomb);
			}
			foreach (var bombToRemove in toRemove)
			{
				player.Bombs.Remove(bombToRemove);
			}
			toRemove.Clear();

			//Draw
			Game.window.Blit();

			//Esc + Return (Enter) closes the game
			if (Game.window.GetKey(KeyCode.Esc) && Game.window.GetKey(KeyCode.Return))
				Game.window.opened = false;
		}

		public static void DrawEnemies()
		{
			foreach (var enemy in EnemiesList)
			{
				enemy.Draw();
			}
		}

		public static void EnemiesAI()
		{
			//double[] directions = new double[Enum.GetValues(typeof(Enemy.Direction)).Length];
			foreach (var enemy in EnemiesList)
			{
				//if (map.Tiles[Utils.GetPos(enemy.X / map.TileSize, (enemy.Y - (int)(enemy.MovSpeed * window.deltaTime) - 15) / map.TileSize, map.Width)] == Map.TileType.None)
				//{
				//	directions[(int)Enemy.Direction.UP] = -Math.Sqrt(Math.Pow(player.X - enemy.X, 2) + Math.Pow(player.Y - enemy.Y - (int)(enemy.MovSpeed * window.deltaTime), 2));
				//	//Utils.DrawRectFilled(window, enemy.X - 16, enemy.Y - 16 - (int)(enemy.MovSpeed * window.deltaTime), 32, 32, 128, 0, 218);
				//}
				//if (map.Tiles[Utils.GetPos((enemy.X + (int)(enemy.MovSpeed * window.deltaTime) + 15) / map.TileSize, enemy.Y / map.TileSize, map.Width)] == Map.TileType.None)
				//{
				//	directions[(int)Enemy.Direction.RIGHT] = -Math.Sqrt(Math.Pow(player.X - enemy.X + (int)(enemy.MovSpeed * window.deltaTime), 2) + Math.Pow(player.Y - enemy.Y, 2));
				//	//Utils.DrawRectFilled(window, enemy.X - 16 + (int)(enemy.MovSpeed * window.deltaTime), enemy.Y - 16, 32, 32, 128, 0, 218);
				//}
				//if (map.Tiles[Utils.GetPos(enemy.X / map.TileSize, (enemy.Y + (int)(enemy.MovSpeed * window.deltaTime + 15)) / map.TileSize, map.Width)] == Map.TileType.None)
				//{
				//	directions[(int)Enemy.Direction.DOWN] = -Math.Sqrt(Math.Pow(player.X - enemy.X, 2) + Math.Pow(player.Y - enemy.Y + (int)(enemy.MovSpeed * window.deltaTime), 2));
				//	//Utils.DrawRectFilled(window, enemy.X - 16, enemy.Y - 16 + (int)(enemy.MovSpeed * window.deltaTime), 32, 32, 128, 0, 218);
				//}
				//if (map.Tiles[Utils.GetPos((enemy.X - (int)(enemy.MovSpeed * window.deltaTime) - 15) / map.TileSize, enemy.Y / map.TileSize, map.Width)] == Map.TileType.None)
				//{
				//	directions[(int)Enemy.Direction.LEFT] = -Math.Sqrt(Math.Pow(player.X - enemy.X - (int)(enemy.MovSpeed * window.deltaTime), 2) + Math.Pow(player.Y - enemy.Y, 2));
				//	//Utils.DrawRectFilled(window, enemy.X - 16 + (int)(enemy.MovSpeed * window.deltaTime), enemy.Y - 16, 32, 32, 128, 0, 218);
				//}
				//int min = 0;
				//for (int i = 1; i < directions.Length; i++)
				//{
				//	if ((directions[min] > directions[i] || directions[min] == 0) && directions[i] != 0)
				//		min = i;
				//}
				if (Utils.Randomize(0, 100) == 0 && enemy.X % 32 >= 14 && enemy.X % 32 <= 18 && enemy.Y % 32 >= 14 && enemy.Y % 32 <= 18)
				{
					enemy.DirectionMoving = (Enemy.Direction)Utils.Randomize(0, 4);
				}
				switch (enemy.DirectionMoving)
				{
					case Enemy.Direction.UP:
						if (map.Tiles[Utils.GetPos(enemy.X / map.TileSize, (enemy.Y - (int)(enemy.MovSpeed * window.deltaTime) - 16) / map.TileSize, map.Width)] != Map.TileType.None)
						{
							if (map.Tiles[Utils.GetPos(enemy.X / map.TileSize, (enemy.Y + (int)(enemy.MovSpeed * window.deltaTime) + 16) / map.TileSize, map.Width)] != Map.TileType.None)
								enemy.RandomDirection();
							else
							{
								if (Utils.Randomize(0, 5) == 0)
									enemy.RandomDirection();
								else
									enemy.DirectionMoving = Enemy.Direction.DOWN;
							}
						}
						break;
					case Enemy.Direction.RIGHT:
						if (map.Tiles[Utils.GetPos((enemy.X + (int)(enemy.MovSpeed * window.deltaTime) + 16) / map.TileSize, enemy.Y / map.TileSize, map.Width)] != Map.TileType.None)
							if (map.Tiles[Utils.GetPos((enemy.X - (int)(enemy.MovSpeed * window.deltaTime) - 16) / map.TileSize, enemy.Y / map.TileSize, map.Width)] != Map.TileType.None)
								enemy.RandomDirection();
							else
							{
								if (Utils.Randomize(0, 5) == 0)
									enemy.RandomDirection();
								else
									enemy.DirectionMoving = Enemy.Direction.LEFT;
							}
						break;
					case Enemy.Direction.DOWN:
						if (map.Tiles[Utils.GetPos(enemy.X / map.TileSize, (enemy.Y + (int)(enemy.MovSpeed * window.deltaTime) + 16) / map.TileSize, map.Width)] != Map.TileType.None)
							if (map.Tiles[Utils.GetPos(enemy.X / map.TileSize, (enemy.Y - (int)(enemy.MovSpeed * window.deltaTime) - 16) / map.TileSize, map.Width)] != Map.TileType.None)
								enemy.RandomDirection();
							else
							{
								if (Utils.Randomize(0, 5) == 0)
									enemy.RandomDirection();
								else
									enemy.DirectionMoving = Enemy.Direction.UP;
							}
						break;
					case Enemy.Direction.LEFT:
						if (map.Tiles[Utils.GetPos((enemy.X - (int)(enemy.MovSpeed * window.deltaTime) - 16) / map.TileSize, enemy.Y / map.TileSize, map.Width)] != Map.TileType.None)
							if (map.Tiles[Utils.GetPos((enemy.X + (int)(enemy.MovSpeed * window.deltaTime) + 16) / map.TileSize, enemy.Y / map.TileSize, map.Width)] != Map.TileType.None)
								enemy.RandomDirection();
							else
							{
								if (Utils.Randomize(0, 5) == 0)
									enemy.RandomDirection();
								else
									enemy.DirectionMoving = Enemy.Direction.RIGHT;
							}
						break;
				}
				enemy.Move();
			}
		}
	}
}
