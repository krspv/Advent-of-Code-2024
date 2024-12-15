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
char[,] warehouse = new char[lines.Count, lines[0].Length];
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
foreach (char chMove in strMoves) {
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
    continue;
  }

  // Vertical push
  Stack<(int, int)> pushLine = [];
  pushLine.Push(warehouse[row, col] == '[' ? (col, col + 1) : (col - 1, col));
  KType type;
  while (true) {
    row += dR;
    type = Next(row, col, pushLine.Peek());
    if (type != KType.Box)
      break;
    (int c1, int c2) = pushLine.Peek();
    if (warehouse[row, c1] == ']') c1--;
    else while (c1 < c2 && warehouse[row, c1] == '.') c1++;
    if (warehouse[row, c2] == '[') c2++;
    else while (c2 > c1 && warehouse[row, c2] == '.') c2--;
    pushLine.Push((c1, c2));
  }

  if (type == KType.Free) {
    while (pushLine.Count > 0) {
      (int c1, int c2) = pushLine.Pop();
      for (int c = c1; c <= c2; ++c) {
        warehouse[row, c] = warehouse[row - dR, c];
        warehouse[row - dR, c] = '.';
      }
      row -= dR;
    }
    warehouse[robotPos.r, robotPos.c] = '.';
    robotPos.r += dR;
    warehouse[robotPos.r, robotPos.c] = '@';
  }
}

KType Next(int row, int col, (int c1, int c2) span) {
  bool bFree = true;
  for (int c = span.c1; c <= span.c2; ++c) {
    if (warehouse[row, c] == '#')
      return KType.Blocker;
    if (warehouse[row, c] != '.')
      bFree = false;
  }
  if (bFree) return KType.Free;
  return KType.Box;
}

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





// Types
enum KType {
  Blocker,
  Free,
  Box,
};
