using System;

using Server;
using Server.Duels;
using Server.Mobiles;

namespace Server.Items
{
	public class DuelGate : Moongate
	{
		[Constructable]
		public DuelGate()
			: base()
		{
			Hue = 1175;
			Name = "Arena Gate";
			Dispellable = false;
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			if (m is PlayerMobile)
			{
				if (!Utility.InRange(m.Location, this.Location, 1) && Utility.InRange(oldLocation, this.Location, 1))
					m.CloseGump(typeof(DuelGateGump));
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			UseGate(from);
		}

		public override bool OnMoveOver(Mobile m)
		{
			UseGate(m);
			return true;
		}

		public override void UseGate(Mobile m)
		{
			if (Factions.Sigil.ExistsOn(m))
			{
				m.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
			}
			else if (m.Spell != null)
			{
				m.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
			}
			else
			{
				m.Send(new Network.PlaySound(0x20E, m.Location));
				m.CloseGump(typeof(DuelGateGump));
				m.SendGump(new DuelGateGump(m));
			}
		}

		public DuelGate(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); //version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}