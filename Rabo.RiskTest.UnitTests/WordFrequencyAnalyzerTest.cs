using Rabo.RiskTest.Interfaces;
using System.Linq;
using Xunit;

namespace Rabo.RiskTest.UnitTests
{
    public class WordFrequencyAnalyzerTest
    {
        private readonly IWordFrequencyAnalyzer wordFrequencyAnalyzer;

        public WordFrequencyAnalyzerTest()
        {
            wordFrequencyAnalyzer = new WordFrequencyAnalyzer();
        }

        [Theory]
        [InlineData("", "test", 0)]
        [InlineData("!@#$%^&*()", "test", 0)]
        [InlineData("one two", "test", 0)]
        [InlineData("t3st", "t3st", 0)]
        [InlineData("tEst.test,test$test", "test", 4)]
        [InlineData("test testtest", "test", 1)]
        [InlineData("test-test(test)test2test", "test", 3)]
        public void CalculateFrequencyForWord_ReturnsCorrectCount(string text, string word, int expected)
        {
            // Act
            var count = wordFrequencyAnalyzer.CalculateFrequencyForWord(text, word);

            // Assert
            Assert.Equal(expected, count);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("!@#$%^&*(", 0)]
        [InlineData("Test teSt", 2)]
        [InlineData("The sun shines over the lake", 2)]
        [InlineData("one.two two,one three one", 3)]
        public void CalculateHighestFrequency_ReturnHighestFrequency(string text, int expected)
        {
            // Act
            var count = wordFrequencyAnalyzer.CalculateHighestFrequency(text);

            // Assert
            Assert.Equal(expected, count);
        }

        [Fact]
        public void CalculateMostFrequentNWords_ReturnsFrequenciesWithCorrectCount()
        {
            // Arrange
            var text = "one two three one two one";

            // Act
            var wordFrequencies = wordFrequencyAnalyzer.CalculateMostFrequentNWords(text, 2);

            // Assert
            Assert.Equal(2, wordFrequencies.Count);
            Assert.Equal("one", wordFrequencies.First().Word);
            Assert.Equal(3, wordFrequencies.First().Frequency);
            Assert.Equal("two", wordFrequencies.Skip(1).First().Word);
            Assert.Equal(2, wordFrequencies.Skip(1).First().Frequency);
        }

        [Fact]
        public void CalculateMostFrequentNWords_ReturnsFrequenciesInCorrectOrder()
        {
            // Arrange
            var text = "one three four two one four three two four three";

            // Act
            var wordFrequencies = wordFrequencyAnalyzer.CalculateMostFrequentNWords(text, 2);

            // Assert
            Assert.Equal(2, wordFrequencies.Count);

            var firstWord = wordFrequencies.First();
            var secondWord = wordFrequencies.Skip(1).First();

            Assert.Equal("four", firstWord.Word);
            Assert.Equal(3, firstWord.Frequency);

            Assert.Equal("three", secondWord.Word);
            Assert.Equal(3, secondWord.Frequency);
        }

        [Fact]
        public void CalculateMostFrequentNWords_ReturnsEmptyList_WhenTextContainsNoWords()
        {
            // Arrange
            var text = string.Empty;

            // Act
            var wordFrequencies = wordFrequencyAnalyzer.CalculateMostFrequentNWords(text, 2);

            // Assert
            Assert.Equal(0, wordFrequencies.Count);
        }
    }
}