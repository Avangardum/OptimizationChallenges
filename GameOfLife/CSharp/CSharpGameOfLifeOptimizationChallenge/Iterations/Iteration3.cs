using static Avangardum.OptimizationChallenges.GameOfLife.CSharp.Constants;

namespace Avangardum.OptimizationChallenges.GameOfLife.CSharp.Iterations;

/// <summary>
/// From Iteration2.<br/>
/// Replaced bool[,] with a custom Grid class that stores the grid as a byte array.<br/>
/// Runtime increased.<br/>
/// Memory allocation reduced.
/// </summary>
internal static class Iteration3
{
    private class Grid
    {
        private const int BitsInByte = 8;
        
        private readonly byte[] _bytes;
        
        public Grid(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            _bytes = new byte[rowCount * columnCount / BitsInByte + 1];
        }
        
        public int RowCount { get; }
        public int ColumnCount { get; }

        public bool this[int row, int column]
        {
            get
            {
                var (iByteIndex, iBitRelIndex) = LocateBitAddress(row, column);
                var iResult = (_bytes[iByteIndex] & (1 << iBitRelIndex)) != 0;
                return iResult;
            }
            set
            {
                var (iByteIndex, iBitRelIndex) = LocateBitAddress(row, column);
                if (value) _bytes[iByteIndex] = (byte)(_bytes[iByteIndex] | (1 << iBitRelIndex));
                else _bytes[iByteIndex] = (byte)(_bytes[iByteIndex] & ~(1 << iBitRelIndex));
            }
        }

        /// <summary>
        /// Converts row and column of a grid cell to index of a byte containing the corresponding bit
        /// and the index of the corresponding bit in the containing byte (zero-indexed, from low to high). 
        /// </summary>
        private (int ByteIndex, byte bitRelIndex) LocateBitAddress(int row, int column)
        {
            var iBitAbsIndex = row * ColumnCount + column;
            var iByteIndex = iBitAbsIndex / BitsInByte;
            var iBitRelIndex = (byte)(iBitAbsIndex % BitsInByte);
            return (iByteIndex, iBitRelIndex);
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
        using var iWriter = new StreamWriter(OutputPath);
        for (int mRow = 0; mRow < outputGrid.RowCount; mRow++)
        {
            for (int mColumn = 0; mColumn < outputGrid.ColumnCount; mColumn++) 
                iWriter.Write(outputGrid[mRow, mColumn] ? LivingCellChar : DeadCellChar);
            if (mRow < outputGrid.RowCount - 1) iWriter.WriteLine();
        }
    }

    private static Grid RunSimulation(Grid inputGrid, int generations)
    {
        Grid mGrid = inputGrid;
        for (var mGeneration = 0; mGeneration < generations; mGeneration++) mGrid = RunGeneration(mGrid);
        return mGrid;
    }

    private static Grid RunGeneration(Grid inputGrid)
    {
        var iRowCount = inputGrid.RowCount;
        var iColumnCount = inputGrid.ColumnCount;
        var iOutputGrid = new Grid(iRowCount, iColumnCount);
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

    private static int CountLivingNeighbors(Grid grid, int row, int column)
    {
        var iLastRow = grid.RowCount - 1;
        var iLastColumn = grid.ColumnCount - 1;
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