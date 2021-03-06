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

        public GameLobbyService()
        {
            #if DEBUG
            // TODO: Remove, just for debug
            string uid = this.CreateLobby("Test game");

            var game = this.GetByUid<DurakPresenter>(uid);
            game.Uid = "test";
            game.AddPlayer("Rib0");
            game.AddPlayer("Exide");
            game.AddPlayer("Leviy hren");
            game.Start();
            #endif
        }

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
                    Players = g.PlayerNames,
                    HasSlots = g.HasSlots
                };
            });
        }

        public string CreateLobby(string name)
        {
            DurakPresenter game = new DurakPresenter() { Name = name };
            this.Games.Add(game);
            return game.Uid;
        }

        public void DeleteLobby(string uid)
        {
            this.Games.RemoveAll(g => g.Uid == uid);
        }
    }
}