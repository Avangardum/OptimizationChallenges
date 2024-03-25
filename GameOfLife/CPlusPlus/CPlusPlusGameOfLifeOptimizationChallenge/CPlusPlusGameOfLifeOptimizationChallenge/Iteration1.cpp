#include <iostream>
#include <fstream>
#include "Iteration1.h"

#include <string>
#include <vector>

using namespace std;

class Grid
{
public:
    Grid(int rowCount, int columnCount, bool* cells)
        : _rowCount(rowCount), _columnCount(columnCount), _cells(cells) {}

    int GetRowCount() const { return _rowCount; }

    int GetColumnCount() const { return _columnCount; }

    bool GetCell(int row, int column) const
    {
        return _cells[row * _columnCount + column];
    }

    void SetCell(int row, int column, bool value) const
    {
        _cells[row * _columnCount + column] = value;
    }

private:
    int _rowCount;
    int _columnCount;
    bool* _cells;
};

const char Iteration1::DeadCellChar = '0';
const char Iteration1::LivingCellChar = '1';

void Iteration1::Run()
{
    cout << "Iteration 1" << endl;
}