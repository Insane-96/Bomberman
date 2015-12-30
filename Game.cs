using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Aiv.Draw.OpenGL;

namespace Bomberman
{
	static class Game
	{
		public static Window window;
		public static Map map;
		public static Sprite BombSprite = new Sprite("../../assets/bomb.png");
		public static Sprite WallSprite = new Sprite("../../assets/wall.png");
		public static Sprite DestrWallSprite = new Sprite("../../assets/destrWall.png");
	}
}
