using System;

namespace CardGames.Core.Presenters
{
    public abstract class PresenterBase
    {
        public string Uid { get; } = Guid.NewGuid().ToString();

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