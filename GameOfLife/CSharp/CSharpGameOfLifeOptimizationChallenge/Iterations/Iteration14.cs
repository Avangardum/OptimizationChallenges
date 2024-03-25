using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp.Iterations;

/// <summary>
/// From Iteration12.<br/>
/// Refactored code.
/// No runtime effect, but better readability.
/// --- Best ---
/// </summary>
internal static unsafe class Iteration14
{
    private class Grid
    {
        private readonly bool* _cells;

        public Grid(int rowCount, int columnCount, bool* cells)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            _cells = cells;
        }
        
        public int RowCount { get; }
        public int ColumnCount { get; }
        
        public bool this[int row, int column]
        {
            get => _cells[row * ColumnCount + column];
            set => _cells[row * ColumnCount + column] = value;
        }
    }
    
    const char DeadCellChar = '0';
    const char LivingCellChar = '1';

    public static void Run()
    {
        var (iInputRawGrid, iGenerations) = ReadInput();
        var iRowCount = iInputRawGrid.GetLength(0);
        var iColumnCount = iInputRawGrid.GetLength(1);
        var iPrimaryGridArray = new bool[iRowCount * iColumnCount];
        var iSecondaryGridArray = new bool[iRowCount * iColumnCount];
        fixed (bool* iPrimaryGridCells = iPrimaryGridArray, iSecondaryGridCells = iSecondaryGridArray)
        {
            CopyRawGridToGridCells(iInputRawGrid, iPrimaryGridCells);
            var iPrimaryGrid = new Grid(iRowCount, iColumnCount, iPrimaryGridCells);
            var iSecondaryGrid = new Grid(iRowCount, iColumnCount, iSecondaryGridCells);
            var iOutputGrid = RunSimulation(iPrimaryGrid, iSecondaryGrid, iGenerations);
            WriteOutput(iOutputGrid);
        }
    }

    private static void CopyRawGridToGridCells(bool[,] rawGrid, bool* gridCells)
    {
        var iRowCount = rawGrid.GetLength(0);
        var iColumnCount = rawGrid.GetLength(1);
        for (var mRow = 0; mRow < iRowCount; mRow++)
        {
            for (var mColumn = 0; mColumn < iColumnCount; mColumn++)
            {
                gridCells[mRow * iColumnCount + mColumn] = rawGrid[mRow, mColumn];
            }
        }
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

    private static Grid RunSimulation(Grid primaryGrid, Grid secondaryGrid, int generations)
    {
        var mOldGrid = primaryGrid;
        var mNewGrid = secondaryGrid;
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
        Parallel.For(0, iRowCount, row =>
        {
            for (var mColumn = 0; mColumn < iColumnCount; mColumn++)
            {
                var iInputCellLivingNeighbors = CountLivingNeighbors(inputGrid, row, mColumn);
                var iIsInputCellAlive = inputGrid[row, mColumn];
                var iIsOutputCellAlive = iIsInputCellAlive ? 
                    iInputCellLivingNeighbors is 2 or 3 : 
                    iInputCellLivingNeighbors is 3;
                outputGrid[row, mColumn] = iIsOutputCellAlive;
            }
        });
    }

    private static int CountLivingNeighbors(Grid grid, int row, int column)
    {
        var iLastRow = grid.RowCount - 1;
        var iLastColumn = grid.ColumnCount - 1;
        var mLivingNeighbors = 0;
        var iFirstNeighborRow = Math.Max(row - 1, 0);
        var iLastNeighborRow = Math.Min(row + 1, iLastRow);
        var iFirstNeighborColumn = Math.Max(column - 1, 0);
        var iLastNeighborColumn = Math.Min(column + 1, iLastColumn);
        for (var mNeighborRow = iFirstNeighborRow; mNeighborRow <= iLastNeighborRow; mNeighborRow++)
        {
            for (var mNeighborColumn = iFirstNeighborColumn; mNeighborColumn <= iLastNeighborColumn; mNeighborColumn++)
            {
                if (mNeighborRow == row && mNeighborColumn == column) continue;
                if (grid[mNeighborRow, mNeighborColumn])
                {
                    mLivingNeighbors++;
                    if (mLivingNeighbors == 4) return 4;
                }
            }
        }
        return mLivingNeighbors;
    }
}