using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.Misc;

namespace Server.Duels
{
	interface ISerializable
	{
		void Serialize(GenericWriter writer);
		void Deserialize(GenericReader reader);
	}

	public class DuelCore
	{
		private static bool m_Enabled;
		private static List<DuelArena> m_Arenas;
		private static Dictionary<Serial, DuelInfo> m_Infos;
		private static List<Duel> m_Duels;

		public static bool Enabled { get { return m_Enabled; } set { m_Enabled = value; } }
		public static List<DuelArena> Arenas { get { return m_Arenas; } set { m_Arenas = value; } }
		public static Dictionary<Serial, DuelInfo> Infos { get { return m_Infos; } set { m_Infos = value; } }
		public static List<Duel> Duels { get { return m_Duels; } set { m_Duels = value; } }

		private static readonly string SavePath = Path.Combine(Core.BaseDirectory, "Saves\\Duels\\DuelInfos.bin");

		public static Layer[] EquipmentLayers = new Layer[]
		{
			Layer.Cloak,
			Layer.Bracelet,
			Layer.Ring,
			Layer.Shirt,
			Layer.Pants,
			Layer.InnerLegs,
			Layer.Shoes,
			Layer.Arms,
			Layer.InnerTorso,
			Layer.MiddleTorso,
			Layer.OuterLegs,
			Layer.Neck,
			Layer.Waist,
			Layer.Gloves,
			Layer.OuterTorso,
			Layer.OneHanded,
			Layer.TwoHanded,
			Layer.FacialHair,
			Layer.Hair,
			Layer.Helm
		};

		public static void Initialize()
		{
			Console.Write("Initializing Milt's Dueling System...");

			CommandSystem.Register("DuelConfig", AccessLevel.GameMaster, new CommandEventHandler(DuelConfig_OnCommand));

			EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);
			EventSink.Speech += new SpeechEventHandler(EventSink_Speech);
			EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_PlayerDeath);

			//Notoriety.Handler += new NotorietyHandler(Mobile_Notoriety);
			//Mobile.AllowBeneficialHandler += new AllowBeneficialHandler(Mobile_AllowBeneficial);
			//Mobile.AllowHarmfulHandler += new AllowHarmfulHandler(Mobile_AllowHarmful);

			m_Infos = new Dictionary<Serial, DuelInfo>();
			m_Arenas = new List<DuelArena>();
			m_Duels = new List<Duel>();

			LoadData();

			for (int i = 0; i < m_Arenas.Count; ++i)
				m_Arenas[i].UpdateRegion();

