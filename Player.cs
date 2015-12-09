using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw;
using windowApp;

namespace Bomberman
{
	class Player
	{
		public int BombsPlaced;
		public int BombsAvailable;
		public Bomb[] Bombs;
		public int X { get; private set; }
		public int Y { get; private set; }
		public bool justMoved { get; private set; }

		public Player(int x, int y, Map map, string filepath)
		{
			X = x;
			Y = y;
			BombsPlaced = 0;
			BombsAvailable = 5;

			string file = System.IO.File.ReadAllText(filepath);
			file = file.Replace("\r\n", ",");
			bool playerStartFound = false;
			for (int i = 0; i < file.Split(',').Length - 1; i++)
			{
				if (int.Parse(file.Split(',')[i]) == 1)
				{
					X = i % map.Width;
					Y = i / map.Width;
					playerStartFound = true;
				}
			}
			if (!playerStartFound)
			{
				X = 1;
				Y = 1;
			}

			Bombs = new Bomb[BombsAvailable];
			for (int i = 0; i < Bombs.Length; i++)
			{
				Bombs[i] = new Bomb();
			}
		}

		public void PlaceBomb(Window window, Map map)
		{
			Console.WriteLine(BombsAvailable);
			Console.WriteLine(BombsPlaced);
			if (BombsAvailable > 0 && map.Tiles[Utils.GetPos(X, Y, map.Width)] == Tile.TileType.None)
			{
				Bombs[BombsPlaced].X = X;
				Bombs[BombsPlaced].Y = Y;
				Bombs[BombsPlaced].TimeToExplode = 3;

				map.Tiles[Utils.GetPos(X, Y, map.Width)] = Tile.TileType.Bomb;

				Utils.DrawRectFilled(window, (X * map.TileSize) + 8, (Y * map.TileSize) + 8, 16, 16, 128, 128, 0);
				
				BombsAvailable--;
				BombsPlaced++;
			}
			Console.WriteLine(BombsAvailable);
			Console.WriteLine(BombsPlaced);
			Console.WriteLine();
		}

		public void Move(int x, int y, Map map)
		{
			if (!justMoved && map.Tiles[Utils.GetPos(x, y, map.Width)] == Tile.TileType.None)
			{
				X = x;
				Y = y;
				justMoved = true;
			}
		}

		public void CanMove()
		{
			justMoved = false;
		}

		public void PrintPlayer(Window window, Map map)
		{
			Utils.DrawRectFilled(window, (X * map.TileSize) + 4, (Y * map.TileSize) + 4, 24, 24, 0, 150, 0);
		}
	}
}
