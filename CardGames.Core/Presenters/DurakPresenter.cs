using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Durak;
using System.Collections;
using static CardGames.Core.Durak.Card;
using CardGames.Core.Presenters.Models.Durak;
using static CardGames.Core.Presenters.Models.Durak.AdditionResult;

namespace CardGames.Core.Presenters
{
    public class DurakPresenter: PresenterBase
    {         
        #region PresenterBase implementation
        public override GameType Type => GameType.Durak;
        public override IEnumerable<string> Players => _nameMap.Values;
        public override bool HasSlots => _game.Players.Count < 5;
        #endregion

        Dictionary<int, string> _nameMap = new Dictionary<int, string>();
        DurakGame _game = new DurakGame();

        public AdditionResult AddPlayer(string name)
        {
            if (_nameMap.Values.Any(n => 
                string.Equals(n, name, StringComparison.CurrentCultureIgnoreCase)))
                return new AdditionResult(AdditionResultType.NameConflict)
                {
                    Message = $"There is already player with name {name}"
                };

            var players = _game.Players.OrderBy(p => p.Id);
            int id = players.Any() ? players.Last().Id + 1 : 0;

            try
            {
                _game.AddPlayer(id);
            }
            catch (GameStateException)
            {
                return new AdditionResult(AdditionResultType.NotPreparationState)
                {
                    Message = $"You cant add a player when game state is {_game.State}"
                };
            }
            catch (MaxPlayersException)
            {
                return new AdditionResult(AdditionResultType.MaxPlayersCount)
                {
                    Message = "There is already max player count"
                };
            }

            return new AdditionResult(id);
        }

        public ResultModel Turn(int playerId, CardSuit suit, int value)
        {
            _game.Turn(playerId, new Card(suit, value));
            throw new NotImplementedException();
        }

        public ResultModel SkipTurn(int playerId)
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