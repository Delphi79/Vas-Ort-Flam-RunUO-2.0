using System;
using System.Collections.Generic;
using System.Text;

namespace Server.BugTracker
{
    public enum AssignedTo
    {
        Jeff,
        Pokey,
        Aragorn,
        Myst,
        Aceybree,
		Quill
    }

    public enum BugStatus
    {
        New,
        Confirmed,
        Fixed,
        WontFix,
        Implemented,
        WontImplement,
        NotABug,
        WorkingAsIntended,
        Closed
    }

    public class BugEntry
    {
        private string _Submitter;
        private DateTime _CreationTime;
        private DateTime _LastUpdatedTime;
        private AssignedTo _AssignedTo;
        private string _Description;
        private string _Title;
        private List<CommentEntry> _Comments;
        private BugStatus _Status;

        public string Submitter { get { return _Submitter; } set { _Submitter = value; } }
        public DateTime CreationTime { get { return _CreationTime; } }
        public DateTime LastUpdatedTime { get { return _LastUpdatedTime; } }
        [CommandProperty( AccessLevel.Seer )]
        public AssignedTo AssignedTo { get { return _AssignedTo; } set { _AssignedTo = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }
        [CommandProperty(AccessLevel.Seer)]
        public string Title { get { return _Title; } set { _Title = value; } }
        public List<CommentEntry> Comments { get { return _Comments; } set { _Comments = value; } }
        [CommandProperty(AccessLevel.Seer)]
        public BugStatus Status { get { return _Status; } set { _Status = value; } }

        public BugEntry() {}

        public BugEntry(string submitter, string title, string description)
        {
            _Submitter = submitter;
            _Title = title;
            _Description = description;

            _CreationTime = DateTime.Now;
            _LastUpdatedTime = _CreationTime;

            _AssignedTo = AssignedTo.Jeff;
            _Status = BugStatus.New;

            _Comments = new List<CommentEntry>();
        }

        public void AddComment(Mobile from, string comment)
        {
            if (from == null)
                return;

            _Comments.Add(new CommentEntry( from.Name, comment ));
            _LastUpdatedTime = DateTime.Now;
        }

        public void Assign(AssignedTo assignie)
        {
            _AssignedTo = assignie;            
            _LastUpdatedTime = DateTime.Now;
        }

        public void UpdateStatus( BugStatus status )
        {
            _Status = status;
        }

        public void Serilize(GenericWriter writer)
        {
            if( _Comments == null )
                _Comments = new List<CommentEntry>();

            writer.Write((int)0);

            writer.Write((string)_Submitter);
            writer.Write((DateTime)_CreationTime);
            writer.Write((DateTime)_LastUpdatedTime);
            writer.Write((int)_AssignedTo);
            writer.Write((string)_Description);
            writer.Write((string)_Title);

            int count = _Comments.Count;

            writer.Write((int)count);

            for (int i = 0; i < count; i++)
            {
                _Comments[i].Serilize(writer);
            }

            writer.Write((int)_Status);
        }

        public void Deserialize(GenericReader reader)
        {
            int version = reader.ReadInt();

            switch(version)
            {
                case 0:
                    {
                        _Submitter = reader.ReadString();
                        _CreationTime = reader.ReadDateTime();
                        _LastUpdatedTime = reader.ReadDateTime();
                        _AssignedTo = (Server.BugTracker.AssignedTo)reader.ReadInt();
                        _Description = reader.ReadString();
                        _Title = reader.ReadString();

                        int count = reader.ReadInt();

                        for (int i = 0; i < count; i++)
                        {
                            CommentEntry e = new CommentEntry();
                            e.Deserialize(reader);

                            if (_Comments == null)
                                _Comments = new List<CommentEntry>();

                            _Comments.Add(e);
                        }

                        _Status = (BugStatus)reader.ReadInt();
                        break;
                    }
        }
        }
    }
}
