using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Presenters;

namespace CardGames.Services
{
    public class GameLobbyService
    {
        public List<PresenterBase> Games { get; } = new List<PresenterBase>();

        public PresenterBase GetByUid<T>(string uid) where T: PresenterBase
        {
            return (T)this.Games.FirstOrDefault(g => g.Uid == uid);
        }
    }
}