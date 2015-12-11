using System;
using System.IO;
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

		public Map(int tileSize, string filepath)
		{
			TileSize = tileSize;

			string file = File.ReadAllText(filepath);
			string[] tmp = file.Split('\r');
			this.Height = tmp.Length - 1;
			this.Width = tmp[0].Split(',').Length;
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
