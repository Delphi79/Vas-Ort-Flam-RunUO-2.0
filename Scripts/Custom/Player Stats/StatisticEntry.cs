using System;
using System.Collections.Generic;
using System.Text;

using Server;
using Server.Mobiles;

namespace Server.Statistics
{
    public class StatisticEntry
    {
        private Mobile _Mobile;

        public Mobile Mobile { get { return _Mobile; } }

        private int _Deaths;//
        private int _DeathsByPlayers;//
        private int _DeathsByGuilds;//
        private int _DeathsByFactionPlayer;//
        private int _DeathsByFactionGuard;//
        private int _DeathsByMonsters;//
        private int _DeathsByGuards;//
        private int _DeathsByKillCommand;
        private int _DeathsInDeathMatch;//

        public int Deaths { get { return _Deaths; } }
        public int DeathsByPlayers { get { return _DeathsByPlayers; } }
        public int DeathsByGuilds { get { return _DeathsByGuilds; } }
        public int DeathsByFactionPlayer { get { return _DeathsByFactionPlayer; } }
        public int DeathsByFactionGuard { get { return _DeathsByFactionGuard; } }
        public int DeathsByMonsters { get { return _DeathsByMonsters; } }
        public int DeathsByGuards { get { return _DeathsByGuards; } }
        public int DeathsInDeathMatch { get { return _DeathsInDeathMatch; } }

        private int _Kills;//
        private int _KillsPlayers;//
        private int _KillsGuild;//
        private int _KillsFactionPlayer;//
        private int _KillsFationGuard;//
        private int _KillsMonsters;//
        private int _KillsGuards;//
        private int _KillsInDeathMatch;//


        public int Kills { get { return _Kills; } }
        public int KillsPlayers { get { return _KillsPlayers; } }
        public int KillsGuild { get { return _KillsGuild; } }
        public int KillsFactionPlayer { get { return _KillsFactionPlayer; } }
        public int KillsFationGuard { get { return _KillsFationGuard; } }
        public int KillsMonsters { get { return _KillsMonsters; } }
        public int KillsGuards { get { return _KillsGuards; } }
        public int KillsInDeathMatch { get { return _KillsInDeathMatch; } }

        private int _DeathMatchesWonFirst;
        private int _DeathMatchesWonSecond;
        private int _DeathMatchesWonThird;

        private int _DuelsWon;
        private int _DuelsLost;

        public StatisticEntry(Mobile m)
        {
            _Mobile = m;
        }

        public StatisticEntry()
        {
            
        }

        public void HandleKill(Mobile m)
        {
            if (m == null)
                return;

            _Kills++;

            if (m is BaseCreature)
                _KillsMonsters++;
            if (m is Factions.BaseFactionGuard)
                _KillsFationGuard++;
            if (m is BaseGuard)
                _KillsGuards++;
            if (m is PlayerMobile)
            {
                _KillsPlayers++;

                PlayerMobile pm = (PlayerMobile)m;

                if (pm != null)
                {
                    if (_Mobile.Guild != null && m.Guild != null &&
                        !Server.Custom.PvpToolkit.PvpCore.IsInDeathmatch(pm))
                    {
                        Server.Guilds.Guild guildOne = (Server.Guilds.Guild)_Mobile.Guild;
                        Server.Guilds.Guild guildTwo = (Server.Guilds.Guild)m.Guild;

                        if (guildOne.IsEnemy(guildTwo) || guildOne.IsWar(guildTwo))
                            _KillsGuild++;
                    }

                    if (Factions.Faction.Find(pm) != null &&
                        Factions.Faction.Find(_Mobile) != null &&
                        !Server.Custom.PvpToolkit.PvpCore.IsInDeathmatch(pm))
                    {
                        if (Factions.Faction.Find(pm) != Factions.Faction.Find(_Mobile))
                            _KillsFactionPlayer++;
                    }
                }

                if (Server.Custom.PvpToolkit.PvpCore.IsInDeathmatch(pm))
                    _KillsInDeathMatch++;
            }
        }

