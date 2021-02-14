using System;
using System.IO;
using System.Text;
using System.Collections;

using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targets;
using Server.Targeting;
using Server.Network;
using Server.Gumps;
using Server.Factions;
using Server.Custom.PvpToolkit;
using Server.Custom.PvpToolkit.Gumps;
using Server.Custom.PvpToolkit.Items;

namespace Server.Custom.PvpToolkit.DMatch.Items
{
    public class DMStone : BasePvpStone
    {
        #region Variables

        private int m_MatchMin;
        private bool m_Started;
        private bool m_EndlessMatches;
        private bool m_Active; 
        private bool m_AcceptingContestants;
        private Point3D m_LeaveLocation;
        private Map m_LeaveMap;
        private MatchTimer m_MatchTimer;
        private ArrayList m_Contestants;
        private ArrayList m_DMSpawnPoints;
        private Hashtable m_ScoreTable;
        private Hashtable m_MountCollection;

        public ArrayList MoveList;

        public ArrayList Contestants { get { return m_Contestants; } set { m_Contestants = value; } }
        public ArrayList DMSpawnPoints { get { return m_DMSpawnPoints; } set { m_DMSpawnPoints = value; } }
        public Hashtable ScoreTable { get { return m_ScoreTable; } set { m_ScoreTable = value; } }        
        public bool Active { get { return m_Active; } set { m_Active = value; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public int MatchMinutes { get { return m_MatchMin; } set { m_MatchMin = value; } } 

        [CommandProperty( AccessLevel.GameMaster )]
        public Map LeaveMap { get { return m_LeaveMap; } set { m_LeaveMap = value; } } 

        [CommandProperty( AccessLevel.GameMaster )]
        public Point3D LeaveLocation { get { return m_LeaveLocation; } set { m_LeaveLocation = value; } } 

        [CommandProperty( AccessLevel.GameMaster )]
        public bool AcceptingContestants { get { return m_AcceptingContestants; } set { m_AcceptingContestants = value; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool EndlessMatches { get { return m_EndlessMatches; } set { m_EndlessMatches = value; } }

        public Hashtable MountCollection { get { return m_MountCollection; } set { m_MountCollection = value; } }

        [CommandProperty( AccessLevel.GameMaster )]
        public bool Started 
        { 
            get 
            { 
                return m_Started; 
            } 
            set 
            {
                if( m_MatchTimer == null )
                {
                    m_MatchTimer = new MatchTimer( this, m_MatchMin );
                }

                if( DMSpawnPoints.Count < 1 )
                {
                    World.Broadcast( 38, false, "The deathmatch system was not started due to the lack of spawnpoints" );
                    m_AcceptingContestants = false;
                    m_Started = false;
                    return;
                }

                if( value )
                {
                    if( !m_MatchTimer.Running )
                    {
                        BeginTimer();
                        m_AcceptingContestants = true;
                        World.Broadcast( 38, false, "a deathmatch has been started" );
                    }
                }
                else
                {

                    if( m_MatchTimer.Running )
                    {
                        m_MatchTimer.Stop();
                        EndDeathmatch( true );
                        World.Broadcast( 38, false, "a deathmatch has ended" );
                    }
                }                

                m_Started = value;                
            }
        }
        #endregion

        [Constructable]
        public DMStone() : base()
        {            
            Name = "a deathmatch stone";
            m_MatchMin = 60;
            PvpCore.DMStones.Add( this );
            m_Contestants = new ArrayList();
            m_DMSpawnPoints = new ArrayList();
            m_MountCollection = new Hashtable();
            m_ScoreTable = new Hashtable();
            m_AcceptingContestants = false;
            m_Started = false;
            m_LeaveLocation = new Point3D( 1495, 1629, 10 );
            m_LeaveMap = Map.Felucca;
            MoveList = new ArrayList();
        }

        public DMStone( Serial serial ) : base( serial )
        {
            PvpCore.DMStones.Add( this );
            m_Contestants = new ArrayList();
            m_DMSpawnPoints = new ArrayList();
            m_MountCollection = new Hashtable();
            m_ScoreTable = new Hashtable();
            MoveList = new ArrayList();
        }

        public void RemovePlayer( Mobile m )
        {
            m.MoveToWorld( m_LeaveLocation, m_LeaveMap );
            m.Hidden = true;

            if( m_MountCollection.ContainsKey( m.Serial ) )
            {
                BaseCreature bc = (BaseCreature)m_MountCollection[m.Serial];
                if( bc != null )
                {
                    bc.ControlTarget = m;
                    bc.ControlOrder = OrderType.Stay;
                    bc.SetControlMaster( m );
                    bc.SummonMaster = m;
                    bc.IsStabled = false;
                    bc.MoveToWorld( m.Location, m.Map );
                    m.Stabled.Remove( bc );

                    m.Aggressed.Clear();
                    m.Aggressors.Clear();

                    m.Hits = m.HitsMax;
                    m.Stam = m.StamMax;
                    m.Mana = m.ManaMax;

                    m.DamageEntries.Clear();

                    m.Combatant = null;
                }
            }

            if( m_Contestants.Contains( m ) )
                m_Contestants.Remove( m );

            if( m.NetState != null )
                m.SendMessage( 38, "You have left the deathmatch" );

            World.Broadcast( 1174, false, String.Format( "{0} has left a deathmatch", m.Name ) );
        }

        public override void AddPlayer( Mobile m )
        {
            if( m_Started && m_AcceptingContestants )
            {
                if( CheckPlayer( m ) )
                {
                    ReadyPlayer( m );
                    World.Broadcast( 1174, false, String.Format( "{0} has entered a deathmatch", m.Name ) );
                    m.SendMessage( 38, "If you wish to leave at any time please say \"I wish to leave the deathmatch\"." );
                }
                else
                    m.SendMessage( "You do not meet the requirements for this current match" );
            }
            else
                m.SendMessage( "This event is either closed or full, please try again later" );
        }

        private bool CheckPlayer( Mobile m )
        {
            if( CheckSkills( m ) )
                if( CheckClass( m ) )
                    return true;

            return false;
        }

        private bool CheckClass( Mobile m )
        {
            switch( ClassRulesRule )
            {
                case pClassRules.PureMageOnly:
                    {
                        if( m.Skills.Swords.Base > 0 )
                            return false;
                        else if( m.Skills.Macing.Base > 0 )
                              return false;
                        else if( m.Skills.Archery.Base > 0 )
                              return false;
                        else if( m.Skills.Fencing.Base > 0 )
                              return false;

                        return true;
                    }
                case pClassRules.TamerOnly:
                    {
                        if( m.Skills.AnimalLore.Base <= 25 )
                            return false;
                        else if( m.Skills.AnimalTaming.Base <= 25 )
                            return false;

                        return true;
                    }
                case pClassRules.TankMageOnly:
                    {
                        if( m.Skills.Inscribe.Base > 0 )
                            return false;
                        else if( m.Skills.Poisoning.Base > 0 )
                            return false;
                        else if( m.Skills.Anatomy.Base > 0 )
                            return false;
                        else if( m.Skills.ArmsLore.Base > 0 )
                            return false;
                        else if( m.Skills.Healing.Base > 0 )
                            return false;
                        
                        return true;
                    }
                case pClassRules.DexerOnly:
                    {
                        if( m.Skills.EvalInt.Base > 0 )
                            return false;

                        return true;
                    }
                default:
                    {
                        return true;
                    }
            }
        }

        private bool CheckSkills( Mobile m )
        {
            if( m.Skills.Total < MinSkill * 10 )
                return false;
            if( m.Skills.Total > MaxSkill * 10 )
                return false;
            return true;
        }

        private void ReadyPlayer( Mobile m )
        {
            if( !m_ScoreTable.ContainsKey( m.Serial ) )
                m_ScoreTable.Add( m.Serial, new ScoreKeeper( m ) );

            bool MagicWeapons = MagicWeaponRule == pMagicWeaponRule.Allowed;
            bool MagicArmor = MagicArmorRule == pMagicArmorRule.Allowed;
            bool Potions = PotionRule == pPotionRule.Allowed;
            bool Bandages = BandageRule == pBandaidRule.Allowed;
            bool Pets = PetRule == pPetRule.Allowed;
            bool Mounts = MountRule == pMountRule.Allowed;
            
            if( !m.Alive )
                m.Resurrect();

            Container bp = m.Backpack;
            Container bag = new Bag();
            bag.Hue = 38;
            BankBox bank = m.BankBox;
            Item oncurs = m.Holding;

            if( oncurs != null )
                bp.DropItem( oncurs );

            m.CurePoison( m );

            m.Hits = m.HitsMax;
            m.Mana = m.ManaMax;
            m.Stam = m.StamMax;

            m.StatMods.Clear();

            ArrayList items = new ArrayList();

            foreach( Layer layer in PvpCore.EquipmentLayers )
            {
                Item item = m.FindItemOnLayer( layer );

                if( item != null )
                {
                    if( item is BaseWeapon && !MagicWeapons )
                    {
                        BaseWeapon weapon = ( BaseWeapon )item;

                        if( weapon.AccuracyLevel != WeaponAccuracyLevel.Regular )
                            items.Add( weapon );
                        else if( weapon.DamageLevel != WeaponDamageLevel.Regular )
                            items.Add( weapon );
                        else if( weapon.DurabilityLevel != WeaponDurabilityLevel.Regular )
                            items.Add( weapon );

                    }
                    else if( item is BaseArmor && !MagicArmor )
                    {
                        BaseArmor armor = ( BaseArmor )item;

                        if( armor.Durability != ArmorDurabilityLevel.Regular )
                            items.Add( armor );
                        else if( armor.ProtectionLevel != ArmorProtectionLevel.Regular )
                            items.Add( armor );

                    }
                }
            }

            foreach( Item item in m.Backpack.Items )
            {
                if( item != null )
                {

                    if( item is BaseWeapon && !MagicWeapons )
                    {
                        BaseWeapon weapon = ( BaseWeapon )item;

                        if( weapon.AccuracyLevel != WeaponAccuracyLevel.Regular )
                            items.Add( weapon );
                        else if( weapon.DamageLevel != WeaponDamageLevel.Regular )
                            items.Add( weapon );
                        else if( weapon.DurabilityLevel != WeaponDurabilityLevel.Regular )
                            items.Add( weapon );

                    }
                    else if( item is BaseArmor && !MagicArmor )
                    {
                        BaseArmor armor = ( BaseArmor )item;

                        if( armor.Durability != ArmorDurabilityLevel.Regular )
                            items.Add( armor );
                        else if( armor.ProtectionLevel != ArmorProtectionLevel.Regular )
                            items.Add( armor );
                    }
                    else if( item is BasePotion && !Potions )
                        items.Add( item );
                    else if( item is EtherealMount && !Mounts )
                        items.Add( item );
                    else if( item is Bandage && !Bandages )
                        items.Add( item );
                }
            }

            if( !Mounts )
            {

                if( m.Mount != null )
                {
                    IMount mount = m.Mount;
                    mount.Rider = null;
                    if( mount is BaseMount )
                    {
                        if( mount is BaseCreature )
                        {
                            BaseCreature bc = ( BaseCreature )mount;
                            bc.ControlTarget = null;
                            bc.ControlOrder = OrderType.Stay;
                            bc.Internalize();

                            bc.SetControlMaster( null );
                            bc.SummonMaster = null;

                            bc.IsStabled = true;
                            m.Stabled.Add( bc );
                            MountCollection.Add( m.Serial, bc );
                            m.SendMessage( 38, "Your mount has been moved to the your stables" );
                        }
                    }
                }
            }

            if( items.Count > 0 )
                m.SendMessage( 38, "You had items that did not meet the requirements for the deathmatch and were thus moved to your bank." );
            foreach( Item item in items )
            {
                bag.AddItem( item );
            }

            if( bag.Items.Count > 0 )
                bank.DropItem( bag );
            else
                bag.Delete();

            Contestants.Add( m );
  
            SpawnMobile( m );

            if( m.NetState != null )
            {
                m.SendMessage( 38, "You have joined the deathmatch" );
                m.SendMessage( 38, "You can check the score with \"scoreboard\"" );
            }
            
        }        
               
        public override void OnDoubleClick( Mobile from )
        {            
            base.OnDoubleClick( from );

            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                from.CloseGump( typeof( PvpKitRulesGump ) );
                from.SendGump( new PvpKitRulesGump( this, "Deathmatch", PvpToolkit.PvpCore.DeathmatchVersion ) );
            }
        }

        public override void OnSingleClick( Mobile from )
        {
            LabelTo( from, Name );
            LabelTo( from, Started ? "Active" : "Not Active" );
        }

        public void SpawnMobile( Mobile m )
        {
            if( DMSpawnPoints.Count > 0 )
            {
                int rand = Utility.RandomMinMax( 0, DMSpawnPoints.Count - 1 );

                DMSpawnPoint sp = (DMSpawnPoint)DMSpawnPoints[rand];
                if( sp == null )
                {
                    World.Broadcast( 38, false, "Error: Cannot spawn player {0} to deathmatch {1} when spawnpoints do not exist", m.Serial, this.Serial );
                    RemovePlayer( m );
                }
                else
                {
                    m.MoveToWorld( sp.Location, sp.Map );
                }
            }
            else
            {
                World.Broadcast( 38, false, "Error: Cannot spawn player {0} to deathmatch {1} when spawnpoints do not exist", m.Serial, this.Serial );
                RemovePlayer( m );
            }
        }

        public void HandleDeath( Mobile m )
        {
            HandleCorpse( m );            
            SpawnMobile( m );
            m.Resurrect();
            FixPlayer( m, true );
            FixPlayer( m.LastKiller, false );
            if( m.LastKiller != null )
                m.LastKiller.SendMessage( 38, "You have been rewarded health, mana, and stamina for your kill" );
            UpdateScores( m, m.LastKiller );
        }

        private void FixPlayer( Mobile m, bool removeMods )
        {
            if( m != null )
            {
                m.CurePoison( m );
                m.Hits = m.HitsMax;
                m.Mana = m.ManaMax;
                m.Stam = m.StamMax;
                if( removeMods )
                    m.StatMods.Clear();
            }
        }

        private void UpdateScores( Mobile died, Mobile killer )
        {
            if( died == null || killer == null )
                return;

            if( m_ScoreTable != null && m_ScoreTable.ContainsKey( died.Serial ) )
            {
                ScoreKeeper s = ( ScoreKeeper )m_ScoreTable[died.Serial];

                if( s != null )
                {
                    s.Deaths++;
                }
            }

            if( m_ScoreTable != null && m_ScoreTable.ContainsKey( killer.Serial ) )
            {
                ScoreKeeper s = ( ScoreKeeper )m_ScoreTable[killer.Serial];

                if( s != null )
                {
                    s.Kills++;
                }
            }
        }

        public void HandleCorpse( Mobile from )
        {
            if( from.Corpse != null )
            {
                //Console.WriteLine("{0} corpse was found.", from.Name);
                Corpse c = ( Corpse )from.Corpse;
                c.Open( from, true );
                //for (int i = 0; i < c.Items.Count; i++)
                    //if (c.Items[i] != null && c.Items[i].Map != Map.Internal)
                        //from.Backpack.AddItem(c.Items[i]);

                //c.Delete();
                from.SendMessage( 38, "The contents of your corpse have been safely placed into your backpack" );
            }
        }

        public void ShowScore( Mobile m )
        {
            IDictionaryEnumerator myEnum = m_ScoreTable.GetEnumerator();
            ArrayList list = new ArrayList();

            while( myEnum.MoveNext() )
            {
                ScoreKeeper s = ( ScoreKeeper )myEnum.Value;

                if( s != null )
                    list.Add( s );
            }

            m.SendGump( new DMScoreGump( m, list ) );
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( ( int )1 );
            writer.Write( ( int )m_MatchMin );
            writer.Write( (Point3D)m_LeaveLocation );
            writer.Write( (Map)m_LeaveMap );
            writer.Write((bool)m_Started);
            writer.Write((bool)m_Active);
            writer.Write((bool)m_AcceptingContestants);
            writer.WriteItemList( m_DMSpawnPoints );
            WriteMountCollection( m_MountCollection, writer );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();

            switch( version )
            {
                case 1:
                    {
                        m_MatchMin = reader.ReadInt();
                        m_LeaveLocation = reader.ReadPoint3D();
                        m_LeaveMap = reader.ReadMap();
                        goto case 0;
                    }
                case 0:
                    {
                        Started = reader.ReadBool();
                        m_Active = reader.ReadBool();
                        m_AcceptingContestants = reader.ReadBool();
                        m_DMSpawnPoints = reader.ReadItemList();
                        m_MountCollection = ReadMountCollection( reader );
                        break;
                    }
            }
        }

        public void BeginTimer()
        {
            AnnounceDeathMatch();
            m_ScoreTable = new Hashtable();
            m_MatchTimer = new MatchTimer( this, m_MatchMin );
            m_MatchTimer.Start();
        }

        public void EndDeathmatch( bool worldLoaded )
        {
            m_Started = false;
            m_AcceptingContestants = false;

            if( m_MatchTimer != null )
                m_MatchTimer.Stop();

            ArrayList list = new ArrayList();
            if( Contestants != null && Contestants.Count > 0 )
            {
                foreach( Mobile m in Contestants )
                {
                    list.Add( m );
                }

                foreach( Mobile m in list )
                {
                    RemovePlayer( m );

                }
            }
            
            Contestants = new ArrayList();

            if( m_ScoreTable != null && m_ScoreTable.Count > 0 )
            {
                IDictionaryEnumerator myEnum = m_ScoreTable.GetEnumerator();

                ScoreKeeper sk = new ScoreKeeper();

                int i = 0;
                while( myEnum.MoveNext() )
                {
                    if( i == 0 )
                    {
                        sk = ( ScoreKeeper )myEnum.Value;
                    }
                    else
                    {
                        ScoreKeeper compare = ( ScoreKeeper )myEnum.Value;

                        if( sk != null && compare != null && compare.Kills >= sk.Kills )
                            if( compare.Deaths < sk.Kills )
                                sk = compare;
                    }
                    i++;
                }


                if( sk != null && worldLoaded )
                    World.Broadcast( 53, false, String.Format( "{0} has won the deathmatch!", sk.Player.Name ) );

            }            

            if( EndlessMatches )
                Started = true;
        }

        public void AnnounceDeathMatch()
        {
            World.Broadcast( 38, false, "A deathmatch has begun, you may join by saying \"i wish to join the deathmatch\"" );
        }

        public class MatchTimer : Timer
        {
            private DMStone m_DMStone;
            private int count;

            public MatchTimer( DMStone stone, int mincount )
                : base(TimeSpan.FromMinutes(0), TimeSpan.FromSeconds(1.0))
            {
                m_DMStone = stone;
                count = mincount * 60;
            }

            protected override void OnTick()
            {
                if (count <= 0)
                {
                    m_DMStone.EndDeathmatch( true );
                    Stop();
                }
     
                if (count % 300 == 0)
                {
                    foreach (Mobile m in m_DMStone.Contestants)
                        m.SendMessage(38, String.Format("{0} minutes remaining...", (count / 60)));                    
                }

                count--;
            }
        }       
    }
}