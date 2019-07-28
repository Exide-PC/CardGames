using System;
using System.Linq;
using CardGames.Core.Durak;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests_Deck
    {
        [Test]
        public void Test_Shuffle_CardAreUnique()
        {
            Deck deck = Deck.Shuffle(36);

            Assert.That(deck.All( // check if there's only 1 instance of every card 
                first => deck.Where(
                    second => first.Suit == second.Suit && first.Value == second.Value)
                    .Count() == 1));
        }

        [Test]
        public void Test_Shuffle_FailsWhenCantBeDividedBy4()
        {
            Assert.Throws<InvalidOperationException>(() => Deck.Shuffle(35));
            Assert.Throws<InvalidOperationException>(() => Deck.Shuffle(37));
            Assert.DoesNotThrow(() => Deck.Shuffle(36));
        }
    }
}