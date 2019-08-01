using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Durak;
using NUnit.Framework;
using Moq;
using static CardGames.Core.Durak.Card;

namespace Tests
{
    [TestFixture]
    public class Tests_Card
    {
        [Test]
        public void Test_Equals_HandlesNullCorrectly()
        {
            Card c1 = new Card(CardSuit.Clubs, 0);
            Card c2 = new Card(CardSuit.Clubs, 0);
            Card c3 = new Card(CardSuit.Clubs, 2);
            Card c4 = null;
            Card c5 = null;

            Assert.That(c1 == c2);
            Assert.That(c1 != c3);
            Assert.That(c1 != c4);
            Assert.That(c4 != c1);
            Assert.That(c4 == c5);
            Assert.That(c5 == c4);
        }
    }
}