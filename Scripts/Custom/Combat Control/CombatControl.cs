using System;
using Server;
using Server.Spells;
using Server.Items;
using Server.Commands;
using Server.Gumps;

namespace Server.Gumps
{
	public class CombatSystem
	{
		public static void Initialize()
		{
			CommandSystem.Register("CombatControl", AccessLevel.Administrator, new CommandEventHandler(OnCommand_CombatControl));
		}

		private static void OnCommand_CombatControl(CommandEventArgs args)
		{
			Mobile m = args.Mobile;

			if (m == null)
				return;

			m.CloseGump(typeof(CombatSystemGump));
			m.SendGump(new CombatSystemGump());
		}
	}

	public class CombatSystemGump : Gump
	{
		public CombatSystemGump()
			: base(50, 50)
		{
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(109, 44, 497, 397, 2600);
			this.AddLabel(277, 63, 43, @"Project X Combat System");
			this.AddLabel(276, 63, 43, @"Project X Combat System");
			this.AddLabel(276, 62, 43, @"Project X Combat System");
			this.AddButton(170, 126, 2118, 2117, (int)Buttons.WeaponControl, GumpButtonType.Reply, 0);
			this.AddLabel(195, 123, 95, @"Weapon Control");
			this.AddButton(170, 155, 2118, 2117, (int)Buttons.SpellControl, GumpButtonType.Reply, 0);
			this.AddLabel(195, 153, 95, @"Spell Control");
			this.AddItem(137, 118, 5118);
			this.AddItem(120, 118, 5119);
			this.AddItem(131, 152, 8036);
			this.AddImage(189, 235, 9000);
			this.AddImage(574, -10, 10441);
			this.AddHtml(313, 103, 257, 284, @"This system is based on a simplified dice system.  All damage entries are based on number of dice, number of sides per dice and a bonus value.  A example of this would be 2d5+2.  This means 2 Dice with 5 sides and a bonus of 2.  This means it could roll a minimum of 4 and a maximum of 12.  If you do not enter the information correctly this could cause a potential server crash and/or cause the desired change to not take effect.", (bool)true, (bool)true);

		}

		public enum Buttons
		{
			WeaponControl = 1,
			SpellControl  = 2,
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			Mobile m = sender.Mobile;

			if (m == null)
				return;

			switch (info.ButtonID)
			{
				case (int)Buttons.WeaponControl:
					{

						m.CloseGump(typeof(PropertiesGump));
						m.SendGump(new PropertiesGump(m, Server.Items.WeaponControl.Instance));
						break;
					}
				case (int)Buttons.SpellControl:
					{

						m.CloseGump(typeof(PropertiesGump));
						m.SendGump(new PropertiesGump(m, Server.Spells.SpellController.Instance));
						break;
					}
			}
		}

	}
}