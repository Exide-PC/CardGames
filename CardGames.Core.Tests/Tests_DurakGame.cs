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

        [Test]
        public void Test_Foo()
        {
            var players = GetPlayers(
                new Card[]
                {
                    new Card(CardSuit.Clubs, 0),
                    new Card(CardSuit.Clubs, 2)
                },
                new Card[]
                {
                    new Card(CardSuit.Clubs, 1),
                    new Card(CardSuit.Clubs, 3)
                }
            );

            DurakGame game = new DurakGame(deck: null, players, new Card(CardSuit.Clubs, 4));

            Assert.That(false);
        }

        List<Player> GetPlayers(params IEnumerable<Card>[] playerCards)
        {
            List<Player> players = new List<Player>();

            for (int i = 0; i < playerCards.Length; i++)
            {
                players.Add(
                    new Player(i) 
                    { 
                        Hand = playerCards[i].ToList() 
                    });
            }

            return players;
        }
    }
}