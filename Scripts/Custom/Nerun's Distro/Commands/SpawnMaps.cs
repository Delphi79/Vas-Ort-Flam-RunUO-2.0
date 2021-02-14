// By Nerun
// Spawner gump for Trammel/Felucca/Ilshenar/Malas/Tokuno
// version L
using System;
using System.Collections;
using System.IO;
using Server;
using Server.Commands;
using Server.Mobiles; 
using Server.Items;
using Server.Scripts.Commands; 
using Server.Network;
using Server.Gumps;

namespace Server.Scripts.Commands 
{
	public class Spawn
	{
		public Spawn()
		{
		}

        public static void Initialize() 
        { 
            CommandSystem.Register( "Spawn", AccessLevel.Administrator, new CommandEventHandler( SpawnMain_OnCommand ) );
	    CommandSystem.Register( "SpawnMalas", AccessLevel.Administrator, new CommandEventHandler( SpawnMalas_OnCommand ) );
	    CommandSystem.Register( "SpawnIlshenar", AccessLevel.Administrator, new CommandEventHandler( SpawnIlshenar_OnCommand ) );
	    CommandSystem.Register( "SpawnTokuno", AccessLevel.Administrator, new CommandEventHandler( SpawnTokuno_OnCommand ) );
        }

        [Usage( "[spawn" )]
        [Description( "Spawn Trammel/Felucca with a menu." )] 
        private static void SpawnMain_OnCommand( CommandEventArgs e )
        { 
		e.Mobile.SendGump( new MainGump( e ) );
	}

        [Usage( "[spawnmalas" )]
        [Description( "Spawn Malas with a menu." )] 
        private static void SpawnMalas_OnCommand( CommandEventArgs e )
        { 
		e.Mobile.SendGump( new MalasGump( e ) );
	}

        [Usage( "[spawnilshenar" )]
        [Description( "Spawn Ilshenar with a menu." )] 
        private static void SpawnIlshenar_OnCommand( CommandEventArgs e )
        { 
		e.Mobile.SendGump( new IlshenarGump( e ) );
	}

        [Usage( "[spawntokuno" )]
        [Description( "Spawn Tokuno with a menu." )] 
        private static void SpawnTokuno_OnCommand( CommandEventArgs e )
        { 
		e.Mobile.SendGump( new TokunoGump( e ) );
	}

	}
}

namespace Server.Gumps
{

