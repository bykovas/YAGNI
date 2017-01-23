using System.Data.Entity;

namespace VytaTask.Dal.Data
{
    public class BaseContext<TContext> : DbContext where TContext : DbContext
    {
        static BaseContext()
        {
            Database.SetInitializer<TContext>(null);
        }
        protected BaseContext() : base("name=VytasDatabase")
        { }
    }
}
