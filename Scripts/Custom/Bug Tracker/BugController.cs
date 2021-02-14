using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Server;
using Server.Commands;
using Server.BugTracker.Gumps;

namespace Server.BugTracker
{
    public class BugController
    {
        private static bool Enabled = true;
        public static string Version = "1.00";
        public static List<BugEntry> GlobalList { get { return _GlobalList; } set { _GlobalList = value; } }

        public static void Initialize()
        {
            _GlobalList = new List<BugEntry>();
            EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);
            Load();

            CommandSystem.Register("AddBug", AccessLevel.Player, new CommandEventHandler(OnCommand_AddBug));
            CommandSystem.Register("ViewBugs", AccessLevel.Player, new CommandEventHandler(OnCommand_ViewBugs));
        }

        private static void OnCommand_AddBug(CommandEventArgs e)
        {
            Mobile m = e.Mobile;            

            if (m == null)
                return;

            if (!Enabled)
            {
                m.SendMessage("The bug tracker is currently offline.");
                return;
            }

            m.CloseGump( typeof( AddNewBugGump ) );
            m.SendGump( new AddNewBugGump() );
        }

        private static void OnCommand_ViewBugs(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            if (!Enabled)
            {
                m.SendMessage("The bug tracker is currently offline.");
                return;
            }

            m.CloseGump(typeof(BugTrackerGump));
            m.SendGump( new BugTrackerGump( _GlobalList, 0 ) );
        }

        private static void Load()
        {
            if (!File.Exists(Path.Combine(Core.BaseDirectory, "Saves\\BugTracker\\bugs.bin")))
                return;

            BinaryFileReader read = new BinaryFileReader(
                new BinaryReader( new FileStream( Path.Combine( Core.BaseDirectory, "Saves\\BugTracker\\bugs.bin" ),
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
                            BugEntry b = new BugEntry();
                            b.Deserialize(reader);

                            if (_GlobalList == null)
                                _GlobalList = new List<BugEntry>();

                            _GlobalList.Add(b);
                        }

                        break;
                    }
            }

            read.Close();
        }

        private static void EventSink_WorldSave(WorldSaveEventArgs e)
        {
            if (!Directory.Exists(Path.Combine(Core.BaseDirectory, "Saves\\BugTracker")))
                Directory.CreateDirectory(Path.Combine(Core.BaseDirectory, "Saves\\BugTracker"));

            GenericWriter writer = new BinaryFileWriter(new FileStream(Path.Combine(Core.BaseDirectory, "Saves\\BugTracker\\bugs.bin"), FileMode.OpenOrCreate), true);

            List<int> toDelete = new List<int>();

            for (int i = 0; i < _GlobalList.Count; i++)
                if (_GlobalList[i].Status == BugStatus.Closed)
                    toDelete.Add(i);

            for( int i = 0; i < toDelete.Count; i++ )
                _GlobalList.RemoveAt(toDelete[i]);

            writer.Write((int)0);

            int count = _GlobalList.Count;

            writer.Write((int)count);

            for (int i = 0; i < count; i++)
            {
                _GlobalList[i].Serilize(writer);
            }

            writer.Close();
        }

        private static List<BugEntry> _GlobalList;


    }
}
