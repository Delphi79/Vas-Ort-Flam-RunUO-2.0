using System;
using System.Collections.Generic;

using Server;

namespace Server.TournamentSystem
{
	public class Match
	{
		private State m_State;
		private MatchType m_MatchType;

		private Team m_Attackers;
		private Team m_Defenders;

		public State State { get { return m_State; } set { m_State = value; } }
		public MatchType MatchType { get { return m_MatchType; } set { m_MatchType = value; } }

		public Team Attackers { get { return m_Attackers; } set { m_Attackers = value; } }
		public Team Defenders { get { return m_Defenders; } set { m_Defenders = value; } }

		public List<Fighter> Participants
		{
			get
			{
				if (m_MatchType == MatchType.Null)
					return null;

				List<Fighter> members = new List<Fighter>(m_Attackers.Members);
				members.AddRange(m_Defenders.Members);

				return members;
			}
		}
	}
}