using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPhraseMatcher
{
    public class Phrase
    {
        public Phrase(string phrase)
        {
            Words = new HashSet<string>();

            foreach (var word in phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                Words.Add(word);
            }
        }

        public HashSet<string> Words { get; set; }
    }
}
