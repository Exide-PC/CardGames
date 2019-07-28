using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CardGames.Core.Durak
{
    public class DurakGame
    {       
        #region Public props
        public ReadOnlyCollection<Player> Players => _players.AsReadOnly();
        public Deck Deck => _deck;
        public GameState State => _state;
        public Card Trump => _trump;
        
        public bool IsAttack => this._attacks.Count == 0 || this._attacks.Last().IsBeaten;
        // returns left or right player's index depending on whose order to attack it is now
        public int AttackerIndex => this.NormalizeIndex(_defenderIndex + (_attackerOrderFlag ? 1 : -1));
        public int CurrentPlayerIndex => this.IsAttack ? this.AttackerIndex : _defenderIndex;
        #endregion

        #region Private props 
        private List<Player> _players = new List<Player>();
        private Deck _deck = null;
        private GameState _state = GameState.Preparation;
        private Card _trump = null;
        private int _defenderIndex = -1;
        private bool _attackerOrderFlag = false;
        private List<AttackEntry> _attacks = null;
        #endregion

        public DurakGame()
        {
        }

        public void AddPlayer(int id)
        {
            // player additions are only allowed during preparation 
            if (_state != GameState.Preparation || _players.Count == 5 || _players.Any(p => p.Id == id))
                return;

            _players.Add(new Player(id));
        }

        public void RemovePlayer(int id)
        {
            // we cant remove players during game
            if (_state != GameState.Preparation)
                return;
                
            _players.RemoveAll(p => p.Id == id);
        }
        
        public void Start()
        {
            if (_state != GameState.Preparation || _players.Count < 2)
                return;

            // shuffling new deck, giving away 6 cards to each player
            _deck = Deck.Shuffle(36);


            foreach (Player player in _players)
                for (int i = 0; i < 6; i++)
                    player.Hand.Add(_deck.Next());

            // getting the trump and finding the player with the lowest trump card
            _trump = _deck.Last();

            Player attacker = _players.OrderBy(p => 
            {
                // if player hasn't any trumps, then we return max value
                // to place him in the end of sorted collection
                Card lowTrump = p.Hand.GetLowestTrump(_trump.Suit);
                return lowTrump?.Value ?? int.MaxValue;
            }).First();

            // getting attacker index in collection
            int attackerIndex = _players.IndexOf(attacker);
            // defender's index is 1 greater, but ensure the bounds are correct
            _defenderIndex = this.NormalizeIndex(attackerIndex + 1);

             // first attacker is player on the right
            _attackerOrderFlag = false;

            // the game starts
            _state = GameState.Started;
        }

        public void Turn(int playerId, Card usedCard)
            => this.Turn(playerId, new Card[] { usedCard });

        public void Turn(int playerId, IEnumerable<Card> usedCards)
        {
            if (playerId != this.CurrentPlayerIndex)
                return;

            int count = usedCards.Count();


        }

        private int NormalizeIndex(int index)
        {
            int delta = _players.Count;

            while (index < 0)
                index += delta;
            while (index >= delta)
                index -= delta;

            return index;
        }

        public enum GameState { Preparation, Started, Finished }

        public class AttackEntry
        {
            public Card Attacker { get; }

            public Card Defender { get; set; }

            public bool IsBeaten => this.Defender != null;

            public AttackEntry(Card attacker)
            {
                this.Attacker = attacker;
            }
        }
    }
}
