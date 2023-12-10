using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPhraseMatcher
{
    public class Keyword
    {
        public Keyword(string word)
        {
            Word = word;
        }

        public string Word { get; set; } = String.Empty;
        public List<Phrase> Phrases { get; set; } = new List<Phrase>();
        public override int GetHashCode()
        {
            return Word.GetHashCode();
        }
    }
}
