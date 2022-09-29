using Autofac;
using DanskeBank.Domain.CompanyAggregate.Factories;
using DanskeBank.Domain.CompanyAggregate.Services;
using System.Reflection;

namespace DanskeBank.Application.Configuration.DI;

public class DomainModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var types = Assembly
            .GetAssembly(typeof(ICreateNotificationsStrategy))!
            .GetTypes()
            .Where(t => !t.IsAbstract && t.GetInterface(nameof(ICreateNotificationsStrategy)) is not null);

        foreach (var @type in types)
            builder
                .RegisterType(@type)
                .As<ICreateNotificationsStrategy>()
                .SingleInstance();

        builder
            .RegisterType<CompanyFactory>()
            .AsImplementedInterfaces()
            .SingleInstance();
    }
}
