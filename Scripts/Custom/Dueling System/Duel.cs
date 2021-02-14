using System;
using System.Collections.Generic;
using System.Text;

using Server;

namespace Server.Dueling
{
    public enum DuelFlags
    {
        RulesSetup = 0x01,
        TeamSetup = 0x02,
        BettingSetup = 0x04,
        Started = 0x08,
        Finished = 0x10
    }

    public class Duel : EventController
    {
        private List<Mobile> _Contestants;
        private List<Team> _Teams;        
        private DuelFlags _DuelFlags;

        public int ContestantCount { get { return _Contestants.Count; } }
        public int TeamCount { get { return _Teams.Count; } }
        public bool Started
        {
            get
            {
                return ((_DuelFlags & DuelFlags.Started) != 0);
            }
        }

        public Duel() : base()
        {
            _Contestants = new List<Mobile>();
            _Teams = new List<Team>();
        }

        public bool Contains(Mobile m)
        {
            return _Contestants.Contains(m);
        }

        public void AddContestant(Mobile m)
        {
            if (!_Contestants.Contains(m))
                _Contestants.Add(m);
        }

        public void RemoveContestant(Mobile m)
        {
            if (_Contestants.Contains(m))
                _Contestants.Remove(m);
        }

        internal void HandleDeath(Mobile m)
        {
            
        }
    }
}
