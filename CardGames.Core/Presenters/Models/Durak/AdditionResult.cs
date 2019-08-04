namespace CardGames.Core.Presenters.Models.Durak
{
    public class AdditionResult: Result
    {
        public enum AdditionResultType
        {
            Success,
            NameConflict,
            NotPreparationState,
            MaxPlayersCount
        }

        public AdditionResultType Type { get; }
        public int? Id { get; }

        public override bool Success => this.Type == AdditionResultType.Success;

        public AdditionResult(AdditionResultType type)
        {
            this.Type = type;
            this.Id = null;
        }

        public AdditionResult(int id)
        {
            this.Type = AdditionResultType.Success;
            this.Id = id;
        }
    }
}