namespace VytaTask.CrossCutting.Events
{
    public interface IEventPublisher
    {
        /// <summary>
        /// Publish event
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="eventMessage">Event message</param>
        /// <returns>true if no exceptions occured</returns>
        bool Publish<T>(T eventMessage);
    }
}
