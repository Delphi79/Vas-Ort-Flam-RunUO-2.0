using System;

using Server;
using Server.Duels;
using Server.Mobiles;

namespace Server.Items
{
	public class MagicalTeleporter : Item
	{
		[Constructable]
		public MagicalTeleporter()
			: base( 0x1822 )
		{
			Hue = 1157;
			Name = "a magical teleporter";
			Movable = false;
		}

		public override bool OnMoveOver( Mobile from )
		{
			DuelInfo info = DuelCore.GetInfoNoCreate( from );

			if ( info != null && info.Last != Point3D.Zero )
			{
				from.MoveToWorld( info.Last, Map.Felucca );
				info.Last = Point3D.Zero;
			}
			else
				from.SendMessage( "This does not work for you." );

			return false;
		}

		public MagicalTeleporter( Serial serial )
			: base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( ( int )1 ); //version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}