using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core;
using CardGames.Core.Presenters;
using CardGames.Models;

namespace CardGames.Services
{
    public class GameLobbyService
    {
        public List<PresenterBase> Games { get; } = new List<PresenterBase>();

        public T GetByUid<T>(string uid) where T: PresenterBase
        {
            return (T)this.Games.FirstOrDefault(g => g.Uid == uid);
        }

        public IEnumerable<LobbyInfo> GetLobbies()
        {
            return this.Games.Select(g =>
            {
                return new LobbyInfo
                {
                    Uid = g.Uid,
                    Players = g.Players,
                    HasSlots = g.HasSlots
                };
            });
        }

        public string CreateLobby()
        {
            DurakPresenter game = new DurakPresenter();
            this.Games.Add(game);
            return game.Uid;
        }
    }
}