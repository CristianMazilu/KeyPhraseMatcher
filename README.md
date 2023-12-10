## Solution to the following problem:

Suppose you have two text datasets, composed of phrases of around 6 words each:
1. A 1 million search phrases dataset, and
2. A 30 million title phrases dataset.

You would like to precisely determine all the search-to-title correspondences that satisfy the following condition: the title phrase must contain at least all search phrase words at least once.

## Possible approaches:
Note that: n - number of searches, m - number of titles, u - number of unique words in the dataset.

# Brute force
Checking word by word, all pairings of search and title phrases.
Implemented, works for up to 1000 searches more or less.
Big O: n * m
Per comparison time: Long, each comparison has to check all words in the search phrase versus almost all words in the title.

# Inverted index
For each unique word, create a list of all titles phrases that contain it. Then for each search phrase, do a intersect operation on the sets of title phrases corresponding to each word in the search phrase.
Implemented, works with up to 100 000 searches, time: 1 hour.
Big O:
a. Index creation: m * number of words in title phrase (
b. Search querying: n * number of words in title phrase * number of title phrases per word
Per comparison time: Potentially better, as it uses the intersect operation of multiple HashSets.

# Bitmask approach
Just like the inverted index approach, but instead of storing lists of phrases, store a bit array in which each of the 30 000 000 bits tells which title phrase a word can be found in. The intersect operation would be a series of bitwise AND gates, which should execute faster.
(not implemented, but taken into consideration)
Big O: Same as inverted index.
Per comparison time: Should be significantly faster, as it is based on bit operation AND applied consecutively on each bit array. However the size of the bit array might be concerning.
Dealbreaker: For an estimated 100 000 unique words in a dataset, and 30 million titles, the collective bit array sizes would surpass a couple hundred gigabytes.
