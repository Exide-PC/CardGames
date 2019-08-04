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

        public ResultModel Turn(int playerId, CardSuit suit, int value)
        {
            _game.Turn(playerId, new Card(suit, value));
            throw new NotImplementedException();
        }

        public void SkipTurn(int playerId)
        {
            _game.SkipTurn(playerId);
            throw new NotImplementedException();
        }

        public void GetState(int playerId)
        {
            throw new NotImplementedException();
        }
    }
}