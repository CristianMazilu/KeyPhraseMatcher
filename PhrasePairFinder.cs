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
            using (var titlesReader = new StreamReader(titles))
            {
                foreach (var word in Extensions.GetWords(titlesReader))
                {
                    Keywords.Add(new Keyword(word));
                }
            }
        }

        public List<Phrase> Searches { get; set; } = new List<Phrase>();
        public HashSet<Keyword> Keywords { get; set; } = new HashSet<Keyword>();
    }
}
