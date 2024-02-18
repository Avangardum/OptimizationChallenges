using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp.Iterations;

/// <summary>
/// From Iteration4.<br/>
/// In CountLivingNeighbors added caching of loop bounds.<br/>
/// Runtime decreased for 1000 and 10000 generations, but not for 10 generations, all on a 100x100 grid.
/// </summary>
internal static class Iteration5
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
        var mOldGrid = inputGrid;
        var mNewGrid = new bool[mOldGrid.GetLength(0), mOldGrid.GetLength(1)];
        for (var mGeneration = 0; mGeneration < generations; mGeneration++)
        {
            RunGeneration(mOldGrid, mNewGrid);
            (mOldGrid, mNewGrid) = (mNewGrid, mOldGrid);
        }
        return mOldGrid;
    }

    private static void RunGeneration(bool[,] inputGrid, bool[,] outputGrid)
    {
        var iRowCount = inputGrid.GetLength(0);
        var iColumnCount = inputGrid.GetLength(1);
        for (var mRow = 0; mRow < iRowCount; mRow++)
        {
            for (var mColumn = 0; mColumn < iColumnCount; mColumn++)
            {
                var iInputCellLivingNeighbors = CountLivingNeighbors(inputGrid, mRow, mColumn);
                var iIsInputCellAlive = inputGrid[mRow, mColumn];
                var iIsOutputCellAlive = iIsInputCellAlive ? 
                    iInputCellLivingNeighbors is 2 or 3 : 
                    iInputCellLivingNeighbors is 3;
                outputGrid[mRow, mColumn] = iIsOutputCellAlive;
            }
        }
    }

    private static int CountLivingNeighbors(bool[,] grid, int row, int column)
    {
        var iLastRow = grid.GetLength(0) - 1;
        var iLastColumn = grid.GetLength(1) - 1;
        var mLivingNeighbors = 0;
        var firstNeighborRow = Math.Max(row - 1, 0);
        var lastNeighborRow = Math.Min(row + 1, iLastRow);
        var firstNeighborColumn = Math.Max(column - 1, 0);
        var lastNeighborColumn = Math.Min(column + 1, iLastColumn);
        for (int neighborRow = firstNeighborRow; neighborRow <= lastNeighborRow; neighborRow++)
        {
            for (int neighborColumn = firstNeighborColumn; neighborColumn <= lastNeighborColumn; neighborColumn++)
            {
                if (neighborRow == row && neighborColumn == column) continue;
                if (grid[neighborRow, neighborColumn]) mLivingNeighbors++;
            }
        }
        return mLivingNeighbors;
    }
}