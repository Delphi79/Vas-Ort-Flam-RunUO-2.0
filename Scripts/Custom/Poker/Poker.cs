using System;
using System.Collections;

using Server;

namespace Server.Poker
{
	public enum Suit
	{
		None,
		Diamonds,
		Hearts,
		Clubs,
		Spades
	}

	public enum Rank
	{
		None,
		Ace,
		Two,
		Three,
		Four,
		Five,
		Six,
		Seven,
		Eight,
		Nine,
		Ten,
		Jack,
		Queen,
		King
	}

	public enum PokerScore
	{
		None,
		JacksOrBetter,
		TwoPair,
		ThreeOfAKind,
		Straight,
		Flush,
		FullHouse,
		FourOfAKind,
		StraightFlush,
		RoyalFlush
	}

	public class Card : IComparable
	{
		private Suit m_Suit;
		private Rank m_Rank;

		public Suit Suit { get { return m_Suit; } }
		public Rank Rank { get { return m_Rank; } }

		public bool IsJacksOrBetter { get { return ( m_Rank == Rank.Ace || m_Rank >= Rank.Jack ); } }

		public string Name { get { return String.Format( "{0} of {1}", m_Rank, m_Suit ); } }

		public Card( Suit suit, Rank rank )
		{
			m_Suit = suit;
			m_Rank = rank;
		}

		public Card()
			: this( Suit.None, Rank.None )
		{
		}

		#region IComparable Members

		public int CompareTo( object obj )
		{
			if ( obj is Card )
			{
				Card card = (Card)obj;

				if ( m_Rank < card.Rank )
					return -1;

				if ( m_Rank > card.Rank )
					return 1;
			}

			return 0;
		}

		#endregion
	}

	public class Hand
	{
		private Deck m_Deck;
		private Card[] m_Hand;

		public Hand( Deck deck )
		{
			m_Deck = deck;
			m_Hand = new Card[2];
		}

		public void PullCards()
		{
			for ( int i = 0; i < m_Hand.Length; ++i )
				m_Hand[i] = m_Deck.PullCard();
		}

		public void Sort() { Array.Sort( m_Hand ); }

		public Card this[int index] { get { return m_Hand[index]; } }
	}

	public class Deck
	{
		private Card[] m_Deck;
		private int m_Index = 0;

		public Deck()
		{
			Produce();
		}

		private void Produce()
		{
			m_Index = 0; //Ensures
			m_Deck = new Card[52];
			int counter = 0;

			for ( int s = 0; s < Enum.GetValues( typeof( Suit ) ).Length; ++s )
				for ( int r = 0; r < Enum.GetValues( typeof( Rank ) ).Length; ++r )
					if ( s != (int)Suit.None && r != (int)Rank.None )
						m_Deck[++counter] = new Card( (Suit)s, (Rank)r );
		}

		public Card PullCard() { return m_Deck[++m_Index]; }

		public Card PeekCard() { return m_Deck[m_Index]; }

		private void SwapCards( int i, int j )
		{
			Card temp = m_Deck[i];

			m_Deck[i] = m_Deck[j];
			m_Deck[j] = temp;
		}

		public void Shuffle( int count )
		{
			m_Index = 0;

			for ( int i = 0; i < count; ++i )
			{
				for ( int x = 0; x < m_Deck.Length; ++x )
				{
					int r = Utility.Random( m_Deck.Length );
					SwapCards( x, r );
				}
			}
		}
	}
}