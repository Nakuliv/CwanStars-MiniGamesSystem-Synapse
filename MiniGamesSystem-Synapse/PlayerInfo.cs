using System.Collections.Generic;
using MiniGamesSystem.Pets;
using UnityEngine;

namespace MiniGamesSystem
{
	public class PlayerInfo
	{
		public string nick;
		public string custompetName;
		public RoleType custompetClass;
		public ItemType custompetItem;
		public Vector3 custompetSize;
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
