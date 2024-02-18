using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp.Iterations;

/// <summary>
/// From Iteration5.<br/>
/// Replaced bool[,] with a custom Grid class that uses bool[,] internally in order to be able to customize the
/// implementation of the grid in further iterations.<br/>
/// No consitent runtime effect.<br/>
/// Small memory allocation increase.
/// </summary>
internal static class Iteration6
{
    private class Grid
    {
        private readonly bool[,] _cells;

        public Grid(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            _cells = new bool[rowCount, columnCount];
        }
        
        public int RowCount { get; }
        public int ColumnCount { get; }
        
        public bool this[int row, int column]
        {
            get => _cells[row, column];
            set => _cells[row, column] = value;
        }
    }
    
    const char DeadCellChar = '0';
    const char LivingCellChar = '1';

    public static void Run()
    {
        var (iInputGrid, iGenerations) = ReadInput();
        var iOutputGrid = RunSimulation(iInputGrid, iGenerations);
        WriteOutput(iOutputGrid);
    }

    private static (Grid InputGrid, int Genereations) ReadInput()
    {
        var iInputLines = File.ReadAllLines(InputPath);
        var iGenerations = int.Parse(iInputLines[0]);
        var iColumnCount = iInputLines[1].Length;
        var iRowCount = iInputLines.Length - 1;
        var iInputGrid = new Grid(iRowCount, iColumnCount);
        for (var mRow = 0; mRow < iRowCount; mRow++)
        {
            for (var mColumn = 0; mColumn < iColumnCount; mColumn++)
            {
                iInputGrid[mRow, mColumn] = iInputLines[mRow + 1][mColumn] == LivingCellChar;
            }
        }
        return (iInputGrid, iGenerations);
    }

    private static void WriteOutput(Grid outputGrid)
    {
        var iRowCount = outputGrid.RowCount;
        var iColumnCount = outputGrid.ColumnCount;
        using var iWriter = new StreamWriter(OutputPath);
        for (int mRow = 0; mRow < iRowCount; mRow++)
        {
            for (int mColumn = 0; mColumn < iColumnCount; mColumn++)
                iWriter.Write(outputGrid[mRow, mColumn] ? LivingCellChar : DeadCellChar);
            if (mRow < iRowCount - 1) iWriter.WriteLine();
        }
    }

    private static Grid RunSimulation(Grid inputGrid, int generations)
    {
        var mOldGrid = inputGrid;
        var mNewGrid = new Grid(mOldGrid.RowCount, mOldGrid.ColumnCount);
        for (var mGeneration = 0; mGeneration < generations; mGeneration++)
        {
            RunGeneration(mOldGrid, mNewGrid);
            (mOldGrid, mNewGrid) = (mNewGrid, mOldGrid);
        }
        return mOldGrid;
    }

    private static void RunGeneration(Grid inputGrid, Grid outputGrid)
    {
        var iRowCount = inputGrid.RowCount;
        var iColumnCount = inputGrid.ColumnCount;
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

    private static int CountLivingNeighbors(Grid grid, int row, int column)
    {
        var iLastRow = grid.RowCount - 1;
        var iLastColumn = grid.ColumnCount - 1;
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