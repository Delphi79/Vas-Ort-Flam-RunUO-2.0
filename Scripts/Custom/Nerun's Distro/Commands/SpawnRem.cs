// By Nerun
// PREMIUM SPAWNER v1.1
// [Spawnrem - Wipes all spawners on the current map

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Net;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Scripts.Commands
{
   public class SpawnRemCommand
   {
      public static void Initialize()
      {
         Register( "SpawnRem", AccessLevel.Administrator, new CommandEventHandler( SpawnRem_OnCommand ) );
      }

      public static void Register( string command, AccessLevel access, CommandEventHandler handler )
      {
         CommandSystem.Register( command, access, handler );
      }

      [Usage( "SpawnRem" )]
      [Description( "Removes all spawners in the current map." )]
      public static void SpawnRem_OnCommand( CommandEventArgs e )
      {
	Mobile from = e.Mobile;
	World.Broadcast( 0x35, true, "Spawns are being removed, please wait." );
	DateTime startTime = DateTime.Now;
	int count = 0;
	ArrayList itemsremove = new ArrayList();

	foreach ( Item itemremove in World.Items.Values )
	{ 
		if ( itemremove is Spawner && itemremove.Map == from.Map && itemremove.Parent == null )
		{
			itemsremove.Add( itemremove );
			count +=1;
		}
	}

	foreach ( Item itemremove2 in itemsremove )
	{
		itemremove2.Delete();
	}

	DateTime endTime = DateTime.Now;
	World.Broadcast( 0x35, true, "{0} spawns have been removed in {1:F1} seconds.", count, (endTime - startTime).TotalSeconds );
      }
   }
}