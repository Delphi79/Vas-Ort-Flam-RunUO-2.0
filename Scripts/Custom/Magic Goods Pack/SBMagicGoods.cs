using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBMagicGoods : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBMagicGoods()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{  
				Add( new GenericBuyInfo( typeof( FullSpellbook ), 6000, 10, 0xEFA, 0 ) );
				Add( new GenericBuyInfo( typeof( Runebook ), 2500, 10, 0xEFA, 0x461 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}
}
