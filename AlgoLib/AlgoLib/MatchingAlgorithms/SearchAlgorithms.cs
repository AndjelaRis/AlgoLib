﻿using System;
using System.IO;

namespace AlgoLib.MatchingAlgorithams
{
    class SearchAlgorithms
    {
        #region MatchingMethods
        public static void RabinKarp_Match(string text, string pattern, string fileOfIndexes, int numChar, int primeNumb)
        {
            string Text = ReadFile(text);
            string Pattern = ReadFile(pattern);

            int patternLength = Pattern.Length;
            int textLength = Text.Length;
            int patternHash = 0, textHash = 0;
            int i, j;

            int h = ((int)Math.Pow(numChar, patternLength - 1)) % primeNumb;

            for (i = 0; i < patternLength; i++)
            {
                patternHash = (patternHash * numChar + Pattern[i]) % primeNumb;
                textHash = (textHash * numChar + Text[i]) % primeNumb;
            }

            File.Delete(fileOfIndexes);

            for (i = 0; i <= textLength - patternLength; i++)
            {
                if (patternHash == textHash)
                {
                    for (j = 0; j < patternLength; j++)
                    {
                        if (Text[i + j] != Pattern[j])
                        {
                            break;
                        }

                    }
                    if (j == patternLength)
                    {
                        string index = "" + i;
                        WriteInFile(fileOfIndexes, index);
                    }
                }

                if (i < textLength - patternLength)
                {
                    textHash = (numChar * (textHash - Text[i] * h) + Text[i + patternLength]) % primeNumb;
                    if (textHash < 0)
                    {
                        textHash += primeNumb;
                    }
                }

            }


        }

        public static void KnuthMorrisPratt_Match(string text, string pattern, string fileOfIndexes)
        {
            string Text = ReadFile(text);
            string Pattern = ReadFile(pattern);

            int patternLength = Pattern.Length;
            int textLength = Text.Length;

            int[] PiArray = ComputePrefix(Pattern);
            int i = 0, j = 0;

            File.Delete(fileOfIndexes);

            while (j < textLength)
            {
                if (Pattern[i] != Text[j])
                {
                    if (i == 0)
                    {
                        j++;
                    }
                    else
                    {
                        i = PiArray[i - 1];
                    }
                }
                else
                {
                    i++; j++;
                    if (i == patternLength)
                    {
                        string index = "" + (j - i);
                        WriteInFile(fileOfIndexes, index);
                        i = PiArray[i - 1];
                    }
                }

            }

        }

        public static void SoundEx(string text, string pattern, string sameCodeWords)
        {
            string Text = ReadFile(text);
            string Pattern = ReadFile(pattern);


            string patternCode = GetSoundExCode(Pattern);
            string textCode = GetSoundExCode(Text);
            Console.WriteLine(patternCode);

        }

        public static void Lavenshtein()
        {

        }

        #endregion


        #region HelperMethods
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

        public static void WriteInFile(string textPath, string index)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(textPath))
                {
                    sw.WriteLine(index);
                }

            }
            catch (IOException error)
            {
                Console.WriteLine("Error: Couldn't write in file");
                Console.WriteLine(error.Message);
            }
        }

        public static int[] ComputePrefix(string Pattern)
        {
            int patternLength = Pattern.Length;
            int[] PiArray = new int[patternLength];
            PiArray[0] = 0;
            int indexOfMatch = 0, j = 1;

            while (j < patternLength)
            {
                if (Pattern[indexOfMatch] == Pattern[j])
                {
                    indexOfMatch++;
                    PiArray[j] = indexOfMatch;
                    j++;
                }
                if (indexOfMatch != 0 && Pattern[indexOfMatch] != Pattern[j])
                {
                    indexOfMatch = PiArray[indexOfMatch - 1];
                }
                else
                {
                    PiArray[j] = 0;
                    j++;
                }
            }

            return PiArray;
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
            string textCode = "";
            textCode += textUpper[0];
            int patternNumOfDig = 1, i;

            char[] notNeedLetters = { 'A', 'E', 'I', 'O', 'U', 'H', 'W', 'Y' };
            char[] oneLetters = { 'B', 'F', 'P', 'V' };
            char[] twoLetters = { 'C', 'G', 'J', 'K', 'Q', 'S', 'X', 'Z' };
            char[] threeLetters = { 'D', 'T' };
            char[] fiveLetters = { 'M', 'N' };
            for (i = 0; i < notNeedLetters.Length; i++)
            {
                textUpper = String.Concat(textUpper.Split(notNeedLetters[i]));
            }

            for (i = 1; i < textUpper.Length; i++)
            {
                int j;
                for (j = 0; j < oneLetters.Length; j++)
                {
                    if (textUpper[i] == oneLetters[j])
                    {
                        textCode += "1";
                    }
                }

               // for ()


            }

            return textUpper;
        }

        #endregion
    }
}
