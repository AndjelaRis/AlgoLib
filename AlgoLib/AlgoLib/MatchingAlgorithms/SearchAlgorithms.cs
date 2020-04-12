using System;
using System.Collections.Generic;
using System.IO;

namespace AlgoLib.MatchingAlgorithams
{
    public static class SearchAlgorithms
    {
        public const string outputFileName = "/Indexes.txt";


        public static void RabinKarp_Match(string textToCompare, string patternToCompare, int numberOfCharacters, int primeNumber)
        {
            string text = ReadFile(textToCompare);
            string pattern = ReadFile(patternToCompare);
            int patternHash = 0, textHash = 0;
            int charWeight = ((int)Math.Pow(numberOfCharacters, pattern.Length - 1)) % primeNumber;

            List<int> patternIndexes = new List<int>(text.Length - pattern.Length);

            for (int i = 0; i < pattern.Length; i++)
            {
                patternHash = (patternHash * numberOfCharacters + pattern[i]) % primeNumber;
                textHash = (textHash * numberOfCharacters + text[i]) % primeNumber;
            }
            for (int i = 0; i <= text.Length - pattern.Length; i++)
            {
                if (patternHash == textHash && BruteForceComparison(text, pattern, i))
                    patternIndexes.Add(i);
                if (i == text.Length-pattern.Length)
                    break;
                textHash = ((textHash - text[i] * charWeight) * numberOfCharacters + text[i + pattern.Length]) % primeNumber;
                if (textHash < 0)
                    textHash += primeNumber;
            }
            WriteInFile(Directory.GetCurrentDirectory()+outputFileName,patternIndexes);
        }

        public static void KnuthMorrisPratt_Match(string textToCompare, string patternToCompare)
        {
            string text = ReadFile(textToCompare);
            string pattern = ReadFile(patternToCompare);
            List<int> prefixArray = CalculatePrefix(pattern);
            List<int> patternIndexes = new List<int>(text.Length - pattern.Length);
            int i = 0, j = 0;

            while (j < text.Length)
            {
                if (pattern[i] != text[j])
                    if (i == 0)
                        j++;
                    else
                        i = prefixArray[i - 1];
                else
                {
                    i++; j++;
                    if (i == pattern.Length)
                    {
                        patternIndexes.Add(j - i);
                        i = prefixArray[i - 1];
                    }
                }
            }
            WriteInFile(Directory.GetCurrentDirectory() + outputFileName, patternIndexes);
        }

        public static void SoundEx(string textFilePath, string patternFilePath)
        {
            string text = ReadFile(textFilePath);
            string pattern = ReadFile(patternFilePath);
            string patternCode = GetSoundExCode(pattern);
            List<string> result = new List<string>();

            char[] separator = { ' ', '.', ',', '?', '!', '"', ':', ';', '\n', '\t', '\r', '_', '-' };
            string[] textStrings = text.Split(separator);

            for (int i = 0; i < textStrings.Length; i++)
            {
                string textCode;
                if (string.Compare(string.Empty, textStrings[i]) == 0)
                    continue;
                textCode = GetSoundExCode(textStrings[i]);
                if (string.Compare(patternCode, textCode) == 0)
                    result.Add(textStrings[i]);
            }

            WriteInFile(Directory.GetCurrentDirectory() + outputFileName, textStrings);
        }

        public static void LavenshteinDistance(string textToCompare, string patternToCompare)
        {
            string text = ReadFile(textToCompare);
            string pattern = ReadFile(patternToCompare);

        }

        public static bool BruteForceComparison(string stringToCompare, string pattern, int startingIndex)
        {
            int i;
            for (i = 0; i < pattern.Length; i++)
                if (stringToCompare[startingIndex + i] != pattern[i])
                    break;
            if (i == pattern.Length)
                return true;
            return false;
        }


        #region HelperMethods

