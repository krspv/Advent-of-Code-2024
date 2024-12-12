// Day 12 - Garden Groups
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(12, "Garden Groups");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> garden = [..File.ReadAllLines(fileName)];
int nSize = garden.Count;
Debug.Assert(garden.All(line => line.Length == nSize));





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
List<KGroup> groups = [];
bool[,] visited = new bool[nSize, nSize];

for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < nSize; ++col)
    if (!visited[row, col]) {
      KGroup group = new() { Plant = garden[row][col], Area = 0, Perimeter = 0 };
      ProcessPlot(row, col, ref group);
      groups.Add(group);
    }

void ProcessPlot(int row, int col, ref KGroup group) {
  visited[row, col] = true;

  group.Area++;
  if (row == 0 || garden[row - 1][col] != group.Plant) group.Perimeter++;
  if (row == nSize - 1 || garden[row + 1][col] != group.Plant) group.Perimeter++;
  if (col == 0 || garden[row][col - 1] != group.Plant) group.Perimeter++;
  if (col == nSize - 1 || garden[row][col + 1] != group.Plant) group.Perimeter++;

  if (row > 0 && garden[row - 1][col] == group.Plant && !visited[row - 1, col])
    ProcessPlot(row - 1, col, ref group);
  if (row < nSize - 1 && garden[row + 1][col] == group.Plant && !visited[row + 1, col])
    ProcessPlot(row + 1, col, ref group);
  if (col > 0 && garden[row][col - 1] == group.Plant && !visited[row, col - 1])
    ProcessPlot(row, col - 1, ref group);
  if (col < nSize - 1 && garden[row][col + 1] == group.Plant && !visited[row, col + 1])
    ProcessPlot(row, col + 1, ref group);
}

int nFencePrice = groups.Sum(gr => gr.Area * gr.Perimeter);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nFencePrice);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
ESide[,] sides = new ESide[nSize, nSize];
visited = new bool[nSize, nSize];
groups = [];

for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < nSize; ++col)
    if (!visited[row, col]) {
      KGroup group = new() { Plant = garden[row][col], Area = 0, Perimeter = 0 };
      ProcessPlotForSides(row, col, ref group);
      groups.Add(group);
    }

void ProcessPlotForSides(int row, int col, ref KGroup group) {
  visited[row, col] = true;

  group.Area++;
  if ((row == 0 || garden[row - 1][col] != group.Plant) && ((sides[row,col] & ESide.Top) == 0)) {
    group.Perimeter++;
    sides[row, col] |= ESide.Top;
    for (int tempC = col-1; tempC >= 0; --tempC) {
      if (garden[row][tempC] != group.Plant || (row > 0 && garden[row - 1][tempC] == group.Plant))
        break;
      sides[row, tempC] |= ESide.Top;
    }
    for (int tempC = col + 1; tempC < nSize; ++tempC) {
      if (garden[row][tempC] != group.Plant || (row > 0 && garden[row - 1][tempC] == group.Plant))
        break;
      sides[row, tempC] |= ESide.Top;
    }
  }
  if ((row == nSize - 1 || garden[row + 1][col] != group.Plant) && ((sides[row, col] & ESide.Bottom) == 0)) {
    group.Perimeter++;
    sides[row, col] |= ESide.Bottom;
    for (int tempC = col - 1; tempC >= 0; --tempC) {
      if (garden[row][tempC] != group.Plant || (row < nSize - 1 && garden[row + 1][tempC] == group.Plant))
        break;
      sides[row, tempC] |= ESide.Bottom;
    }
    for (int tempC = col + 1; tempC < nSize; ++tempC) {
      if (garden[row][tempC] != group.Plant || (row < nSize - 1 && garden[row + 1][tempC] == group.Plant))
        break;
      sides[row, tempC] |= ESide.Bottom;
    }
  }
  if ((col == 0 || garden[row][col - 1] != group.Plant) && ((sides[row, col] & ESide.Left) == 0)) {
    group.Perimeter++;
    sides[row, col] |= ESide.Left;
    for (int tempR = row - 1; tempR >= 0; --tempR) {
      if (garden[tempR][col] != group.Plant || (col > 0 && garden[tempR][col - 1] == group.Plant))
        break;
      sides[tempR, col] |= ESide.Left;
    }
    for (int tempR = row + 1; tempR < nSize; ++tempR) {
      if (garden[tempR][col] != group.Plant || (col > 0 && garden[tempR][col - 1] == group.Plant))
        break;
      sides[tempR, col] |= ESide.Left;
    }
  }
  if ((col == nSize - 1 || garden[row][col + 1] != group.Plant) && ((sides[row, col] & ESide.Right) == 0)) {
    group.Perimeter++;
    sides[row, col] |= ESide.Right;
    for (int tempR = row - 1; tempR >= 0; --tempR) {
      if (garden[tempR][col] != group.Plant || (col < nSize - 1 && garden[tempR][col + 1] == group.Plant))
        break;
      sides[tempR, col] |= ESide.Right;
    }
    for (int tempR = row + 1; tempR < nSize; ++tempR) {
      if (garden[tempR][col] != group.Plant || (col < nSize - 1 && garden[tempR][col + 1] == group.Plant))
        break;
      sides[tempR, col] |= ESide.Right;
    }
  }

  if (row > 0 && garden[row - 1][col] == group.Plant && !visited[row - 1, col])
    ProcessPlotForSides(row - 1, col, ref group);
  if (row < nSize - 1 && garden[row + 1][col] == group.Plant && !visited[row + 1, col])
    ProcessPlotForSides(row + 1, col, ref group);
  if (col > 0 && garden[row][col - 1] == group.Plant && !visited[row, col - 1])
    ProcessPlotForSides(row, col - 1, ref group);
  if (col < nSize - 1 && garden[row][col + 1] == group.Plant && !visited[row, col + 1])
    ProcessPlotForSides(row, col + 1, ref group);
}

nFencePrice = groups.Sum(gr => gr.Area * gr.Perimeter);
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nFencePrice);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
struct KGroup {
  public char Plant { get; set; }
  public int Area { get; set; }
  public int Perimeter { get; set; }  // This means Side for Part2
}

enum ESide : byte {
  None = 0x00,
  Top = 0x01,
  Bottom = 0x02,
  Left = 0x04,
  Right = 0x08,
}
