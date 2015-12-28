using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
	static class Tile
	{
		public enum TileType
		{
			None,
			Wall,
			DestrWall,
			Bomb,
		}

		public static TileType type { get; private set; }
	}
}
