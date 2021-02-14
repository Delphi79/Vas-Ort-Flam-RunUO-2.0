// By Nerun
// Unload gump for Trammel/Felucca/Ilshenar/Malas/Tokuno
// version F
using System;
using System.Collections;
using System.IO;
using Server;
using Server.Mobiles; 
using Server.Items;
using Server.Scripts.Commands;
using Server.Network;
using Server.Commands;
using Server.Gumps;

namespace Server.Scripts.Commands 
{
	public class Unload
	{
		public Unload()
		{
		}

        public static void Initialize() 
        { 
            CommandSystem.Register( "Unload", AccessLevel.Administrator, new CommandEventHandler( UnloadMain_OnCommand ) );
	        CommandSystem.Register( "UnloadMalas", AccessLevel.Administrator, new CommandEventHandler( UnloadMalas_OnCommand ) );
	        CommandSystem.Register( "UnloadIlshenar", AccessLevel.Administrator, new CommandEventHandler( UnloadIlshenar_OnCommand ) );
	        CommandSystem.Register( "UnloadTokuno", AccessLevel.Administrator, new CommandEventHandler( UnloadTokuno_OnCommand ) );
        }

        [Usage( "[Unload" )]
        [Description( "Unload Trammel/Felucca maps with a menu." )] 
        private static void UnloadMain_OnCommand( CommandEventArgs e )
        { 
		e.Mobile.SendGump( new UnloadMainGump( e ) );
	}

        [Usage( "[Unloadmalas" )]
        [Description( "Unload Malas maps with a menu." )] 
        private static void UnloadMalas_OnCommand( CommandEventArgs e )
        { 
		e.Mobile.SendGump( new UnloadMalasGump( e ) );
	}

        [Usage( "[Unloadilshenar" )]
        [Description( "Unload Ilshenar maps with a menu." )] 
        private static void UnloadIlshenar_OnCommand( CommandEventArgs e )
        { 
		e.Mobile.SendGump( new UnloadIlshenarGump( e ) );
	}

        [Usage( "[Unloadtokuno" )]
        [Description( "Unload Tokuno maps with a menu." )] 
        private static void UnloadTokuno_OnCommand( CommandEventArgs e )
        { 
		e.Mobile.SendGump( new UnloadTokunoGump( e ) );
	}

	}
}

namespace Server.Gumps
{

