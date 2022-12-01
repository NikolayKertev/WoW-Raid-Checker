using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RaidChecker
{
    public class StartUp
    {
        static readonly string[] textFiles = new string[9] 
        { 
        @".\Input\raid1.txt",
        @".\Input\raid2.txt",
        @".\Input\raid3.txt",
        @".\Input\raid4.txt",
        @".\Input\raid5.txt",
        @".\Input\raid6.txt",
        @".\Input\raid7.txt",
        @".\Input\raid8.txt",
        @".\Input\raid9.txt"
        };
        
        static int documentCounter = 0;

        static void Main(string[] args)
        {
            Console.Title = "RaidChecker";

            if (!File.Exists(textFiles[0]))
            {
                using (File.Create(textFiles[0]));
            }

            List<player> players = new List<player>();
            List<player> mirroredPlayers = new List<player>();

            Regex regex = new Regex(@"""class"":""[A-z]*[0-9]*"",""spec"":""[A-z]*[0-9]*"",""name"":""\D*"",");

            for (int i = 0; i < textFiles.Length; i++)
            {
                if (File.Exists(textFiles[i]))
                {
                    PlayersInput(players, mirroredPlayers, regex, textFiles[i]);
                    documentCounter++;
                    continue;
                }

                break;
            }

            string input = null;

            while (true)
            {
                if (input == "end")
                {
                    EndProgram(documentCounter);
                }

                PrintMenu();

                input = Console.ReadLine().ToLower();

                switch (input)
                {
                    case "1":
                        PrintAll(players, null);
                        Console.ReadKey(true);
                        break;
                    case "2":
                        PrintAll(mirroredPlayers, "duped ");
                        Console.ReadKey(true);
                        break;
                    case "3":
                        var playersDictionary = new Dictionary<string, Dictionary<string, List<string>>>();

                        DictionaryAdd(players, playersDictionary);
                        DictionaryAdd(mirroredPlayers, playersDictionary);

                        var sortedDictionary = playersDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                        PrintDictionary(sortedDictionary);
                        Console.ReadKey(true);
                        break;
                    case "4":
                        Console.Clear();
                        CreateDocument();
                        Console.ReadKey(true);
                        break;
                    case "0":
                        DeleteAll();
                        Console.ReadKey(true);
                        break;
                    case "5":
                        Main(args);
                        break;
                }
            }
        }

        static void DeleteAll()
        {
            for (int i = 0; i < textFiles.Length; i++)
            {
                if (File.Exists(textFiles[i]))
                {
                    File.Delete(textFiles[i]);
                    documentCounter = 0;
                    Console.Clear();
                    Console.WriteLine($"Successfuly deleted all documents.");
                    Console.WriteLine();
                    Console.WriteLine($"Press any key to continue...");
                }
            }
        }

        static void DictionaryAdd(List<player> players, Dictionary<string, Dictionary<string, List<string>>> dictionary)
        {
            foreach (var player in players)
            {
                if (!dictionary.ContainsKey(player.Name))
                {
                    dictionary.Add(player.Name, new Dictionary<string, List<string>>());
                }
                if (!dictionary[player.Name].ContainsKey(player.Clas.ClassName))
                {
                    dictionary[player.Name].Add(player.Clas.ClassName, new List<string>());
                }
                dictionary[player.Name][player.Clas.ClassName].Add(player.Clas.Spec);
            }
        }

        static void EndProgram(int documentCounter)
        {
            string input;
            Console.Clear();
            Console.WriteLine("Would you like to delete all the input documents? Yes/No");
            input = Console.ReadLine().ToLower();

            if (input == "yes")
            {
                DeleteAll();
            }

            Environment.Exit(0);
        }

        static void PlayersInput(List<player> players, List<player> mirroredPlayers, Regex regex, string textFile)
        {
            string input = File.ReadAllText(textFile);

            var collection = regex.Matches(input);

            List<string> collectionList = new List<string>();

            foreach (var item in collection)
            {
                collectionList.Add(item.ToString());
            }


            for (int i = 0; i < collectionList.Count; i++)
            {
                string[] arguments = collectionList[i].Split(",", StringSplitOptions.RemoveEmptyEntries);

                string name = (arguments[2].Split(":"))[1];
                string clas = (arguments[0].Split(":"))[1];
                string spec = (arguments[1].Split(":"))[1];

                name = name.Substring(1, name.Length - 2);
                clas = clas.Substring(1, clas.Length - 2);
                spec = spec.Substring(1, spec.Length - 2);

                switch (spec)
                {
                    case "Blood_Tank":
                        clas = "DK";
                        spec = "Blood_Tank";
                        break;
                    case "Protection":
                        clas = "Warrior";
                        break;
                    case "Protection1":
                        clas = "Paladin";
                        spec = "Protection";
                        break;
                    case "Guardian":
                        clas = "Druid";
                        spec = "Feral (Tank)";
                        break;
                    case "Holy1":
                        spec = "Holy";
                        break;
                    case "Restoration1":
                        spec = "Restoration";
                        break;
                    default:
                        break;
                }

                bool isAddedAlready = false;

                player player = new player(name, clas, spec);

                foreach (var p in players)
                {
                    if (p.CompareTo(player) == 1)
                    {
                        isAddedAlready = true;
                        break;
                    }
                }

                if (isAddedAlready)
                {
                    mirroredPlayers.Add(player);
                    continue;
                }

                players.Add(player);
            }
        }

        private static void CreateDocument()
        {
            int filesCreated = 0;

            if (documentCounter >= 8)
            {
                Console.Clear();
                Console.WriteLine("You have reached the limit of created documents.");
                Console.WriteLine("Please close the program and delete some of the documents before trying again.");
                Console.Beep(400, 500);
                Console.WriteLine();
                Console.WriteLine($"Press any key to continue...");
            }

            for (int i = 0; i < textFiles.Length; i++)
            {
                if (!File.Exists(textFiles[i]))
                {
                    using (File.Create(textFiles[i]));
                    documentCounter++;
                    filesCreated++;
                }

                if (filesCreated == 2)
                {
                    Console.WriteLine("You have created 2 new documents in the Input folder.");
                    Console.WriteLine();
                    Console.WriteLine($"Press any key to continue...");
                    break;
                }
            }
        }

        private static void PrintDictionary(Dictionary<string, Dictionary<string, List<string>>> sortedDictionary)
        {
            Console.Clear();

            foreach (var (Name, Value) in sortedDictionary)
            {
                Console.WriteLine($"Name: {Name}");

                foreach (var (Clas, SpecList) in Value)
                {
                    foreach (var spec in SpecList)
                    {
                        Console.WriteLine($"Class: {Clas}, Specc: {spec}");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine($"Total players: {sortedDictionary.Count}");
            Console.WriteLine();
            Console.WriteLine($"Press any key to continue...");
        }

        static void PrintAll(List<player> players, string dupedOrNot)
        {
            Console.Clear();

            foreach (var player in players)
            {
                Console.WriteLine(player);
                Console.WriteLine();
            }

            Console.WriteLine($"Total {dupedOrNot}characters: {players.Count}");
            Console.WriteLine();
            Console.WriteLine($"Press any key to continue...");
        }

        static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("* * * * * * * MENU * * * * * * *");
            Console.WriteLine("1. Print all characters");
            Console.WriteLine("2. Print any duped characters");
            Console.WriteLine("3. Print all players");
            Console.WriteLine("4. Create 2 new \"Raid\" documents");
            Console.WriteLine("5. Refresh the data");
            Console.WriteLine("0. Delete all \"Raid\" documents ");
            Console.WriteLine("Type \"end\" to close the program.");
        }
    }
}
