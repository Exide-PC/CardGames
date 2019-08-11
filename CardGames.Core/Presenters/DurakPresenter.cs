using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Durak;
using static CardGames.Core.Durak.Card;

namespace CardGames.Core.Presenters
{
    public class DurakPresenter: PresenterBase
    {         
        #region PresenterBase implementation
        public override GameType Type => GameType.Durak;
        public override IEnumerable<string> PlayerNames => _players.Select(p => p.Name);
        public override bool HasSlots => _game.Players.Count < 5;
        #endregion

        DurakGame _game = new DurakGame();
        List<Player> _players = new List<Player>();

        public int AddPlayer(string name)
        {
            if (_players.Any(p => p.Name == name))
                throw new ArgumentException($"There is already player with name {name}");
               
            var players = _game.Players.OrderBy(p => p.Id);
            int id = players.Any() ? players.Last().Id + 1 : 0;

            _game.AddPlayer(id);
            _players.Add(new Player() { Id = id, Name = name });
            return id;
        }

        public void Start()
        {
            _game.Start();
        }

        public void Turn(int playerId, Card card)
        {
            _game.Turn(playerId, card);
        }

        public void SkipTurn(int playerId)
        {
            _game.SkipTurn(playerId);
        }

        public void SetPlayerReady(int playerId, bool isReady)
        {
            _players.First(p => p.Id == playerId).IsReady = isReady;

            if (isReady && _players.All(p => p.IsReady) && _game.Players.Count >= 2)
                _game.Start();
        }

        public GameStateHolder GetState(int playerId)
        {
            return new GameStateHolder
            {
                Trump = _game.Trump.ToString(),
                GameState = _game.State.ToString(),
                DeckCount = _game.Deck.Count,
                IsAttack = _game.IsAttack,
                IsInitialAttack = _game.IsInitialAttack,
                DefenderId = _game.Players[_game.DefenderIndex].Id,
                AttackerId = _game.Players[_game.InitialAttacker].Id,
                CurrentPlayerId = _game.CurrentPlayer.Id,
                Players = _players,
                Hand = _game.Players.First(p => p.Id == playerId).Hand.Select(c => new NamedCard(c)),
                CardsForTurn = _game.GetCardsForTurn(playerId).Select(c => new NamedCard(c)),
                Attacks = _game.Attacks.Select(a => (new NamedCard(a.Attacker), new NamedCard(a.Defender)))
            };
        }

        public class GameStateHolder
        {
            public string Trump { get; set; }
            public string GameState { get; set; }
            public int DeckCount { get; set; }
            public bool IsAttack { get; set; }
            public bool IsInitialAttack { get; set; }
            public int DefenderId { get; set; }
            public int AttackerId { get; set; }
            public int CurrentPlayerId { get; set; }
            public IEnumerable<object> Players { get; set; }
            public IEnumerable<NamedCard> Hand { get; set; }
            public IEnumerable<NamedCard> CardsForTurn { get; set; }
            public IEnumerable<(NamedCard attacker, NamedCard defender)> Attacks { get; set; }
        }

        public class NamedCard
        {
            public string Suit { get; }
            public int Value { get; }
            public string Name { get; }

            public NamedCard(Card card)
            {
                this.Suit = card.Suit.ToString();
                this.Value = card.Value;
                this.Name = GetName(card.Value);
            }

            private string GetName(int value)
            {
                switch (value)
                {
                    case int v when v <= 4: // 0,1,2,3,4 => 6,7,8,9,10
                        return (v + 6).ToString();
                    case 5:
                        return "Jack";
                    case 6:
                        return "Queen";
                    case 7:
                        return "King";
                    case 8:
                        return "Ace";
                    default:
                        throw new Exception($"No semantic name for value {value}");
                }
            }
        }

        public class Player
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool IsReady { get; set; } = false;
        }
    }
}