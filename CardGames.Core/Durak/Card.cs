using System;

namespace CardGames.Core.Durak
{
    public class Card: IEquatable<Card>
    {
        public enum CardSuit { Spades, Clubs, Diamonds, Hearts }

        public CardSuit Suit { get; }
        public int Value { get; }

        public Card(CardSuit suit, int value)
        {
            this.Suit = suit;
            this.Value = value;
        }

        public override string ToString() => $"{this.Value}-{this.Suit}";

        public override int GetHashCode()
        {
            // auto-generated hash function
            var hashCode = -1625629942;
            hashCode = hashCode * -1521134295 + Suit.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return this.Equals((Card)obj);
        }

        public bool Equals(Card other)
        {
            return (object)other != null // here we need a reference check
                ? this.Suit == other.Suit && this.Value == other.Value
                : false;
        }

        public static bool operator == (Card lhs, Card rhs)
        {
            // semantic requirements of the equality operator:
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
            return object.ReferenceEquals(lhs, rhs)
                || !object.ReferenceEquals(lhs, null) && lhs.Equals(rhs);
        }

        public static bool operator != (Card one, Card another)
        {
            return !(one == another);
        }
    }
}
