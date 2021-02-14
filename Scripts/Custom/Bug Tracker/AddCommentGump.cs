using System;
using System.Collections.Generic;
using System.Text;

using Server.Gumps;

namespace Server.BugTracker.Gumps
{
    public class AddCommentGump : Gump
    {
        private BugEntry _Entry;

        public AddCommentGump(BugEntry entry)
            : base(30, 20)
        {
            _Entry = entry;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(6, 22, 423, 171, 9200);
            this.AddAlphaRegion(17, 48, 399, 114);
            this.AddTextEntry(21, 52, 394, 108, 0, 0, @"");
            this.AddButton(383, 166, 4014, 4015, 1, GumpButtonType.Reply, 0);
            this.AddLabel(18, 26, 0, "Please fill out the box below.");
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null)
                return;

            if (info.TextEntries[0].Text == "")
            {
                m.SendMessage("You did not fill out al lthe required information.");
                m.SendGump(this);
                return;
            }

            if (info.ButtonID == 1)
            {
                _Entry.AddComment( m, info.TextEntries[0].Text);
                m.SendMessage("Your comment was added");
            }
        }
    }
}
