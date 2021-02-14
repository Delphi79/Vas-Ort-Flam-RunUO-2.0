using System;
using System.Net;
using Server;
using Server.Accounting;
using Server.Network;
using Server.Targets;
using Server.Gumps;
using Server.Commands;
using Server.Mobiles;

namespace Server.Gumps
{
    public class OnlineClientGump : Gump
    {
        private NetState m_State;

        private void Resend(Mobile to, RelayInfo info)
        {
            TextRelay te = info.GetTextEntry(0);

            to.SendGump(new OnlineClientGump(to, m_State, te == null ? "" : te.Text));
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (m_State == null)
                return;

            Mobile focus = m_State.Mobile;
            Mobile from = state.Mobile;

            if (focus == null)
            {
                from.SendMessage("That character is no longer online.");
                return;
            }
            else if (focus.Deleted)
            {
                from.SendMessage("That character no longer exists.");
                return;
            }
            else if (from != focus && focus.Hidden && from.AccessLevel < focus.AccessLevel)
            {
                from.SendMessage("That character is no longer visible.");
                return;
            }

            switch (info.ButtonID)
            {
                case 1: // Tell
                    {
                        TextRelay text = info.GetTextEntry(0);

                        if (text != null)
                        {
                            Console.WriteLine("{0} tells {1}:{2}", from.Name, focus.Name, text.Text);
                            focus.SendMessage(0x482, "{0} tells you:", from.Name);
                            focus.SendMessage(0x482, text.Text);
                        }

                        from.SendGump(new OnlineClientGump(from, m_State));
                        break;
                    }
            }
        }

        public OnlineClientGump(Mobile from, NetState state)
            : this(from, state, "")
        {
        }

        public OnlineClientGump(Mobile from, NetState state, string initialText)
            : base(30, 20)
        {
            if (state == null)
                return;

            m_State = state;

            Account a = state.Account as Account;
            Mobile m = state.Mobile;

            if (m != null)
            {
                this.Closable = true;
                this.Disposable = true;
                this.Dragable = true;
                this.Resizable = false;
                this.AddPage(0);
                this.AddBackground(6, 22, 423, 171, 9200);
                this.AddAlphaRegion(17, 48, 399, 114);
                this.AddTextEntry(21, 52, 394, 108, 0, 0, @"");
                this.AddButton(383, 166, 4014, 4015, 1, GumpButtonType.Reply, 0);
                this.AddLabel(18, 26, 0, @"Private Message box for " + m_State.Mobile.Name);
            }

        }
    }
}