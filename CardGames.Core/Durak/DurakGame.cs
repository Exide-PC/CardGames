﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using static CardGames.Core.Durak.Card;

namespace CardGames.Core.Durak
{
    public class DurakGame
    {       
        public enum GameState { Preparation, Started, Finished }
        public enum PlayerRole { None, Defender, Attacker }

        #region Public props
        public IReadOnlyList<Player> Players => _players.AsReadOnly();
        public Deck Deck => _deck;
        public GameState State => _state;
        public CardSuit Trump => _trump;
        public IReadOnlyList<AttackEntry> Attacks => _attacks.AsReadOnly();
        
        public bool IsAttack => _attacks.Unbeaten().Any();
        public bool IsInitialAttack => !_attacks.Any();
        public int DefenderIndex
        { 
            get => _defenderIndex; 
            set => _defenderIndex = NormalizeIndex(value);
        }
        // returns left or right player's index depending on whose order to attack it is now
        public int InitialAttacker => this.NormalizeIndex(this.DefenderIndex - 1);
        #endregion

        #region Private props 
        private List<Player> _players = new List<Player>();
        private Deck _deck = null;
        private GameState _state = GameState.Preparation;
        private CardSuit _trump;
        private int _defenderIndex = -1;
        private List<AttackEntry> _attacks = new List<AttackEntry>();
        private bool _isAnyAttackBeatenOff = false;
        private HashSet<int> _attackersSkippedTurns = new HashSet<int>();
        #endregion

        public DurakGame(IEnumerable<Player> players, IEnumerable<Card> deck, CardSuit? trump = null)
        {
            if (!trump.HasValue && !(deck?.Any() == true))
                throw new Exception("No trump is specified");
            if (players?.Count() < 2)
                throw new Exception("Less than 2 players");

            _trump = trump.HasValue ? trump.Value : deck.Last().Suit;
            _deck = new Deck(deck ?? Enumerable.Empty<Card>());
            _players = players.ToList();

            this.DefenderIndex = 1; // 0 is attacker, 1 is defender
            _state = GameState.Started;          
        }

        public DurakGame()
        {            
        }

        public void AddPlayer(int id)
        {
            if (_state != GameState.Preparation)
                throw new GameException($"You cant add a player when game state is {_state}");
            if (_players.Count == 5 )
                throw new GameException("There is already max player count");
            if (_players.ContainsId(id))
                throw new GameException($"There is already player with id {id}");

            _players.Add(new Player(id));
        }

        public void RemovePlayer(int id)
        {
            // we cant remove players during game
            if (_state != GameState.Preparation)
                throw new GameException();
                
            _players.RemoveAll(p => p.Id == id);
        }
        
        public void Start()
        {
            if (_state != GameState.Preparation || _players.Count < 2)
                throw new GameException();

            // shuffling new deck, giving away 6 cards to each player
            _deck = Deck.Shuffle(36);


            foreach (Player player in _players)
                for (int i = 0; i < 6; i++)
                    player.Hand.Add(_deck.Next());

            // getting the trump and finding the player with the lowest trump card
            _trump = _deck.Last().Suit;

            Player attacker = _players.OrderBy(p => 
            {
                // if player hasn't any trumps, then we return max value
                // to place him in the end of sorted collection
                Card lowTrump = p.Hand.GetLowestTrump(_trump);
                return lowTrump?.Value ?? int.MaxValue;
            }).First();

            // getting attacker index in collection
            int attackerIndex = _players.IndexOf(attacker);
            // defender's index is 1 greater, but ensure the bounds are correct
            this.DefenderIndex = attackerIndex + 1;

            // the game starts
            _state = GameState.Started;
        }

        public void Turn(int playerId, Card usedCard, Card target = null)
        {
            Player player = _players.Get(playerId);
            PlayerRole role = this.GetPlayerRole(playerId);

            if (role == PlayerRole.None)
                throw new GameException($"Id {playerId} can't make turn in current stage");
            if (usedCard != null && player.Hand.All(c => c != usedCard))
                throw new GameException($"User with {playerId} doesn't have card {usedCard}");

            if (role == PlayerRole.Attacker)
            {
                // check that card can be used for attack
                if (_attacks.Count != 0 && _attacks.All(a => 
                    a.Attacker.Value != usedCard.Value && 
                    a.Defender.Value != usedCard.Value))
                    throw new GameException($"Card {usedCard} can't be used for attack");

                _attacks.Add(new AttackEntry(usedCard));     
                player.Hand.RemoveAll(inHand => inHand == usedCard);

                // defender skips turn if he can't handle all unbeaten cards
                Player defender = this.GetByIndex(_defenderIndex);

                IReadOnlyList<Card> attackers = _attacks.Unbeaten();

                // If any attacker can't be beaten by defender's hand - defender takes all cards
                if (attackers.Any(a => defender.Hand.Beating(a, _trump).Count == 0))
                    this.SkipTurn(defender.Id);
            }
            else
            {
                IReadOnlyList<Card> unbeaten = _attacks.Unbeaten();

                if (unbeaten.Count == 0)
                    throw new GameException($"There are no unbeaten cards on the table");
                if (target != null && !unbeaten.Contains(target))
                    throw new GameException($"There is no unbeaten {target} on the table");

                target = target ?? _attacks.Last().Attacker;

                // check that used card can beat attacking one
                if (!usedCard.DoesBeat(target, _trump))
                    throw new GameException($"{usedCard} doesn't beat {target}");

                _attacks.First(a => a.Attacker == target).Beat(usedCard);
                player.Hand.RemoveAll(inHand => inHand == usedCard);
                 // both attackers got a chance to use a card again
                _attackersSkippedTurns.Clear();

                // first attack is 5 cards maximum, in this case 
                // current attackers forcibly skip their turns
                if (!_isAnyAttackBeatenOff && _attacks.Count == 5)
                    this.NextStage(hasDefended: true);

                // if all attackers will skip turn, 
                // the NextStage() will be called eventually
                foreach (Player attacker in this.GetAttackers())
                    if (!this.GetCardsForTurn(attacker.Id).Any())
                        this.SkipTurn(attacker.Id); 
            }

            // end game check
            if (_deck.Count == 0 && _players.Where(p => p.Hand.Count > 0).Count() == 1)
                _state = GameState.Finished;
        }

