using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp.Iterations;

/// <summary>
/// From Iteration6.<br/>
/// Changed Grid to use a stack allocated span instead of a 2D array. But had to leave ReadInput using a 2D array
/// because it is incorrect to stack allocate a span there due to lifetime issues, and it is also not possible
/// to preemptively allocate it in Run due to the input size being unknown at that point.<br/>
/// Runtime decreased by 1.38x for 1000 generations and 100x100 grid.<br/>
/// Memory allocation decreased by 1.19x fir 10, 1000 and 10000 generations and 100x100 grid.
/// </summary>
internal static class Iteration7
{
    private readonly ref struct Grid
    {
        private readonly Span<bool> _cells;

        public Grid(int rowCount, int columnCount, Span<bool> cells)
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
        Span<bool> primaryGridCells = stackalloc bool[iRowCount * iColumnCount];
        CopyRawGridToGridCellsSpan(iInputRawGrid, primaryGridCells);
        var primaryGrid = new Grid(iRowCount, iColumnCount, primaryGridCells);
        var secondaryGrid = new Grid(iRowCount, iColumnCount, stackalloc bool[iRowCount * iColumnCount]);
        var outputGrid = RunSimulation(primaryGrid, secondaryGrid, iGenerations);
        WriteOutput(outputGrid);
    }

    private static void CopyRawGridToGridCellsSpan(bool[,] rawGrid, Span<bool> gridCellsSpan)
    {
        var iRowCount = rawGrid.GetLength(0);
        var iColumnCount = rawGrid.GetLength(1);
        for (var mRow = 0; mRow < iRowCount; mRow++)
        {
            for (var mColumn = 0; mColumn < iColumnCount; mColumn++)
            {
                gridCellsSpan[mRow * iColumnCount + mColumn] = rawGrid[mRow, mColumn];
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
            SwapGrids(ref mOldGrid, ref mNewGrid);
        }
        return mOldGrid;
    }

    private static void SwapGrids(ref Grid grid1, ref Grid grid2)
    {
        var tempGrid = grid1;
        grid1 = grid2;
        grid2 = tempGrid;
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