using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace KeyPhraseMatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("generate", StringComparison.OrdinalIgnoreCase))
            {
                GenerateFiles();
            }
            else
            {
                ShowMenu();
            }
        }

        private static void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Generate Files");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice (1-2): ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        GenerateFiles();
                        break;
                    case "2":
                        Console.WriteLine("Exiting the program.");
                        return; // Exits the method, thus ending the program
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void GenerateFiles()
        {
            var basePath = "D:\\Projects\\2023.12.06 KeyPhraseMatcher\\KeyPhraseMatcher";
            var phrasesDirectory = Path.Combine(basePath, "Phrases");
            var rawPhrasesDirectory = Path.Combine(phrasesDirectory, "RawPhrases");
            var aggregatedFilePath = Path.Combine(phrasesDirectory, "aggregated.txt");
            var searchesFilePath = Path.Combine(phrasesDirectory, "searches.txt");
            var titlesFilePath = Path.Combine(phrasesDirectory, "titles.txt");

            try
            {
                AggregatePhrases(rawPhrasesDirectory, aggregatedFilePath, out int phraseCount);
                Console.WriteLine($"{phraseCount} phrases found.");

                SplitPhrasesToSearchesAndTitle(aggregatedFilePath, searchesFilePath, titlesFilePath, 0.3, out int searchesCount, out int titlesCount);
                Console.WriteLine($"Phrases split into {searchesCount} search phrases and {titlesCount} title phrases.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void AggregatePhrases(string rawPhrasesDirectory, string aggregatedFilePath, out int phraseCount)
        {
            phraseCount = 0;
            using (var output = File.Create(aggregatedFilePath))
            using (var writer = new StreamWriter(output))
            {
                foreach (var filePath in Directory.EnumerateFiles(rawPhrasesDirectory))
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (var reader = new StreamReader(fileStream))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                            phraseCount++;
                        }
                    }
                }
            }
        }

        private static void SplitPhrasesToSearchesAndTitle(string inputFilePath, string searchesFilePath, string titlesFilePath, double splitRatio, out int searchesCount, out int titlesCount)
        {
            searchesCount = 0;
            titlesCount = 0;
            var rnd = new Random();

            using (var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(input))
            using (var searchesOutput = File.Create(searchesFilePath))
            using (var searchesWriter = new StreamWriter(searchesOutput))
            using (var titlesOutput = File.Create(titlesFilePath))
            using (var titlesWriter = new StreamWriter(titlesOutput))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (rnd.NextDouble() < splitRatio)
                    {
                        searchesWriter.WriteLine(line);
                        searchesCount++;
                    }
                    else
                    {
                        titlesWriter.WriteLine(line);
                        titlesCount++;
                    }
                }
            }
        }
    }
}
