using RpcContract;
using StreamJsonRpc;
using System;
using System.Threading.Tasks;

namespace ClientProcess
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                var client = await DaytonaClient.DaytonaClient.CreateDaytonaClientAsync();

                while (true)
                {
                    Console.Out.Write("Do something? ");
                    var input = Console.In.ReadLine();
                    if (input.Equals("n", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    try
                    {
                        string response = await client.DoSomethingAsync();
                        Console.Out.WriteLine(response);
                    }
                    catch (ConnectionLostException)
                    {
                        break;
                    }
                }

                await client.DisposeAsync();

                Console.Out.Write("Start another server? ");
                var exitCommand = Console.In.ReadLine();
                if (exitCommand.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
        }
    }
}
