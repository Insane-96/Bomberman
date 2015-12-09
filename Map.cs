using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using windowApp;

namespace Bomberman
{
	class Map
	{

		public int Width { get; private set; }
		public int Height { get; private set; }
		public int TileSize { get; private set; }
		public Tile.TileType[] Tiles { get; private set; }

		public Map(int width, int height, int tileSize, string filepath)
		{
			Width = width;
			Height = height;
			TileSize = tileSize;

			string file = System.IO.File.ReadAllText(filepath);
			file = file.Replace("\r\n", ",");

			Tiles = new Tile.TileType[file.Split(',').Length];
			for (int i = 0; i < file.Split(',').Length - 1; i++)
			{
				if ((Tile.TileType) int.Parse(file.Split(',')[i]) == 0)
					Tiles[i] = Tile.TileType.Wall;

			}
		}
	}
}
