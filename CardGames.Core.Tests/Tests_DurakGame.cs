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

            Assert.That(game.Trump == game.Deck.Last().Suit);
        }

        [Test]
        public void Test_Turn()
        {
            var players = GetPlayers(
                new Card[]
                {
                    new Card(CardSuit.Diamonds, 0),
                    new Card(CardSuit.Diamonds, 2)
                },
                new Card[]
                {
                    new Card(CardSuit.Diamonds, 1),
                    new Card(CardSuit.Diamonds, 5),
                    new Card(CardSuit.Clubs, 5)                    
                }
            );

            DurakGame game = new DurakGame(players, deck: null, CardSuit.Clubs);
            
            Assert.Throws<GameException>(() => game.Turn(0, new Card(CardSuit.Clubs, 0)));
            Assert.DoesNotThrow(() => game.Turn(0, new Card(CardSuit.Diamonds, 2)));
            Assert.That(game.CurrentPlayerIndex == 1);
            Assert.That(game.Players[0].Hand.Count == 1);

            Assert.Throws<GameException>(() => game.Turn(0, new Card(CardSuit.Diamonds, 0)));
            Assert.Throws<GameException>(() => game.Turn(1, new Card(CardSuit.Diamonds, 1)));
            Assert.DoesNotThrow(() => game.Turn(1, new Card(CardSuit.Clubs, 5)));
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