        public void SkipTurn(int playerId)
        {
            Player player = _players.Get(playerId);
            PlayerRole role = this.GetPlayerRole(playerId);

            if (role == PlayerRole.None)
                throw new GameException($"Id {playerId} can't skip turn, because he has no role for this stage");

            if (role == PlayerRole.Attacker) // skip for attacker means nothing more to attack with
            {
                if (_attacks.Count == 0)
                    throw new GameException($"Id {playerId} can't skip the turn because he is initial attacker");

                if (this.GetAttackers().ContainsId(playerId))
                    throw new GameException($"Id {playerId} can't skip turn, because he is not an attacker");

                _attackersSkippedTurns.Add(playerId);
                
                // if all attackers skipped turns then we move to the next stage
                if (_attackersSkippedTurns.Count == GetAttackers().Count)
                    this.NextStage(hasDefended: true);
            }
            else // for defender it means that he cant beat the attacker and takes all cards
            {
                Player defender = this.GetByIndex(_defenderIndex);

                _attacks.ForEach(attack => {
                    if (attack.Defender != null)
                        defender.Hand.Add(attack.Defender);
                    defender.Hand.Add(attack.Attacker);
                });

                this.NextStage(hasDefended: false);
            }
        }

        public IReadOnlyList<Card> GetCardsForTurn(int playerId)
        {
            Player player = _players.Get(playerId);
            PlayerRole role = this.GetPlayerRole(playerId);
            IEnumerable<Card> cards = new List<Card>();

            if (role == PlayerRole.Attacker)
            {
                if (this.IsInitialAttack)
                {
                    if (this.InitialAttacker == playerId)
                        cards = player.Hand;
                }
                else if (this.GetAttackers().ContainsId(playerId))
                {
                    var cardsOnTable = this.GetCardsOnTable();

                    cards = player.Hand
                        .Where(inHand => cardsOnTable.Any(onTable => 
                            inHand.Value == onTable.Value));
                }
            }
            else if (role == PlayerRole.Defender)
            {
                if (this.DefenderIndex == playerId)
                {
                    IReadOnlyList<Card> attackers = _attacks.Unbeaten();

                    cards = player
                        .Hand.Where(inHand => 
                            attackers.Any(a => 
                                inHand.DoesBeat(a, _trump)));
                }
            }

            return cards.ToArray();
        }

        public PlayerRole GetPlayerRole(int playerId)
        {
            if (this.GetAttackers().ContainsId(playerId))
                return PlayerRole.Attacker;
            else if (playerId == this.DefenderIndex)
                return PlayerRole.Defender;
            else
                return PlayerRole.None;
        }

        private void GiveAway()
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
        
        private void NextStage(bool hasDefended)
        {
            _isAnyAttackBeatenOff = hasDefended ? true : _isAnyAttackBeatenOff;
            _attackersSkippedTurns.Clear();
            _attacks.Clear();

            this.GiveAway();
            this.DefenderIndex += hasDefended ? 1 : 2;
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

        private IReadOnlyList<Card> GetCardsOnTable()
        {
            List<Card> cardsOnTable = new List<Card>();

            foreach (var attack in _attacks)
            {
                if (attack.Defender != null)
                    cardsOnTable.Add(attack.Defender);
                cardsOnTable.Add(attack.Attacker);
            }

            return cardsOnTable;
        }

        private IReadOnlyList<Player> GetAttackers()
        {
            List<Player> attackers = new List<Player>
            {
                this.GetByIndex(this.DefenderIndex - 1),
                this.GetByIndex(this.DefenderIndex + 1)
            };

            // remove duplicated attacker if there is actually only one
            if (_players.Count == 2)
                attackers.RemoveAt(0);
            return attackers;
        }        
    }
}
