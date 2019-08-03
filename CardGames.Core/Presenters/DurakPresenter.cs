using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Durak;
using static CardGames.Core.Durak.Card;

namespace CardGames.Core.Presenters
{
    public class DurakPresenter: PresenterBase
    {       
        Dictionary<int, string> _nameMap = new Dictionary<int, string>();
        DurakGame _game = new DurakGame();
        
        public ResultModel AddPlayer(string name)
        {
            if (_nameMap.Values.Any(n => 
                string.Equals(n, name, StringComparison.CurrentCultureIgnoreCase)))
                return Bad($"There is already player with name {name}");

            var players = _game.Players.OrderBy(p => p.Id);
            _game.AddPlayer(players.Any() ? players.Last().Id + 1 : 0);

            return Ok();
        }

        public ResultModel Turn(int playerId, CardSuit cardSuit, int cardValue)
        {
            throw new NotImplementedException();
        }

        public ResultModel SkipTurn(int playerId)
        {
            throw new NotImplementedException();
        }

        public void GetState(int playerId)
        {
            throw new NotImplementedException();
        }
    }
}