using System;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace RedisApp
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var redis = await ConnectionMultiplexer.ConnectAsync("localhost,allowAdmin=true");

            foreach (var endPoint in redis.GetEndPoints())
            {
                var server = redis.GetServer(endPoint);
                
                await server.ConfigSetAsync("notify-keyspace-events", "AKE");
            }
            
            var subscriber = redis.GetSubscriber();

            var channel = await subscriber.SubscribeAsync("*");

            while (true)
            {
                var message = await channel.ReadAsync();

                Console.WriteLine($"{message.Channel}: {message.Message}");
            }
        }
    }
}