using System;

using Server;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Accounting
{
    public class EmailGump : Gump
    {
        public EmailGump()
            : base( 200, 200 )
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            this.AddPage( 0 );
            this.AddBackground( 86, 61, 391, 121, 9200 );
            this.AddLabel( 236, 68, 53, @"Register Email " );
            this.AddLabel( 95, 93, 53, @"Email Address:" );
            this.AddLabel( 95, 126, 53, @"Confirm Email:" );
            this.AddBackground(196, 91, 273, 26, 9350);
            this.AddBackground(196, 123, 273, 26, 9350);
            this.AddTextEntry( 200, 94, 263, 20, 0, ( int )Buttons.TextEntry1, @"" );
            this.AddTextEntry( 200, 126, 263, 20, 0, ( int )Buttons.TextEntry2, @"" );
            this.AddButton( 219, 154, 247, 248, ( int )Buttons.okBtn, GumpButtonType.Reply, 0 );
            this.AddButton( 288, 154, 242, 241, ( int )Buttons.cancelBtn, GumpButtonType.Reply, 0 );
        }

        public enum Buttons
        {
            TextEntry1 = 1,
            TextEntry2,
            okBtn,
            cancelBtn,
        }

        public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
        {
            Mobile m = sender.Mobile;

            if( m == null )
                return;

            if( ( Buttons )info.ButtonID == Buttons.okBtn )
            {
                string email = info.TextEntries[0].Text;
                string confirm = info.TextEntries[1].Text;

                if( ( email == null || email == "" ) || email.IndexOf( '@' ) == -1 )
                {
                    m.SendMessage( "That email address was not valid, Please try again." );
                    m.CloseGump( typeof( EmailGump ) );
                    m.SendGump( new EmailGump() );
                }
                else if( email != confirm )
                {
                    m.SendMessage( "Your email address did not match the address in the confirm field, Please try again." );
                    m.CloseGump( typeof( EmailGump ) );
                    m.SendGump( new EmailGump() );
                }
                else
                {
                    AccountManager.HandleEmailEntry(m, email);
                }
            }
        }
    }
}