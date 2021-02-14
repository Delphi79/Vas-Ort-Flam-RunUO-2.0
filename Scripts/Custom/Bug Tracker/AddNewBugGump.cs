using System;
using System.Collections.Generic;
using System.Text;

using Server.Gumps;

namespace Server.BugTracker.Gumps
{
    public class AddNewBugGump : Gump
    {
        public AddNewBugGump() : base( 20, 20 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(12, 63, 493, 277, 9200);
			this.AddLabel(18, 68, 254, @"Please make sure the bug isnt already listed in [viewbugs before you create");
			this.AddLabel(15, 83, 254, @" a new bug entry.");
            this.AddLabel( 20, 109, 254, @"Title" );
            this.AddAlphaRegion( 20, 133, 471, 20 ); 
			this.AddTextEntry(20, 133, 471, 20, 0, (int)Buttons.TextEntry1, @"");
            this.AddLabel( 20, 157, 254, @"Description" );
            this.AddAlphaRegion( 20, 178, 469, 129 );            
			this.AddTextEntry(20, 178, 469, 129, 0, (int)Buttons.TextEntry2, @"");
			this.AddButton(427, 310, 247, 248, (int)Buttons.okBtn, GumpButtonType.Reply, 0);

		}

        public enum Buttons
        {
            TextEntry1 = 1,
            TextEntry2 = 2,
            okBtn = 3
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null)
                return;

            TextRelay[] relays = info.TextEntries;

            if( info.ButtonID == 0 )
                return;

            if( relays[0].Text == "" || relays[1].Text == "" )
            {
                m.SendMessage("You did not fill out al lthe required information.");
                m.SendGump( this );
                return;
            }

            if (info.ButtonID == 3)
            {
                BugController.GlobalList.Add(new BugEntry(m.Name, relays[0].Text, relays[1].Text));
                m.SendMessage("The bug was added");
            }
        }
    }
}
