using System;

using Server;
using Server.Items;

namespace Server.TournamentSystem
{
	interface IControllable
	{
		bool Start();
		bool Stop();
		bool Pause();
		bool Resume();
	}

	interface ISerializable
	{
		void Serialize(GenericWriter writer);
		void Deserialize(GenericReader reader);
	}
}
