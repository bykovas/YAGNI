using VytaTask.Business.Contracts;

namespace VytaTask.CrossCutting.Events
{
    /// <summary>
    /// A container for passing entities that have been deleted. This is not used for entities that are deleted logicaly via a bit column.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class EntityDeleted<T1, T2> where T1 : BaseEntity<T2>
    {
        public EntityDeleted(T1 entity)
        {
            this.Entity = entity;
        }

        public T1 Entity { get; private set; }
    }
}
