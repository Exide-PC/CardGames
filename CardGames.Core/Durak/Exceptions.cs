using System;

namespace CardGames.Core.Durak
{
    public class GameException: Exception
    {
        public GameException(string msg): base(msg)
        {
        }

        public GameException()
        {
            
        }
    }
}