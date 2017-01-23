namespace VytaTask.Business.Contracts.DataAccess
{
    public enum State
    {
        Added,
        Unchanged,
        Modified,
        Deleted
    }

    public interface IObjectWithState
    {
        State State { get; set; }
    }
}
