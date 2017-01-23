using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using VytaTask.Business.Contracts.Infrastructure;
using VytaTask.Business.Contracts.Managers;
using VytaTask.CrossCutting.Infrastructure.Configuration;
using VytaTask.CrossCutting.Infrastructure.DependencyManagement;
using VytaTask.Dal.Caching;

namespace VytaTask.Framework
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        [Obsolete]
        public void Register(object builder, object typeFinder, object config)
        {
            throw new NotImplementedException();
        }

        public int Order => 0;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, VytaTaskConfig config)
        {
            //cache managers
            builder.RegisterType<MemoryCacheManager>()
                .As<ICacheManager>()
                .Named<ICacheManager>("cache_static")
                .SingleInstance();
            builder.RegisterType<PerRequestCacheManager>()
                .As<ICacheManager>()
                .Named<ICacheManager>("cache_per_request")
                .InstancePerLifetimeScope();

            #region signalR

            ////signalR custom user id provider
            //builder.RegisterInstance(new SignalRUserIdProvider()).As<IUserIdProvider>();
            ////signalR hubs
            //builder.RegisterHubs(typeFinder.GetAssemblies().ToArray());
            ////other
            //builder.RegisterType<SignalRContext>().As<ISignalRContext>().InstancePerLifetimeScope();

            #endregion
        }
    }

    public class SettingsSource : IRegistrationSource
    {
        static readonly MethodInfo BuildMethod = typeof(SettingsSource).GetMethod(
            "BuildRegistration",
            BindingFlags.Static | BindingFlags.NonPublic);

        public IEnumerable<IComponentRegistration> RegistrationsFor(
            Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrations)
        {
            var ts = service as TypedService;
            if (ts != null && typeof(ISettings).IsAssignableFrom(ts.ServiceType))
            {
                var buildMethod = BuildMethod.MakeGenericMethod(ts.ServiceType);
                yield return (IComponentRegistration) buildMethod.Invoke(null, null);
            }
        }

        static IComponentRegistration BuildRegistration<TSettings>() where TSettings : ISettings, new()
        {
            return RegistrationBuilder
                .ForDelegate((c, p) => c.Resolve<ISettingService>().LoadSetting<TSettings>())
                .InstancePerLifetimeScope()
                .CreateRegistration();
        }

        public bool IsAdapterForIndividualComponents => false;
    }
}