    public class MainGump : Gump
    {
        private CommandEventArgs m_CommandEventArgs;
        public MainGump( CommandEventArgs e ) : base( 50,50 )
        {
            m_CommandEventArgs = e;
            Closable = true;
            Dragable = true;

            AddPage(1);

	//fundo cinza
            AddBackground( 0, 0, 295, 310, 5054 );
	//----------
            AddLabel( 90, 2, 200, "PREMIUM SPAWNER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 272, 232, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Trammel" );
            AddLabel( 225, 27, 200, "Felucca" );
	//colunas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 2, 222, 10003 );
            AddImageTiled( 163, 25, 2, 222, 10003 );
            AddImageTiled( 218, 25, 2, 222, 10003 );
            AddImageTiled( 270, 25, 2, 222, 10003 );
	//Linhas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 250, 2, 10001 );
            AddImageTiled( 20, 45, 250, 2, 10001 );
            AddImageTiled( 20, 70, 250, 2, 10001 );
            AddImageTiled( 20, 95, 250, 2, 10001 );
            AddImageTiled( 20, 120, 250, 2, 10001 );
            AddImageTiled( 20, 145, 250, 2, 10001 );
            AddImageTiled( 20, 170, 250, 2, 10001 );
            AddImageTiled( 20, 195, 250, 2, 10001 );
            AddImageTiled( 20, 220, 250, 2, 10001 );
            AddImageTiled( 20, 245, 250, 2, 10001 );
	//----------
            AddLabel( 35, 51, 200, "Animals" );
            AddLabel( 35, 76, 200, "Camps" );
            AddLabel( 35, 101, 200, "Covetous" );
            AddLabel( 35, 126, 200, "Deceit" );
            AddLabel( 35, 151, 200, "Despise" );
            AddLabel( 35, 176, 200, "Destard" );
            AddLabel( 35, 201, 200, "Escorts" );
            AddLabel( 35, 226, 200, "Fire" );

	//Options Trammel
            AddCheck( 182, 48, 210, 211, true, 101 ); // animals
            AddCheck( 182, 73, 210, 211, true, 102 ); // camps
            AddCheck( 182, 98, 210, 211, true, 103 ); // covetous
            AddCheck( 182, 123, 210, 211, true, 104 ); // deceit
	    AddCheck( 182, 148, 210, 211, true, 105 ); // despise
            AddCheck( 182, 173, 210, 211, true, 106 ); // destard
            AddCheck( 182, 198, 210, 211, true, 107 ); // escorts
            AddCheck( 182, 223, 210, 211, true, 108 ); // fire
	//Options Felucca
            AddCheck( 235, 48, 210, 211, true, 128 ); // animals
            AddCheck( 235, 73, 210, 211, true, 129 ); // camps
            AddCheck( 235, 98, 210, 211, true, 130 ); // covetous
            AddCheck( 235, 123, 210, 211, true, 131 ); // deceit
	    AddCheck( 235, 148, 210, 211, true, 132 ); // despise
            AddCheck( 235, 173, 210, 211, true, 133 ); // destard
            AddCheck( 235, 198, 210, 211, true, 134 ); // escorts
            AddCheck( 235, 223, 210, 211, true, 135 ); // fire

            AddLabel( 135, 255, 200, "1/4" );
	    AddButton( 255, 255, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2 );

            AddPage(2);

	//fundo cinza
            AddBackground( 0, 0, 295, 310, 5054 );
	//----------
            AddLabel( 90, 2, 200, "PREMIUM SPAWNER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 272, 232, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Trammel" );
            AddLabel( 225, 27, 200, "Felucca" );
	//colunas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 2, 222, 10003 );
            AddImageTiled( 163, 25, 2, 222, 10003 );
            AddImageTiled( 218, 25, 2, 222, 10003 );
            AddImageTiled( 270, 25, 2, 222, 10003 );
	//Linhas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 250, 2, 10001 );
            AddImageTiled( 20, 45, 250, 2, 10001 );
            AddImageTiled( 20, 70, 250, 2, 10001 );
            AddImageTiled( 20, 95, 250, 2, 10001 );
            AddImageTiled( 20, 120, 250, 2, 10001 );
            AddImageTiled( 20, 145, 250, 2, 10001 );
            AddImageTiled( 20, 170, 250, 2, 10001 );
            AddImageTiled( 20, 195, 250, 2, 10001 );
            AddImageTiled( 20, 220, 250, 2, 10001 );
            AddImageTiled( 20, 245, 250, 2, 10001 );
	//----------
            AddLabel( 35, 51, 200, "Graveyards" );
            AddLabel( 35, 76, 200, "Hythloth" );
            AddLabel( 35, 101, 200, "Ice" );
            AddLabel( 35, 126, 200, "Khaldun" );
            AddLabel( 35, 151, 200, "Lost Lands" );
            AddLabel( 35, 176, 200, "Miscellaneous" );
            AddLabel( 35, 201, 200, "Moonglow Zoo" );
            AddLabel( 35, 226, 200, "Orc Caves" );

	//Options Trammel
            AddCheck( 182, 48, 210, 211, true, 109 ); // graveyards
            AddCheck( 182, 73, 210, 211, true, 110 ); // hythloth
            AddCheck( 182, 98, 210, 211, true, 111 ); // ice
//AddLabel(x, y, color, "label")
            AddLabel( 178, 126, 200, "N/A" );          // khaldun (not in trammel)
	    AddCheck( 182, 148, 210, 211, true, 113 ); // lost lands
            AddCheck( 182, 173, 210, 211, true, 114 ); // miscellaneous
            AddCheck( 182, 198, 210, 211, true, 115 ); // moonglow zoo
            AddCheck( 182, 223, 210, 211, true, 116 ); // orc caves
	//Options Felucca
            AddCheck( 235, 48, 210, 211, true, 136 ); // graveyards
            AddCheck( 235, 73, 210, 211, true, 137 ); // hythloth
            AddCheck( 235, 98, 210, 211, true, 138 ); // ice
            AddCheck( 235, 123, 210, 211, true, 139 ); // khaldun
	    AddCheck( 235, 148, 210, 211, true, 140 ); // lost lands
            AddCheck( 235, 173, 210, 211, true, 141 ); // miscellaneous
            AddCheck( 235, 198, 210, 211, true, 142 ); // moonglow zoo
            AddCheck( 235, 223, 210, 211, true, 143 ); // orc caves

            AddLabel( 135, 255, 200, "2/4" );
	    AddButton( 255, 255, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 3 );
	    AddButton( 10, 255, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 1 );

            AddPage(3);

	//fundo cinza
            AddBackground( 0, 0, 295, 310, 5054 );
	//----------
            AddLabel( 90, 2, 200, "PREMIUM SPAWNER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 272, 232, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Trammel" );
            AddLabel( 225, 27, 200, "Felucca" );
	//colunas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 2, 222, 10003 );
            AddImageTiled( 163, 25, 2, 222, 10003 );
            AddImageTiled( 218, 25, 2, 222, 10003 );
            AddImageTiled( 270, 25, 2, 222, 10003 );
	//Linhas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 250, 2, 10001 );
            AddImageTiled( 20, 45, 250, 2, 10001 );
            AddImageTiled( 20, 70, 250, 2, 10001 );
            AddImageTiled( 20, 95, 250, 2, 10001 );
            AddImageTiled( 20, 120, 250, 2, 10001 );
            AddImageTiled( 20, 145, 250, 2, 10001 );
            AddImageTiled( 20, 170, 250, 2, 10001 );
            AddImageTiled( 20, 195, 250, 2, 10001 );
            AddImageTiled( 20, 220, 250, 2, 10001 );
            AddImageTiled( 20, 245, 250, 2, 10001 );
	//----------
            AddLabel( 35, 51, 200, "OSI Heavy" );
            AddLabel( 35, 76, 200, "OSI Light" );
            AddLabel( 35, 101, 200, "OSI Medium" );
            AddLabel( 35, 126, 200, "Shame" );
            AddLabel( 35, 151, 200, "Solen Hive" );
            AddLabel( 35, 176, 200, "Terathan Keep" );
            AddLabel( 35, 201, 200, "Town Criers" );
            AddLabel( 35, 226, 200, "Towns (animals)" );

	//Options Trammel
            AddCheck( 182, 48, 210, 211, true, 117 ); // osi heavy
            AddCheck( 182, 73, 210, 211, true, 118 ); // osi light
            AddCheck( 182, 98, 210, 211, true, 119 ); // osi medium
            AddCheck( 182, 123, 210, 211, true, 120 ); // shame
	    AddCheck( 182, 148, 210, 211, true, 121 ); // solen hive
            AddCheck( 182, 173, 210, 211, true, 122 ); // terathan keep
            AddCheck( 182, 198, 210, 211, true, 123 ); // town criers
            AddCheck( 182, 223, 210, 211, true, 124 ); // towns animals
	//Options Felucca
            AddCheck( 235, 48, 210, 211, true, 144 ); // osi heavy
            AddCheck( 235, 73, 210, 211, true, 145 ); // osi light
            AddCheck( 235, 98, 210, 211, true, 146 ); // osi medium
            AddCheck( 235, 123, 210, 211, true, 147 ); // shame
	    AddCheck( 235, 148, 210, 211, true, 148 ); // solen hive
            AddCheck( 235, 173, 210, 211, true, 149 ); // terathan keep
            AddCheck( 235, 198, 210, 211, true, 150 ); // town criers
            AddCheck( 235, 223, 210, 211, true, 151 ); // towns animals

            AddLabel( 135, 255, 200, "3/4" );
	    AddButton( 255, 255, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 4 );
	    AddButton( 10, 255, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 2 );

            AddPage(4);

	//fundo cinza
            AddBackground( 0, 0, 295, 310, 5054 );
	//----------
            AddLabel( 90, 2, 200, "PREMIUM SPAWNER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 272, 107, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Trammel" );
            AddLabel( 225, 27, 200, "Felucca" );
	//colunas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 2, 96, 10003 );
            AddImageTiled( 163, 25, 2, 96, 10003 );
            AddImageTiled( 218, 25, 2, 96, 10003 );
            AddImageTiled( 270, 25, 2, 96, 10003 );
	//Linhas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 250, 2, 10001 );
            AddImageTiled( 20, 45, 250, 2, 10001 );
            AddImageTiled( 20, 70, 250, 2, 10001 );
            AddImageTiled( 20, 95, 250, 2, 10001 );
            AddImageTiled( 20, 120, 250, 2, 10001 );
	//----------
            AddLabel( 35, 51, 200, "Trinsic Passage" );
            AddLabel( 35, 76, 200, "Vendors" );
            AddLabel( 35, 101, 200, "Wrong" );

            AddLabel( 35, 150, 200, "You are ready to spawn the world" );
            AddLabel( 35, 165, 200, "with the selected options?" );


	//Options Trammel
            AddCheck( 182, 48, 210, 211, true, 125 ); // trinsic passage
            AddCheck( 182, 73, 210, 211, true, 126 ); // vendors
            AddCheck( 182, 98, 210, 211, true, 127 ); // wrong
	//Options Felucca
            AddCheck( 235, 48, 210, 211, true, 152 ); // trinsic passage
            AddCheck( 235, 73, 210, 211, true, 153 ); // vendors
            AddCheck( 235, 98, 210, 211, true, 154 ); // wrong

            AddLabel( 135, 255, 200, "4/4" );
	    AddButton( 10, 255, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 3 );

	//Ok, Cancel
            AddButton( 80, 280, 247, 249, 1, GumpButtonType.Reply, 0 );
            AddButton( 150, 280, 241, 243, 0, GumpButtonType.Reply, 0 );

        }

