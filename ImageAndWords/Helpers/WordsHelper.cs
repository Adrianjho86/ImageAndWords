using HtmlAgilityPack;
using ImageAndWords.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ImageAndWords.Helpers
{
    public class WordsHelper
    {
        public RankedWords GetWordsFromUrl(string validUrl)
        {
            var document = new HtmlWeb().Load(validUrl);
            IEnumerable<string> stringsInUrl = document.DocumentNode.DescendantsAndSelf("body")
                        .Select(x => RemoveSpecialChars(x.InnerText));
            List<string> wordsUrl = new List<string>();
            foreach (var str1 in stringsInUrl)
            {
                wordsUrl.AddRange(str1.ToLower().Split(';').Where(w => w.Length > 3).ToList());
            }
            Dictionary<string, int> wordsDictionary = new Dictionary<string, int>();
            foreach (string str2 in wordsUrl)
            {
                if (wordsDictionary.ContainsKey(str2))
                    wordsDictionary[str2] = wordsDictionary[str2] + 1;
                else
                    wordsDictionary[str2] = 1;
            }
            RankedWords rankWordsInfo = new RankedWords
            {
                TotalWords = wordsUrl.Count,
                TotalDistinct = wordsDictionary.Count,
                WordsInWeb = wordsDictionary.OrderByDescending(v => v.Value)
                            .Take(10)
                            .ToDictionary(d => d.Key, d => d.Value)
            };
            return rankWordsInfo;
        }


        private string RemoveSpecialChars(string node)
        {
            string removeFirst = Regex.Replace(node, "^[^a-zA-Z]+", "");
            string result = Regex.Replace(removeFirst, "[^a-zA-Z]+", ";");
            return result;
        }
    }
}