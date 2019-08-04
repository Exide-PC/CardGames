namespace CardGames.Core.Presenters.Models
{
    public abstract class Result
    {
        public abstract bool Success { get; }
        public virtual string Message { get; set; } = null;
    }
}