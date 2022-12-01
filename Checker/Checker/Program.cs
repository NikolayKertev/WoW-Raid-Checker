using System;
using System.Collections.Generic;
using System.Linq;

namespace Checker
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> players = new Dictionary<string, int>();
            int concurrentPlayers = 0;
            string input = Console.ReadLine();

            while (input != "end")
            {
                if (!players.ContainsKey(input))
                {
                    players.Add(input, 0);
                    concurrentPlayers++;
                }

                players[input]++;
                 
                input = Console.ReadLine();
            }

            var ordered = players.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            int totalCount = 0;

            Console.Clear();

            foreach (var player in ordered)
            {
                Console.WriteLine($"Name: {player.Key}, {player.Value}");
                totalCount += player.Value;
            }
            Console.WriteLine();
            Console.WriteLine($"Total characters: {totalCount}");
            Console.WriteLine();
            Console.WriteLine($"Total players: {concurrentPlayers}");
            Console.WriteLine();
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
