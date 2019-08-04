using System;
using System.Collections.Generic;

namespace CardGames.Core.Presenters
{
    public abstract class PresenterBase
    {
        public string Uid { get; set; } = Guid.NewGuid().ToString();
        
        public abstract GameType Type { get; }
        public abstract IEnumerable<string> Players { get; }
        public abstract bool HasSlots { get; }

        protected ResultModel Bad(string msg) => new ResultModel(false, msg);
        protected ResultModel Ok() => new ResultModel(true, null);


        public class ResultModel
        {
            public bool Success { get; set; }
            public string Message { get; set; }

            public ResultModel(bool success, string message)
            {
                this.Success = success;
                this.Message = message;
            }
        }
    }
}