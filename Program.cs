using System;
using System.IO;

namespace KeyPhraseMatcer
{
    class Program
    {
        static void Main(string[] args)
        {
            var phrasesDirectory = "D:\\Projects\\2023.12.06 KeyPhraseMatcher\\KeyPhraseMatcher\\Phrases";
            var rawPhrasesDirectory = "D:\\Projects\\2023.12.06 KeyPhraseMatcher\\KeyPhraseMatcher\\Phrases\\RawPhrases";
            var aggregatedFileName = "aggregated.txt";
            var searchesFileName = "searches.txt";
            var titlesFileName = "titles.txt";

            AggregatePhrases(
                Directory.EnumerateFiles(rawPhrasesDirectory).Select(filePath => new FileInfo(filePath).OpenRead()),
                File.Create(phrasesDirectory + "\\" + aggregatedFileName),
                out int phraseCount);
            Console.WriteLine(phraseCount + " phrases found.");

            SplitPhrasesToSearchesAndTitle(
                new FileInfo(phrasesDirectory + "\\" + aggregatedFileName).OpenRead(),
                File.Create(phrasesDirectory + "\\" + searchesFileName),
                File.Create(phrasesDirectory + "\\" + titlesFileName),
                0.3,
                out int searchesCount,
                out int titlesCount);
            Console.WriteLine(string.Format("Phrases split into {} search phrases and {} title phrases.", searchesCount, titlesCount));
        }

        private static void SplitPhrasesToSearchesAndTitle(Stream input, Stream searches, Stream titles, double splitRation, out int searchesCount, out int titlesCount)
        {
            throw new NotImplementedException();
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