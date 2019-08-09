namespace CardGames.Core.Durak
{
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