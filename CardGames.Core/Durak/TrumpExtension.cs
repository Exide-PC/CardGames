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

        public static bool DoesBeat(this Card defender, Card attacker, CardSuit trumpSuit)
        {
            if (attacker.Suit != trumpSuit && defender.Suit == trumpSuit)
                return true;
            else if (attacker.Suit == trumpSuit && defender.Suit != trumpSuit)
                return false;
            else if (attacker.Suit == defender.Suit)
                return defender.Value > attacker.Value;
            else
                return false;
        }
    }
}