        public static int CalculateLavenshteinDistance(string firstString, string secondString)
        {
            int[,] distanceMatrix = new int[firstString.Length + 1, secondString.Length + 1];
            for (int i = 0; i < firstString.Length +1; i++)
                for (int j = 0; j < secondString.Length + 1; j++)
                    distanceMatrix[i, j] = 0;
            for (int i = 0; i < firstString.Length + 1; i++)
                distanceMatrix[i, 0] = i;
            for (int i = 0; i < secondString.Length + 1; i++)
                distanceMatrix[0, i] = i;

            for (int i = 0; i < secondString.Length; i++)
                for (int j = 0; j < firstString.Length; j++)
                {
                    int substitutionCost;
                    if (firstString[j] == secondString[i])
                        substitutionCost = 0;
                    else
                        substitutionCost = 1;
                    distanceMatrix[j+1, i+1] = getMin(distanceMatrix[j, i + 1] + 1,
                                distanceMatrix[j + 1, i] + 1,
                                distanceMatrix[j, i] + substitutionCost);
                }

            return distanceMatrix[firstString.Length, secondString.Length];
        }


        private static List<int> CalculatePrefix(string pattern)
        {
            List<int> prefixArray = new List<int>(pattern.Length);
            for (int i = 0; i < pattern.Length; i++)
                prefixArray.Add(0);
            int indexOfMatch = 0, j = 1;

            while (j < pattern.Length)
            {
                if (pattern[indexOfMatch] == pattern[j])
                    prefixArray[j++] = ++indexOfMatch;
                if (indexOfMatch != 0 && pattern[indexOfMatch] != pattern[j])
                    indexOfMatch = prefixArray[indexOfMatch - 1];
                else
                    prefixArray[j++] = 0;
            }
            return prefixArray;
        }

        public static string ReadFile(string textPath)
        {
            try
            {
                if (File.Exists(textPath))
                {
                    string text = File.ReadAllText(textPath);
                    return text;
                }
            }
            catch (IOException error)
            {
                Console.WriteLine("Error: file doesn't exist or wrong path");
                Console.WriteLine(error.Message);
            }

            return "";
        }

        public static void WriteInFile(string path, IEnumerable<object> objectList)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path, false))
                {
                    foreach (int index in objectList)
                    {
                        streamWriter.WriteLine(index.ToString());
                    }
                }
            }
            catch (IOException error)
            {
                Console.WriteLine("Error: Couldn't write in file");
                Console.WriteLine(error.Message);
            }
        }

        

        public static string EncodeChar(char c)
        {
            switch (Char.ToLower(c))
            {
                case 'B':
                case 'F':
                case 'P':
                case 'V':
                    return "1";
                case 'C':
                case 'G':
                case 'J':
                case 'K':
                case 'Q':
                case 'S':
                case 'X':
                case 'Z':
                    return "2";
                case 'D':
                case 'T':
                    return "3";
                case 'L':
                    return "4";
                case 'M':
                case 'N':
                    return "5";
                case 'R':
                    return "6";
                case 'H':
                case 'W':
                case 'Y':
                    return "0";
                default:
                    return string.Empty;
            }
        }

        public static string GetSoundExCode(string text)
        {
            string textUpper = text.ToUpper();
            string textCode = string.Empty;
            textCode += textUpper[0];
            int patternNumOfDig = 1, i;

            for (i = 1; i < text.Length; i++)
            {
                string encodedChar = EncodeChar(textUpper[i]);
                if (encodedChar != "0" && encodedChar != EncodeChar(textUpper[i - 1]))
                    textCode += encodedChar;
            }

            if (textCode.Length >= 4)
                textCode = textCode.Substring(0, 4);
            else
                textCode += new string('0', 4 - textCode.Length);

            return textCode.ToString();
        }

       
        public static int getMin(params int[] integers)
        {
            int min = int.MaxValue;
            foreach (int singleInt in integers)
                if (min >= singleInt)
                    min = singleInt;
            return min;
        }
        
        #endregion
    }
}
