using AlgoLib.GreedyAlgorithms.ShannonFano;

namespace TestProject
{
    class Program
    {
       static void Main(string[] args)
        {
            /*HuffmanService service = new HuffmanService();
            HuffmanTree tree = new HuffmanTree();
            tree.Build(@"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\Huffman\Apology Plato.txt");
            tree.Encode(@"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\Huffman\Apology Plato.txt", @"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\Huffman\Encoded.txt");
            tree.Decode(@"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\Huffman\Encoded.txt", @"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\Huffman\Decoded.txt");*/
            //LZWService lZWService = new LZWService();
            //lZWService.Encode(@"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\LZW\Anatomy.txt", @"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\LZW\Encoded.txt");
            //lZWService.Decode(@"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\LZW\Encoded.txt", @"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\LZW\Decoded.txt");
            ShannonFannoTree tree = new ShannonFannoTree();
            tree.BuildTree(@"D:\Projects\AlgoLib\AlgoLib\AlgoLib\GreedyAlgorithms\Huffman\Test.txt");
        }
    }
}