    public class UnloadMainGump : Gump
    {
        private CommandEventArgs m_CommandEventArgs;
        public UnloadMainGump( CommandEventArgs e ) : base( 50,50 )
        {
            m_CommandEventArgs = e;
            Closable = true;
            Dragable = true;

            AddPage(1);

	//fundo cinza
            AddBackground( 0, 0, 295, 310, 5054 );
	//----------
            AddLabel( 90, 2, 200, "PREMIUM UNLOADER" );
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
            AddCheck( 182, 48, 210, 211, false, 101 );
            AddCheck( 182, 73, 210, 211, false, 102 );
            AddCheck( 182, 98, 210, 211, false, 103 );
            AddCheck( 182, 123, 210, 211, false, 104 );
	    AddCheck( 182, 148, 210, 211, false, 105 );
            AddCheck( 182, 173, 210, 211, false, 106 );
            AddCheck( 182, 198, 210, 211, false, 107 );
            AddCheck( 182, 223, 210, 211, false, 108 );
	//Options Felucca
            AddCheck( 235, 48, 210, 211, false, 128 );
            AddCheck( 235, 73, 210, 211, false, 129 );
            AddCheck( 235, 98, 210, 211, false, 130 );
            AddCheck( 235, 123, 210, 211, false, 131 );
	    AddCheck( 235, 148, 210, 211, false, 132 );
            AddCheck( 235, 173, 210, 211, false, 133 );
            AddCheck( 235, 198, 210, 211, false, 134 );
            AddCheck( 235, 223, 210, 211, false, 135 );

            AddLabel( 135, 255, 200, "1/4" );
	    AddButton( 255, 255, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2 );

            AddPage(2);

	//fundo cinza
            AddBackground( 0, 0, 295, 310, 5054 );
	//----------
            AddLabel( 90, 2, 200, "PREMIUM UNLOADER" );
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
            AddCheck( 182, 48, 210, 211, false, 109 );
            AddCheck( 182, 73, 210, 211, false, 110 );
            AddCheck( 182, 98, 210, 211, false, 111 );
            AddLabel( 178, 126, 200, "N/A" );
	    AddCheck( 182, 148, 210, 211, false, 113 );
            AddCheck( 182, 173, 210, 211, false, 114 );
            AddCheck( 182, 198, 210, 211, false, 115 );
            AddCheck( 182, 223, 210, 211, false, 116 );
	//Options Felucca
            AddCheck( 235, 48, 210, 211, false, 136 );
            AddCheck( 235, 73, 210, 211, false, 137 );
            AddCheck( 235, 98, 210, 211, false, 138 );
            AddCheck( 235, 123, 210, 211, false, 139 );
	    AddCheck( 235, 148, 210, 211, false, 140 );
            AddCheck( 235, 173, 210, 211, false, 141 );
            AddCheck( 235, 198, 210, 211, false, 142 );
            AddCheck( 235, 223, 210, 211, false, 143 );

            AddLabel( 135, 255, 200, "2/4" );
	    AddButton( 255, 255, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 3 );
	    AddButton( 10, 255, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 1 );

            AddPage(3);

	//fundo cinza
            AddBackground( 0, 0, 295, 310, 5054 );
	//----------
            AddLabel( 90, 2, 200, "PREMIUM UNLOADER" );
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
            AddCheck( 182, 48, 210, 211, false, 117 );
            AddCheck( 182, 73, 210, 211, false, 118 );
            AddCheck( 182, 98, 210, 211, false, 119 );
            AddCheck( 182, 123, 210, 211, false, 120 );
	    AddCheck( 182, 148, 210, 211, false, 121 );
            AddCheck( 182, 173, 210, 211, false, 122 );
            AddCheck( 182, 198, 210, 211, false, 123 );
            AddCheck( 182, 223, 210, 211, false, 124 );
	//Options Felucca
            AddCheck( 235, 48, 210, 211, false, 144 );
            AddCheck( 235, 73, 210, 211, false, 145 );
            AddCheck( 235, 98, 210, 211, false, 146 );
            AddCheck( 235, 123, 210, 211, false, 147 );
	    AddCheck( 235, 148, 210, 211, false, 148 );
            AddCheck( 235, 173, 210, 211, false, 149 );
            AddCheck( 235, 198, 210, 211, false, 150 );
            AddCheck( 235, 223, 210, 211, false, 151 );

            AddLabel( 135, 255, 200, "3/4" );
	    AddButton( 255, 255, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 4 );
	    AddButton( 10, 255, 0xFAE, 0xFB0, 0, GumpButtonType.Page, 2 );

            AddPage(4);

	//fundo cinza
            AddBackground( 0, 0, 295, 310, 5054 );
	//----------
            AddLabel( 90, 2, 200, "PREMIUM UNLOADER" );
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

            AddLabel( 35, 150, 200, "You are ready to unload the" );
            AddLabel( 35, 165, 200, "selected maps?" );


	//Options Trammel
            AddCheck( 182, 48, 210, 211, false, 125 );
            AddCheck( 182, 73, 210, 211, false, 126 );
            AddCheck( 182, 98, 210, 211, false, 127 );
	//Options Felucca
            AddCheck( 235, 48, 210, 211, false, 152 );
            AddCheck( 235, 73, 210, 211, false, 153 );
            AddCheck( 235, 98, 210, 211, false, 154 );

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

// Trammel -------------------

                        // Now Unloading any selected maps
                        if( Selections.Contains( 101 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading animals.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 10", prefix ) );
                        }

                        if( Selections.Contains( 102 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Camps.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 20", prefix ) );
                        }

                        if( Selections.Contains( 103 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Covetous.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 30", prefix ) );
                        }

                        if( Selections.Contains( 104 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Deceit.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 40", prefix ) );
                        }

                        if( Selections.Contains( 105 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Despise.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 50", prefix ) );
                        }

                        if( Selections.Contains( 106 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Destard.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 60", prefix ) );
                        }

                        if( Selections.Contains( 107 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Escorts.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 70", prefix ) );
                        }

                        if( Selections.Contains( 108 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Fire.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 80", prefix ) );
                        }

                        if( Selections.Contains( 109 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Graveyards.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 90", prefix ) );
                        }

                        if( Selections.Contains( 110 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Hythloth.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 100", prefix ) );
                        }

                        if( Selections.Contains( 111 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Ice.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 110", prefix ) );
                        }

                        if( Selections.Contains( 113 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Lostlands.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 120", prefix ) );
                        }

                        if( Selections.Contains( 114 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Miscellaneous.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 130", prefix ) );
                        }

                        if( Selections.Contains( 115 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Moonglowzoo.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 140", prefix ) );
                        }

                        if( Selections.Contains( 116 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading OrcCaves.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 150", prefix ) );
                        }

                        if( Selections.Contains( 117 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading OSIHeavy.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 160", prefix ) );
                        }

                        if( Selections.Contains( 118 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading OSILight.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 170", prefix ) );
                        }

                        if( Selections.Contains( 119 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading OSIMedium.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 180", prefix ) );
                        }

                        if( Selections.Contains( 120 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Shame.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 190", prefix ) );
                        }

                        if( Selections.Contains( 121 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading SolenHive.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 200", prefix ) );
                        }

                        if( Selections.Contains( 122 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading TerathanKeep.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 210", prefix ) );
                        }

                        if( Selections.Contains( 123 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading TownCriers.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 220", prefix ) );
                        }

                        if( Selections.Contains( 124 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Towns.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 230", prefix ) );
                        }

                        if( Selections.Contains( 125 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading TrinsicPassage.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 240", prefix ) );
                        }

                        if( Selections.Contains( 126 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Vendors.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 250", prefix ) );
                        }

                        if( Selections.Contains( 127 ) == true )
                        {
                            from.Say( "TRAMMEL: Unloading Wrong.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 260", prefix ) );
                        }

// Felucca -------------------

                        // Now Unloading any selected maps
                        if( Selections.Contains( 128 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading animals.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 11", prefix ) );
                        }

                        if( Selections.Contains( 129 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Camps.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 21", prefix ) );
                        }

                        if( Selections.Contains( 130 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Covetous.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 31", prefix ) );
                        }

                        if( Selections.Contains( 131 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Deceit.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 41", prefix ) );
                        }

                        if( Selections.Contains( 132 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Despise.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 51", prefix ) );
                        }

                        if( Selections.Contains( 133 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Destard.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 61", prefix ) );
                        }

                        if( Selections.Contains( 134 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Escorts.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 71", prefix ) );
                        }

                        if( Selections.Contains( 135 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Fire.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 81", prefix ) );
                        }

                        if( Selections.Contains( 136 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Graveyards.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 91", prefix ) );
                        }

                        if( Selections.Contains( 137 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Hythloth.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 101", prefix ) );
                        }

                        if( Selections.Contains( 138 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Ice.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 111", prefix ) );
                        }

                        if( Selections.Contains( 139 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Khaldun.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 116", prefix ) );
                        }

                        if( Selections.Contains( 140 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Lostlands.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 121", prefix ) );
                        }

                        if( Selections.Contains( 141 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Miscellaneous.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 131", prefix ) );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 132", prefix ) );
                        }

                        if( Selections.Contains( 142 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Moonglowzoo.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 141", prefix ) );
                        }

                        if( Selections.Contains( 143 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading OrcCaves.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 151", prefix ) );
                        }

                        if( Selections.Contains( 144 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading OSIHeavy.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 161", prefix ) );
                        }

                        if( Selections.Contains( 145 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading OSILight.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 171", prefix ) );
                        }

                        if( Selections.Contains( 146 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading OSIMedium.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 181", prefix ) );
                        }

                        if( Selections.Contains( 147 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Shame.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 191", prefix ) );
                        }

                        if( Selections.Contains( 148 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading SolenHive.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 201", prefix ) );
                        }

                        if( Selections.Contains( 149 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading TerathanKeep.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 211", prefix ) );
                        }

                        if( Selections.Contains( 150 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading TownCriers.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 221", prefix ) );
                        }

                        if( Selections.Contains( 151 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Towns.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 231", prefix ) );
                        }

                        if( Selections.Contains( 152 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading TrinsicPassage.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 241", prefix ) );
                        }

                        if( Selections.Contains( 153 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Vendors.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 251", prefix ) );
                        }

                        if( Selections.Contains( 154 ) == true )
                        {
                            from.Say( "FELUCCA: Unloading Wrong.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 261", prefix ) );
                        }
                    }

                    from.Say( "Unload completed!" );

                    break;
                } 
            } 
        }
}

    public class UnloadIlshenarGump : Gump
    {
        private CommandEventArgs m_CommandEventArgs;
        public UnloadIlshenarGump( CommandEventArgs e ) : base( 50,50 )
        {
            m_CommandEventArgs = e;
            Closable = true;
            Dragable = true;

            AddPage(1);

	//fundo cinza
            AddBackground( 0, 0, 243, 310, 5054 );
	//----------
            AddLabel( 60, 2, 200, "PREMIUM UNLOADER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 220, 232, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Unload It" );
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
            AddCheck( 182, 48, 210, 211, false, 101 );
            AddCheck( 182, 73, 210, 211, false, 102 );
            AddCheck( 182, 98, 210, 211, false, 103 );
            AddCheck( 182, 123, 210, 211, false, 104 );
	    AddCheck( 182, 148, 210, 211, false, 105 );
            AddCheck( 182, 173, 210, 211, false, 106 );
            AddCheck( 182, 198, 210, 211, false, 107 );
            AddCheck( 182, 223, 210, 211, false, 108 );

            AddLabel( 110, 255, 200, "1/2" );
	    AddButton( 200, 255, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2 );

            AddPage(2);

	//fundo cinza
            AddBackground( 0, 0, 243, 310, 5054 );
	//----------
            AddLabel( 60, 2, 200, "PREMIUM UNLOADER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 220, 232, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Unload It" );
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
            AddCheck( 182, 48, 210, 211, false, 109 );
            AddCheck( 182, 73, 210, 211, false, 110 );
            AddCheck( 182, 98, 210, 211, false, 111 );
            AddCheck( 182, 123, 210, 211, false, 112 );
	    AddCheck( 182, 148, 210, 211, false, 113 );
            AddCheck( 182, 173, 210, 211, false, 114 );
            AddCheck( 182, 198, 210, 211, false, 115 );
            AddCheck( 182, 223, 210, 211, false, 116 );

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

                        // Now unloading any selected maps
                        if( Selections.Contains( 101 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading AncientLair.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 300", prefix ) );
                        }

                        if( Selections.Contains( 102 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Ankh.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 301", prefix ) );
                        }

                        if( Selections.Contains( 103 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Blood.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 302", prefix ) );
                        }

                        if( Selections.Contains( 104 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Exodus.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 303", prefix ) );
                        }

                        if( Selections.Contains( 105 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Mushroom.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 304", prefix ) );
                        }

                        if( Selections.Contains( 106 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Northeast.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 305", prefix ) );
                        }

                        if( Selections.Contains( 107 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Northwest.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 306", prefix ) );
                        }

                        if( Selections.Contains( 108 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading RatmanCave.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 307", prefix ) );
                        }

                        if( Selections.Contains( 109 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Rock.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 308", prefix ) );
                        }

                        if( Selections.Contains( 110 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Sorcerers.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 309", prefix ) );
                        }

                        if( Selections.Contains( 111 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Southeast.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 310", prefix ) );
                        }

                        if( Selections.Contains( 112 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Southwest.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 311", prefix ) );
                        }

                        if( Selections.Contains( 113 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Spectre.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 312", prefix ) );
                        }

                        if( Selections.Contains( 114 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Towns.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 313", prefix ) );
                        }

                        if( Selections.Contains( 115 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Vendors.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 314", prefix ) );
                        }

                        if( Selections.Contains( 116 ) == true )
                        {
                            from.Say( "ILSHENAR: Unloading Wisp.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 315", prefix ) );
                        }
                    }

                    from.Say( "Unload completed!" );

                    break;
                } 
            } 
        }
}

    public class UnloadMalasGump : Gump
    {
        private CommandEventArgs m_CommandEventArgs;
        public UnloadMalasGump( CommandEventArgs e ) : base( 50,50 )
        {
            m_CommandEventArgs = e;
            Closable = true;
            Dragable = true;

            AddPage(1);

	//fundo cinza
//alt era 310
            AddBackground( 0, 0, 243, 250, 5054 );
	//----------
            AddLabel( 60, 2, 200, "PREMIUM UNLOADER" );
	//fundo branco
	//x, y, largura, altura, item
//alt era 232
            AddImageTiled( 10, 20, 220, 158, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Unload It" );
	//colunas
	//x, y, comprimento, ?, item
//alt era 222
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
            AddCheck( 182, 48, 210, 211, false, 101 );
            AddCheck( 182, 73, 210, 211, false, 102 );
            AddCheck( 182, 98, 210, 211, false, 103 );
            AddCheck( 182, 123, 210, 211, false, 104 );
	    AddCheck( 182, 148, 210, 211, false, 105 );

	//Ok, Cancel
//alt era 280
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

                        // Now spawn any selected maps
                        if( Selections.Contains( 101 ) == true )
                        {
                            from.Say( "MALAS: Unloading Doom.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 400", prefix ) );
                        }

                        if( Selections.Contains( 102 ) == true )
                        {
                            from.Say( "MALAS: Unloading North.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 403", prefix ) );
                        }

                        if( Selections.Contains( 103 ) == true )
                        {
                            from.Say( "MALAS: Unloading OrcForts.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 401", prefix ) );
                        }

                        if( Selections.Contains( 104 ) == true )
                        {
                            from.Say( "MALAS: Unloading South.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 404", prefix ) );
                        }

                        if( Selections.Contains( 105 ) == true )
                        {
                            from.Say( "MALAS: Unloading Vendors.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 402", prefix ) );
                        }

		}

                    from.Say( "Unload completed!" );

                    break;
                } 
            } 
        }
}

    public class UnloadTokunoGump : Gump
    {
        private CommandEventArgs m_CommandEventArgs;
        public UnloadTokunoGump( CommandEventArgs e ) : base( 50,50 )
        {
            m_CommandEventArgs = e;
            Closable = true;
            Dragable = true;

            AddPage(1);

	//fundo cinza
//alt era 310
            AddBackground( 0, 0, 243, 250, 5054 );
	//----------
            AddLabel( 60, 2, 200, "PREMIUM UNLOADER" );
	//fundo branco
	//x, y, largura, altura, item
            AddImageTiled( 10, 20, 220, 183, 3004 );
	//----------
            AddLabel( 30, 27, 200, "Map name" );
            AddLabel( 167, 27, 200, "Unload It" );
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

	//Options Malas
            AddCheck( 182, 48, 210, 211, false, 101 );
            AddCheck( 182, 73, 210, 211, false, 102 );
            AddCheck( 182, 98, 210, 211, false, 103 );
            AddCheck( 182, 123, 210, 211, false, 104 );
	    AddCheck( 182, 148, 210, 211, false, 105 );
	    AddCheck( 182, 172, 210, 211, false, 106 );

	//Ok, Cancel
//alt era 280
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

                        // Now spawn any selected maps
                        if( Selections.Contains( 101 ) == true )
                        {
                            from.Say( "TOKUNO: Unloading Animals.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 500", prefix ) );
                        }

                        if( Selections.Contains( 102 ) == true )
                        {
                            from.Say( "TOKUNO: Unloading FanDancersDojo.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 501", prefix ) );
                        }

                        if( Selections.Contains( 103 ) == true )
                        {
                            from.Say( "TOKUNO: Unloading Monsters.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 502", prefix ) );
                        }

                        if( Selections.Contains( 104 ) == true )
                        {
                            from.Say( "TOKUNO: Unloading Vendors.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 503", prefix ) );
                        }

                        if( Selections.Contains( 105 ) == true )
                        {
                            from.Say( "TOKUNO: Unloading YomutsoMines.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 504", prefix ) );
                        }

                        if( Selections.Contains( 106 ) == true )
                        {
                            from.Say( "TOKUNO: Unloading Zento.map..." );
                            CommandSystem.Handle( from, String.Format( "{0}Spawngen unload 505", prefix ) );
                        }

		}

                    from.Say( "Unload completed!" );

                    break;
                } 
            } 
        }
}
}