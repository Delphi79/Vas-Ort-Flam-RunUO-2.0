using System;
using System.Collections.Generic;

using Server;

namespace Server.Duels
{
	public class DuelInfo : ISerializable, IComparable
	{
		private Mobile m_Mobile;

		private int m_Wins;
		private int m_Losses;
		private List<string> m_Log;

		private Point3D m_Last;

		public Mobile Mobile { get { return m_Mobile; } set { m_Mobile = value; } }

		public int Wins { get { return m_Wins; } set { m_Wins = value; } }
		public int Losses { get { return m_Losses; } set { m_Losses = value; } }
		public List<string> Log { get { return m_Log; } set { m_Log = value; } }

		public Point3D Last { get { return m_Last; } set { m_Last = value; } }

		public DuelInfo(Mobile from)
		{
			m_Mobile = from;
			m_Log = new List<string>();
		}

		public void AddLogEntry(string entry)
		{
			if (m_Log.Count > 50) //Only 50 entries max, else cut old ones out
				m_Log.RemoveAt(0);

			DateTime created = DateTime.Now;

			m_Log.Insert(0, String.Format("{0}: {1}\n", created.ToString(), entry));
		}

		#region ISerializable Members

		public void Serialize(GenericWriter writer)
		{
			writer.Write((int) 0); //version

			writer.Write(m_Mobile);
			writer.Write(m_Wins);
			writer.Write(m_Losses);

			writer.Write(m_Log.Count);

			for (int i = 0; i < m_Log.Count; ++i)
				writer.Write(m_Log[i]);
		}

		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();

			m_Mobile = reader.ReadMobile();
			m_Wins = reader.ReadInt();
			m_Losses = reader.ReadInt();

			int count = reader.ReadInt();

			for (int i = 0; i < count; ++i)
				m_Log.Add(reader.ReadString());
		}

		#endregion

		#region IComparable Members

		public int CompareTo(object obj)
		{
			DuelInfo info = (DuelInfo)obj;

			if (info.Wins - Wins == 0)
				return info.Losses - Losses;

			return info.Wins - Wins;
		}

		#endregion
	}
}