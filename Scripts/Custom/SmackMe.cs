using System;

using Server;
using Server.Items;

namespace Server.TSystem
{
	public class SmackMe : Mobile
	{
		[Constructable]
		public SmackMe()
		{
			InitStats(100, 100, 25);

			Name = "a training dummy";
			Body = 0x23E;
			Hue = 1175;
			Kills = 100;
		}

		public override bool CanBeDamaged() { return false; }

		public override bool CanBeHarmful(Mobile target, bool message)
		{
			return false;
		}

		public SmackMe(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}