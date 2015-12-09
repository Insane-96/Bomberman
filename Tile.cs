using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
	class Tile
	{
		public enum TileType
		{
			None,
			Wall,
			Bomb,
		}

		public TileType type { get; private set; }
	}
}
