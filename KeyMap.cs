using Aiv.Fast2D;

namespace Bomberman
{
	class KeyMap
	{
		public KeyCode Left;
		public KeyCode Right;
		public KeyCode Up;
		public KeyCode Down;
		public KeyCode PlaceBomb;

		public KeyMap(KeyCode left, KeyCode right, KeyCode up, KeyCode down, KeyCode placeBomb)
		{
			Left = left;
			Right = right;
			Up = up;
			Down = down;
			PlaceBomb = placeBomb;
		}
	}
}
