using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.BugTracker;

namespace Server.BugTracker.Gumps
{
    public class BugTrackerGump : Gump
    {
        public static string Ver = BugController.Version;

        private List<BugEntry> _Entries;
        private int _Page;

        public BugTrackerGump( List<BugEntry> entries, int page )
            : base(0, 0)
        {
            _Page = page;
            _Entries = entries;
            entries.Sort(new Comparison<BugEntry>(Compare));

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false; 
            this.AddPage(0);
            this.AddBackground(21, 27, 745, 519, 9200);
            this.AddHtml(212, 39, 356, 40,
                String.Format("<CENTER>Project X - Bug Tracker v.{0}</CENTER><br><Center>Open Issues - {1}</center>",
                Ver, GetOpenCount(entries)), (bool)false, (bool)false);
            this.AddLabel(187, 96, 254, @"Title");
            this.AddLabel(363, 96, 254, @"Submitter");
            this.AddLabel(459, 96, 254, @"Status");
            this.AddLabel(534, 96, 254, @"Assigned To");
            this.AddLabel(635, 96, 254, @"Last Updated");
          
            int remaining = entries.Count -( _Page * 15 );
            int count = Math.Min(15, remaining);

            bool next = remaining > 15;

            for (int i = 0; i < count; i++)
            {
                int index = (_Page * 15) + i;

                    AddEntry(entries[index], i); 
            }


            if (page > 0)
                this.AddButton(711, 36, 5538, 5537, 59999, GumpButtonType.Reply, 0);
            if (next)
                this.AddButton(733, 36, 5541, 5540, 60000, GumpButtonType.Reply, 0);
        }

        private void AddEntry(BugEntry bugEntry, int index )
        {
            int y = (index * 28) + 118;
            this.AddHtml(29, y, 317, 26, bugEntry.Title, (bool)true, (bool)false); //Description
            this.AddHtml(345, y, 101, 26, bugEntry.Submitter, (bool)true, (bool)false); //Submitter
            this.AddHtml(445, y, 77, 26, Enum.GetName( typeof( BugStatus ), bugEntry.Status ), (bool)true, (bool)false); //Status
            this.AddHtml(521, y, 101, 26, Enum.GetName( typeof( AssignedTo ), bugEntry.AssignedTo ), (bool)true, (bool)false); //Assigned To
            this.AddHtml(621, y, 117, 26, bugEntry.LastUpdatedTime.ToShortDateString() + " " + bugEntry.LastUpdatedTime.ToShortTimeString(), (bool)true, (bool)false); //Last Updated
            this.AddButton(739, y, 208, 209, ((_Page * 15) + index )+ 1, GumpButtonType.Reply, 0);
        }

        public static int Compare(BugEntry one, BugEntry two)
        {
            if ((int)one.Status < (int)two.Status)
                return -1;
            else if ((int)one.Status > (int)two.Status)
                return 1;
            else
                return CompareDate(one, two);
        }

        private static int CompareDate(BugEntry one, BugEntry two)
        {
            if (one.CreationTime > two.CreationTime)
                return -1;
            else if (one.CreationTime < two.CreationTime)
                return 1;
            
            return 0;
        }

        public static int GetOpenCount(List<BugEntry> entries)
        {
            int open = 0;
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Status == BugStatus.New ||
                    entries[i].Status == BugStatus.Confirmed)
                    open++;
            }

            return open;
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            Mobile m = sender.Mobile;

            if (m == null)
                return;

            switch (info.ButtonID)
            {
                case 0: break; //Cancel
                case 60000:
                    {
                        if (_Page < 5)
                            _Page++;

                        m.CloseGump(typeof(BugTrackerGump));
                        m.SendGump(new BugTrackerGump(_Entries, _Page));
                        break;
                    }
                case 59999:
                    {
                        if (_Page > 0)
                            _Page--;

                        m.CloseGump(typeof(BugTrackerGump));
                        m.SendGump(new BugTrackerGump(_Entries, _Page));
                        break;
                    }
                default: //Any Entry
                    {
                        try
                        {
                            int index = info.ButtonID - 1;
                            BugEntry entry = _Entries[index];

                            if (entry == null)
                            {
                                m.SendMessage("Entry was deleted during a world save.");
                                return;
                            }

                            bool access = (m.AccessLevel >= AccessLevel.Seer);

                            m.CloseGump(typeof(BugEntryGump));
                            m.SendGump(new BugEntryGump(entry, access));
                        }
                        catch
                        {
                            Console.WriteLine("Bug Tracker: Invlid index selected.");
                        }
                        break;

                    }
                    
            }
        }
    }
}