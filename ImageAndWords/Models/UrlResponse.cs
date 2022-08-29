using System.Collections.Generic;

namespace ImageAndWords.Models
{
    public class UrlResponse
    {
        public bool IsValid { get; set; }
        public string NotValidMessage { get; set; }
        public string Scheme { get; set; }
        public string HostPath { get; set; }
        public IEnumerable<string> ImagesUrls { get; set; }
        public RankedWords RankedWords { get; set; }
    }
}