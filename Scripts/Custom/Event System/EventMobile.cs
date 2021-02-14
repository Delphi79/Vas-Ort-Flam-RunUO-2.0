using System;

using Server;

namespace Server.Events
{
	public class EventMobile
	{
		private Mobile m_Mobile;

		private Point3D m_Location;
		private Map m_Map;

		public Mobile Mobile { get { return m_Mobile; } set { m_Mobile = value; } }

		public Point3D Location { get { return m_Location; } set { m_Location = value; } }
		public Map Map { get { return m_Map; } set { m_Map = value; } }
	}
}