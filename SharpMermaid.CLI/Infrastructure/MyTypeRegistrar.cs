using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace SharpMermaid.CLI.Infrastructure;
public sealed class MyTypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _builder;

    public MyTypeRegistrar(IServiceCollection builder)
    {
        _builder = builder;
    }

    public ITypeResolver Build()
    {
        return new MyTypeResolver(_builder.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _builder.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _builder.AddSingleton(service, (provider) => factory());
    }
}