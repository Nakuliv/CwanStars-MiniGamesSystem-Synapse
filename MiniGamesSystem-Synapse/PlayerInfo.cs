using System.Collections.Generic;
using MiniGamesSystem.Pets;

namespace MiniGamesSystem
{
	public class PlayerInfo
	{
		public string nick;
		public int Coins;
		public List<string> ListaCzapek = new List<string>();
		public List<PetType> ListaPetow = new List<PetType>();

		public PlayerInfo(string nick)
		{
			this.nick = nick;
			Coins = 0;
		}
	}
}
