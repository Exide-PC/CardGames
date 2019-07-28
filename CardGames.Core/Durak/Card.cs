using System;

namespace CardGames.Core.Durak
{
    public class Card
    {
        public enum CardSuit { Spades, Clubs, Diamonds, Hearts }

        public CardSuit Suit { get; }
        public int Value { get; }

        public Card(CardSuit suit, int value)
        {
            this.Suit = suit;
            this.Value = value;
        }

        public override bool Equals(object obj)
        {
            return this == (Card)obj;
        }

        public override int GetHashCode()
        {
            // auto-generated hash function
            var hashCode = -1625629942;
            hashCode = hashCode * -1521134295 + Suit.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        public static bool operator == (Card one, Card another)
        {
            return one.Suit == another.Suit && one.Value == another.Value;
        }

        public static bool operator != (Card one, Card another)
        {
            return !(one == another);
        }
    }
}
