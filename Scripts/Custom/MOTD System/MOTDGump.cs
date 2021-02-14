using System;
using System.Collections.Generic;

using Server;
using Server.Gumps;
using Server.Accounting;

namespace Server.MOTD
{
	public class MOTDGump : Gump
	{
        private List<Publish> _Publishes;

        public MOTDGump(List<Publish> publishes)
			: base( 0, 0 )
		{
            _Publishes = publishes;

			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=true;

            for (int i = 0; i < _Publishes.Count; i++)
            {
                AddPage(i+1);
                AddBackground(38, 35, 393, 524, 9380);
                AddLabel(63, 38, 2123, String.Format("Publish {0} Details", _Publishes[i].Name));
                AddLabel(174, 535, 2123, String.Format("Page {0} of {1}", i + 1, _Publishes.Count));
                
                if( i != 0 )
                    AddButton(58, 538, 5603, 5607, (int)Buttons.prevPage, GumpButtonType.Page, i);

                if (i + 1 != _Publishes.Count)
                    AddButton(400, 538, 5601, 5605, (int)Buttons.nextPage, GumpButtonType.Page, i + 2);

                AddHtml(63, 72, 347, 433, "<BASEFONT COLOR=#3f3f3f>"+_Publishes[i].Info, (bool)false, (bool)true);
                AddCheck(63, 507, 210, 211, false, i);
                AddLabel(83, 508, 2123, "Don't show until next publish");
            }
		}
		
		public enum Buttons
		{
			prevPage,
			nextPage,
			checkbox,
		}

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null)
                return;

            int[] switches = info.Switches;

            if (switches.Length > 0)
            {
                int index = switches[0];
                Account acc = (Account)m.Account;

                if (acc != null && acc.GetTag(_Publishes[0].Name) == null)
                    acc.AddTag(_Publishes[0].Name, "true");              
            }
        }
    }
}