			Console.WriteLine("done.");
		}

		public static void DropInfos()
		{
			IDictionaryEnumerator dict = m_Infos.GetEnumerator();

			List<Serial> keys = new List<Serial>();
			List<DuelInfo> values = new List<DuelInfo>();

			while (dict.MoveNext())
			{
				keys.Add((Serial)dict.Key);
				values.Add((DuelInfo)dict.Value);
			}

			for (int i = 0; i < values.Count; ++i)
			{
				if (values[i].Mobile == null)
					m_Infos.Remove(keys[i]);
			}
		}

		public static void DuelConfig_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendGump(new DuelConfigGump());
		}

		public static void EventSink_WorldSave(WorldSaveEventArgs e)
		{
			if (!Directory.Exists(Path.Combine(Core.BaseDirectory, "Saves\\Duels")))
				Directory.CreateDirectory(Path.Combine(Core.BaseDirectory, "Saves\\Duels"));

			GenericWriter writer = new BinaryFileWriter(SavePath, true);

			try
			{
				writer.Write((int) 0); //version

				List<Serial> keyList = new List<Serial>();
				List<DuelInfo> valueList = new List<DuelInfo>();

				if (m_Infos == null)
					m_Infos = new Dictionary<Serial, DuelInfo>();

				IDictionaryEnumerator myEnum = m_Infos.GetEnumerator();

				while (myEnum.MoveNext())
				{
					keyList.Add((Serial)myEnum.Key);
					valueList.Add((DuelInfo)myEnum.Value);
				}

				writer.Write(keyList.Count);

				for (int i = 0; i < keyList.Count; ++i)
				{
					writer.Write((int) keyList[i]);
					valueList[i].Serialize(writer);
				}

				if (m_Arenas == null)
					m_Arenas = new List<DuelArena>();

				writer.Write(m_Arenas.Count);

				for (int i = 0; i < m_Arenas.Count; ++i)
					m_Arenas[i].Serialize(writer);

				writer.Write(m_Enabled);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				writer.Close();
			}
		}

		public static void EventSink_Speech(SpeechEventArgs e)
		{
			Mobile from = e.Mobile;

			if (e.Speech.ToLower().IndexOf("showladder") != -1)
			{
				if (m_Enabled)
				{
					if (from != null)
					{
						if (m_Infos.ContainsKey(from.Serial))
						{
							DuelInfo info = m_Infos[from.Serial];

							from.NonlocalOverheadMessage(Server.Network.MessageType.Regular, 1161, true, String.Format("I have {0} win{1} and {2} loss{3}.", info.Wins, (info.Wins != 1 ? "s" : ""), info.Losses, (info.Losses != 1 ? "es" : "")));
							from.LocalOverheadMessage(Server.Network.MessageType.Regular, 1161, true, String.Format("I have {0} win{1} and {2} loss{3}.", info.Wins, (info.Wins != 1 ? "s" : ""), info.Losses, (info.Losses != 1 ? "es" : "")));
						}
						else
						{
							from.NonlocalOverheadMessage(Server.Network.MessageType.Regular, 1161, true, String.Format("I have not participated in any duels yet."));
							from.LocalOverheadMessage(Server.Network.MessageType.Regular, 1161, true, String.Format("You have not participated in any duels yet."));
						}
					}
				}
				else
					from.SendMessage("The dueling system has been disabled.");
			}

			if (e.Speech.ToLower().IndexOf("bank") != -1)
			{
				if (FindDuel(from) != null)
				{
					e.Handled = true;
					return;
				}
			}

			if (e.Speech.ToLower().IndexOf("top 10") != -1)
			{
				if (m_Enabled)
				{
					from.CloseGump(typeof(TopTenGump));
					from.SendGump(new TopTenGump(from));
				}
				else
					from.SendMessage("The dueling system has been disabled");
			}

			if (e.Speech.ToLower().IndexOf("i wish to duel") != -1)
			{
				if (m_Enabled)
				{
					if (from == null)
						return;

					if (!CanDuel(from))
						return;

					Duel duel = new Duel(from, null);

					m_Duels.Add(duel);

					duel.State = DuelState.Setup;

					from.CloseGump(typeof(PlayerDuelAcceptGump));
					from.CloseGump(typeof(PlayerDuelGump));
					from.SendGump(new PlayerDuelGump(duel));
				}
				else
					from.SendMessage("The dueling system has been disabled.");
			}
		}

		public static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
		{
			Mobile from = e.Mobile;

			if (!m_Enabled)
				return;

			Duel duel = FindDuel(from);

			if (duel == null)
				return;

			if (duel.State != DuelState.Duel)
				return;

			EndDuel(duel, from);
		}

		/*public static bool Mobile_AllowBeneficial(Mobile from, Mobile target)
		{
			if (m_Enabled && from != target)
			{
				Duel fDuel = FindActiveDuel(from);
				Duel tDuel = FindActiveDuel(target);

				if (fDuel != null || tDuel != null)
					return (fDuel == tDuel);
			}

			return NotorietyHandlers.Mobile_AllowBeneficial(from, target);
		}

		public static bool Mobile_AllowHarmful(Mobile from, Mobile target)
		{
			if (m_Enabled && from != target)
			{
				Duel fDuel = FindActiveDuel(from);
				Duel tDuel = FindActiveDuel(target);

				if (fDuel != null || tDuel != null)
					return (fDuel == tDuel);
			}

			return NotorietyHandlers.Mobile_AllowHarmful(from, target);
		}

		public static int Mobile_Notoriety(Mobile source, Mobile target)
		{
			if (m_Enabled)
			{
				Duel fDuel = FindActiveDuel(source);
				Duel tDuel = FindActiveDuel(target);

				if (fDuel != null || tDuel != null)
				{
					if (fDuel == tDuel)
						return (source == target ? Notoriety.Ally : Notoriety.Enemy);
					else
						return Notoriety.Invulnerable;
				}
			}

			return NotorietyHandlers.MobileNotoriety(source, target);
		}*/

		public static void StopAllDuels()
		{
		}

		public static void PauseAllDuels()
		{
		}

		public static void ResumeAllDuels()
		{
		}

		public static Duel FindDuel(Mobile from)
		{
			for (int i = 0; i < m_Duels.Count; ++i)
			{
				if (m_Duels[i].Attacker == null || m_Duels[i].Defender == null)
					continue;

				if (m_Duels[i].Attacker == from || m_Duels[i].Defender == from)
					return m_Duels[i];
			}

			return null;
		}

		public static Duel FindActiveDuel(Mobile from)
		{
			for (int i = 0; i < m_Duels.Count; ++i)
			{
				if (m_Duels[i].Attacker == null || m_Duels[i].Defender == null)
					continue;

				if (m_Duels[i].Attacker == from || m_Duels[i].Defender == from && m_Duels[i].State == DuelState.Duel)
					return m_Duels[i];
			}

			return null;
		}

		public static DuelInfo GetInfo(Mobile from) //Gets or creates DuelInfo for player
		{
			DuelInfo info;

			if (m_Infos.ContainsKey(from.Serial))
				info = m_Infos[from.Serial];

			else
			{
				info = new DuelInfo(from);
				info.AddLogEntry("DuelInfo created.");

				m_Infos.Add(from.Serial, info);
			}

			return info;
		}

		public static DuelInfo GetInfoNoCreate( Mobile from ) //Gets or creates DuelInfo for player
		{
			DuelInfo info = null;

			if ( m_Infos.ContainsKey( from.Serial ) )
				info = m_Infos[from.Serial];

			return info;
		}

		public static bool CanDuel(Mobile from)
		{
			if (from.Criminal)
			{
				from.SendMessage("You may not take part in a duel while flagged criminal.");
				return false;
			}
			else if (Spells.SpellHelper.CheckCombat(from))
			{
				from.SendMessage("You may not take part in a duel while in combat.");
				return false;
			}
			else if (from.HasGump(typeof(PlayerDuelGump)))
			{
				from.SendMessage("You are already initiating a duel.");
				return false;
			}
			else if (from.HasGump(typeof(PlayerDuelAcceptGump)))
			{
				from.SendMessage("You are already approving a duel.");
				return false;
			}
			else if (FindDuel(from) != null)
			{
				from.SendMessage("You are either already in a duel, or in the process of setting one up.");
				return false;
			}
			else if (from.Target is DuelTarget)
			{
				from.SendMessage("You are already starting a duel.");
				return false;
			}
			else if (Factions.Sigil.ExistsOn(from))
			{
				from.SendMessage("You may not take part in a duel while you have a faction sigil.");
				return false;
			}

			return true;
		}

		public static void PrepareFighter(Mobile from, Duel duel)
		{
			if (!from.Alive)
				from.Resurrect();

			Container pack = from.Backpack;
			BankBox bank = from.BankBox;
			Item holding = from.Holding;

			if (holding != null && pack != null)
				pack.DropItem(holding);

			from.CurePoison(from);
			from.StatMods.Clear();

			from.Hits = from.HitsMax;
			from.Mana = from.ManaMax;
			from.Stam = from.StamMax;

			Targeting.Target.Cancel(from);
			from.MagicDamageAbsorb = 0;
			from.MeleeDamageAbsorb = 0;
			Spells.Second.ProtectionSpell.Registry.Remove(from);
			DefensiveSpell.Nullify(from);
			from.Combatant = null;

			from.Delta(MobileDelta.Noto); //Update notoriety

			List<Item> items = new List<Item>();

			if ( pack != null )
			{
				if ( !duel.MagicWeapons )
				{
					for ( int i = 0; i < EquipmentLayers.Length; ++i )
					{
						Item item = from.FindItemOnLayer( EquipmentLayers[i] );

						if ( item != null && item is BaseWeapon )
						{
							BaseWeapon weapon = (BaseWeapon)item;

							if ( weapon.AccuracyLevel > 0 ||
								weapon.DamageLevel > 0 ||
								weapon.DurabilityLevel > 0 )

								items.Add( item );
						}
					}
				}

				if ( !duel.MagicArmor )
				{
					for ( int i = 0; i < EquipmentLayers.Length; ++i )
					{
						Item item = from.FindItemOnLayer( EquipmentLayers[i] );

						if ( item != null && item is BaseArmor )
						{
							BaseArmor armor = (BaseArmor)item;

							if ( armor.Durability > 0 ||
								armor.ProtectionLevel > 0 )

								items.Add( item );
						}
					}
				}

				for ( int i = 0; i < pack.Items.Count; ++i )
				{
					Item item = pack.Items[i];

					if ( item != null )
					{
						if ( !duel.MagicWeapons && item is BaseWeapon )
						{
							BaseWeapon weapon = (BaseWeapon)item;

							if ( weapon.AccuracyLevel > 0 ||
								weapon.DamageLevel > 0 ||
								weapon.DurabilityLevel > 0 )

								items.Add( item );
						}

						else if ( !duel.MagicArmor && item is BaseArmor )
						{
							BaseArmor armor = (BaseArmor)item;

							if ( armor.Durability > 0 ||
								armor.ProtectionLevel > 0 )

								items.Add( item );
						}
						//else if ( !duel.Potions && item is BasePotion )
							//items.Add( item );
						else if ( !duel.Bandages && item is Bandage )
							items.Add( item );
					}
				}
			}

			if (!duel.Mounts)
			{
				if (from.Mount != null)
				{
					IMount mount = from.Mount;
					mount.Rider = null;

					if (mount is BaseCreature)
						((BaseCreature)mount).ControlOrder = OrderType.Stay;
				}
			}

			if (items.Count > 0)
			{
				foreach (Item item in items)
				{
					bank.DropItem(item);
				}

				from.SendMessage("Some equipment has been moved to your bankbox due to restrictions on this duel.");
			}
		}

		public static void CancelDuel(Duel duel)
		{
			if (m_Duels.Contains(duel))
			{
				Console.WriteLine("Duel removed");
				m_Duels.Remove(duel);
			}
		}

		public static void InitializeDuel(Duel duel)
		{
			if (duel.Attacker == null || duel.Defender == null)
			{
				CancelDuel(duel);
				return;
			}

			List<DuelArena> open = new List<DuelArena>();

			for (int i = 0; i < m_Arenas.Count; ++i)
			{
				if (m_Arenas[i].Usable)
					open.Add(m_Arenas[i]);
			}

			if (open.Count == 0)
			{
				duel.Attacker.SendMessage("There were no available arenas to duel in. Please try again later.");
				duel.Defender.SendMessage("There were no available arenas to duel in. Please try again later.");
				CancelDuel(duel);
				return;
			}

			DuelArena arena = open[Utility.Random(open.Count)]; //Random open arena

			arena.Duel = duel;
			duel.Arena = arena;

			PrepareFighter(duel.Attacker, duel);
			PrepareFighter(duel.Defender, duel);

			GetInfo( duel.Attacker ).Last = duel.Attacker.Location;
			GetInfo( duel.Defender ).Last = duel.Defender.Location;

			Moongate gate = new Moongate( duel.Arena.Home, duel.Arena.Map );
			gate.Dispellable = false;
			gate.Hue = 1015;
			gate.Name = "a spectator gate";
			gate.MoveToWorld( duel.Attacker.Location, Map.Felucca );

			Timer.DelayCall( TimeSpan.FromSeconds( 15.0 ), new TimerStateCallback( delegate( object state )
			{
				if ( gate != null && !gate.Deleted )
					gate.Delete();

			} ), null );

			duel.Attacker.MoveToWorld(arena.GetAttackerLocation(), arena.Map);
			duel.Defender.MoveToWorld(arena.GetDefenderLocation(), arena.Map);

			duel.Attacker.Blessed = true;
			duel.Defender.Blessed = true;

			duel.Attacker.Frozen = true;
			duel.Defender.Frozen = true;

			WallOfStoneEast wall = new WallOfStoneEast();
			wall.MoveToWorld(arena.GetCenter(), arena.Map);

			duel.Attacker.SendMessage("The duel will start in 10 seconds.");
			duel.Defender.SendMessage("The duel will start in 10 seconds.");

			duel.State = DuelState.Duel;

			Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerStateCallback(FinalizeDuel), new object[] { wall, duel });
		}

		public static void FinalizeDuel(object state)
		{
			object[] states = (object[])state;

			Item wall = (Item)states[0];
			Duel duel = (Duel)states[1];

			wall.Delete();

			duel.Attacker.Blessed = false;
			duel.Defender.Blessed = false;

			duel.Attacker.Frozen = false;
			duel.Defender.Frozen = false;
		}

		public static void EndDuel(Duel duel, Mobile loser)
		{
			duel.State = DuelState.End;

			Mobile winner;

			if (loser == duel.Defender)
				winner = duel.Attacker;
			else
				winner = duel.Defender;

			DuelInfo wInfo = GetInfo(winner);
			DuelInfo lInfo = GetInfo(loser);

			wInfo.AddLogEntry(String.Format("Won in a{0}duel against {1}.", (duel.Ranked ? " ranked " : "n unranked "), loser.Name));
			lInfo.AddLogEntry(String.Format("Lost in a{0}duel against {1}.", (duel.Ranked ? " ranked " : "n unranked "), winner.Name));

			if (duel.Ranked)
			{
				++wInfo.Wins;
				++lInfo.Losses;
			}

			if (!loser.Alive)
			{
				loser.Resurrect();

				if (loser.Corpse != null && loser.Corpse is Corpse)
				{
					Corpse c = (Corpse)loser.Corpse;

					for (int i = 0; i < c.Items.Count; ++i)
						c.SetRestoreInfo(c.Items[i], c.Items[i].Location);

					c.Open(loser, true);
					c.Delete();
				}
			}

			Mobile[] mobs = new Mobile[] { winner, loser };

            winner.Delta(MobileDelta.Noto); /*	  Update	*/
            loser.Delta(MobileDelta.Noto);  /*   Notoriety	*/

			foreach (Mobile m in mobs)
			{
				m.CurePoison(m);
				m.StatMods.Clear();
				m.Combatant = null;

				m.Hits = m.HitsMax;
				m.Mana = m.ManaMax;
				m.Stam = m.StamMax;

				m.Location = duel.Arena.Home;

				m.Warmode = false;
				m.Criminal = false;

                m.Aggressed.Clear();
                m.Aggressors.Clear();

                m.Delta(MobileDelta.Noto);
                m.InvalidateProperties();
			}

			winner.Say(String.Format("{0} has won the duel.", winner.Name));
			loser.Say(String.Format("{0} has lost the duel.", loser.Name));

			CancelDuel(duel);

			duel.Arena.Duel = null;
			duel.Arena = null;
		}

		public static void LoadData()
		{
			if (File.Exists(SavePath))
			{
				using (FileStream fs = new FileStream(SavePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					try
					{
						BinaryReader br = new BinaryReader(fs);
						BinaryFileReader reader = new BinaryFileReader(br);

						int version = reader.ReadInt();

						int count = reader.ReadInt();

						for (int i = 0; i < count; ++i)
						{
							Serial serial = reader.ReadInt();
							DuelInfo info = new DuelInfo(null);

							info.Deserialize(reader);

							m_Infos.Add(serial, info);
						}

						count = reader.ReadInt();

						for (int i = 0; i < count; ++i)
						{
							DuelArena arena = new DuelArena("Loading...");
							arena.Deserialize(reader);

							m_Arenas.Add(arena);
						}

						m_Enabled = reader.ReadBool();
					}
					catch(Exception e)
					{
						Console.WriteLine(e);
					}
					finally
					{
						fs.Close();
					}
				}
			}
		}
	}
}