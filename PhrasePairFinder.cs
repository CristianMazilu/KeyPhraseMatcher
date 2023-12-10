using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPhraseMatcher
{
    public class PhrasePairFinder
    {
        private Dictionary<string, HashSet<Phrase>> _keywords = new Dictionary<string, HashSet<Phrase>>();

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

        public Dictionary<string, HashSet<Phrase>> Keywords
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

        public int MatchSearches()
        {
            int totalCount = 0;
            foreach (var search in Searches)
            {
                totalCount += MatchSearch(search);
            }

            return totalCount;
        }

        private int MatchSearch(Phrase search)
        {
            var titlesByKeyword = new List<HashSet<Phrase>>();
            foreach (var word in search.Words)
            {
                if (Keywords.ContainsKey(word))
                {
                    titlesByKeyword.Add(Keywords[word]);
                }
            }

            var commonTitles = titlesByKeyword
                .Skip(1)
                .Aggregate(
                    new HashSet<Phrase>(titlesByKeyword.FirstOrDefault() ?? Enumerable.Empty<Phrase>()),
                    (intersection, nextList) =>
                    {
                        intersection.IntersectWith(nextList);
                        return intersection;
                    }
                );

            return commonTitles.Count;
        }

        private void AddToKeywords(string key, Phrase phrase)
        {
            if (!_keywords.ContainsKey(key))
            {
                _keywords[key] = new HashSet<Phrase> { phrase };
            }
            else if (!_keywords[key].Contains(phrase))
            {
                _keywords[key].Add(phrase);
            }
        }

        private void AddToKeywords(string key, HashSet<Phrase> phrases)
        {
            if (!_keywords.ContainsKey(key))
            {
                _keywords[key] = phrases;
            }
            else
            {
                _keywords[key] = _keywords[key].Union(phrases).ToHashSet();
            }
        }
    }
}
