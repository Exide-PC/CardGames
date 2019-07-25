using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static CardGames.Core.Durak.Card;

namespace CardGames.Core.Durak
{
    public class Deck : IEnumerable<Card>
    {
        public Stack<Card> Cards { get; set; }

        public int Count => this.Cards.Count;

        public Deck(IEnumerable<Card> cards)
        {
            this.Cards = new Stack<Card>(cards);
        }

        public Card GetNext()
        {
            if (this.Cards.Count == 0)
                throw new InvalidOperationException("The deck is empty");

            return this.Cards.Pop();
        }

        public static Deck Shuffle()
        {
            CardSuit[] suits = 
            {
                CardSuit.Diamonds,
                CardSuit.Hearts,
                CardSuit.Spades,
                CardSuit.Clubs
            };

            Random rnd = new Random();

            IEnumerable<Card> cards = Enumerable.Range(0, 36)
                .Select(n => new Card(suits[n / 9], n % 9))
                .OrderBy(c => rnd.Next());
            
            return new Deck(cards);
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return this.Cards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
