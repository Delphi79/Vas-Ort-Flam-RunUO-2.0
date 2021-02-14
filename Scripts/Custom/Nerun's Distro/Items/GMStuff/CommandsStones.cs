//By Nerun
// [Admin, [Props, [m tele, [m remove, "[send to britain bank", [spawner
using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Targeting;
using Server.Commands;

namespace Server.Items 
{
	public class AdminStone : Item
	{ 
		[Constructable] 
		public AdminStone() :  base( 0x1870 ) 
		{ 
			Weight = 1.0; 
			Hue = 0x5A; 
			Name = "[Admin"; 
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			string prefix = Server.Commands.CommandSystem.Prefix;

			if ( m.AccessLevel >= AccessLevel.Administrator )
			{
				CommandSystem.Handle( m, String.Format( "{0}Admin", prefix ) );	
			}

			else
			{
				m.SendMessage( "You are unable to use that!" );
				this.Delete();
			}
		} 

		public AdminStone( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	}

	public class PropsStone : Item
	{ 
		[Constructable] 
		public PropsStone() :  base( 0x1870 ) 
		{ 
			Weight = 1.0; 
			Hue = 0x40; 
			Name = "[props"; 
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			string prefix = Server.Commands.CommandSystem.Prefix;

			if ( m.AccessLevel > AccessLevel.Counselor )
			{
				CommandSystem.Handle( m, String.Format( "{0}props", prefix ) );	
			}

			else
			{
				m.SendMessage( "You are unable to use that!" );
				this.Delete();
			}
		} 

		public PropsStone( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	}

	public class TeleportStone : Item
	{ 
		[Constructable] 
		public TeleportStone() :  base( 0x1870 ) 
		{ 
			Weight = 1.0; 
			Hue = 0x1A; 
			Name = "[m tele"; 
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			string prefix = Server.Commands.CommandSystem.Prefix;

			if ( m.AccessLevel > AccessLevel.Counselor )
			{
				CommandSystem.Handle( m, String.Format( "{0}m tele", prefix ) );	
			}

			else
			{
				m.SendMessage( "You are unable to use that!" );
				this.Delete();
			}
		} 

		public TeleportStone( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	}

	public class RemoveStone : Item
	{ 
		[Constructable] 
		public RemoveStone() :  base( 0x1870 ) 
		{ 
			Weight = 1.0; 
			Hue = 0x27; 
			Name = "[m remove"; 
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			string prefix = Server.Commands.CommandSystem.Prefix;

			if ( m.AccessLevel > AccessLevel.Counselor )
			{
				CommandSystem.Handle( m, String.Format( "{0}m remove", prefix ) );	
			}

			else
			{
				m.SendMessage( "You are unable to use that!" );
				this.Delete();
			}
		} 

		public RemoveStone( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	}

	public class GoStone : Item
	{ 
		[Constructable] 
		public GoStone() :  base( 0x1870 ) 
		{ 
			Weight = 1.0; 
			Hue = 3; 
			Name = "[go";
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			string prefix = Server.Commands.CommandSystem.Prefix;

			if ( m.AccessLevel > AccessLevel.Counselor )
			{
				CommandSystem.Handle( m, String.Format( "{0}go", prefix ) );	
			}

			else
			{
				m.SendMessage( "You are unable to use that!" );
				this.Delete();
			}
		} 

		public GoStone( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	}

	public class PremiumStone : Item
	{ 
		[Constructable] 
		public PremiumStone() :  base( 0x1870 ) 
		{ 
			Weight = 1.0; 
			Hue = 1001; 
			Name = "Premium Spawner System";
			LootType = LootType.Blessed;
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			string prefix = Server.Commands.CommandSystem.Prefix;

			if ( m.AccessLevel >= AccessLevel.Administrator )
			{
				CommandSystem.Handle( m, String.Format( "{0}spawner", prefix ) );
			}

			else
			{
				m.SendMessage( "You are unable to use that!" );
				this.Delete();
			}
		} 

		public PremiumStone( Serial serial ) : base( serial ) 
		{ 
		} 
       
		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 
		}

		public override void Deserialize( GenericReader reader ) 
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}