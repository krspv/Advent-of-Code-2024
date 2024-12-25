// Day 06 - Guard Gallivant
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(6, "Guard Gallivant");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> lines = [.. File.ReadAllLines(fileName)];
bool[,] visited = new bool[lines.Count, lines[0].Length];
Direction dir = Direction.Up;





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1

// Find the starting position
int nRowPos = 0, nColPos = -1;
for (; nColPos == -1; ++nRowPos)
  nColPos = lines[nRowPos].IndexOf('^');

int nCount = 0;
int nStartRow = nRowPos, nStartCol = nColPos; // Save for Part 2

// Move the guard until she exits the area
while (true) {
  switch (dir) {
    case Direction.Up:
      if (nRowPos > 0 && lines[nRowPos - 1][nColPos] == '#')
        dir = Direction.Right;
      else
        nRowPos--;
      break;

    case Direction.Down:
      if (nRowPos < lines.Count - 1 && lines[nRowPos + 1][nColPos] == '#')
        dir = Direction.Left;
      else
        nRowPos++;
      break;

    case Direction.Right:
      if (nColPos < lines[nRowPos].Length - 1 && lines[nRowPos][nColPos + 1] == '#')
        dir = Direction.Down;
      else
        nColPos++;
      break;

    case Direction.Left:
      if (nColPos > 0 && lines[nRowPos][nColPos - 1] == '#')
        dir = Direction.Up;
      else
        nColPos--;
      break;
  }

  if (nRowPos < 0 || nColPos < 0 || nRowPos == lines[0].Length || nColPos == lines.Count) // The guard has exited the area
    break;

  if (!visited[nRowPos, nColPos]) {
    visited[nRowPos, nColPos] = true;
    nCount++;
  }
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nCount);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2

// Keep track what side each obstruction was approached from
Dictionary<(int, int), HashSet<Direction>> approaches = [];
for (int row = 0; row < lines.Count; ++row)
  for (int col = 0; col < lines[row].Length; ++col)
    if (lines[row][col] == '#')
      approaches[(row, col)] = [];

nCount = 0;
for (int row = 0; row < lines.Count; ++row)
  for (int col = 0; col < lines[row].Length; ++col)
    if (visited[row, col]) { // Check only those positions we know are in the Guard's path
      if (LoopCheck(row, col))
        nCount++;
    }

bool LoopCheck(int nObstructRow, int nObstructCol) {
  approaches[(nObstructRow, nObstructCol)] = [];
  int nGuardRow = nStartRow, nGuardCol = nStartCol;
  Direction dir = Direction.Up;

  // Move the guard until she has exited the area or she has entered a loop
  while (true) {
    switch (dir) {
      case Direction.Up: nGuardRow--; break;
      case Direction.Down: nGuardRow++; break;
      case Direction.Right: nGuardCol++; break;
      case Direction.Left: nGuardCol--; break;
    }

    if (approaches.ContainsKey((nGuardRow, nGuardCol))) {
      if (approaches[(nGuardRow, nGuardCol)].Contains(dir)) { // Found a loop
        // Clean up
        approaches.Remove((nObstructRow, nObstructCol));
        foreach (var key in approaches.Keys)
          approaches[key] = [];

        return true;
      } else {
        approaches[(nGuardRow, nGuardCol)].Add(dir);

        // Step back and turn
        switch (dir) {
          case Direction.Up: nGuardRow++; dir = Direction.Right; break;
          case Direction.Down: nGuardRow--; dir = Direction.Left; break;
          case Direction.Right: nGuardCol--; dir = Direction.Down; break;
          case Direction.Left: nGuardCol++; dir = Direction.Up; break;
        }
      }
    }

    if (nGuardRow < 0 || nGuardCol < 0 || nGuardRow == lines[0].Length || nGuardCol == lines.Count) { // The guard has exited the area
      // Clean up
      approaches.Remove((nObstructRow, nObstructCol));
      foreach (var key in approaches.Keys)
        approaches[key] = [];

      return false;
    }
  }
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nCount);





PrintHelper.ПечатиЕлкаЗаКрај();



// Types
enum Direction {
  Up,
  Down,
  Left,
  Right,
};
