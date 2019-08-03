using CardGames.Core.Durak;

namespace CardGames.Core.Presenters.DurakModels
{
    public class PlayerModel
    {
        public int PlayerId { get; set; }
    }

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

    public class TurnAttempt: PlayerModel
    {
        public Card UsedCard { get; set; }
    }
}