        public override void OnResponse( NetState state, RelayInfo info ) 
        { 
            Mobile from = state.Mobile; 

            switch( info.ButtonID ) 
            { 
                case 0: // Closed or Cancel
                {
                    return;
                }

                default: 
                { 
                    // Make sure that the OK, button was pressed
                    if( info.ButtonID == 1 )
                    {
                        // Get the array of switches selected
                        ArrayList Selections = new ArrayList( info.Switches );
			string prefix = CommandSystem.Prefix;

                        if( Selections.Contains( 101 ) == true || Selections.Contains( 102 ) == true || Selections.Contains( 103 ) == true || Selections.Contains( 104 ) == true || Selections.Contains( 105 ) == true || Selections.Contains( 106 ) == true || Selections.Contains( 107 ) == true || Selections.Contains( 108 ) == true || Selections.Contains( 109 ) == true || Selections.Contains( 110 ) == true || Selections.Contains( 111 ) == true || Selections.Contains( 112 ) == true || Selections.Contains( 113 ) == true || Selections.Contains( 114 ) == true || Selections.Contains( 115 ) == true || Selections.Contains( 116 ) == true || Selections.Contains( 117 ) == true || Selections.Contains( 118 ) == true || Selections.Contains( 119 ) == true || Selections.Contains( 120 ) == true || Selections.Contains( 121 ) == true || Selections.Contains( 122 ) == true || Selections.Contains( 123 ) == true || Selections.Contains( 124 ) == true || Selections.Contains( 125 ) == true || Selections.Contains( 126 ) == true || Selections.Contains( 127 ) == true )
                        {
				from.Say( "SPAWNING TRAMMEL..." );
                        }

// Trammel -------------------

                        // Now spawn any selected maps
                        if( Selections.Contains( 101 ) == true )
                        {
                            from.Say( "Spawning animals around the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/animals.map", prefix ) );
                        }

                        if( Selections.Contains( 102 ) == true )
                        {
                            from.Say( "Spawning Camps arount the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/camps.map", prefix ) );
                        }

                        if( Selections.Contains( 103 ) == true )
                        {
                            from.Say( "Spawning Covetous dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/covetous.map", prefix ) );
                        }

                        if( Selections.Contains( 104 ) == true )
                        {
                            from.Say( "Spawning Deceit dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/deceit.map", prefix ) );
                        }

