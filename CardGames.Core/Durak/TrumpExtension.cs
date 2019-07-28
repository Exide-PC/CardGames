using System;
using System.Collections.Generic;
using System.Linq;
using static CardGames.Core.Durak.Card;

namespace CardGames.Core.Durak
{
    public static  class TrumpExtension
    {
        public static Card GetLowestTrump(this IEnumerable<Card> hand, CardSuit trumpSuit)
        {
            return hand
                .Where(c => c.Suit == trumpSuit)
                .OrderBy(c => c.Value)
                .FirstOrDefault();
        }
    }
}
