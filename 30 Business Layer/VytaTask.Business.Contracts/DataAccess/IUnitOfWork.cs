namespace VytaTask.Business.Contracts.DataAccess
{
    public interface IUnitOfWork<out TContext>
    {
        int Save();
        TContext Context { get; }
    }
}
