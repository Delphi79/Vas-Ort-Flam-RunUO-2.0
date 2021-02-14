using System;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Items;
using Server.Gumps;
using Server.Prompts;
using Server.Network;
using Server.ContextMenus;

namespace Server.Mobiles
{
	public class PoleDancer : BaseVendor
	{
		private Direction m_Facing;

		private bool m_IsBusy;

		[CommandProperty(AccessLevel.GameMaster)]
		public Direction Facing { get { return m_Facing; } set { m_Facing = value; } }

		public bool IsBusy { get { return m_IsBusy; } set { m_IsBusy = value; } }

		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBPoleDancer());
		}

		public override void InitOutfit()
		{
			Backpack b = new Backpack();
			b.Movable = false;
			b.LootType = LootType.Blessed;
			AddItem(b);
		}

		[Constructable]
		public PoleDancer()
			: base("the pole dancer")
		{
			HairItemID = 8265;
			HairHue    = 1161;

			Female = true;
			BodyValue = 401;

			switch (Utility.Random(10))
			{
				default:
				case 0: Name = "Cin-a-mon";	  break;
				case 1: Name = "Candy";		  break;
				case 2: Name = "Sugar";		  break;
				case 3: Name = "Precious";    break;
				case 4: Name = "Marshmallow"; break;
				case 5: Name = "Daisy";		  break;
				case 6: Name = "Caramel";	  break;
				case 7: Name = "Cherry";	  break;
				case 8: Name = "Daisy";		  break;
				case 9: Name = "Raisen";	  break;
			}

			Frozen = true;
			Direction = Direction.South;

			Item body = new FemaleStuddedChest();
			body.Hue = 1175;
			body.LootType = LootType.Blessed;
			AddItem(body);

			Item sand = new Sandals();
			sand.Hue = 1175;
			sand.LootType = LootType.Blessed;
			AddItem(sand);

			Item glov = new BoneGloves();
			glov.Hue = 1175;
			glov.LootType = LootType.Blessed;
			glov.Name = "gloves";
			AddItem(glov);
		}

		public PoleDancer(Serial serial)
			: base(serial)
		{
		}

		public void Strip()
		{
			m_IsBusy = true;

			Item toStrip = FindItemOnLayer(Layer.InnerTorso);

			if (toStrip != null && toStrip is FemaleStuddedChest)
			{
				AddToBackpack(toStrip);
				Animate(34, 7, 1, true, false, 0);
				Say("*strips*");

				Timer.DelayCall(TimeSpan.FromSeconds(5.0), new TimerStateCallback(delegate(object state)
				{
					AddItem(toStrip);
					Animate(34, 7, 1, true, false, 0);
					m_IsBusy = false;
				}), null );
			}
		}

		public void Dance()
		{
			m_IsBusy = true;

			Animate(23, 7, 1, true, false, 0);
			Say("*dances*");

			m_IsBusy = false;
		}

		public void LapDance(Mobile from)
		{
			m_IsBusy = true;

			Point3D oldLocation = Location;

			Location = from.Location;
			Animate(23, 7, 1, true, false, 10);
			Say("*lap dance*");

			Timer.DelayCall(TimeSpan.FromSeconds(2.5), new TimerStateCallback(delegate(object state)
			{
				Location = oldLocation;
				m_IsBusy = false;
			}), null);
		}

		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			if (!m_IsBusy)
			{
				if (dropped is Gold && dropped.Amount >= 100)
				{
					dropped.Delete();

					switch (Utility.Random(3))
					{
						default:
						case 0: Strip(); break;
						case 1: Dance(); break;
						case 2: LapDance(from); break;
					}

					return true;
				}
				else
				{
					Say("I will only work for at least 100 gold.");
					return false;
				}

			}
			else
			{
				Say(String.Format("Can't you see I'm busy, {0}?", from.Name));
				return false;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write((int)m_Facing);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Facing = (Direction)reader.ReadInt();

			Frozen = true;
		}
	}
}