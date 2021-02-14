using System;
using System.Collections.Generic;

using Server;

namespace Server.TournamentSystem
{
	public class Round
	{
		private List<Match> m_Matches;
		private int m_ID;

		public Match this[int index]
		{
			get { return m_Matches[index]; }
			set { m_Matches[index] = value; }
		}

		public int Count
		{
			get
			{
				if (m_Matches != null)
					return m_Matches.Count;

				return 0;
			}
		}
	}
}