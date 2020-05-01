namespace AlgoLib.GreedyAlgorithms
{
    public interface IService
    {
        void Encode(string sourcePath, string outputPath);

        void Decode(string inputPath, string outputPath);
    }
}
