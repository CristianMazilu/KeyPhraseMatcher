using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPhraseMatcher
{
    public class PhrasePairFinder
    {
        private Dictionary<string, List<Phrase>> _keywords = new Dictionary<string, List<Phrase>>();

        public PhrasePairFinder(Stream searches, Stream titles)
        {
            using (var titlesReader = new StreamReader(titles))
            {
                foreach (var line in Extensions.GetLines(titlesReader))
                {
                    var title = new Phrase(line);
                    foreach (var word in title.Words)
                    {
                        AddToKeywords(word, title);
                    }
                }
            }

            using (var searchesReader = new StreamReader(searches))
            {
                foreach (var line in Extensions.GetLines(searchesReader))
                {
                    Searches.Add(new Phrase(line));
                }
            }
        }

        public List<Phrase> Searches { get; set; } = new List<Phrase>();

        public Dictionary<string, List<Phrase>> Keywords
        {
            get { return _keywords; }
            set
            {
                foreach (var entry in value)
                {
                    AddToKeywords(entry.Key, entry.Value);
                }
            }
        }

        private void AddToKeywords(string key, Phrase phrase)
        {
            if (!_keywords.ContainsKey(key))
            {
                _keywords[key] = new List<Phrase> { phrase };
            }
            else if (!_keywords[key].Contains(phrase))
            {
                _keywords[key].Add(phrase);
            }
        }

        private void AddToKeywords(string key, List<Phrase> phrases)
        {
            if (!_keywords.ContainsKey(key))
            {
                _keywords[key] = phrases;
            }
            else
            {
                _keywords[key] = _keywords[key].Union(phrases).ToList();
            }
        }
    }
}
