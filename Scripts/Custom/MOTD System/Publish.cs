using System;
using System.Collections.Generic;
using System.Text;

namespace Server.MOTD
{
    public class Publish
    {
        private string _Name;
        private string _Info;

        public string Name { get { return _Name; } }
        public string Info { get { return _Info; } }

        public Publish(string name, string info)
        {
            _Name = name;
            _Info = info;
        }
    }
}
