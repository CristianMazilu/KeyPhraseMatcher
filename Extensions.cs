using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPhraseMatcher
{
    public class Extensions
    {
        public static IEnumerable<string> GenerateSubPhrases(string largerSearchPhrase)
        {
            var words = largerSearchPhrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < (int)(words.Length / 2); i++)
            {
                var rnd = new Random();
                int n = rnd.Next(1, words.Length + 1);
                string[] selectedWords = Enumerable.Range(0, n)
                    .Select(j => words[rnd.Next(words.Length)])
                    .ToArray();
                selectedWords = selectedWords.Distinct().ToArray();
                yield return string.Join(" ", selectedWords);
            }

        }

        public static IEnumerable<string> GetLines(TextReader stream)
        {
            string line;
            while ((line = stream.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public static IEnumerable<string> GetWords(TextReader stream)
        {
            foreach (var line in Extensions.GetLines(stream))
            {
                foreach (var word in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                {
                    yield return word;
                }
            }
        }
    }
}
