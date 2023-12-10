using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPhraseMatcher
{
    public class PhrasePairFinder
    {
        public PhrasePairFinder(Stream searches, Stream titles)
        {
            Words = new SortedSet<string>();

            using (var searchesReader = new StreamReader(searches))
            using (var titlesReader = new StreamReader(titles))
            {
                foreach (var word in Extensions.GetWords(titlesReader))
                {
                    Words.Add(word);
                }

                foreach (var word in Extensions.GetWords(searchesReader))
                {
                    Words.Add(word);
                }
            }
        }

        public SortedSet<string> Words { get; set; }
    }
}
