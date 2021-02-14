using System;
using System.Collections.Generic;
using System.Text;

namespace Server.BugTracker
{
    public class CommentEntry
    {
        private string _Submitter;
        private DateTime _Created;
        private string _Comment;

        public string Submitter { get { return _Submitter; } }
        public DateTime Created { get { return _Created; } }
        public string Comment { get { return _Comment; } }

        public CommentEntry() { }

        public CommentEntry(string submitter, string comment)
        {
            _Submitter = submitter;
            _Created = DateTime.Now;
            _Comment = comment;
        }

        internal void Serilize(GenericWriter writer)
        {
            writer.Write((int)0);

            writer.Write((string)_Submitter);
            writer.Write((DateTime)_Created);
            writer.Write((string)_Comment);
        }

        internal void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        _Submitter = reader.ReadString();
                        _Created = reader.ReadDateTime();
                        _Comment = reader.ReadString();
                        break;
                    }
            }
        }
    }
}
