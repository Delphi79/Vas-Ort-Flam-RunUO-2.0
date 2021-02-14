using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using Server;
using Server.Misc;
using Server.Items;
using Server.Gumps;
using Server.Regions;
using Server.Mobiles;
using Server.Commands;

namespace Server.Dueling
{
    public class DuelCore
    {
        private static string _DataPath = Path.Combine(Path.Combine(Core.BaseDirectory, "Saves\\"), Path.Combine("Dueling System\\", "dueling.bin"));

        private static Dictionary<Serial, Duel> _DuelTable;
        public static Dictionary<Serial, Duel> DuelTable { get { return _DuelTable; } set { _DuelTable = value; } }

        public static void Initialize()
        {           
            Notoriety.Handler = new NotorietyHandler(Notoriety_Handler);

            Mobile.AllowBeneficialHandler = new AllowBeneficialHandler(Mobile_AllowBeneficial);
            Mobile.AllowHarmfulHandler = new AllowHarmfulHandler(Mobile_AllowHarmful);

            EventSink.PlayerDeath += new PlayerDeathEventHandler(EventSink_PlayerDeath);
            EventSink.WorldSave += new WorldSaveEventHandler(EventSink_WorldSave);

            LoadData();
        }

        private static void LoadData()
        {
            if (File.Exists(_DataPath))
            {
                BinaryFileReader reader = new BinaryFileReader(new BinaryReader(File.OpenRead(_DataPath)));
                int version = reader.ReadInt();

                switch (version)
                {
                    case 0:
                        {
                            break;
                        }
                }

                reader.Close();
            }

            if (_DuelTable == null)
                _DuelTable = new Dictionary<Serial, Duel>();
        }

        private static bool Mobile_AllowBeneficial(Mobile source, Mobile target)
        {
            if (source == null || target == null)
                return Server.TSystem.TSystemStone.TSystem_AllowBeneficial(source, target);

            Duel fromDuel, targetDuel;
            bool fromInDuel = CheckDuel(source, out fromDuel);
            bool targetInDuel = CheckDuel(target, out targetDuel);

            if (fromInDuel && targetInDuel)
            {
                if (fromDuel == null || targetDuel == null)
                    return Server.TSystem.TSystemStone.TSystem_AllowBeneficial(source, target);

                return (fromDuel == targetDuel);
            }
            else if ((fromInDuel && !targetInDuel) || (targetInDuel && !fromInDuel))
                if (source.Player && target.Player)
                    return false;

            return Server.TSystem.TSystemStone.TSystem_AllowBeneficial(source, target);
        }

        private static bool Mobile_AllowHarmful(Mobile source, Mobile target)
        {
            if (source == null || target == null)
                return Server.TSystem.TSystemStone.TSystem_AllowHarmful(source, target);

            Duel fromDuel, targetDuel;
            bool fromInDuel = CheckDuel(source, out fromDuel);
            bool targetInDuel = CheckDuel(target, out targetDuel);

            if (fromInDuel && targetInDuel)
            {
                if (fromDuel == null || targetDuel == null)
                    return Server.TSystem.TSystemStone.TSystem_AllowHarmful(source, target);

                return (fromDuel == targetDuel);
            }
            else if ((fromInDuel && !targetInDuel) || (targetInDuel && !fromInDuel))
                if (source.Player && target.Player)
                    return false;

            return Server.TSystem.TSystemStone.TSystem_AllowHarmful(source, target);
        }

        private static int Notoriety_Handler(Mobile source, Mobile target)
        {
            if (source == null || target == null)
                return Server.TSystem.TSystemStone.TSystem_Notoriety(source, target);

            Duel fromDuel, targetDuel;
            bool fromInDuel = CheckDuel(source, out fromDuel);
            bool targetInDuel = CheckDuel(target, out targetDuel);

            if (fromInDuel && targetInDuel)
            {
                if (fromDuel == null || targetDuel == null)
                    return Server.TSystem.TSystemStone.TSystem_Notoriety(source, target);

                if (fromDuel == targetDuel)
                {
                    if (fromDuel.Started)
                    {
                        if ((fromDuel.Contains(source) && fromDuel.Contains(target)) || (fromDuel.Contains(source) && fromDuel.Contains(target)))
                            return Notoriety.Ally;
                        else
                            return Notoriety.Enemy;
                    }
                    else
                        return Server.TSystem.TSystemStone.TSystem_Notoriety(source, target);
                }
                else
                    return Notoriety.Invulnerable;
            }
            else if ((fromInDuel && !targetInDuel) || (!fromInDuel && targetInDuel))
            {
                if (!target.Player || !source.Player)
                    return Server.TSystem.TSystemStone.TSystem_Notoriety(source, target);
                else if (!(target.Region is GuardedRegion))
                    return Server.TSystem.TSystemStone.TSystem_Notoriety(source, target);
                else
                {
                    if ((fromInDuel && fromDuel.Started) || (targetInDuel && targetDuel.Started))
                        return Notoriety.Invulnerable;
                    else
                        return Server.TSystem.TSystemStone.TSystem_Notoriety(source, target);
                }
            }
            else
                return Server.TSystem.TSystemStone.TSystem_Notoriety(source, target);
        }

        private static void EventSink_WorldSave(WorldSaveEventArgs e)
        {
            string dir = Path.GetDirectoryName(_DataPath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            BinaryFileWriter writer = null;

            if (File.Exists(_DataPath))
                writer = new BinaryFileWriter(File.OpenWrite(_DataPath), true);
            else
                writer = new BinaryFileWriter(File.Create(_DataPath), true);

            int version = 0;

            writer.Write((int)version);

            writer.Close();
        }

        private static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null)
                return;

            if( _DuelTable.ContainsKey( m.Serial ) )
            {
                Duel duel = _DuelTable[m.Serial];
                if( duel.Started )
                    duel.HandleDeath( m );
            }
        }

        public static bool CheckDuel(Mobile m)
        {
            Duel d;
            return CheckDuel(m, out d);
        }

        public static bool CheckDuel(Mobile m, out Duel duel)
        {
            duel = null;

            if (m == null)
                return false;

            if (_DuelTable.ContainsKey(m.Serial))
                duel = _DuelTable[m.Serial];

            return duel != null;
        }
    }
}
