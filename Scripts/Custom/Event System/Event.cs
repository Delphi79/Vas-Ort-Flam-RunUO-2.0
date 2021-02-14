using System;
using System.Collections.Generic;

using Server;

namespace Server.Events
{
	public class Event
	{
		private string m_Name;

		private EventParticipants m_Participants;

		private List<Mobile> m_SignedUp;
		private Dictionary<Serial, EventMobile> m_Table;

		private Point3D m_DeathLocation;
		private Map m_DeathMap;

		private Event m_Instance;

		public string Name { get { return m_Name; } set { m_Name = value; } }

		public EventParticipants Participants { get { return m_Participants; } set { m_Participants = value; } }

		public List<Mobile> SignedUp { get { return m_SignedUp; } set { m_SignedUp = value; } }
		public Dictionary<Serial, EventMobile> Table { get { return m_Table; } set { m_Table = value; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D DeathLocation { get { return m_DeathLocation; } set { m_DeathLocation = value; } }
		[CommandProperty( AccessLevel.GameMaster )]
		public Map DeathMap { get { return m_DeathMap; } set { m_DeathMap = value; } }

		public Event Instance { get { return m_Instance; } }

		public static void Initialize()
		{
		}

		public Event( string name )
		{
			m_Name = name;
		}

		public override string ToString()
		{
			if ( m_Name != null )
				return m_Name;

			return base.ToString();
		}

		public virtual bool Contains( Mobile from )
		{
			return m_Participants.Global.Contains( from );
		}

		public virtual bool Start()
		{
			return true;
		}

		public virtual void Kick( Mobile from ) //Kicks the mobile and sends to spectator area
		{

		}

		public virtual void Ban( Mobile from ) //Completley removes the mobile and sends back to location before event
		{
			if ( m_Participants.Local.Contains( from ) )
			{
				//TODO: Ending action (overridable)
				//TODO: Remove from Local
			}
			if ( m_Participants.Global.Contains( from ) )
			{
				//TODO: Ending action (overridable)
				//TODO: Remove from Global
			}
			if ( !m_Participants.Banned.Contains( from ) )
				m_Participants.Banned.Add( from );
		}
	}
}