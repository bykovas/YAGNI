using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VytaTask.Business.Contracts;

namespace VytaTask.CrossCutting.Events
{
    public static class EventPublisherExtensions
    {
        /// <typeparam name="T1">entity type</typeparam>
        /// <typeparam name="T2">entity id type</typeparam>
        public static void EntityInserted<T1, T2>(this IEventPublisher eventPublisher, T1 entity) where T1 : BaseEntity<T2>
        {
            eventPublisher.Publish(new EntityInserted<T1, T2>(entity));
        }

        /// <typeparam name="T1">entity type</typeparam>
        /// <typeparam name="T2">entity id type</typeparam>
        public static void EntityUpdated<T1, T2>(this IEventPublisher eventPublisher, T1 entity) where T1 : BaseEntity<T2>
        {
            eventPublisher.Publish(new EntityUpdated<T1, T2>(entity));
        }

        /// <typeparam name="T1">entity type</typeparam>
        /// <typeparam name="T2">entity id type</typeparam>
        public static void EntityDeleted<T1, T2>(this IEventPublisher eventPublisher, T1 entity) where T1 : BaseEntity<T2>
        {
            eventPublisher.Publish(new EntityDeleted<T1, T2>(entity));
        }
    }
}
