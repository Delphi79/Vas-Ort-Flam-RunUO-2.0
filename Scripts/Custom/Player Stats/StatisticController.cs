using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Server;

namespace Server.Statistics
{
    public class StatisticController
    {
        private static Dictionary<Serial, StatisticEntry> _StatisticsTable;
        private static string HTMLPath = Path.Combine("C:\\Program Files\\Abyss Web Server\\htdocs", "playerstatistics.html");

        public static void Initialize()
        {
            _StatisticsTable = new Dictionary<Serial, StatisticEntry>();
            EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);

            Load();
        }

        private static void Load()
        {
            if (!File.Exists(Path.Combine(Core.BaseDirectory, "Saves\\Statistics\\statistics.bin")))
                return;

            BinaryFileReader read = new BinaryFileReader(
                new BinaryReader(new FileStream(Path.Combine(Core.BaseDirectory, "Saves\\Statistics\\statistics.bin"),
                FileMode.Open))
                );

            GenericReader reader = read;

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        int count = reader.ReadInt();

                        for (int i = 0; i < count; i++)
                        {
                            Serial serial = (Serial)reader.ReadInt();
                            StatisticEntry entry = new StatisticEntry();
                            entry.Deserialize(reader);

                            _StatisticsTable.Add(serial, entry);
                        }

                        break;
                    }
            }
        }

        static void EventSink_WorldSave(WorldSaveEventArgs e)
        {
            if (!Directory.Exists(Path.Combine(Core.BaseDirectory, "Saves\\Statistics")))
                Directory.CreateDirectory(Path.Combine(Core.BaseDirectory, "Saves\\Statistics"));

            GenericWriter writer = new BinaryFileWriter(new FileStream(Path.Combine(Core.BaseDirectory, "Saves\\Statistics\\statistics.bin"), FileMode.OpenOrCreate), true);

            writer.Write((int)0);//Version

            int count = _StatisticsTable.Count;
            writer.Write((int)count);

            List<Serial> keys = new List<Serial>(_StatisticsTable.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                Serial key = keys[i];
                StatisticEntry entry = _StatisticsTable[key];

                writer.Write((int)key);
                entry.Serialize(writer);
            }

            OutputToHTML();
        }

        private static string[] _PageLayout = new string[] 
            {
                "<html>",
                "<head>",
                "<title>Player Statistics</title>",
                "</head>",
                "<body>",
                "<table width=\"100%\"  border=\"1\" align=\"left\" bgcolor=\"#FFFFFF\">",
                "  <tr>",
                "    <td height=\"80\"><div align=\"center\">Player</div></td>",
                "    <td><div align=\"center\">Kills</div></td>",
                "    <td><div align=\"center\">Players Killed</div></td>",
                "    <td><div align=\"center\">Guild War Kills</div></td>",
                "    <td><div align=\"center\">Faction Kills</div></td>",
                "    <td><div align=\"center\">Faction Guard Kills</div></td>",
                "    <td><div align=\"center\">Monsters Killed</div></td>",
                "    <td><div align=\"center\">Guards Killed</div></td>",
                "    <td><div align=\"center\">Deathmatch Kills</div></td>",
                "    <td><div align=\"center\">Deaths</div></td>",
                "    <td><div align=\"center\">Deaths by Players</div></td>",
                "    <td><div align=\"center\">Deaths by Guild War</div></td>",
                "    <td><div align=\"center\">Deaths by Faction Players</div></td>",
                "    <td><div align=\"center\">Deaths by Faction Guards</div></td>",
                "    <td><div align=\"center\">Deaths by Monsters</div></td>",
                "    <td><div align=\"center\">Deaths by Guards</div></td>",
                "    <td><div align=\"center\">Deaths In Deathmatch</div></td>",
                "  </tr>"
            };

        private static void OutputToHTML()
        {
            using (StreamWriter writer = new StreamWriter(HTMLPath))
            {
                for (int i = 0; i < _PageLayout.Length; i++)
                    writer.WriteLine(_PageLayout[i]);

                List<StatisticEntry> stats = new List<StatisticEntry>( _StatisticsTable.Values );

                for (int i = 0; i < stats.Count; i++)
                {
                    writer.WriteLine("  <tr>");
                    writer.WriteLine("    <td height=\"80\"><div align=\"center\">{0}</div></td>", stats[i].Mobile.Name);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].Kills);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].KillsPlayers);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].KillsGuild);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].KillsFactionPlayer);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].KillsFationGuard);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].KillsMonsters);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].KillsGuards);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].KillsInDeathMatch);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].Deaths);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].DeathsByPlayers);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].DeathsByGuilds);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].DeathsByFactionPlayer);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].DeathsByFactionGuard);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].DeathsByMonsters);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].DeathsByGuards);
                    writer.WriteLine("    <td><div align=\"center\">{0}</div></td>", stats[i].DeathsInDeathMatch);
                    writer.WriteLine("  </tr>" );
                }

                writer.WriteLine("  </tr>");
                writer.WriteLine("</table>");
                writer.WriteLine("</body>");
                writer.WriteLine("</html>");

                writer.Close();
            }
        }

        internal static void HandleDeath(Mobile died, Mobile from)
        {
            if (_StatisticsTable.ContainsKey(died.Serial))
                _StatisticsTable[died.Serial].HandleDeath(from);
            else
            {
                StatisticEntry entry = new StatisticEntry(died);
                entry.HandleDeath(from);

                _StatisticsTable.Add(died.Serial, entry);
            }
        }

        internal static void HandleKill(Mobile killer, Mobile died)
        {
            if (_StatisticsTable.ContainsKey(killer.Serial))
                _StatisticsTable[killer.Serial].HandleKill(died);
            else
            {
                StatisticEntry entry = new StatisticEntry(killer);
                entry.HandleKill(died);

                _StatisticsTable.Add(killer.Serial, entry);
            }
        }
    }
}
