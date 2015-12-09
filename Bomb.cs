using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
	class Bomb
	{
		public int X;
		public int Y;
		public float TimeToExplode;

		public Bomb()
		{
			TimeToExplode = -1f;
			X = -1;
			Y = -1;
		}
	}
}
