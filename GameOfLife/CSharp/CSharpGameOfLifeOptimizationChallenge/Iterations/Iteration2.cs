using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp.Iterations;

/// <summary>
/// From Iteration1.<br/>
/// Replaced writing char by char to a StringBuilder and then writing to a file with writing char by char directly
/// to a file.<br/>
/// No noticeable runtime effect.<br/>
/// Memory allocation is reduced by 1.69x at 1 iteration, by 1.04x at 100 iterations, all on a 100x100 grid.
/// </summary>
internal static class Iteration2
{
    const char DeadCellChar = '0';
    const char LivingCellChar = '1';

    public static void Run()
    {
        var (iInputGrid, iGenerations) = ReadInput();
        var iOutputGrid = RunSimulation(iInputGrid, iGenerations);
        WriteOutput(iOutputGrid);
    }

    private static (bool[,] InputGrid, int Genereations) ReadInput()
    {
        var iInputLines = File.ReadAllLines(InputPath);
        var iGenerations = int.Parse(iInputLines[0]);
        var iColumnCount = iInputLines[1].Length;
        var iRowCount = iInputLines.Length - 1;
        var iInputGrid = new bool[iRowCount, iColumnCount];
        for (var mRow = 0; mRow < iRowCount; mRow++)
        {
            for (var mColumn = 0; mColumn < iColumnCount; mColumn++)
            {
                iInputGrid[mRow, mColumn] = iInputLines[mRow + 1][mColumn] == LivingCellChar;
            }
        }
        return (iInputGrid, iGenerations);
    }

    private static void WriteOutput(bool[,] outputGrid)
    {
        var iRowCount = outputGrid.GetLength(0);
        var iColumnCount = outputGrid.GetLength(1);
        using var iWriter = new StreamWriter(OutputPath);
        for (int mRow = 0; mRow < iRowCount; mRow++)
        {
            for (int mColumn = 0; mColumn < iColumnCount; mColumn++) 
                iWriter.Write(outputGrid[mRow, mColumn] ? LivingCellChar : DeadCellChar);
            if (mRow < iRowCount - 1) iWriter.WriteLine();
        }
    }

    private static bool[,] RunSimulation(bool[,] inputGrid, int generations)
    {
        bool[,] mGrid = inputGrid;
        for (var mGeneration = 0; mGeneration < generations; mGeneration++) mGrid = RunGeneration(mGrid);
        return mGrid;
    }

    private static bool[,] RunGeneration(bool[,] inputGrid)
    {
        var iRowCount = inputGrid.GetLength(0);
        var iColumnCount = inputGrid.GetLength(1);
        var iOutputGrid = new bool[iRowCount, iColumnCount];
        for (var mRow = 0; mRow < iRowCount; mRow++)
        {
            for (var mColumn = 0; mColumn < iColumnCount; mColumn++)
            {
                var iInputCellLivingNeighbors = CountLivingNeighbors(inputGrid, mRow, mColumn);
                var iIsInputCellAlive = inputGrid[mRow, mColumn];
                var iIsOutputCellAlive = iIsInputCellAlive ? 
                    iInputCellLivingNeighbors is 2 or 3 : 
                    iInputCellLivingNeighbors is 3;
                iOutputGrid[mRow, mColumn] = iIsOutputCellAlive;
            }
        }
        return iOutputGrid;
    }

    private static int CountLivingNeighbors(bool[,] grid, int row, int column)
    {
        var iLastRow = grid.GetLength(0) - 1;
        var iLastColumn = grid.GetLength(1) - 1;
        var mLivingNeighbors = 0;
        for (int neighborRow = Math.Max(row - 1, 0); neighborRow <= Math.Min(row + 1, iLastRow); neighborRow++)
        {
            for (int neighborColumn = Math.Max(column - 1, 0); neighborColumn <= Math.Min(column + 1, iLastColumn); 
                 neighborColumn++)
            {
                if (neighborRow == row && neighborColumn == column) continue;
                if (grid[neighborRow, neighborColumn]) mLivingNeighbors++;
            }
        }
        return mLivingNeighbors;
    }
}