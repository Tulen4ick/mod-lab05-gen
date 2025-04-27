using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using generator;
using System.Linq;

namespace GeneratorTests
{
    [TestClass]
    public class CharGeneratorTests
    {
        private string _testDataPath;

        [TestInitialize]
        public void Setup()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var directory = new DirectoryInfo(currentDir);
            while (directory != null && !directory.GetFiles("*.csproj").Any())
            {
                directory = directory.Parent;
            }
            _testDataPath = Path.Combine(directory.FullName, "TestData");
        }

        [TestMethod]
        public void CharGenerator_Initialization_ValidFile_LoadsData()
        {
            var generator = new CharGenerator(Path.Combine(_testDataPath, "valid_bigrams.txt"));
            Assert.AreEqual(3, generator.GetSize());
            Assert.AreEqual(150, generator.summ);
        }

        [TestMethod]
        public void CharGenerator_GetSym_ReturnsValidBigram()
        {
            var generator = new CharGenerator(Path.Combine(_testDataPath, "valid_bigrams.txt"));
            var result = generator.getSym();
            CollectionAssert.Contains(new[] { "ab", "cd", "ef" }, result);
        }

        [TestMethod]
        public void CharGenerator_GetBigramWeight_ReturnsCorrectValue()
        {
            var generator = new CharGenerator(Path.Combine(_testDataPath, "valid_bigrams.txt"));
            Assert.AreEqual(50, generator.GetBigrammWeight("ab"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CharGenerator_EmptyFile_ThrowsException()
        {
            new CharGenerator(Path.Combine(_testDataPath, "empty.txt"));
        }

        [TestMethod]
        public void CharGenerator_DistributionMatchesWeights()
        {
            var generator = new CharGenerator(Path.Combine(_testDataPath, "valid_bigrams.txt"));
            var results = Enumerable.Range(0, 1000).Select(_ => generator.getSym()).ToList();
            var abCount = results.Count(s => s == "ab");
            Assert.IsTrue(abCount > 300 && abCount < 400);
        }
    }

    [TestClass]
    public class WordGeneratorTests
    {
        private string _testDataPath;

        [TestInitialize]
        public void Setup()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var directory = new DirectoryInfo(currentDir);
            while (directory != null && !directory.GetFiles("*.csproj").Any())
            {
                directory = directory.Parent;
            }
            _testDataPath = Path.Combine(directory.FullName, "TestData");
        }

        [TestMethod]
        public void WordGenerator_Initialization_ValidFile_LoadsData()
        {
            var generator = new WordGenerator(Path.Combine(_testDataPath, "valid_words.txt"));
            Assert.AreEqual(2, generator.GetSize());
            Assert.AreEqual(8.5, generator.summ);
        }

        [TestMethod]
        public void WordGenerator_GetSym_ReturnsValidWord()
        {
            var generator = new WordGenerator(Path.Combine(_testDataPath, "valid_words.txt"));
            var result = generator.getSym();
            CollectionAssert.Contains(new[] { "apple", "banana" }, result);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WordGenerator_InvalidData_ThrowsException()
        {
            new WordGenerator(Path.Combine(_testDataPath, "invalid_words.txt"));
        }

        [TestMethod]
        public void WordGenerator_GetWordWeight_ReturnsCorrectValue()
        {
            var generator = new WordGenerator(Path.Combine(_testDataPath, "valid_words.txt"));
            Assert.AreEqual(3.5, generator.GetWordWeight("apple"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WordGenerator_ZeroWeights_ThrowsException()
        {
            new WordGenerator(Path.Combine(_testDataPath, "zero_weights.txt"));
        }
    }
}
