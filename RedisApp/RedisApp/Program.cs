using System;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace RedisApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var redis = await ConnectionMultiplexer.ConnectAsync("localhost");

            var database = redis.GetDatabase();

            var result = await database.lock("Foo:Bar:Baz");

            Console.WriteLine("Result: " + result);
        }
    }
}