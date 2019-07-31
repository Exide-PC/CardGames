using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames.Core.Durak
{
    public class Player
    {
        public int Id { get; }
        public List<Card> Hand { get; set; } = new List<Card>();

        public Player(int id)
        {
            this.Id = id;
        }
    }
}
