using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CardGames.Core.Durak
{
    public class DurakGame
    {        
        public enum GameState { Preparation, Started, Finished }

        private List<Player> _players = new List<Player>();
        private Deck _deck = null;
        private GameState _state = GameState.Preparation;

        public ReadOnlyCollection<Player> Players => _players.AsReadOnly();
        public Deck Deck => _deck;
        public GameState State => _state;
        
        public DurakGame()
        {
        }

        public void AddPlayer(int id)
        {
            // player additions are only allowed during preparation 
            if (_state != GameState.Preparation || _players.Any(p => p.Id == id))
                return;

            _players.Add(new Player(id));
        }

        public void RemovePlayer(int id)
        {
            // we cant remove players during game
            if (_state != GameState.Preparation)
                return;
                
            _players = _players.Where(p => p.Id != id).ToList();
        }
    }
}
