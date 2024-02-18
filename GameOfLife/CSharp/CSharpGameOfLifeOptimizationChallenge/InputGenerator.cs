using System.Text;
using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp;

internal static class InputGenerator
{
    public static void GenerateInput(int size, int generations)
    {
        var iRandom = new Random(42);
        var inputFileBuilder = new StringBuilder();
        inputFileBuilder.AppendLine(generations.ToString());
        for (int mRow = 0; mRow < size; mRow++)
        {
            for (int mColumn = 0; mColumn < size; mColumn++)
            {
                inputFileBuilder.Append(iRandom.Next(2));
            }
            if (mRow < size - 1) inputFileBuilder.AppendLine();
        }
        File.WriteAllText(InputPath, inputFileBuilder.ToString());
    }
}