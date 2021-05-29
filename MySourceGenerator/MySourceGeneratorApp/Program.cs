using System;
using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

namespace MySourceGeneratorApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddTransient<IFooService, FooService>();
            services.AddTransient<IBarService, BarService>();
            services.AddTransient<MyService>();

            var serviceProvider = services.BuildServiceProvider();

            var service = serviceProvider.GetRequiredService<MyService>();

            service.Test();
        }
    }

    public interface IFooService
    {
        public void Test();
    }

    public class FooService : IFooService
    {
        public void Test()
        {
            Console.WriteLine("Hello, Foo!");
        }
    }

    public interface IBarService
    {
        public void Test();
    }

    public class BarService : IBarService
    {
        public void Test()
        {
            Console.WriteLine("Hello, Bar!");
        }
    }

    public sealed partial class MyService
    {
        [Dependency]
        private IFooService Foo { get; }

        [Dependency]
        private IBarService Bar { get; }

        public void Test()
        {
            Foo.Test();
            Bar.Test();
        }
    }
}