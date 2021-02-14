using System;

using Server;

namespace Server.Items
{
	public class PayRegStone : Item
	{
		private int m_RegAmount;
		private int m_Cost;

		[CommandProperty( AccessLevel.GameMaster )]
		public int RegAmount { get { return m_RegAmount; } set { m_RegAmount = value; Name = GetName(); } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int Cost { get { return m_Cost; } set { m_Cost = value; Name = GetName(); } }

		[Constructable]
		public PayRegStone()
			: base(0xED4)
		{
			m_RegAmount = 100;
			m_Cost = 2800;

			Name = GetName();
			Hue = 1175;
			Movable = false;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.InRange(this.GetWorldLocation(), 2))
			{
				Container pack = from.Backpack;

				if (pack != null)
				{
					if (pack.ConsumeTotal(typeof(Gold), m_Cost))
					{
						from.AddToBackpack(new BagOfReagents(m_RegAmount));
						from.SendMessage("You have bought a bag of reagents.");
					}
					else
						from.SendMessage("You do not have enough money in your backpack to use this!");
				}
			}
			else
				from.SendMessage("You are too far away to use this.");
		}

		public PayRegStone(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); //version

			writer.Write(m_RegAmount);
			writer.Write(m_Cost);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_RegAmount = reader.ReadInt();
			m_Cost = reader.ReadInt();
		}

		public string GetName()
		{
			return String.Format("a reagent stone [{0}gp for {1} of all]", m_Cost, m_RegAmount);
		}
	}
}