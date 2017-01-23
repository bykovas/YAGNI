using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VytaTask.Business.Contracts.Infrastructure
{
    /// <summary>
    /// Dependency registrar interface
    /// </summary>
    public interface IBaseDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        void Register(object builder, object typeFinder, object config);

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}
