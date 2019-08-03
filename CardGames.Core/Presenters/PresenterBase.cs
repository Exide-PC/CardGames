using CardGames.Core.Presenters.DurakModels;

namespace CardGames.Core.Presenters
{
    public abstract class PresenterBase
    {
        protected ResultModel Bad(string msg) => new ResultModel(false, msg);
        protected ResultModel Ok() => new ResultModel(true, null);
    }
}