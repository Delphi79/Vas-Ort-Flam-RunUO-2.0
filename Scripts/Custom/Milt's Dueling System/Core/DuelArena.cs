using System;
using System.Collections.Generic;

using Server;

namespace Server.Duels
{
	public class DuelArena : ISerializable
	{
		private bool m_Enabled;

		private Rectangle3D m_SpectatorCoords;
		private Rectangle3D m_DuelCoords;

		private DuelRegion m_DuelRegion;
		private DuelSpectatorRegion m_DuelSpectatorRegion;

		private Point3D m_Home;
		private string m_Name;

		private Map m_Map;

		private Duel m_Duel;

		private static DuelArena m_Instance;

		public bool Enabled { get { return m_Enabled; } set { m_Enabled = value; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public Rectangle3D SpectatorCoords { get { return m_SpectatorCoords; } set { m_SpectatorCoords = value; UpdateRegion(); } }
		[CommandProperty(AccessLevel.GameMaster)]
		public Rectangle3D DuelCoords { get { return m_DuelCoords; } set { m_DuelCoords = value; UpdateRegion(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public Point3D Home { get { return m_Home; } set { m_Home = value; } }

		public string Name { get { return m_Name; } set { m_Name = value; UpdateRegion(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public Map Map { get { return m_Map; } set { m_Map = value; } }

		public Duel Duel { get { return m_Duel; } set { m_Duel = value; } }

		public static DuelArena Instance { get { return m_Instance; } }

		public bool Usable
		{
			get
			{
				if (m_SpectatorCoords.Start != Point3D.Zero     &&
					m_DuelCoords.Start      != Point3D.Zero     &&
					m_Home                  != Point3D.Zero     &&
					m_Enabled									&&
					m_Duel                  == null ) //Arena may not have a duel fighting in it already

					return true;

				return false;
			}
		}

		public DuelArena(string name)
		{
			m_Name = name;
			m_Instance = this;
			m_Map = Map.Felucca;
		}

		public Point3D GetCenter()
		{
			int wMid = m_DuelCoords.Start.X + (m_DuelCoords.Width / 2);
			int hMid = m_DuelCoords.Start.Y + (m_DuelCoords.Height / 2);

			return new Point3D(wMid, hMid, m_DuelCoords.Start.Z);
		}

		public Point3D GetAttackerLocation()
		{
			Point3D center = GetCenter();

			if (m_DuelCoords.Height > m_DuelCoords.Width)
				return new Point3D(center.X, center.Y + 3, center.Z);

			return new Point3D(center.X - 3, center.Y, center.Z);
		}

		public Point3D GetDefenderLocation()
		{
			Point3D center = GetCenter();

			if (m_DuelCoords.Height > m_DuelCoords.Width)
				return new Point3D(center.X, center.Y - 3, center.Z);

			return new Point3D(center.X + 3, center.Y, center.Z);
		}

		public void UpdateRegion()
		{
			if (m_DuelRegion != null)
				m_DuelRegion.Unregister();

			if (m_DuelSpectatorRegion != null)
				m_DuelSpectatorRegion.Unregister();

			CheckRectFix();

			m_DuelRegion = new DuelRegion(this, m_Map, new Rectangle3D[] { m_DuelCoords });
			m_DuelSpectatorRegion = new DuelSpectatorRegion(this, m_Map, new Rectangle3D[] { m_SpectatorCoords });

			m_DuelRegion.Disabled = true;
			m_DuelSpectatorRegion.Disabled = true;

			m_DuelRegion.Register();
			m_DuelSpectatorRegion.Register();
		}

		private void CheckRectFix() //Makes sure rectangles have a depth of at least 1
		{
			if (m_DuelCoords.Start.Z == m_DuelCoords.End.Z)
				m_DuelCoords = new Rectangle3D(m_DuelCoords.Start, new Point3D(m_DuelCoords.End.X, m_DuelCoords.End.Y, m_DuelCoords.Start.Z + 1));
			if (m_SpectatorCoords.Start.Z == m_SpectatorCoords.End.Z)
				m_SpectatorCoords = new Rectangle3D(m_SpectatorCoords.Start, new Point3D(m_SpectatorCoords.End.X, m_SpectatorCoords.End.Y, m_SpectatorCoords.Start.Z + 1));
		}

		public void ChooseArea(Mobile from, bool duel)
		{
			BoundingBoxPicker.Begin(from, new BoundingBoxCallback(TArenaRegion_Callback), duel);
		}

		private static void TArenaRegion_Callback(Mobile from, Map map, Point3D start, Point3D end, object state)
		{
			DoChooseArea(from, map, start, end, (bool)state);
		}

		private static void DoChooseArea(Mobile from, Map map, Point3D start, Point3D end, bool duel)
		{
			if (duel)
				Instance.DuelCoords = new Rectangle3D(start, end);
			else
				Instance.SpectatorCoords = new Rectangle3D(start, end);

			Instance.UpdateRegion();

			from.SendGump(new DuelArenaEditorGump(Instance));
		}

		public void ShowBounds(Rectangle3D r)
		{
			int depth = r.Depth;
			int width = r.Start.X + r.Width + 1;
			int height = r.Start.Y + r.Height + 1;

			do
			{
				for (int x = r.Start.X; x < width; ++x)
					for (int y = r.Start.Y; y < height; ++y)
						if (((x == r.Start.X || x == r.End.X) || (y == r.Start.Y || y == r.End.Y)))
							Effects.SendLocationEffect(new Point3D(x, y, r.Start.Z + depth), m_Map, 437, 75, 1, 1156, 3);

				if (depth != 0)
					--depth;
			}
			while (depth != 0);
		}

		public void Unregister()
		{
			if (m_DuelRegion != null)
				m_DuelRegion.Unregister();
			if (m_DuelSpectatorRegion != null)
				m_DuelSpectatorRegion.Unregister();
		}

		#region ISerializable Members

		public void Serialize(GenericWriter writer)
		{
			writer.Write((int) 0); //version

			writer.Write(m_SpectatorCoords.Start);
			writer.Write(m_SpectatorCoords.End);

			writer.Write(m_DuelCoords.Start);
			writer.Write(m_DuelCoords.End);

			writer.Write(m_Home);
			writer.Write(m_Name);

			writer.Write(m_Map);
			writer.Write(m_Enabled);
		}

		public void Deserialize(GenericReader reader)
		{
			int version = reader.ReadInt();

			m_SpectatorCoords.Start = reader.ReadPoint3D();
			m_SpectatorCoords.End = reader.ReadPoint3D();

			m_DuelCoords.Start = reader.ReadPoint3D();
			m_DuelCoords.End = reader.ReadPoint3D();

			m_Home = reader.ReadPoint3D();
			m_Name = reader.ReadString();

			m_Map = reader.ReadMap();
			m_Enabled = reader.ReadBool();

			m_Instance = this;
		}

		#endregion
	}
}