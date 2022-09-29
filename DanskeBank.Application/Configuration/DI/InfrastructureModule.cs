using Autofac;
using DanskeBank.Infrastructure.Repositories;

namespace DanskeBank.Application.Configuration.DI;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterType<CompanyRepository>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
