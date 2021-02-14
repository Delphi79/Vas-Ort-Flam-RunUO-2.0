using System;
using Server;
using Server.Gumps;

namespace Server.Accounting
{
	public class AccountConfigGump : Gump
	{
		public AccountConfigGump()
			: base( 0, 0 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(42, 53, 209, 89, 9200);
			this.AddButton(56, 72, 4005, 4006, (int)Buttons.emailBtn, GumpButtonType.Reply, 0);
			this.AddLabel(94, 73, 53, @"Change Email Address");
			this.AddButton(56, 101, 4005, 4006, (int)Buttons.passwordBtn, GumpButtonType.Reply, 0);
			this.AddLabel(94, 102, 53, @"Change Password");
		}
		
		public enum Buttons
		{
			emailBtn = 1,
			passwordBtn,
		}

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null)
                return;

            if (info.ButtonID == (int)Buttons.emailBtn)
            {
                if (AccountManager.AuthList.ContainsKey(m.Serial))
                {
                    m.SendMessage("There is already a request registered for this account.  Please finish the request by checking your email.  Or wait till the request expires.");
                    return;
                }

                m.CloseGump(typeof(ChangeRequestGump));
                m.SendGump(new ChangeRequestGump(RequestType.ChangeEmail));
            }
            else if (info.ButtonID == (int)Buttons.passwordBtn)
            {
                if (AccountManager.AuthList.ContainsKey(m.Serial))
                {
                    m.SendMessage("There is already a request registered for this account.  Please finish the request by checking your email.  Or wait till the request expires.");
                    return;
                }

                m.CloseGump(typeof(ChangeRequestGump));
                m.SendGump(new ChangeRequestGump(RequestType.Password));
            }
        }
	}
}