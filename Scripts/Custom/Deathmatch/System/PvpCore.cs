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

using Server.Custom.PvpToolkit.DMatch;
using Server.Custom.PvpToolkit.Gumps;
using Server.Custom.PvpToolkit.DMatch.Items;

namespace Server.Custom.PvpToolkit
{
    public class PvpCore
    {
        public static string DeathmatchVersion { get { return "1.00"; } }
        public static string CTFVersion { get { return "1.00"; } }
        public static string TournamentVersion { get { return "1.00"; } }
        public static string ColorWars { get { return "1.00"; } }
        public static string FreeForAllVersion { get { return "1.00"; } }

        private static ArrayList m_CtfStones = new ArrayList();
        private static ArrayList m_DMStones = new ArrayList();
        private static ArrayList m_FFAStones = new ArrayList();
        private static ArrayList m_TourneyStones = new ArrayList();
        private static ArrayList m_DuelStones = new ArrayList();

        public static ArrayList CtfStones { get { return m_CtfStones; } set { m_CtfStones = value; } }
        public static ArrayList DMStones { get { return m_DMStones; } set { m_DMStones = value; } }
        public static ArrayList FFAStones { get { return m_FFAStones; } set { m_FFAStones = value; } }
        public static ArrayList TourneyStones { get { return m_TourneyStones; } set { m_TourneyStones = value; } }
        public static ArrayList DuelStones { get { return m_DuelStones; } set { m_DuelStones = value; } }

        public static void Initialize()
        {
            OnLoad();

            EventSink.PlayerDeath += new PlayerDeathEventHandler( EventSink_PlayerDeath );
            EventSink.Movement += new MovementEventHandler( EventSink_Movement );
            EventSink.Speech += new SpeechEventHandler( EventSink_Speech );
        }

        private static void EventSink_Speech( SpeechEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null || !m.Player )
                return;

            if( e.Speech.ToLower().IndexOf( "i wish to join the deathmatch" ) >= 0 )
            {
                if( Factions.Sigil.ExistsOn( m ) )
                {
                    m.SendLocalizedMessage( 1061632 ); // You can't do that while carrying the sigil.
                }
                else if( IsInDeathmatch( m ) )
                {
                    m.SendMessage( "You are already in a deathmatch. Say \"i wish to leave the deathmatch\" to leave." );
                }
                else if( ( ( PlayerMobile )m ).Young )
                {
                    m.SendMessage( "You cannot join a deathmatch while Young" );
                }
                else if( m.Spell != null )
                {
                    m.SendLocalizedMessage( 1049616 ); // You are too busy to do that at the moment.
                }
                else if( !m.Alive )
                {
                    m.SendMessage( "You cannot join a deathmatch while dead." );
                }
                else if( DMStones.Count < 1 )
                {
                    m.SendMessage( "No deathmatch system exists on the server." );
                }
                else
                {
                    AllowDMJoin( m );
                }
            }

            if( e.Speech.ToLower().IndexOf( "i wish to leave the deathmatch" ) >= 0 )
            {
                if( IsInDeathmatch( m ) )
                {
                    DMStone s = GetPlayerStone( m );

                    if( s != null )
                        s.RemovePlayer( m );
                }
            }

            if( e.Speech.ToLower().IndexOf( "scoreboard" ) >= 0 )
            {
                if( IsInDeathmatch( m ) )
                {
                    DMStone s = GetPlayerStone( m );

                    if( s != null )
                        s.ShowScore( m );
                }
            }
        }

        private static void AllowDMJoin( Mobile m )
        {
            bool found = false;

            foreach( DMStone s in DMStones )
            {
                if( s != null && s.Started && s.AcceptingContestants )
                {
                    m.SendGump( new PvpAcceptGump( s ) );
                    found = true;
                    break;
                }
            }

            if( !found )
                m.SendMessage( "Either a deathmatch has not been started or is full and not accepting players." );
        }
        
        private static void OnLoad()
        {
            
        }

        private static void EventSink_Movement( MovementEventArgs e )
        {
            
        }

        private static void EventSink_PlayerDeath( PlayerDeathEventArgs e )
        {
            Mobile m = e.Mobile;

            if( m == null )
                return;

            if( IsInDeathmatch( m ) )
            {
                DMStone stone = GetPlayerStone( m );

                if( stone != null )
                    stone.HandleDeath( m );
            }
            
        }

        public static DMStone GetPlayerStone( Mobile m )
        {
 	        foreach( DMStone stone in DMStones )
                if( stone != null && stone.Contestants.Contains( m ) )
                    return stone;

            return null;
        }

        public static bool IsInDeathmatch( Mobile m )
        {
            foreach( DMStone stone in DMStones )
                if( stone != null && stone.Contestants.Contains( m ) )
                    return true;
            
            return false;
        }

        public static Layer[] EquipmentLayers = new Layer[]
		{
			Layer.Cloak,
			Layer.Bracelet,
			Layer.Ring,
			Layer.Shirt,
			Layer.Pants,
			Layer.InnerLegs,
			Layer.Shoes,
			Layer.Arms,
			Layer.InnerTorso,
			Layer.MiddleTorso,
			Layer.OuterLegs,
			Layer.Neck,
			Layer.Waist,
			Layer.Gloves,
			Layer.OuterTorso,
			Layer.OneHanded,
			Layer.TwoHanded,
			Layer.FacialHair,
			Layer.Hair,
			Layer.Helm
		}; 
    }
}