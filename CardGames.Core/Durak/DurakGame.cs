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
        
        public bool IsAttack => _attacks.Count == 0 || _attacks.Last().IsBeaten;
        public int DefenderIndex
        { 
            get => _defenderIndex; 
            set => _defenderIndex = NormalizeIndex(value);
        }
        // returns left or right player's index depending on whose order to attack it is now
        public int AttackerIndex => this.NormalizeIndex(this.DefenderIndex + (_attackerOrderFlag ? 1 : -1));
        public int CurrentPlayerIndex => this.IsAttack ? this.AttackerIndex : this.DefenderIndex;
        public Player CurrentPlayer => _players[this.CurrentPlayerIndex];
        #endregion

        #region Private props 
        private List<Player> _players = new List<Player>();
        private Deck _deck = null;
        private GameState _state = GameState.Preparation;
        private Card _trump = null;
        private int _defenderIndex = -1;
        private bool _attackerOrderFlag = false;
        private List<AttackEntry> _attacks = null;
        private bool _isAnyAttackBeatenOff = false;
        private int _attackersSkippedTurns = 0;
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
            this.DefenderIndex = attackerIndex + 1;

             // first attacker is player on the right
            _attackerOrderFlag = false;

            // the game starts
            _state = GameState.Started;
        }

        public void Turn(int playerId, Card usedCard)
        {
            Player player = this.CurrentPlayer;

            if (playerId != player.Id)
                return; // it's not your turn, relax cowboy
            if (usedCard != null && player.Hand.All(c => c != usedCard))
                return; // you dont have this card, relax cheater

            if (this.IsAttack)
            {
                // check that card can be used for attack
                if (_attacks.Count != 0 && _attacks.All(a => 
                    a.Attacker.Value != usedCard.Value && 
                    a.Defender.Value != usedCard.Value))
                    return;

                _attacks.Add(new AttackEntry(usedCard));
            }
            else
            {
                // check that used card can beat attacking one
                Card attack = _attacks.Last().Attacker;

                if (!usedCard.DoesBeat(attack, this.Trump.Suit))
                    return;

                _attacks.Last().Beat(usedCard);
                 // both attackers got a chance to use a card again
                _attackersSkippedTurns = 0;

                // first attack is 5 cards maximum, in this case 
                // current attackers forcibly skip their turns
                if (_isAnyAttackBeatenOff == false && _attacks.Count == 5)
                    this.NextStage();

                // check if current attacker is forced to skip his turn
                if (!CanAttack(this.CurrentPlayer))
                {
                    // attacker index is changed here, 
                    // so CurrentPlayerIndex will be new after this call
                    this.SkipTurn(this.CurrentPlayerIndex);

                    // check if another attacker is forced to skip turn
                    if (!CanAttack(this.CurrentPlayer))
                        // if both skipped, NextStage() will be called inside
                        this.SkipTurn(this.CurrentPlayerIndex);
                }
            }
        }

        public void SkipTurn(int playerId)
        {
            if (playerId != this.CurrentPlayer.Id)
                return;

            if (this.IsAttack) // skip for attacker means nothing more to attack with
            {
                _attackersSkippedTurns++;
                _attackerOrderFlag = !_attackerOrderFlag;

                // if both attackers skipped turns then we move to the next stage
                if (_attackersSkippedTurns == 2)
                    this.NextStage();
            }
            else // for defender it means that he cant beat the attacker and takes all cards
            {
                Player defender = this.CurrentPlayer;

                _attacks.ForEach(attack => {
                    if (attack.Defender != null)
                        defender.Hand.Add(attack.Defender);
                    defender.Hand.Add(attack.Attacker);
                });

                _attacks.Clear();
                this.GiveAway();

                // the player on the left from defender becomes an attacker, so +2
                _attackerOrderFlag = false;
                this.DefenderIndex += 2;
            }
        }

        void GiveAway()
        {
            int defenderIndex = this.DefenderIndex;

            // defender is the last taking cards from deck
            List<Player> players = new List<Player>()
            {
                this.GetByIndex(defenderIndex - 1), // first attacker
                this.GetByIndex(defenderIndex + 1), // second attacker
                this.GetByIndex(defenderIndex) // defender
            };

            players.ForEach(p => 
            {
                // taking cards until there are 6 in hand
                while (_deck.Count > 0 && p.Hand.Count < 6)
                    p.Hand.Add(_deck.Next());
            });
        }
        
        private void NextStage()
        {
            _isAnyAttackBeatenOff = true;
            _attackerOrderFlag = false;
            _attacks.Clear();
            // filling the hands to 6 cards each
            this.GiveAway();
            // if the turn got back to the first attacker 
            // then the defender has succeed and he is the attacker now
            this.DefenderIndex++;
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

        private Player GetByIndex(int index)
        {
            return _players[NormalizeIndex(index)];
        }

        private bool CanAttack(Player player)
        {
            List<Card> cardsOnTable = new List<Card>();

            foreach (var attack in _attacks)
            {
                if (attack.Defender != null)
                    cardsOnTable.Add(attack.Defender);
                cardsOnTable.Add(attack.Attacker);
            }
            
            return 
                player.Hand.Any(inHand => 
                    cardsOnTable.Any(onTable => 
                        inHand.Value == onTable.Value));
        }

        public enum GameState { Preparation, Started, Finished }

        public class AttackEntry
        {
            public Card Attacker { get; }

            public Card Defender { get; private set; }

            public bool IsBeaten => this.Defender != null;

            public AttackEntry(Card attacker)
            {
                this.Attacker = attacker;
            }

            public void Beat(Card defender)
            {
                this.Defender = defender;
            }
        }
    }
}
