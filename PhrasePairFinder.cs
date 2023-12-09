using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPhraseMatcher
{
    public class PhrasePairFinder
    {
        public PhrasePairFinder(TextReader searchesReader, TextReader titlesReader)
        {
            Words = new SortedSet<string>();

            foreach (var word in Extensions.GetWords(titlesReader))
            {
                Words.Add(word);
            }

            foreach (var word in Extensions.GetWords(searchesReader))
            {
                Words.Add(word);
            }
        }

        public SortedSet<string> Words { get; set; }
    }
}
