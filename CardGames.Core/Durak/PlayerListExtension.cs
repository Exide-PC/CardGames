using System.Collections.Generic;
using System.Linq;

namespace CardGames.Core.Durak
{
    public static class PlayerListExtension
    {
        public static bool ContainsId(this IReadOnlyList<Player> players, int id)
        {
            return players.Any(p => p.Id == id);
        }

        public static Player Get(this IReadOnlyList<Player> players, int id)
        {
            return players.First(p => p.Id == id);
        }
    }
}