        public void HandleDeath(Mobile m)
        {
            if (m == null)
                return;

            _Deaths++;

            if (m is BaseCreature)
                _DeathsByMonsters++;
            if (m is Factions.BaseFactionGuard)
                _DeathsByFactionGuard++;
            if (m is BaseGuard)
                _DeathsByGuards++;
            if (m is PlayerMobile)
            {
                _DeathsByPlayers++;

                PlayerMobile pm = (PlayerMobile)m;

                if (pm != null)
                {
                    if (_Mobile.Guild != null && m.Guild != null &&
                        !Server.Custom.PvpToolkit.PvpCore.IsInDeathmatch(pm))
                    {
                        Server.Guilds.Guild guildOne = (Server.Guilds.Guild)_Mobile.Guild;
                        Server.Guilds.Guild guildTwo = (Server.Guilds.Guild)m.Guild;

                        if (guildOne.IsEnemy(guildTwo) || guildOne.IsWar(guildTwo))
                            _DeathsByGuilds++;
                    }

                    if (Factions.Faction.Find(pm) != null &&
                        Factions.Faction.Find(_Mobile) != null &&
                        !Server.Custom.PvpToolkit.PvpCore.IsInDeathmatch(pm))
                    {
                        if (Factions.Faction.Find(pm) != Factions.Faction.Find(_Mobile))
                            _DeathsByFactionPlayer++;
                    }
                }

                if (Server.Custom.PvpToolkit.PvpCore.IsInDeathmatch(pm))
                    _DeathsInDeathMatch++;
            }
        }

        internal void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);//version

            writer.Write((Mobile)_Mobile);

            writer.Write((int)_Deaths);
            writer.Write((int)_DeathsByPlayers);
            writer.Write((int)_DeathsByGuilds);
            writer.Write((int)_DeathsByFactionPlayer);
            writer.Write((int)_DeathsByFactionGuard);
            writer.Write((int)_DeathsByMonsters);
            writer.Write((int)_DeathsByGuards);
            writer.Write((int)_DeathsByKillCommand);
            writer.Write((int)_DeathsInDeathMatch);

            writer.Write((int)_Kills);
            writer.Write((int)_KillsPlayers);
            writer.Write((int)_KillsGuild);
            writer.Write((int)_KillsFactionPlayer);
            writer.Write((int)_KillsFationGuard);
            writer.Write((int)_KillsMonsters);
            writer.Write((int)_KillsGuards);
            writer.Write((int)_KillsInDeathMatch);

            writer.Write((int)_DeathMatchesWonFirst);
            writer.Write((int)_DeathMatchesWonSecond);
            writer.Write((int)_DeathMatchesWonThird);

            writer.Write((int)_DuelsWon);
            writer.Write((int)_DuelsLost);
        }

        internal void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        _Mobile = reader.ReadMobile();

                        _Deaths = reader.ReadInt();
                        _DeathsByPlayers = reader.ReadInt();
                        _DeathsByGuilds = reader.ReadInt();
                        _DeathsByFactionPlayer = reader.ReadInt();
                        _DeathsByFactionGuard = reader.ReadInt();
                        _DeathsByMonsters = reader.ReadInt();
                        _DeathsByGuards = reader.ReadInt();
                        _DeathsByKillCommand = reader.ReadInt();
                        _DeathsInDeathMatch = reader.ReadInt();

                        _Kills = reader.ReadInt();
                        _KillsPlayers = reader.ReadInt();
                        _KillsGuild = reader.ReadInt();
                        _KillsFactionPlayer = reader.ReadInt();
                        _KillsFationGuard = reader.ReadInt();
                        _KillsMonsters = reader.ReadInt();
                        _KillsGuards = reader.ReadInt();
                        _KillsInDeathMatch = reader.ReadInt();

                        _DeathMatchesWonFirst = reader.ReadInt();
                        _DeathMatchesWonSecond = reader.ReadInt();
                        _DeathMatchesWonThird = reader.ReadInt();

                        _DuelsWon = reader.ReadInt();
                        _DuelsLost = reader.ReadInt();

                        break;
                    }
            }
        }
    }
}