                        if( Selections.Contains( 105 ) == true )
                        {
                            from.Say( "Spawning Despise dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/despise.map", prefix ) );
                        }

                        if( Selections.Contains( 106 ) == true )
                        {
                            from.Say( "Spawning Destard dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/destard.map", prefix ) );
                        }

                        if( Selections.Contains( 107 ) == true )
                        {
                            from.Say( "Spawning escortables in some towns..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/escorts.map", prefix ) );
                        }

                        if( Selections.Contains( 108 ) == true )
                        {
                            from.Say( "Spawning Fire dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/fire.map", prefix ) );
                        }

                        if( Selections.Contains( 109 ) == true )
                        {
                            from.Say( "Spawning graveyards..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/graveyards.map", prefix ) );
                        }

                        if( Selections.Contains( 110 ) == true )
                        {
                            from.Say( "Spawning Hythloth dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/hythloth.map", prefix ) );
                        }

                        if( Selections.Contains( 111 ) == true )
                        {
                            from.Say( "Spawning Ice dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/ice.map", prefix ) );
                        }

                        if( Selections.Contains( 113 ) == true )
                        {
                            from.Say( "Spawning Lost Lands..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/lostlands.map", prefix ) );
                        }

                        if( Selections.Contains( 114 ) == true )
                        {
                            from.Say( "Spawning miscellaneous..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/miscellaneous.map", prefix ) );
                        }

                        if( Selections.Contains( 115 ) == true )
                        {
                            from.Say( "Spawning Moonglow Zoo..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/moonglowzoo.map", prefix ) );
                        }

                        if( Selections.Contains( 116 ) == true )
                        {
                            from.Say( "Spawning Orc Caves..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/orccaves.map", prefix ) );
                        }

                        if( Selections.Contains( 117 ) == true )
                        {
                            from.Say( "Spawning heavy spawns around the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/osiheavy.map", prefix ) );
                        }

                        if( Selections.Contains( 118 ) == true )
                        {
                            from.Say( "Spawning light spawns around the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/osilight.map", prefix ) );
                        }

                        if( Selections.Contains( 119 ) == true )
                        {
                            from.Say( "Spawning medium spawns around the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/osimedium.map", prefix ) );
                        }

                        if( Selections.Contains( 120 ) == true )
                        {
                            from.Say( "Spawning Shame dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/shame.map", prefix ) );
                        }

                        if( Selections.Contains( 121 ) == true )
                        {
                            from.Say( "Spawning Solen Hive dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/solenhive.map", prefix ) );
                        }

                        if( Selections.Contains( 122 ) == true )
                        {
                            from.Say( "Spawning Terathan Keep..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/terathankeep.map", prefix ) );
                        }

                        if( Selections.Contains( 123 ) == true )
                        {
                            from.Say( "Spawning Town Criers in some towns..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/towncriers.map", prefix ) );
                        }

                        if( Selections.Contains( 124 ) == true )
                        {
                            from.Say( "Spawning animals in towns..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/towns.map", prefix ) );
                        }

                        if( Selections.Contains( 125 ) == true )
                        {
                            from.Say( "Spawning Trinsic Passage..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/trinsicpassage.map", prefix ) );
                        }

                        if( Selections.Contains( 126 ) == true )
                        {
                            from.Say( "Spawning vendors..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/vendors.map", prefix ) );
                        }

                        if( Selections.Contains( 127 ) == true )
                        {
                            from.Say( "Spawning Wrong dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen trammel/wrong.map", prefix ) );
                        }

