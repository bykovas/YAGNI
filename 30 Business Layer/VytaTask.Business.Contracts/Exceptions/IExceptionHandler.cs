using System;

namespace VytaTask.Business.Contracts.Exceptions
{
    public interface IExceptionHandler
    {
        T FromUnsafeFunction<T>(Func<T> unsafeFunction);
    }
}
