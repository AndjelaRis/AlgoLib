using System;
using System.Diagnostics;

namespace AlgoLib.GreedyAlgorithms
{
    public static class ReportEngine
    {
        public static void TestCompression(IService service, string sourcePath)
        {
            string outputFilePath = GenerateOutputFilePath(sourcePath,"Encoded");
            Stopwatch timer = new Stopwatch();
            timer.Start();
            service.Encode(sourcePath, outputFilePath);
            timer.Stop();
            long initialSize = new System.IO.FileInfo(sourcePath).Length;
            long compressedSize = new System.IO.FileInfo(outputFilePath).Length;
            Console.WriteLine($@"File was compressed in {timer.ElapsedMilliseconds / 1000} seconds by the {(1 - compressedSize / (double)initialSize) * 100} %");
        }

        public static void TestDecompression(IService service, string sourcePath)
        {
            string outputFilePath = GenerateOutputFilePath(sourcePath,"Decoded");
            Stopwatch timer = new Stopwatch();
            timer.Start();
            service.Decode(sourcePath, outputFilePath);
            timer.Stop();
            Console.WriteLine($@"File was decompressed in {timer.ElapsedMilliseconds / 1000} seconds");
        }

        private static string GenerateOutputFilePath(string sourcePath, string prefix)
        {
            int fileNameLocation = sourcePath.LastIndexOf('\\');
            string sourceFileName = prefix + sourcePath.Substring(fileNameLocation + 1, sourcePath.Length - fileNameLocation - 1);
            return $@"C:\Users\Uros Aleksandrovic\source\repos\PAA_LAB3\PAA_LAB3\Output\{sourceFileName}";
        }
    }
}