// Felucca -------------------

                        if( Selections.Contains( 128 ) == true || Selections.Contains( 129 ) == true || Selections.Contains( 130 ) == true || Selections.Contains( 131 ) == true || Selections.Contains( 132 ) == true || Selections.Contains( 133 ) == true || Selections.Contains( 134 ) == true || Selections.Contains( 135 ) == true || Selections.Contains( 136 ) == true || Selections.Contains( 137 ) == true || Selections.Contains( 138 ) == true || Selections.Contains( 139 ) == true || Selections.Contains( 140 ) == true || Selections.Contains( 141 ) == true || Selections.Contains( 142 ) == true || Selections.Contains( 143 ) == true || Selections.Contains( 144 ) == true || Selections.Contains( 145 ) == true || Selections.Contains( 146 ) == true || Selections.Contains( 147 ) == true || Selections.Contains( 148 ) == true || Selections.Contains( 149 ) == true || Selections.Contains( 150 ) == true || Selections.Contains( 151 ) == true || Selections.Contains( 152 ) == true || Selections.Contains( 153 ) == true || Selections.Contains( 154 ) == true )
                        {
				from.Say( "SPAWNING FELUCCA..." );
                        }

                        // Now spawn any selected maps
                        if( Selections.Contains( 128 ) == true )
                        {
                            from.Say( "Spawning animals around the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/animals.map", prefix ) );
                        }

                        if( Selections.Contains( 129 ) == true )
                        {
                            from.Say( "Spawning Camps arount the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/camps.map", prefix ) );
                        }

                        if( Selections.Contains( 130 ) == true )
                        {
                            from.Say( "Spawning Covetous dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/covetous.map", prefix ) );
                        }

                        if( Selections.Contains( 131 ) == true )
                        {
                            from.Say( "Spawning Deceit dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/deceit.map", prefix ) );
                        }

                        if( Selections.Contains( 132 ) == true )
                        {
                            from.Say( "Spawning Despise dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/despise.map", prefix ) );
                        }

                        if( Selections.Contains( 133 ) == true )
                        {
                            from.Say( "Spawning Destard dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/destard.map", prefix ) );
                        }

                        if( Selections.Contains( 134 ) == true )
                        {
                            from.Say( "Spawning escortables in some towns..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/escorts.map", prefix ) );
                        }

                        if( Selections.Contains( 135 ) == true )
                        {
                            from.Say( "Spawning Fire dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/fire.map", prefix ) );
                        }

                        if( Selections.Contains( 136 ) == true )
                        {
                            from.Say( "Spawning graveyards..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/graveyards.map", prefix ) );
                        }

                        if( Selections.Contains( 137 ) == true )
                        {
                            from.Say( "Spawning Hythloth dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/hythloth.map", prefix ) );
                        }

                        if( Selections.Contains( 138 ) == true )
                        {
                            from.Say( "Spawning Ice dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/ice.map", prefix ) );
                        }

                        if( Selections.Contains( 139 ) == true )
                        {
                            from.Say( "Spawning Khaldun dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/khaldun.map", prefix ) );
                        }

                        if( Selections.Contains( 140 ) == true )
                        {
                            from.Say( "Spawning Lost Lands..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/lostlands.map", prefix ) );
                        }

                        if( Selections.Contains( 141 ) == true )
                        {
                            from.Say( "Spawning miscellaneous..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/miscellaneous.map", prefix ) );
                        }

                        if( Selections.Contains( 142 ) == true )
                        {
                            from.Say( "Spawning Moonglow Zoo..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/moonglowzoo.map", prefix ) );
                        }

                        if( Selections.Contains( 143 ) == true )
                        {
                            from.Say( "Spawning Orc Caves..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/orccaves.map", prefix ) );
                        }

                        if( Selections.Contains( 144 ) == true )
                        {
                            from.Say( "Spawning heavy spawns areas around the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/osiheavy.map", prefix ) );
                        }

                        if( Selections.Contains( 145 ) == true )
                        {
                            from.Say( "Spawning light spawns around the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/osilight.map", prefix ) );
                        }

                        if( Selections.Contains( 146 ) == true )
                        {
                            from.Say( "Spawning medium spawns around the world..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/osimedium.map", prefix ) );
                        }

                        if( Selections.Contains( 147 ) == true )
                        {
                            from.Say( "Spawning Shame dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/shame.map", prefix ) );
                        }

                        if( Selections.Contains( 148 ) == true )
                        {
                            from.Say( "Spawning Solen Hive dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/solenhive.map", prefix ) );
                        }

                        if( Selections.Contains( 149 ) == true )
                        {
                            from.Say( "Spawning Terathan Keep..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/terathankeep.map", prefix ) );
                        }

                        if( Selections.Contains( 150 ) == true )
                        {
                            from.Say( "Spawning Town Criers in some towns..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/towncriers.map", prefix ) );
                        }

                        if( Selections.Contains( 151 ) == true )
                        {
                            from.Say( "Spawning animals in towns..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/towns.map", prefix ) );
                        }

                        if( Selections.Contains( 152 ) == true )
                        {
                            from.Say( "Spawning Trinsic Passage..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/trinsicpassage.map", prefix ) );
                        }

                        if( Selections.Contains( 153 ) == true )
                        {
                            from.Say( "Spawning vendors..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/vendors.map", prefix ) );
                        }

                        if( Selections.Contains( 154 ) == true )
                        {
                            from.Say( "Spawning Wrong dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen felucca/wrong.map", prefix ) );
                        }
                    }

		from.Say( "Spawn generation completed!" );
                break;
                } 
            } 
        }
}

    public class IlshenarGump : Gump
    {
        private CommandEventArgs m_CommandEventArgs;
        public IlshenarGump( CommandEventArgs e ) : base( 50,50 )
        {
            m_CommandEventArgs = e;
            Closable = true;
            Dragable = true;

            AddPage(1);

	//fundo cinza
            AddBackground( 0, 0, 243, 310, 5054 );
	//----------
            AddLabel( 60, 2, 200, "PREMIUM SPAWNER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 220, 232, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Spawn It" );
	//colunas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 2, 222, 10003 );
            AddImageTiled( 163, 25, 2, 222, 10003 );
            AddImageTiled( 220, 25, 2, 222, 10003 );
	//Linhas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 200, 2, 10001 );
            AddImageTiled( 20, 45, 200, 2, 10001 );
            AddImageTiled( 20, 70, 200, 2, 10001 );
            AddImageTiled( 20, 95, 200, 2, 10001 );
            AddImageTiled( 20, 120, 200, 2, 10001 );
            AddImageTiled( 20, 145, 200, 2, 10001 );
            AddImageTiled( 20, 170, 200, 2, 10001 );
            AddImageTiled( 20, 195, 200, 2, 10001 );
            AddImageTiled( 20, 220, 200, 2, 10001 );
            AddImageTiled( 20, 245, 200, 2, 10001 );
	//----------
            AddLabel( 35, 51, 200, "Ancient Lair" );
            AddLabel( 35, 76, 200, "Ankh Dungeon" );
            AddLabel( 35, 101, 200, "Blood Dungeon" );
            AddLabel( 35, 126, 200, "Exodus Dungeon" );
            AddLabel( 35, 151, 200, "Mushroom Cave" );
            AddLabel( 35, 176, 200, "Northeast sector" );
            AddLabel( 35, 201, 200, "Northwest sector" );
            AddLabel( 35, 226, 200, "Ratman Cave" );

	//Options Ilshenar
            AddCheck( 182, 48, 210, 211, true, 101 );
            AddCheck( 182, 73, 210, 211, true, 102 );
            AddCheck( 182, 98, 210, 211, true, 103 );
            AddCheck( 182, 123, 210, 211, true, 104 );
	    AddCheck( 182, 148, 210, 211, true, 105 );
            AddCheck( 182, 173, 210, 211, true, 106 );
            AddCheck( 182, 198, 210, 211, true, 107 );
            AddCheck( 182, 223, 210, 211, true, 108 );

            AddLabel( 110, 255, 200, "1/2" );
	    AddButton( 200, 255, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2 );

            AddPage(2);

	//fundo cinza
            AddBackground( 0, 0, 243, 310, 5054 );
	//----------
            AddLabel( 60, 2, 200, "PREMIUM SPAWNER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 220, 232, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Spawn It" );
	//colunas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 2, 222, 10003 );
            AddImageTiled( 163, 25, 2, 222, 10003 );
            AddImageTiled( 220, 25, 2, 222, 10003 );
	//Linhas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 200, 2, 10001 );
            AddImageTiled( 20, 45, 200, 2, 10001 );
            AddImageTiled( 20, 70, 200, 2, 10001 );
            AddImageTiled( 20, 95, 200, 2, 10001 );
            AddImageTiled( 20, 120, 200, 2, 10001 );
            AddImageTiled( 20, 145, 200, 2, 10001 );
            AddImageTiled( 20, 170, 200, 2, 10001 );
            AddImageTiled( 20, 195, 200, 2, 10001 );
            AddImageTiled( 20, 220, 200, 2, 10001 );
            AddImageTiled( 20, 245, 200, 2, 10001 );
	//----------
            AddLabel( 35, 51, 200, "Rock Dungeon" );
            AddLabel( 35, 76, 200, "Sorcerers Dungeon" );
            AddLabel( 35, 101, 200, "Southeast sector" );
            AddLabel( 35, 126, 200, "Southwest sector" );
            AddLabel( 35, 151, 200, "Spectre Dungeon" );
            AddLabel( 35, 176, 200, "Towns" );
            AddLabel( 35, 201, 200, "Vendors" );
            AddLabel( 35, 226, 200, "Wisp Dungeon" );

	//Options Ilshenar
            AddCheck( 182, 48, 210, 211, true, 109 );
            AddCheck( 182, 73, 210, 211, true, 110 );
            AddCheck( 182, 98, 210, 211, true, 111 );
            AddCheck( 182, 123, 210, 211, true, 112 );
	    AddCheck( 182, 148, 210, 211, true, 113 );
            AddCheck( 182, 173, 210, 211, true, 114 );
            AddCheck( 182, 198, 210, 211, true, 115 );
            AddCheck( 182, 223, 210, 211, true, 116 );

            AddLabel( 110, 255, 200, "2/2" );
	    AddButton( 10, 255, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 1 );

	//Ok, Cancel
            AddButton( 55, 280, 247, 249, 1, GumpButtonType.Reply, 0 );
            AddButton( 125, 280, 241, 243, 0, GumpButtonType.Reply, 0 );

        }

        public override void OnResponse( NetState state, RelayInfo info ) 
        { 
            Mobile from = state.Mobile; 

            switch( info.ButtonID ) 
            { 
                case 0: // Closed or Cancel
                {
                    return;
                }

                default: 
                { 
                    // Make sure that the OK, button was pressed
                    if( info.ButtonID == 1 )
                    {
                        // Get the array of switches selected
                        ArrayList Selections = new ArrayList( info.Switches );
			string prefix = CommandSystem.Prefix;

                        if( Selections.Contains( 101 ) == true || Selections.Contains( 102 ) == true || Selections.Contains( 103 ) == true || Selections.Contains( 104 ) == true || Selections.Contains( 105 ) == true || Selections.Contains( 106 ) == true || Selections.Contains( 107 ) == true || Selections.Contains( 108 ) == true || Selections.Contains( 109 ) == true || Selections.Contains( 110 ) == true || Selections.Contains( 111 ) == true || Selections.Contains( 112 ) == true || Selections.Contains( 113 ) == true || Selections.Contains( 114 ) == true || Selections.Contains( 115 ) == true || Selections.Contains( 116 ) == true )
                        {
				from.Say( "SPAWNING ILSHENAR..." );
                        }

                        // Now spawn any selected maps
                        if( Selections.Contains( 101 ) == true )
                        {
                            from.Say( "Spawning Ancient Lair..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/ancientlair.map", prefix ) );
                        }

                        if( Selections.Contains( 102 ) == true )
                        {
                            from.Say( "Spawning Ankh dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/ankh.map", prefix ) );
                        }

                        if( Selections.Contains( 103 ) == true )
                        {
                            from.Say( "Spawning Blood dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/blood.map", prefix ) );
                        }

                        if( Selections.Contains( 104 ) == true )
                        {
                            from.Say( "Spawning Exodus dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/exodus.map", prefix ) );
                        }

                        if( Selections.Contains( 105 ) == true )
                        {
                            from.Say( "Spawning Mushroom Cave..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/mushroom.map", prefix ) );
                        }

                        if( Selections.Contains( 106 ) == true )
                        {
                            from.Say( "Spawning Northeast sector..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/northeast.map", prefix ) );
                        }

                        if( Selections.Contains( 107 ) == true )
                        {
                            from.Say( "Spawning Northwest sector..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/northwest.map", prefix ) );
                        }

                        if( Selections.Contains( 108 ) == true )
                        {
                            from.Say( "Spawning Ratman Cave..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/ratmancave.map", prefix ) );
                        }

                        if( Selections.Contains( 109 ) == true )
                        {
                            from.Say( "Spawning Rock dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/rock.map", prefix ) );
                        }

                        if( Selections.Contains( 110 ) == true )
                        {
                            from.Say( "Spawning Sorcerers dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/sorcerers.map", prefix ) );
                        }

                        if( Selections.Contains( 111 ) == true )
                        {
                            from.Say( "Spawning Southeast sector..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/southeast.map", prefix ) );
                        }

                        if( Selections.Contains( 112 ) == true )
                        {
                            from.Say( "Spawning Southwest sector..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/southwest.map", prefix ) );
                        }

                        if( Selections.Contains( 113 ) == true )
                        {
                            from.Say( "Spawning Spectre dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/spectre.map", prefix ) );
                        }

                        if( Selections.Contains( 114 ) == true )
                        {
                            from.Say( "Spawning Towns..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/towns.map", prefix ) );
                        }

                        if( Selections.Contains( 115 ) == true )
                        {
                            from.Say( "Spawning Vendors..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/vendors.map", prefix ) );
                        }

                        if( Selections.Contains( 116 ) == true )
                        {
                            from.Say( "Spawning Wisp dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen ilshenar/wisp.map", prefix ) );
                        }
                    }

                    from.Say( "Spawn generation completed!" );

                    break;
                } 
            } 
        }
}

    public class MalasGump : Gump
    {
        private CommandEventArgs m_CommandEventArgs;
        public MalasGump( CommandEventArgs e ) : base( 50,50 )
        {
            m_CommandEventArgs = e;
            Closable = true;
            Dragable = true;

            AddPage(1);

	//fundo cinza
//alt era 310
            AddBackground( 0, 0, 243, 250, 5054 );
	//----------
            AddLabel( 60, 2, 200, "PREMIUM SPAWNER" );
	//fundo branco
	//x, y, largura, altura, item
//alt era 232
            AddImageTiled( 10, 20, 220, 158, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Spawn It" );
	//colunas
	//x, y, comprimento, ?, item
//comp era 222
            AddImageTiled( 20, 25, 2, 147, 10003 );
            AddImageTiled( 163, 25, 2, 147, 10003 );
            AddImageTiled( 220, 25, 2, 147, 10003 );
	//Linhas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 200, 2, 10001 );
            AddImageTiled( 20, 45, 200, 2, 10001 );
            AddImageTiled( 20, 70, 200, 2, 10001 );
            AddImageTiled( 20, 95, 200, 2, 10001 );
            AddImageTiled( 20, 120, 200, 2, 10001 );
            AddImageTiled( 20, 145, 200, 2, 10001 );
            AddImageTiled( 20, 170, 200, 2, 10001 );
	//----------
            AddLabel( 35, 51, 200, "Doom Dungeon" );
            AddLabel( 35, 76, 200, "North Malas" );
            AddLabel( 35, 101, 200, "Orc Forts" );
            AddLabel( 35, 126, 200, "South Malas" );
            AddLabel( 35, 151, 200, "Vendors" );

	//Options Malas
            AddCheck( 182, 48, 210, 211, true, 101 );
            AddCheck( 182, 73, 210, 211, true, 102 );
            AddCheck( 182, 98, 210, 211, true, 103 );
            AddCheck( 182, 123, 210, 211, true, 104 );
	    AddCheck( 182, 148, 210, 211, true, 105 );

	//Ok, Cancel
// alt era 280
            AddButton( 55, 220, 247, 249, 1, GumpButtonType.Reply, 0 );
            AddButton( 125, 220, 241, 243, 0, GumpButtonType.Reply, 0 );

        }

        public override void OnResponse( NetState state, RelayInfo info ) 
        { 
            Mobile from = state.Mobile; 

            switch( info.ButtonID ) 
            { 
                case 0: // Closed or Cancel
                {
                    return;
                }

                default: 
                { 
                    // Make sure that the OK, button was pressed
                    if( info.ButtonID == 1 )
                    {
                        // Get the array of switches selected
                        ArrayList Selections = new ArrayList( info.Switches );
			string prefix = CommandSystem.Prefix;

                        if( Selections.Contains( 101 ) == true || Selections.Contains( 102 ) == true || Selections.Contains( 103 ) == true || Selections.Contains( 104 ) == true || Selections.Contains( 105 ) == true )
                        {
				from.Say( "SPAWNING MALAS..." );
                        }

                        // Now spawn any selected maps
                        if( Selections.Contains( 101 ) == true )
                        {
                            from.Say( "Spawning Doom dungeon..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen malas/doom.map", prefix ) );
                        }

                        if( Selections.Contains( 102 ) == true )
                        {
                            from.Say( "Spawning North Malas..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen malas/north.map", prefix ) );
                        }

                        if( Selections.Contains( 103 ) == true )
                        {
                            from.Say( "Spawning Orc Forts..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen malas/orcforts.map", prefix ) );
                        }

                        if( Selections.Contains( 104 ) == true )
                        {
                            from.Say( "Spawning South Malas..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen malas/south.map", prefix ) );
                        }

                        if( Selections.Contains( 105 ) == true )
                        {
                            from.Say( "Spawning Vendors..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen malas/vendors.map", prefix ) );
                        }
                    }

                    from.Say( "Spawn generation completed!" );

                    break;
                } 
            } 
        }
}

    public class TokunoGump : Gump
    {
        private CommandEventArgs m_CommandEventArgs;
        public TokunoGump( CommandEventArgs e ) : base( 50,50 )
        {
            m_CommandEventArgs = e;
            Closable = true;
            Dragable = true;

            AddPage(1);

	//fundo cinza
//alt era 310
            AddBackground( 0, 0, 243, 250, 5054 );
	//----------
            AddLabel( 60, 2, 200, "PREMIUM SPAWNER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 220, 183, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Spawn It" );
	//colunas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 2, 172, 10003 );
            AddImageTiled( 163, 25, 2, 172, 10003 );
            AddImageTiled( 220, 25, 2, 172, 10003 );
	//Linhas
	//x, y, comprimento, ?, item
            AddImageTiled( 20, 25, 200, 2, 10001 );
            AddImageTiled( 20, 45, 200, 2, 10001 );
            AddImageTiled( 20, 70, 200, 2, 10001 );
            AddImageTiled( 20, 95, 200, 2, 10001 );
            AddImageTiled( 20, 120, 200, 2, 10001 );
            AddImageTiled( 20, 145, 200, 2, 10001 );
            AddImageTiled( 20, 170, 200, 2, 10001 );
	    AddImageTiled( 20, 195, 200, 2, 10001 );
	//----------
            AddLabel( 35, 51, 200, "Animals" );
            AddLabel( 35, 76, 200, "Fan Dancer's Dojo" );
            AddLabel( 35, 101, 200, "Monsters" );
            AddLabel( 35, 126, 200, "Vendors" );
            AddLabel( 35, 151, 200, "Yomutso Mines" );
	    AddLabel( 35, 176, 200, "Zento" );

	//Options Tokuno
            AddCheck( 182, 48, 210, 211, true, 101 );
            AddCheck( 182, 73, 210, 211, true, 102 );
            AddCheck( 182, 98, 210, 211, true, 103 );
            AddCheck( 182, 123, 210, 211, true, 104 );
	    AddCheck( 182, 148, 210, 211, true, 105 );
	    AddCheck( 182, 173, 210, 211, true, 106 );

	//Ok, Cancel
// alt era 280
            AddButton( 55, 220, 247, 249, 1, GumpButtonType.Reply, 0 );
            AddButton( 125, 220, 241, 243, 0, GumpButtonType.Reply, 0 );

        }

        public override void OnResponse( NetState state, RelayInfo info ) 
        { 
            Mobile from = state.Mobile; 

            switch( info.ButtonID ) 
            { 
                case 0: // Closed or Cancel
                {
                    return;
                }

                default: 
                { 
                    // Make sure that the OK, button was pressed
                    if( info.ButtonID == 1 )
                    {
                        // Get the array of switches selected
                        ArrayList Selections = new ArrayList( info.Switches );
			string prefix = CommandSystem.Prefix;

                        if( Selections.Contains( 101 ) == true || Selections.Contains( 102 ) == true || Selections.Contains( 103 ) == true || Selections.Contains( 104 ) == true || Selections.Contains( 105 ) == true || Selections.Contains( 106 ) == true )
                        {
				from.Say( "SPAWNING TOKUNO..." );
                        }

                        // Now spawn any selected maps
                        if( Selections.Contains( 101 ) == true )
                        {
                            from.Say( "Spawning Animals..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen tokuno/animals.map", prefix ) );
                        }

                        if( Selections.Contains( 102 ) == true )
                        {
                            from.Say( "Spawning Fan Dancer's Dojo..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen tokuno/fandancersdojo.map", prefix ) );
                        }

                        if( Selections.Contains( 103 ) == true )
                        {
                            from.Say( "Spawning NPCs everywhere..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen tokuno/monsters.map", prefix ) );
                        }

                        if( Selections.Contains( 104 ) == true )
                        {
                            from.Say( "Spawning Vendors..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen tokuno/vendors.map", prefix ) );
                        }

                        if( Selections.Contains( 105 ) == true )
                        {
                            from.Say( "Spawning Yomutso Mines..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen tokuno/yomutsomines.map", prefix ) );
                        }

                        if( Selections.Contains( 106 ) == true )
                        {
                            from.Say( "Spawning the town of Zento..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen tokuno/zento.map", prefix ) );
                        }
                   }

                    from.Say( "Spawn generation completed!" );

                    break;
                } 
            } 
        }
}
}