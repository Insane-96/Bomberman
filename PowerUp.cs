using Aiv.Fast2D;

namespace Bomberman
{
	class PowerUp
	{
		public enum PowerUps
		{
			IncreaseMovementSpeed,
			IncreaseBombRadius,
			IncreaseMaxBombsPlaceable,
			DecreaseBombFuse,
			UnlockSpinyBomb,
		}

		//public Dictionary<PowerUps, string> PowerUpDictionary = new Dictionary<PowerUps, string>()
		//{
		//	{PowerUps.SpeedUp, "SpeedUp" },
		//	{PowerUps.RadiusUp, "RadiusUp" },
		//	{PowerUps.BombsUp, "BombsUp" },
		//	{PowerUps.BombFuseUp, "BombFuseUp" },
		//	{PowerUps.SpinyBomb, "SpinyBomb" }
		//};

		public static string[] PowerUpsList = new string[]
		{
			"SpeedUp", "RadiusUp", "BombsUp", "BombFuseUp", "SpinyBomb"
		};

		public string Name { get; private set; }
		public int PlayerSpeedIncrease { get; private set; }
		public int BombRadiusIncrease { get; private set; }
		public int BombAvailableIncrease { get; private set; }
		public float BombFuseTimeDecrease { get; private set; }
		public bool UnlockSpinyBomb { get; private set; }
		public bool Invinvicility { get; private set; }
		public bool Ghost { get; private set; }
		public int TimeLasting { get; private set; }
		public Sprite sprite;

		public PowerUp(string Name, int PlayerSpeedIncrease = 0, int BombRadiusIncrease = 0, int BombAvailableIncrease = 0, float BombFuseTimeDecrease = 0f, bool UnlockSpinyBomb = false, bool Invinvicility = false, bool Ghost = false, int TimeLasting = -1)
		{
			this.Name = Name;
			switch (Name)
			{
				case "SpeedUp":
					this.PlayerSpeedIncrease = 10;
					break;
				case "RadiusUp":
					this.BombRadiusIncrease = 1;
					break;
				case "BombsUp":
					this.BombAvailableIncrease = 1;
					break;
				case "BombFuseUp":
					this.BombFuseTimeDecrease = 0.5f;
					break;
				case "SpinyBomb":
					this.UnlockSpinyBomb = true;
					break;
				default:
					this.PlayerSpeedIncrease = PlayerSpeedIncrease;
					this.BombRadiusIncrease = BombRadiusIncrease;
					this.BombAvailableIncrease = BombAvailableIncrease;
					this.BombFuseTimeDecrease = BombFuseTimeDecrease;
					this.UnlockSpinyBomb = UnlockSpinyBomb;
					this.Invinvicility = Invinvicility;
					this.Ghost = Ghost;
					this.TimeLasting = TimeLasting;
					break;
			}
			this.sprite = new Sprite(32, 32);
			sprite.DrawTexture(Game.PowerUpTexture);

		}

		public void PickUp(Player player)
		{
			if (player.BombFuseTime > 1.5f)
				player.BombFuseTime -= this.BombFuseTimeDecrease;
			if (player.BombRadius < 8)
				player.BombRadius += this.BombRadiusIncrease;
			if (player.BombsAvailable < 8)
				player.BombsAvailable += this.BombAvailableIncrease;
			if (!player.SpinyBombs)
				player.SpinyBombs = this.UnlockSpinyBomb;
			if (player.MovSpeed < 200)
				player.MovSpeed += this.PlayerSpeedIncrease;


		}
	}
}
