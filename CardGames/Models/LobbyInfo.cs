using System.Collections.Generic;

namespace CardGames.Models
{
    public class LobbyInfo
    {
        public string Uid { get; set; }
        public IEnumerable<string> Players { get; set; }
        public bool HasSlots { get; set; }
    }
}