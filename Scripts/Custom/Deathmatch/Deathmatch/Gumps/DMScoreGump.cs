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
    public class DMScoreGump : Gump
    {
        public class DMRankSorter : IComparable
        {
            public ScoreKeeper Keeper;
            public int Kills;
            public int Deaths;

            public DMRankSorter( ScoreKeeper m )
            {
                Keeper = m;
                if( m is ScoreKeeper )
                {
                    Kills = Keeper.Kills;
                    Deaths = Keeper.Deaths;
                }
            }

            public int CompareTo( object obj )
            {
                DMRankSorter p = ( DMRankSorter )obj;

                if( p.Kills - Kills == 0 )
                {
                    return Deaths - p.Deaths;
                }

                return p.Kills - Kills;
            }
        }

        public Mobile m_From;
        public ArrayList m_List;

        private const int LabelColor = 0x7FFF;
        private const int SelectedColor = 0x421F;
        private const int DisabledColor = 0x4210;

        private const int LabelColor32 = 0xFFFFFF;
        private const int SelectedColor32 = 0x8080FF;
        private const int DisabledColor32 = 0x808080;

        private const int LabelHue = 0x480;
        private const int GreenHue = 0x40;
        private const int RedHue = 0x20;

        public DMScoreGump( Mobile from, ArrayList list )
            : base( 50, 40 )
        {
            from.CloseGump( typeof( DMScoreGump ) );

            m_List = list;
            m_From = from;

            ArrayList playerlist = new ArrayList();

            foreach( ScoreKeeper s in m_List )
            {
                playerlist.Add( new DMRankSorter( s ) );
            }

            AddPage( 0 );
            AddBackground( 0, 0, 420, 540, 5054 );
            AddBlackAlpha( 10, 10, 400, 520 );

            AddLabel( 160, 15, RedHue, "Deathmatch top 20" );
            AddLabel( 20, 40, LabelHue, "Players" );
            AddLabel( 185, 40, LabelHue, "Kills" );
            AddLabel( 345, 40, LabelHue, "Deaths" );

            playerlist.Sort();

            for( int i = 0; i < playerlist.Count; ++i )
            {

                DMRankSorter g = ( DMRankSorter )playerlist[i];

                string name = null;

                if( ( name = g.Keeper.Player.Name ) != null && ( name = name.Trim() ).Length <= 15 )
                    name = g.Keeper.Player.Name;

                string wins = g.Keeper.Kills.ToString();

                string loses = g.Keeper.Deaths.ToString();

                AddLabel( 20, 70 + ( ( i % 20 ) * 20 ), GreenHue, name );
                AddLabel( 198, 70 + ( ( i % 20 ) * 20 ), GreenHue, wins );
                AddLabel( 358, 70 + ( ( i % 20 ) * 20 ), GreenHue, loses );
            }
        }

        public string Color( string text, int color )
        {
            return String.Format( "<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text );
        }

        public void AddBlackAlpha( int x, int y, int width, int height )
        {
            AddImageTiled( x, y, width, height, 2624 );
            AddAlphaRegion( x, y, width, height );
        }
    }
}

