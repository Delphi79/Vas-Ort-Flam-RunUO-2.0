///************************************************///*********************///
/// Script: GlacialStaff.cs                        ///      ,dP""dMb,      ///
/// Revision: 1.1UOR                               ///     d"   dMP"Tb     ///
/// RunUO Version: 2.0 RC1                         ///    (M    YMbodM)    ///
/// Author: XxSp1derxX                             ///    `M, o  )MMMP     ///
/// Modified by: Packer898                         ///      "b,,aMMP"      ///
/// Date: 6/19/2006       TIME: 3:32 PM            ///*********************///
///************************************************************************///
/// Notes: No distro modifications needed. Simply drop into your Custom    ///
/// folder and restart the server.                                         ///
///************************************************************************///
using System;
using Server;
using Server.Misc;
using Server.Spells;
using Server.Network;
using Server.Targeting;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
	[Flags]
	public enum GlacialSpells
	{
		None		= 0x00,
		Freeze		= 0x01,
		IceStrike	= 0x02,
		IceBall		= 0x04
	}
	
	[FlipableAttribute( 0xDF1, 0xDF0 )]
	public class GlacialStaff : BlackStaff, IUsesRemaining
	{
		public override int ArtifactRarity { get{ return 7; } }
		
		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 11; } }
		public override int OldMaxDamage{ get{ return 16; } }
		public override int OldSpeed{ get{ return 30; } }
		public override int InitMinHits{ get{ return 35; } }
		public override int InitMaxHits{ get{ return 40; } }
		
		private GlacialSpells m_GlacialSpells;
		private GlacialSpells m_CurrentSpell;
		private GlacialSpells m_DisplaySpells;
		private int m_UsesRemaining;
		
		public bool ShowUsesRemaining{ get{ return false; } set{} }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int UsesRemaining{ get{ return m_UsesRemaining; } set{ m_UsesRemaining = value; InvalidateProperties(); } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public GlacialSpells CurrentSpell{ get{ return m_CurrentSpell; } set{ m_CurrentSpell = value; } }
		
		[Constructable]
		public GlacialStaff() : this( 30 )
		{
		}
		
		[Constructable]
		public GlacialStaff( int usesremaining ) : base()
		{
			ItemID = 0xDF1;
			Hue = 0x480;
			Weight = 3.0;
			
			UsesRemaining = usesremaining;
			
			m_GlacialSpells = GetRandomSpells();
		}
		
		public bool GetFlag( GlacialSpells flag )
		{
			return ( (m_GlacialSpells & flag) != 0 );
		}
		
		public void SetFlag( GlacialSpells flag, bool value )
		{
			if ( value )
				m_GlacialSpells |= flag;
			else
				m_GlacialSpells &= ~flag;
		}
		
		public GlacialSpells GetRandomSpells()
		{
			return (GlacialSpells)(0x07 & ~( 1 << Utility.Random( 1, 2 ) ));
		}
		
		public override bool HandlesOnSpeech{ get{ return true; } }
		
		public override void OnSpeech( SpeechEventArgs e )
		{
			base.OnSpeech( e );
			
			Mobile from = e.Mobile;
			
			List<Mobile> list = new List<Mobile>();
			
			if ( from == Parent && m_UsesRemaining > 0 )
			{
				if ( GetFlag( GlacialSpells.Freeze ) && e.Speech.ToLower() == "an ex del" ) 
				{
					foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
					{
						if ( m.Player )
						list.Add( m );
					}
					
					foreach ( Mobile m in list )
					{
						m.NetState.Send( new PlaySound( 0xF6, from.Location ) );
					}
					m_DisplaySpells |= (m_CurrentSpell = GlacialSpells.Freeze);
					InvalidateProperties();
				}
				else if ( GetFlag( GlacialSpells.IceStrike ) && e.Speech.ToLower() == "in corp del" ) 
				{
					foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
					{
						if ( m.Player )
						list.Add( m );
					}
					
					foreach ( Mobile m in list )
					{
						m.NetState.Send( new PlaySound( 0xF7, from.Location ) );
					}
					m_DisplaySpells |= (m_CurrentSpell = GlacialSpells.IceStrike);
					InvalidateProperties();
				}
				else if ( GetFlag( GlacialSpells.IceBall ) && e.Speech.ToLower() == "des corp del" ) 
				{
					foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
					{
						if ( m.Player )
						list.Add( m );
					}
					
					foreach ( Mobile m in list )
					{
						m.NetState.Send( new PlaySound( 0xF8, from.Location ) );
					}
					m_DisplaySpells |= (m_CurrentSpell = GlacialSpells.IceBall);
					InvalidateProperties();
				}
			}
		}
		
		public override void OnSingleClick( Mobile from )
		{
			if ( !IdentifiedTo.Contains(from) )
				LabelTo( from, "a magic staff" );
            else if (IdentifiedTo.Contains(from) && m_UsesRemaining == 0 && m_CurrentSpell != GlacialSpells.None)
				LabelTo( from, "a glacial staff" );
            else if (IdentifiedTo.Contains(from) && m_UsesRemaining >= 1 && m_CurrentSpell != GlacialSpells.None)
				LabelTo( from, "a glacial staff with {0} charges", m_UsesRemaining );
            else if (IdentifiedTo.Contains(from) && m_CurrentSpell == GlacialSpells.None)
				LabelTo( from, "a ruined glacial staff" );
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			base.OnDoubleClick( from );
			
			if ( from != Parent )
				from.SendLocalizedMessage( 502641 ); // You must equip this item to use it.
			else if ( m_UsesRemaining <= 0 )
				from.SendLocalizedMessage( 1019073 ); // This item is out of charges.
			else if ( m_CurrentSpell == GlacialSpells.None )
				from.SendMessage( "You do not have a spell set for this staff." ); // You do not have a spell set for this staff.
			else if ( from.Spell != null && from.Spell.IsCasting )
				from.SendLocalizedMessage( 502642 ); // You are already casting a spell.
			else if ( from.Paralyzed || from.Frozen )
				from.SendLocalizedMessage( 502643 ); // You can not cast a spell while frozen.
			else if ( !from.BeginAction( typeof( GlacialStaff ) ) )
				from.SendMessage( "You must rest before using this staff again." ); // You must rest before using this staff again.
			else
			{
				from.SendLocalizedMessage( 502014 );
				from.Target = new SpellTarget( this );
			}
		}
		
		private class SpellTarget : Target
		{
			private GlacialStaff m_Staff;
			
			public SpellTarget( GlacialStaff staff ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Staff = staff;
			}
			
			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				ReleaseIceLock( from );
				base.OnTargetCancel( from, cancelType );
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Staff != null && !m_Staff.Deleted && m_Staff.UsesRemaining > 0 && from == m_Staff.Parent && targeted is Mobile )
				{
					Mobile to = (Mobile)targeted;
					if ( !from.CanSee( to ) || !from.InLOS( to ) )
						from.SendLocalizedMessage( 500237 );
					else if ( from.HarmfulCheck( to ) )
					{
						switch( m_Staff.CurrentSpell )
						{
								case GlacialSpells.Freeze: DoFreeze( from, to ); break;
								case GlacialSpells.IceStrike: DoIceStrike( from, to ); break;
								case GlacialSpells.IceBall: DoIceBall( from, to ); break;
						}
						Timer.DelayCall( TimeSpan.FromSeconds( 10.0 ), new TimerStateCallback( ReleaseIceLock ), from );
						Timer.DelayCall( TimeSpan.FromSeconds( 7.0 ), new TimerStateCallback( ReleaseHueMod ), new object[]{ m_Staff, m_Staff.Hue } );
						m_Staff.Hue = 0x4F0;
						--m_Staff.UsesRemaining;
						return;
					}
				}
				
				ReleaseIceLock( from );
			}
			
			private static void ReleaseIceLock( object state )
			{
				((Mobile)state).EndAction( typeof( GlacialStaff ) );
			}
			
			private static void ReleaseHueMod( object state )
			{
				object[] states = (object[])state;
				((GlacialStaff)states[0]).Hue = (int)states[1];
			}
			
			private static void ReleaseSolidHueOverrideMod( object state )
			{
				object[] states = (object[])state;
				((Mobile)states[0]).SolidHueOverride = (int)states[1];
			}
			
			private void DoFreeze( Mobile from, Mobile to )
			{
				Mobile caster = from;
				caster.RevealingAction();
				SpellHelper.Turn( caster, to );
				SpellHelper.CheckReflect( (int)SpellCircle.Fifth, ref from, ref to );
				
				to.Paralyze( TimeSpan.FromSeconds( 7.0 ) );
				Timer.DelayCall( TimeSpan.FromSeconds( 7.0 ), new TimerStateCallback( ReleaseSolidHueOverrideMod ), new object[]{ to, to.SolidHueOverride } );
				to.SolidHueOverride = 0x4F0;
				
				caster.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1008127, caster.NetState );
				to.PlaySound( 0x204 );
				caster.Animate( 218, 7, 1, true, false, 0 );
				Effects.SendTargetEffect( to, 0x376A, 16 );
			}
			
			private void DoIceStrike( Mobile from, Mobile to )
			{
				Mobile caster = from;
				caster.RevealingAction();
				SpellHelper.Turn( caster, to );
				SpellHelper.CheckReflect( (int)SpellCircle.Sixth, ref from, ref to );
				
				caster.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1008127, caster.NetState );
				caster.PlaySound( 0x208 );
				caster.Animate( 218, 7, 1, true, false, 0 );
				Effects.SendTargetEffect( to, 0x3709, 32, 0x4F0,3 );
				AOS.Damage( to, from, (int)( caster.Mana / 5 + Utility.Random( 8, 27 ) ), 0, 0, 100, 0, 0 );
				caster.Mana = 0;
			}
			
			private void DoIceBall( Mobile from, Mobile to )
			{
				Mobile caster = from;
				caster.RevealingAction();
				SpellHelper.Turn( caster, to );
				SpellHelper.CheckReflect( (int)SpellCircle.Third, ref from, ref to );
				
				caster.PrivateOverheadMessage( MessageType.Regular, 0x3B2, 1008127, caster.NetState );
				caster.PlaySound( 0x15E );
				caster.Animate( 218, 7, 1, true, false, 0 );
				Effects.SendMovingEffect( caster, to, 0x36D4, 7, 0, false, true , 0x4F0 , 3);
				AOS.Damage( to, from, Utility.Random( 10, 6 ), 0, 0, 100, 0, 0 );
			}
		}
		
		public GlacialStaff( Serial serial ) : base( serial )
		{
		}
		
		public override void OnAdded( object parent )
		{
			Mobile from = parent as Mobile;
			if ( from != null && from.AccessLevel < AccessLevel.GameMaster && Utility.RandomBool() )
			{
				AOS.Damage( from, Utility.Random( 5, 11 ), 25, 0, 75, 0, 0 );
				from.PlaySound( 0x10B );
				from.SendMessage( "Your fingers freeze as they grip the staff." ); // Your fingers freeze as they grip the staff.
			}
			
			if ( from != null )
				base.OnAdded( parent );
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.WriteEncodedInt( (int) 0 );
			
			writer.WriteEncodedInt( (int)m_DisplaySpells );
			writer.WriteEncodedInt( (int)m_GlacialSpells );
			writer.Write( m_UsesRemaining );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			
			int version = reader.ReadEncodedInt();
			switch ( version )
			{
				case 0:
					{
						m_DisplaySpells = (GlacialSpells)reader.ReadEncodedInt();
						m_GlacialSpells = (GlacialSpells)reader.ReadEncodedInt();
						m_UsesRemaining = reader.ReadInt();
						break;
					}
			}
		}
	}
}
