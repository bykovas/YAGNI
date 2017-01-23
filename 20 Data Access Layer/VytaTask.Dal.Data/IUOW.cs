using System;
using System.Data.Entity;
using VytaTask.Business.Contracts.DataAccess;

namespace VytaTask.Dal.Data
{
    public interface IUoW<out TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext
    {
    }
}
