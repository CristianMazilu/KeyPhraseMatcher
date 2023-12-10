using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Diagnostics;

namespace KeyPhraseMatcher
{
    public class Program
    {
        private static string basePath = "D:\\Projects\\2023.12.06 KeyPhraseMatcher\\KeyPhraseMatcher";
        private static string phrasesDirectory = Path.Combine(basePath, "Phrases");
        private string rawPhrasesDirectory = Path.Combine(phrasesDirectory, "RawPhrases");
        private string aggregatedFilePath = Path.Combine(phrasesDirectory, "aggregated.txt");
        private string searchesFilePath = Path.Combine(phrasesDirectory, "searches.txt");
        private string titlesFilePath = Path.Combine(phrasesDirectory, "titles.txt");
        private int setSearchesCount = 10000;
        private int setTitlesCount = 300000;

        public static void Main(string[] args)
        {
            var program = new Program();

            if (args.Length > 0 && args[0].Equals("generate", StringComparison.OrdinalIgnoreCase))
            {
                program.GenerateFiles();
            }
            else
            {
                program.ShowMenu();
            }
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Generate files");
                Console.WriteLine("2. Count matches - SLOW");
                Console.WriteLine("3. Count matches - FAST 1.0");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice (1-4): ");

                var choice = Console.ReadLine();
                double matchesCount = 0;

                switch (choice)
                {
                    case "1":
                        GenerateFiles();
                        break;
                    case "2":
                        matchesCount = FindPairsSlow();
                        Console.WriteLine(string.Format("{0} matches found.", matchesCount));
                        break;
                    case "3":
                        matchesCount = FindPairsFast();
                        Console.WriteLine(string.Format("{0} matches found.", matchesCount));
                        break;
                    case "4":
                        Console.WriteLine("Exiting the program.");
                        return; // Exits the method, thus ending the program
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private double FindPairsFast()
        {
            var phrasePairFinder =
                new PhrasePairFinder(
                    new FileStream(searchesFilePath, FileMode.Open, FileAccess.Read),
                    new FileStream(titlesFilePath, FileMode.Open, FileAccess.Read));

            return (double)phrasePairFinder.MatchSearches();
        }

        public double FindPairsSlow()
        {
            double matchesCount = 0;

            using (var searchesReader = new StreamReader(new FileStream(searchesFilePath, FileMode.Open, FileAccess.Read)))
            {
                foreach (var search in Extensions.GetLines(searchesReader))
                {
                    var searchWords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    using (var titlesReader = new StreamReader(new FileStream(titlesFilePath, FileMode.Open, FileAccess.Read)))
                    {
                        foreach (var title in Extensions.GetLines(titlesReader))
                        {
                            var titleWords = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                            if (TitleContainsSearch(titleWords, searchWords))
                            {
                                matchesCount++;
                            }
                        }
                    }
                }
            }

            return matchesCount;
        }

        public bool TitleContainsSearch(string[] titleWords, string[] searchWords)
        {
            foreach (var searchWord in searchWords)
            {
                if (!titleWords.Contains(searchWord))
                {
                    return false;
                }
            }

            return true;
        }

        public void GenerateFiles()
        {
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

        public static void AggregatePhrases(string rawPhrasesDirectory, string aggregatedFilePath, out int phraseCount)
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

        public void SplitPhrasesToSearchesAndTitle(string inputFilePath, string searchesFilePath, string titlesFilePath, double splitRatio, out int searchesCount, out int titlesCount)
        {
            searchesCount = 0;
            titlesCount = 0;
            var rnd = new Random();

            using (var searchesOutput = File.Create(searchesFilePath))
            using (var searchesWriter = new StreamWriter(searchesOutput))
            using (var titlesOutput = File.Create(titlesFilePath))
            using (var titlesWriter = new StreamWriter(titlesOutput))
            {
                searchesWriter.Write("");
                titlesWriter.Write("");
            }

            while (searchesCount < setSearchesCount || titlesCount < setTitlesCount)
            {
                using (var input = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                using (var reader = new StreamReader(input))
                using (var searchesOutput = new FileStream(searchesFilePath, FileMode.Append))
                using (var searchesWriter = new StreamWriter(searchesOutput))
                using (var titlesOutput = new FileStream(titlesFilePath, FileMode.Append))
                using (var titlesWriter = new StreamWriter(titlesOutput))
                {
                    foreach (var line in Extensions.GetLines(reader))
                    {
                        if (rnd.NextDouble() < splitRatio)
                        {
                            foreach (var subSearchPhrase in Extensions.GenerateSubPhrases(line))
                            {
                                searchesWriter.WriteLine(subSearchPhrase);
                                searchesCount++;
                            }
                        }
                        else
                        {
                            titlesWriter.WriteLine(line);
                            foreach (var subSearchPhrase in Extensions.GenerateSubPhrases(line))
                            {
                                titlesWriter.WriteLine(subSearchPhrase);
                                titlesCount++;
                            }
                        }
                    }
                }
            }
        }
    }
}
