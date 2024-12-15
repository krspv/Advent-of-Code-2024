// Day 15 - Warehouse Woes
using Helpers;
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(15, "Warehouse Woes");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> lines = [];
StringBuilder sbMoves = new();
bool bReadingWarehouse = true;
foreach (string line in File.ReadAllLines(fileName)) {
  if (bReadingWarehouse) {
    if (line.Trim().Length == 0)
      bReadingWarehouse = false;
    else
      lines.Add(line);
  } else {
    sbMoves.Append(line.Trim());
  }
}

Debug.Assert(lines.All(line => line.Length == lines.Count));
int nSize = lines.Count;
char[,] warehouse = new char[nSize, nSize];
(int r, int c) robotPos = (0, 0);
for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < nSize; ++col) {
    warehouse[row, col] = lines[row][col];
    if (warehouse[row, col] == '@')
      robotPos = (row, col);
  }
string strMoves = sbMoves.ToString();





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
// string ss;
foreach (char chMove in strMoves) {
  // ss = DebugWarehouse();
  var (dR, dC) = chMove switch {
    '^' => (-1, 0),
    'v' => (1, 0),
    '<' => (0, -1),
    '>' => (0, 1),
    _ => throw new Exception($"Invalid move Encountered {chMove}!"),
  };
  (int row, int col) = (robotPos.r + dR, robotPos.c + dC);
  while (warehouse[row, col] != '.' && warehouse[row, col] != '#') {
    row += dR;
    col += dC;
  }
  if (warehouse[row, col] == '.') {
    do {
      warehouse[row, col] = warehouse[row - dR, col - dC];
      row -= dR;
      col -= dC;
    } while (row != robotPos.r || col != robotPos.c);
    warehouse[robotPos.r, robotPos.c] = '.';
    robotPos.r += dR;
    robotPos.c += dC;
  }
}

// ss = DebugWarehouse();
int nGPS = 0;
for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < nSize; ++col)
    if (warehouse[row, col] == 'O')
      nGPS += 100 * row + col;
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nGPS);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
warehouse = new char[nSize, 2 * nSize];
for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < nSize; ++col) {
    string strDouble = lines[row][col] switch {
      '#' => "##",
      '.' => "..",
      'O' => "[]",
      '@' => "@.",
      _ => throw new Exception($"Invalid position Encountered {lines[row][col]}!"),
    };
    warehouse[row, 2 * col] = strDouble[0];
    warehouse[row, 2 * col + 1] = strDouble[1];
    if (lines[row][col] == '@')
      robotPos = (row, 2 * col);
  }

foreach (char chMove in strMoves) {
  // ss = DebugWarehouse();
  var (dR, dC) = chMove switch {
    '^' => (-1, 0),
    'v' => (1, 0),
    '<' => (0, -1),
    '>' => (0, 1),
    _ => throw new Exception($"Invalid move Encountered {chMove}!"),
  };

  (int row, int col) = (robotPos.r + dR, robotPos.c + dC);

  if (warehouse[row, col] == '#') continue; // Immediate block

  if (warehouse[row, col] == '.') { // Free movement
    warehouse[row, col] = '@';
    warehouse[robotPos.r, robotPos.c] = '.';
    robotPos = (row, col);
    continue;
  }

  if (dR == 0) {  // Horizontal push
    while (warehouse[row, col] != '.' && warehouse[row, col] != '#')
      col += dC;
    if (warehouse[row, col] == '.') {
      do {
        warehouse[row, col] = warehouse[row, col - dC];
        col -= dC;
      } while (col != robotPos.c);
      warehouse[robotPos.r, robotPos.c] = '.';
      robotPos.c += dC;
    }
    continue;
  }

  // Vertical push
  Stack<List<(int, int)>> pushables = [];
  var box = warehouse[row, col] == '[' ? (row, col) : (row, col - 1);
  pushables.Push([box]);

  bool bBlocked = false;
  while (!bBlocked) {
    List<(int, int)> list = pushables.Peek();
    List<(int, int)> next = [];
    foreach ((int rBox, int cBox) in list) {
      if (warehouse[rBox + dR, cBox] == '#' || warehouse[rBox + dR, cBox + 1] == '#') {
        bBlocked = true;
        break;
      }
      for (int dH = -1; dH < 2; ++dH) {
        if (warehouse[rBox + dR, cBox + dH] == '[')
          next.Add((rBox + dR, cBox + dH));
      }
    }
    if (next.Count == 0) break;
    pushables.Push(next);
  }

  if (!bBlocked) {
    while (pushables.Count > 0) {
      List<(int, int)> list = pushables.Pop();
      foreach ((int rBox, int cBox) in list) {
        warehouse[rBox + dR, cBox] = '[';
        warehouse[rBox + dR, cBox + 1] = ']';
        warehouse[rBox, cBox] = '.';
        warehouse[rBox, cBox + 1] = '.';
      }
    }
    warehouse[robotPos.r, robotPos.c] = '.';
    robotPos.r += dR;
    warehouse[robotPos.r, robotPos.c] = '@';
  }
}

// ss = DebugWarehouse();
nGPS = 0;
for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < 2*nSize; ++col)
    if (warehouse[row, col] == '[')
      nGPS += 100 * row + col;
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nGPS);





PrintHelper.ПечатиЕлкаЗаКрај();





// Debug utils
#pragma warning disable CS8321
string DebugWarehouse() {
  StringBuilder sb = new();
  for (int row = 0; row < warehouse.GetLength(0); ++row) {
    for (int col = 0; col < warehouse.GetLength(1); ++col) {
      sb.Append(warehouse[row, col]);
    }
    sb.Append('\n');
  }
  return sb.ToString();
}
#pragma warning restore CS8321
