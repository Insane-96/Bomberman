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
		public int BombRadius;
		public List<Bomb> Bombs;
		public int X { get; private set; }
		public int Y { get; private set; }
		public bool JustMoved { get; private set; }
		public KeyMap KeyMap;

		public Player(int x, int y, Map map, string filepath, KeyMap keyMap)
		{
			X = x;
			Y = y;
			BombsPlaced = 0;
			BombsAvailable = 2;
			BombRadius = 1;
			KeyMap = keyMap;
			X = 1;
			Y = 1;

			Bombs = new List<Bomb>();
		}

		public void PlaceBomb(Window window, Map map)
		{
			if (BombsAvailable > 0 && map.Tiles[Utils.GetPos(X, Y, map.Width)] == Tile.TileType.None)
			{
				Bombs.Add(new Bomb(X, Y, 3));

				map.Tiles[Utils.GetPos(X, Y, map.Width)] = Tile.TileType.Bomb;

				Utils.DrawRectFilled(window, (X * map.TileSize) + 8, (Y * map.TileSize) + 8, 16, 16, 128, 128, 0);
				
				BombsAvailable--;
				BombsPlaced++;
			}
		}

		public void Move(int x, int y, Map map)
		{
			if (!JustMoved && map.Tiles[Utils.GetPos(x, y, map.Width)] == Tile.TileType.None)
			{
				X = x;
				Y = y;
				JustMoved = true;
			}
		}

		public void CanMove()
		{
			JustMoved = false;
		}

		public void PrintPlayer(Window window, Map map)
		{
			Utils.DrawRectFilled(window, (X * map.TileSize) + 4, (Y * map.TileSize) + 4, 24, 24, 0, 150, 0);
		}
	}
}
