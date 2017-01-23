using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using VytaTask.Business.Contracts.Infrastructure;
using VytaTask.CrossCutting.Infrastructure.Configuration;

namespace VytaTask.CrossCutting.Infrastructure.DependencyManagement
{
    public interface IDependencyRegistrar : IBaseDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        void Register(ContainerBuilder builder, ITypeFinder typeFinder, VytaTaskConfig config);
    }
}
