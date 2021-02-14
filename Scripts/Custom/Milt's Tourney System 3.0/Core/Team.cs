using System;
using System.Collections.Generic;

using Server;

namespace Server.TournamentSystem
{
	public class Team
	{
		private List<Fighter> m_Members;

		public List<Fighter> Members { get { return m_Members; } set { m_Members = value; } }

		public Fighter this[int index]
		{
			get { return m_Members[index]; }
			set { m_Members[index] = value; }
		}

		public int Count
		{
			get
			{
				if (m_Members != null)
					return m_Members.Count;

				return 0;
			}
		}
	}
}