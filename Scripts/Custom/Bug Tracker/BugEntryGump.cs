using System;
using System.Collections.Generic;
using System.Text;

using Server;
using Server.Gumps;

namespace Server.BugTracker.Gumps
{
    public class BugEntryGump : Gump
    {
        private BugEntry _Entry;
        private bool _Access;

        public BugEntryGump( BugEntry entry, bool access )
            : base(0, 0)
        {
            _Entry = entry;
            _Access = access;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage(0);
            this.AddBackground(12, 14, 503, 568, 9200);
            this.AddLabel(25, 26, 254, String.Format("Created By: {0}", entry.Submitter));
            this.AddLabel(25, 46, 254, String.Format("Added on: {0}", entry.CreationTime.ToShortDateString()));
            this.AddLabel(25, 66, 254, String.Format("Last updated on: {0}", entry.LastUpdatedTime.ToShortDateString()));
            this.AddLabel(260, 26, 254, String.Format("Assigned To: {0}", Enum.GetName( typeof( AssignedTo ), entry.AssignedTo )));
            this.AddLabel(260, 46, 254, String.Format("Status: {0}", Enum.GetName( typeof( BugStatus ), entry.Status )));
            this.AddHtml(21, 94, 481, 174, entry.Description, (bool)true, (bool)true);//Description
            this.AddHtml(21, 275, 481, 263, CreateComments( entry ), (bool)true, (bool)true);//Comments
            this.AddLabel(26, 550, 254, @"Add Comment:");
            this.AddButton(116, 550, 4014, 4015, (int)Buttons.addBtn, GumpButtonType.Reply, 0);
            
            if( access )
            {
                this.AddLabel(154, 550, 254, @"Edit Entry:");
                this.AddButton(225, 550, 4014, 4015, (int)Buttons.editBtn, GumpButtonType.Reply, 0);
            }
        }

        public static string CreateComments(BugEntry entry)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( "Comments<br>------------------------------------<br>");

            if( entry.Comments == null )
                entry.Comments = new List<CommentEntry>();

            for (int i = 0; i < entry.Comments.Count; i++)
            {
                CommentEntry c = entry.Comments[i];
                if (c != null)
                    sb.AppendFormat("Comment by: {0} - {1} {2}<br><br>{3}", c.Submitter,
                        c.Created.ToShortDateString(), c.Created.ToShortTimeString(), 
                        c.Comment); 
            }

            return sb.ToString();
        }

        public enum Buttons
        {
            addBtn = 1,
            editBtn = 2,
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null)
                return;

            if (info.ButtonID == 1)
            {
                m.CloseGump(typeof(AddCommentGump));
                m.SendGump(new AddCommentGump(_Entry));
            }
            else if (info.ButtonID == 2)
            {
                m.CloseGump(typeof(PropertiesGump));
                m.SendGump(new PropertiesGump(m, _Entry));
            }
        } 

    }
}