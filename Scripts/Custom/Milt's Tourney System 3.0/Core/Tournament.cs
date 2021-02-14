using System;
using System.Collections.Generic;

using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.TournamentSystem
{
	public class Tournament : IControllable, ISerializable
	{
		private State m_State;

		public State State { get { return m_State; } set { m_State = value; } }

		public Tournament()
		{
		}

		#region IControllable Members

		public bool Start()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool Stop()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool Pause()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool Resume()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion

		#region ISerializable Members

		public void Serialize(GenericWriter writer)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Deserialize(GenericReader reader)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}