using VytaTask.Business.Contracts;

namespace VytaTask.CrossCutting.Events
{
    /// <summary>
    /// A container for entities that are updated.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class EntityUpdated<T1, T2> where T1 : BaseEntity<T2>
    {
        public EntityUpdated(T1 entity)
        {
            this.Entity = entity;
        }

        public T1 Entity { get; private set; }
    }
}
