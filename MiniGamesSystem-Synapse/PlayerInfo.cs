using System.Collections.Generic;
using MiniGamesSystem.Pets;
using Synapse.Config;
using UnityEngine;

namespace MiniGamesSystem
{
	public class PlayerInfo
	{
		public string nick;
		public string custompetName;
		public RoleType custompetClass;
		public ItemType custompetItem;
		public SerializedVector3 custompetSize;
		public int Coins;
		public List<string> ListaCzapek = new List<string>();
		public List<PetType> ListaPetow = new List<PetType>();

		public PlayerInfo(string nick)
		{
			this.nick = nick;
			Coins = 0;
			custompetClass = RoleType.ClassD;
			custompetName = "PrzykładowaNazwa";
			custompetSize = new SerializedVector3(0.5f, 0.5f, 0.5f);
			custompetItem = ItemType.GunCOM18;

		}
	}
}
