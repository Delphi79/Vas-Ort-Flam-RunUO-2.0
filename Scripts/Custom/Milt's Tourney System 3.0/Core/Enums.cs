using System;

using Server;

namespace Server.TournamentSystem
{
	public enum State
	{
		Passive,
		Pending,
		Active,
		Complete,
		Paused
	}

	public enum MatchType
	{
		Null,
		Single,
		Multi
	}
}