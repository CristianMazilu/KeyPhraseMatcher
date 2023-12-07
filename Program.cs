using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace KeyPhraseMatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var basePath = "D:\\Projects\\2023.12.06 KeyPhraseMatcher\\KeyPhraseMatcher";
            var phrasesDirectory = Path.Combine(basePath, "Phrases");
            var rawPhrasesDirectory = Path.Combine(phrasesDirectory, "RawPhrases");
            var aggregatedFileName = "aggregated.txt";
            var searchesFileName = "searches.txt";
            var titlesFileName = "titles.txt";

            try
            {
                var aggregatedFilePath = Path.Combine(phrasesDirectory, aggregatedFileName);
                AggregatePhrases(GetFilesAsStreams(rawPhrasesDirectory), CreateFileStream(aggregatedFilePath), out int phraseCount);
                Console.WriteLine($"{phraseCount} phrases found.");

                var searchesFilePath = Path.Combine(phrasesDirectory, searchesFileName);
                var titlesFilePath = Path.Combine(phrasesDirectory, titlesFileName);
                SplitPhrasesToSearchesAndTitle(new FileInfo(aggregatedFilePath).OpenRead(), CreateFileStream(searchesFilePath), CreateFileStream(titlesFilePath), 0.3, out int searchesCount, out int titlesCount);
                Console.WriteLine($"Phrases split into {searchesCount} search phrases and {titlesCount} title phrases.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static Stream CreateFileStream(string filePath)
        {
            return File.Create(filePath);
        }

        private static IEnumerable<Stream> GetFilesAsStreams(string directory)
        {
            return Directory.EnumerateFiles(directory).Select(filePath => new FileStream(filePath, FileMode.Open, FileAccess.Read));
        }

        private static void SplitPhrasesToSearchesAndTitle(Stream input, Stream searches, Stream titles, double splitRatio, out int searchesCount, out int titlesCount)
        {
            var rnd = new Random();
            string line;
            searchesCount = 0;
            titlesCount = 0;

            using (var reader = new StreamReader(input))
            using (var searchesWriter = new StreamWriter(searches))
            using (var titlesWriter = new StreamWriter(titles))
            {
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

        private static void AggregatePhrases(IEnumerable<Stream> input, Stream output, out int phraseCount)
        {
            phraseCount = 0;
            using (var writer = new StreamWriter(output))
            {
                foreach (var file in input)
                {
                    using (var reader = new StreamReader(file))
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
    }
}
