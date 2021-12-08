using Rabo.RiskTest.Interfaces;
using System.Text.RegularExpressions;

namespace Rabo.RiskTest
{
    public class WordFrequencyAnalyzer : IWordFrequencyAnalyzer
    {
        private const string wordPattern = @"\b[a-zA-Z]+\b";

        public int CalculateFrequencyForWord(string text, string word)
        {
            var isWordRegex = new Regex(wordPattern);
            if (!isWordRegex.IsMatch(word))
            {
                return 0;
            }

            var pattern = @$"\b{word}\b";
            var rx = new Regex(pattern, RegexOptions.IgnoreCase);
            var matches = rx.Matches(text);
            return matches.Count;
        }

        public int CalculateHighestFrequency(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            var wordFrequencies = CalculateWordFrequencies(text);

            if (!wordFrequencies.Any())
            {
                return 0;
            }

            return wordFrequencies.First().Frequency;
        }

        public IList<IWordFrequency> CalculateMostFrequentNWords(string text, int n)
        {
            var wordfrequencies = CalculateWordFrequencies(text);

            return wordfrequencies.Take(n).ToList();
        }

        private static IEnumerable<IWordFrequency> CalculateWordFrequencies(string text)
        {
            var regex = new Regex(wordPattern, RegexOptions.IgnoreCase);

            var matches = regex.Matches(text);

            return matches.GroupBy(m => m.Value.ToLower())
                    .Select(m => new WordFrequency(m.Key, m.Count()))
                    .OrderBy(wf => wf.Word)
                    .OrderByDescending(wf => wf.Frequency)
                    .ToList();
        }
    }
}
