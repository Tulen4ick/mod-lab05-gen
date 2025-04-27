using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Text;

namespace generator
{
    public class CharGenerator
    {
        private List<string> syms = new List<string>();
        private List<int> weights = new List<int>();
        public readonly int summ;
        private Random random = new Random();
        public CharGenerator(string charsfile)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var directory = new DirectoryInfo(currentDir);
            while (directory != null && !directory.GetFiles("*.csproj").Any())
            {
                directory = directory.Parent;
            }
            var bigramsAndWeights = File.ReadAllLines(Path.Combine(directory.FullName, charsfile));
            foreach (var bigraminfo in bigramsAndWeights)
            {
                var parts = bigraminfo.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var bigram = parts[1].Trim();
                syms.Add(bigram);
                var weight = int.Parse(parts[2].Trim());
                weights.Add(weight);
                summ += weight;
            }
        }
        public string getSym()
        {
            var indice = random.Next(0, summ);
            int sum = 0;
            for (int i = 0; i < weights.Count; ++i)
            {
                sum += weights[i];
                if (indice <= sum)
                {
                    return syms[i];
                }
            }
            return "";
        }
        public int GetSize()
        {
            return syms.Count;
        }
        public int GetBigrammWeight(string bigram)
        {
            return weights[syms.IndexOf(bigram)];
        }
    }

    public class WordGenerator
    {
        private List<string> words = new List<string>();
        private List<double> weights = new List<double>();
        public readonly double summ;
        private Random random = new Random();
        public WordGenerator(string wordsfile)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var directory = new DirectoryInfo(currentDir);
            while (directory != null && !directory.GetFiles("*.csproj").Any())
            {
                directory = directory.Parent;
            }
            var wordsAndWeights = File.ReadAllLines(Path.Combine(directory.FullName, wordsfile));
            foreach (var wordinfo in wordsAndWeights)
            {
                var parts = wordinfo.Replace('.', ',').Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                var word = parts[1].Trim();
                words.Add(word);
                var weight = double.Parse(parts[4].Trim());
                weights.Add(weight);
                summ += weight;
            }
        }
        public string getSym()
        {
            int indice = random.Next(0, (Int32)summ);
            double sum = 0;
            for (int i = 0; i < weights.Count; ++i)
            {
                sum += weights[i];
                if (indice <= (Int32)sum)
                {
                    return words[i];
                }
            }
            return "";
        }
        public int GetSize()
        {
            return words.Count;
        }
        public double GetWordWeight(string word)
        {
            return weights[words.IndexOf(word)];
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var directory = new DirectoryInfo(currentDir);
            while (directory != null && !directory.GetFiles("*.csproj").Any())
            {
                directory = directory.Parent;
            }
            CharGenerator genBi = new CharGenerator("bigramsAndWeights.txt");
            SortedDictionary<string, int> statBi = new SortedDictionary<string, int>();
            string result = "";
            for (int i = 0; i < 1000; i++)
            {
                string ch = genBi.getSym();
                result += ch;
                if (statBi.ContainsKey(ch))
                    statBi[ch]++;
                else
                    statBi.Add(ch, 1); Console.Write(ch);
            }
            Console.Write('\n');
            var resultPath1 = Path.Combine(directory.Parent.FullName, "Results", "gen-1.txt");
            File.WriteAllText(resultPath1, result, Encoding.UTF8);

            WordGenerator genWord = new WordGenerator("wordsAndWeights.txt");
            SortedDictionary<string, int> statWord = new SortedDictionary<string, int>();
            result = "";
            for (int i = 0; i < 1000; i++)
            {
                string ch = genWord.getSym();
                result += ch + " ";
                if (statWord.ContainsKey(ch))
                    statWord[ch]++;
                else
                    statWord.Add(ch, 1); Console.Write(ch);
            }
            Console.Write('\n');
            var resultPath2 = Path.Combine(directory.Parent.FullName, "Results", "gen-2.txt");
            File.WriteAllText(resultPath2, result, Encoding.UTF8);

            var graphicData1 = Path.Combine(directory.Parent.FullName, "Results", "bigramms_data.txt");
            List<string> dataBi = new List<string>();
            foreach (KeyValuePair<string, int> entry in statBi)
            {
                string data = entry.Key + " " + (entry.Value / 1000.0).ToString() + " " + ((Double)genBi.GetBigrammWeight(entry.Key) / genBi.summ).ToString();
                dataBi.Add(data);
            }
            File.WriteAllLines(graphicData1, dataBi, Encoding.UTF8);

            var graphicData2 = Path.Combine(directory.Parent.FullName, "Results", "words_data.txt");
            List<string> dataWord = new List<string>();
            foreach (KeyValuePair<string, int> entry in statWord)
            {
                string data = entry.Key + " " + (entry.Value / 1000.0).ToString() + " " + ((Double)genWord.GetWordWeight(entry.Key) / genWord.summ).ToString();
                dataWord.Add(data);
            }
            File.WriteAllLines(graphicData2, dataWord, Encoding.UTF8);
        }
    }
}

