using System;
using System.Threading.Tasks;

using StackExchange.Redis;

namespace RedisApp2
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var redis = await ConnectionMultiplexer.ConnectAsync("localhost");

            var database = redis.GetDatabase();

            while (true)
            {
                Console.Write("> ");

                var line = Console.ReadLine();
                var array = line.Split(' ');

                var key = array[0];

                if (array.Length == 1)
                {
                    var value = await database.StringGetAsync(key);

                    Console.WriteLine("< " + (value.IsNull ? "NULL" : value.ToString()));
                }
                else
                {
                    var value = array[1];
                    var expiry = array.Length > 2 ? TimeSpan.FromSeconds(Int32.Parse(array[2])) : default(TimeSpan?);

                    await database.StringSetAsync(key, value, expiry, When.Always);
                }
            }
        }
    }
}