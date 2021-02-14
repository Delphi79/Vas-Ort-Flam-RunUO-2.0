using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Dueling
{
    public class Team
    {
        private List<Mobile> _Members;

        public int DeadCount
        {
            get
            {
                int count = 0;

                for (int i = 0; i < _Members.Count; i++)
                    if (!_Members[i].Alive)
                        ++count;

                return count;
            }
        }

        public int AliveCount
        {
            get
            {
                int alive = _Members.Count - DeadCount;
                return alive;
            }
        }

        public List<Mobile> Members { get { return _Members; } }

        public Team( int count )
        {
            _Members = new List<Mobile>(count);
        }

        public void AddMember(Mobile m)
        {
            if (!_Members.Contains(m))
                _Members.Add(m);
        }

        public void Dispose()
        {
            _Members = null;            
        }
    }
}
