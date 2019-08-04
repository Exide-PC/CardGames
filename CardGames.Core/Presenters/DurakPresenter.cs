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
        public override IEnumerable<string> Players => _nameMap.Values;
        public override bool HasSlots => _game.Players.Count < 5;
        #endregion

        public DurakGame.GameState State => _game.State;

        Dictionary<int, string> _nameMap = new Dictionary<int, string>();
        DurakGame _game = new DurakGame();

        public int AddPlayer(string name)
        {
            if (_nameMap.Values.Contains(name))
                throw new ArgumentException($"There is already player with name {name}");
               
            var players = _game.Players.OrderBy(p => p.Id);
            int id = players.Any() ? players.Last().Id + 1 : 0;

            _nameMap.Add(id, name);
            _game.AddPlayer(id);
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

        public PlayerState GetState(int playerId)
        {
            return new PlayerState
            {
                Hand = _game.Players.First(p => p.Id == playerId).Hand,
                Trump = _game.Trump,
                GameState = _game.State.ToString(),
                DeckCount = _game.Deck.Count,
                IsAttack = _game.IsAttack,
                DefenderId = _game.Players[_game.DefenderIndex].Id,
                AttackerId = _game.Players[_game.AttackerIndex].Id,
                CurrentPlayerId = _game.CurrentPlayer.Id,
                Players = _game.Players.Select(p => new {Id = p.Id, Name = _nameMap[p.Id]})
            };
        }

        public class PlayerState
        {
            public IEnumerable<Card> Hand { get; set; }
            public CardSuit Trump { get; internal set; }
            public string GameState { get; set; }
            public int DeckCount { get; set; }
            public bool IsAttack { get; set; }
            public int DefenderId { get; internal set; }
            public int AttackerId { get; set; }
            public int CurrentPlayerId { get; set; }
            public IEnumerable<object> Players { get; set; }
        }
    }
}