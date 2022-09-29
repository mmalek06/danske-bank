using Autofac;
using DanskeBank.Application.Services;

namespace DanskeBank.Application.Configuration.DI;

public class ApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<CompanyService>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
