using System;
using System.Collections.Generic;

using Server;

namespace Server.Events
{
	public class EventParticipants
	{
		private List<Mobile> m_Global;
		private List<Mobile> m_Local;
		private List<Mobile> m_Banned;

		public List<Mobile> Global { get { return m_Global; } set { m_Global = value; } }
		public List<Mobile> Local { get { return m_Local; } set { m_Local = value; } }
		public List<Mobile> Banned { get { return m_Banned; } set { m_Banned = value; } }

		public EventParticipants()
		{
			m_Global = new List<Mobile>();
			m_Local = new List<Mobile>();
			m_Banned = new List<Mobile>();
		}
	}
}