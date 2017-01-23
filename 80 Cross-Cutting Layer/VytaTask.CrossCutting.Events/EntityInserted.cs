using VytaTask.Business.Contracts;

namespace VytaTask.CrossCutting.Events
{
    /// <summary>
    /// A container for entities that have been inserted.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class EntityInserted<T1, T2> where T1 : BaseEntity<T2>
    {
        public EntityInserted(T1 entity)
        {
            this.Entity = entity;
        }

        public T1 Entity { get; private set; }
    }
}
