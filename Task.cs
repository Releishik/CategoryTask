using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
	public class InteractableMapObject
	{
		public InteractableMapObject()
		{
			MakeInterfacesMethods(this);
		}

		public void Interact(Player player)
		{
			if (this is IHaveArmy iAmIsArmy)
			{
				iAmIsArmy.Fight(player);
			}
			if (!player.Dead)
			{
				if (this is IHaveProduction iAmIsProduction) iAmIsProduction.ChangeOwner(player);
				if (this is IHaveTreasure iAmIsTreasure) iAmIsTreasure.GiveTreasure(player);
			}
		}
		
		public static void MakeInterfacesMethods(InteractableMapObject child)
		{
			if (child is IHaveArmy armyChild)
				armyChild.Fight = new Action<Player>((Player p) => {
					if (!p.CanBeat(armyChild.Army))
						p.Die();
				});

			if (child is IHaveProduction prodChild)
				prodChild.ChangeOwner = new Action<Player>((Player p) => {
					prodChild.Owner = p.Id;
				});

			if (child is IHaveTreasure tresChild)
				tresChild.GiveTreasure = new Action<Player>((Player p) => {
					p.Consume(tresChild.Treasure);
				});
		}
	}

	public interface IHaveProduction
	{
		int Owner { get; set; }
		Action<Player> ChangeOwner { get; set; }
	}

	public interface IHaveTreasure
	{
		Treasure Treasure { get; set; }
		Action<Player> GiveTreasure { get; set; }
	}

	public interface IHaveArmy
	{
		Army Army { get; set; }
		Action<Player> Fight { get; set; }
	}

	public class Dwelling : InteractableMapObject, IHaveProduction
	{
		public int Owner { get; set; }
		Action<Player> IHaveProduction.ChangeOwner { get; set; }
	}

	public class Mine : InteractableMapObject, IHaveProduction, IHaveArmy, IHaveTreasure
	{
		public int Owner { get; set; }
		public Action<Player> ChangeOwner { get; set; }
		public Army Army { get; set; }
		public Action<Player> Fight { get; set; }
		public Treasure Treasure { get; set; }
		public Action<Player> GiveTreasure { get; set; }
	}

	public class Creeps : InteractableMapObject, IHaveArmy, IHaveTreasure
	{
		public Army Army { get; set; }
		public Action<Player> Fight { get; set; }
		public Treasure Treasure { get; set; }
		public Action<Player> GiveTreasure { get; set; }
	}

	public class Wolves : InteractableMapObject, IHaveArmy
    {
		public Army Army { get; set; }
		public Action<Player> Fight { get; set; }
	}

    public class ResourcePile : InteractableMapObject, IHaveTreasure
    {
		public Treasure Treasure { get; set; }
		public Action<Player> GiveTreasure { get; set; }
	}

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
			if (mapObject is InteractableMapObject mObj) mObj.Interact(player);
        }
    }
}
