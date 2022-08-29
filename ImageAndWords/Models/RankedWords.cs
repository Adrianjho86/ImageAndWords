using System.Collections.Generic;

namespace ImageAndWords.Models
{
    public class RankedWords
    {
        public Dictionary<string, int> WordsInWeb { get; set; } //Key=word, Element=count
        public int TotalWords { get; set; }
        public int TotalDistinct { get; set; }
    }
}