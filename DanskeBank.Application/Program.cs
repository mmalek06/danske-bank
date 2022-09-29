using Autofac;
using Autofac.Extensions.DependencyInjection;
using DanskeBank.Application.Configuration.DI;
using DanskeBank.Application.OutputConverters;
using DanskeBank.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder => {
    builder.RegisterModule<ApplicationModule>();
    builder.RegisterModule<DomainModule>();
    builder.RegisterModule<InfrastructureModule>();
    });
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment.EnvironmentName;

    config.SetBasePath(Environment.CurrentDirectory);
    config.AddJsonFile($"appsettings.json", optional: false);
    config.AddJsonFile($"appsettings.{env}.json", optional: true);
    config.AddEnvironmentVariables();
});
builder.Services
    .AddControllers(options => options.RespectBrowserAcceptHeader = true)
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()));
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.MapType<DateOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date",
            Example = new OpenApiString("31/01/2023")
        });
    });
builder.Services.AddDbContext<CompanyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(CompanyContext.DbName)));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<CompanyContext>()?.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
