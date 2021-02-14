using System;
using System.Collections.Generic;

using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Duels
{
	public class TopTenGump : Gump
	{
		public TopTenGump(Mobile from)
			: base( 0, 0 )
		{
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddImageTiled(83, 92, 393, 414, 9354);
			this.AddAlphaRegion(85, 94, 388, 409);
			this.AddImage(394, 105, 9000);
			this.AddLabel(99, 106, 0, @"Project X Top 10 Duelists");
			this.AddLabel(98, 105, 0, @"Project X Top 10 Duelists");
			this.AddLabel(97, 104, 1160, @"Project X Top 10 Duelists");
			this.AddLabel(97, 177, 100, @"Name");
			this.AddLabel(236, 177, 100, @"Wins");
			this.AddLabel(300, 177, 100, @"Losses");

			DuelCore.DropInfos();

			List<DuelInfo> info = new List<DuelInfo>(DuelCore.Infos.Values);

			info.Sort();

			int count = 0;

			for (int i = 0; i < info.Count; ++i)
			{
				if (count > 9)
					break;

				if ( info[i].Mobile.AccessLevel > AccessLevel.Player )
					continue;

				int y = (count + 1) * 30 + 174;

				this.AddLabel(98, y, 63, info[i].Mobile.Name);
				this.AddLabel(236, y, 63, info[i].Wins.ToString());
				this.AddLabel(300, y, 63, info[i].Losses.ToString());

				++count;
			}

			if (from.AccessLevel >= AccessLevel.Administrator)
			{
				this.AddLabel(96, 131, 36, @"Welcome, administrator. Press this button to");
				this.AddLabel(96, 152, 36, @"reset all scores.");
				this.AddButton(205, 157, 2181, 2181, 1, GumpButtonType.Reply, 0);
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 1)
				if (sender.Mobile.AccessLevel >= AccessLevel.Administrator) //Double Check
					DuelCore.Infos.Clear();
		}
	}
}