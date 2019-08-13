using System;
using System.Collections.Generic;

namespace CardGames.Core.Presenters
{
    public abstract class PresenterBase
    {
        public string Uid { get; set; } = Guid.NewGuid().ToString();
        public abstract string Name { get; set; }
        public abstract GameType Type { get; }
        public abstract IEnumerable<string> PlayerNames { get; }
        public abstract bool HasSlots { get; }
    }
}