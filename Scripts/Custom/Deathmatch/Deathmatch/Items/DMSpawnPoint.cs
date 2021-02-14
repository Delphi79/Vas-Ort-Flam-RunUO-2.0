using System;
using System.IO;
using System.Text;
using System.Collections;

using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targets;
using Server.Targeting;
using Server.Network;
using Server.Gumps;
using Server.Factions;
using Server.Custom.PvpToolkit;
using Server.Custom.PvpToolkit.Gumps;
using Server.Custom.PvpToolkit.Items;

namespace Server.Custom.PvpToolkit.DMatch.Items
{
    public class DMSpawnPoint : Item
    {
        private DMStone m_Link;

        [CommandProperty( AccessLevel.GameMaster )]
        public DMStone StoneLink
        {
            get
            {
                return m_Link;
            }
            set
            {
                if( !( value is DMStone ) )
                {
                    World.Broadcast( 38, false, "Unable to bind DMSpawnPoint {0} to {1} as it is not a valid DMStone", this.Serial, value.ToString() );
                    return;
                }
                else
                {
                    m_Link = value;
                    m_Link.DMSpawnPoints.Add( this );                    
                }
            }
        }

        [Constructable]
        public DMSpawnPoint()
            : base( 0x1DB8 )
        {
            Name = "a deathmatch spawnpoint";
            Visible = false;
            Movable = false;
        }

        public DMSpawnPoint( Serial serial )
            : base( serial ) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( ( int )0 );
            writer.Write( ( Item )m_Link );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            switch( version )
            {
                case 0:
                    {
                        m_Link = ( DMStone )reader.ReadItem();
                        break;
                    }
            }
        }
    }
}
