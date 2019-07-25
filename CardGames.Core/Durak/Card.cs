using System;

namespace CardGames.Core.Durak
{
    public class Card
    {
        public enum CardSuit { Spades, Clubs, Diamonds, Hearts }

        public CardSuit Suit { get; set; }
        public int Value { get; set; }

        public Card(CardSuit suit, int value)
        {
            this.Suit = suit;
            this.Value = value;
        }
    }
}
