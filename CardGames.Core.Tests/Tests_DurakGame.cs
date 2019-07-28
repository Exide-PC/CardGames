using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Durak;
using NUnit.Framework;
using Moq;
using static CardGames.Core.Durak.Card;

namespace Tests
{
    [TestFixture]
    public class Tests_DurakGame
    {
        [Test]
        public void Debug()
        {
            
        }

        [Test]
        public void Test_Start_TrumpIsLastInDeck()
        {
            DurakGame game = new DurakGame();
            
            game.AddPlayer(0);
            game.AddPlayer(1);
            game.Start();
            
            Assert.That(game.Trump == game.Deck.Last());
        }
    }
}