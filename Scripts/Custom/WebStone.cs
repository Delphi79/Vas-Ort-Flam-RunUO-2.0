using System;

using Server;

namespace Server.Items
{
	public class WebStone : Item
	{
		private string m_URL;

		[CommandProperty(AccessLevel.GameMaster)]
		public string URL { get { return m_URL; } set { m_URL = value; } }

		[Constructable]
		public WebStone() : base(0xED4)
		{
			Movable = false;
			Name = "a website stone";
			Hue = 1154;

			m_URL = "http://www.projectxuo.net";
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.LaunchBrowser(m_URL);
		}

		public WebStone(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0); //version
			writer.Write(m_URL);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_URL = reader.ReadString();
		}
	}
}