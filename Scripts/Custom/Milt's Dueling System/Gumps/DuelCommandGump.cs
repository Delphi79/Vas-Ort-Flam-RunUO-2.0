using System;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class DuelCommandGump : Gump
	{
		public DuelCommandGump()
			: base(0, 0)
		{
			this.Closable = true;
			this.Disposable = true;
			this.Dragable = true;
			this.Resizable = false;
			this.AddPage(0);
			this.AddBackground(22, 59, 411, 259, 5120);
			this.AddLabel(43, 72, 1153, @"Milt's Dueling System");
			this.AddImage(242, 62, 105);
			this.AddImage(355, 62, 106);
			this.AddImage(301, 129, 107);
			this.AddImage(273, 202, 108);
			this.AddImage(245, 129, 109);
			this.AddImage(298, 62, 110);
			this.AddImage(353, 129, 111);
			this.AddImage(321, 202, 112);
			this.AddButton(45, 163, 4011, 4012, (int)Buttons.btnStopAll, GumpButtonType.Reply, 0);
			this.AddButton(45, 199, 4011, 4012, (int)Buttons.btnPauseAll, GumpButtonType.Reply, 0);
			this.AddLabel(84, 163, 542, @"Stop all duels");
			this.AddButton(45, 235, 4011, 4012, (int)Buttons.btnResumeAll, GumpButtonType.Reply, 0);
			this.AddLabel(43, 95, 1153, @"-Global Commands/Settings");
			this.AddBackground(358, 295, 101, 48, 5120);
			this.AddButton(376, 306, 4014, 4015, (int)Buttons.btnHome, GumpButtonType.Reply, 0);
			this.AddLabel(412, 308, 0, @"Home");
			this.AddLabel(84, 199, 542, @"Pause all duels");
			this.AddLabel(84, 235, 542, @"Resume all duels");
		}

		public enum Buttons
		{
			btnHome = 1,
			btnStopAll,
			btnPauseAll,
			btnResumeAll
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 0: //Right click to close
				case 1: //Home
					{
						from.CloseGump(typeof(DuelCommandGump));

						from.SendGump(new DuelConfigGump());
						break;
					}
				case 2: //Stop All
				case 3: //Pause All
				case 4: //Resume All
					{
						from.CloseGump(typeof(DuelCommandGump));

						from.SendMessage("This feature has not been implimented.");

						from.SendGump(new DuelCommandGump());
						break;
					}
			}
		}
	}
}