using System.Collections.Generic;
using System.Linq;

namespace CardGames.Core.Durak
{
    public static class AttackerListExtension
    {
        public static IReadOnlyList<Card> Unbeaten(this IReadOnlyList<AttackEntry> attackers)
        {
            return attackers.Where(a => !a.IsBeaten).Select(a => a.Attacker).ToArray();
        }
    }
}