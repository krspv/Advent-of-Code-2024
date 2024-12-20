// Day 20 - Race Condition
using Helpers;
using System.ComponentModel;
using System.Diagnostics;
using System.Transactions;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(20, "Race Condition");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> area = [.. File.ReadAllLines(fileName)];
int nSize = area.Count;
Debug.Assert(area.All(line => line.Length == nSize));




stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
Dictionary<(int, int), int> track = [];

(int, int) FindStart() {
  for (int row = 1; row < nSize - 1; ++row)
    for (int col = 1; col < nSize - 1; ++col)
      if ('S' == area[row][col])
        return (row, col);
  return (-1, -1);
}

var (rowS, colS) = FindStart();

var (nRow, nCol) = (rowS, colS);
int nPos = 0;
(int, int)[] directions = [(-1, 0), (1, 0), (0, 1), (0, -1)];

while (true) {
  track.Add((nRow, nCol), nPos++);
  if (area[nRow][nCol] == 'E') break;
  foreach (var (dR, dC) in directions) {
    var (nNextR, nNextC) = (nRow + dR, nCol + dC);
    if ((area[nNextR][nNextC] == '.' || area[nNextR][nNextC] == 'E') && !track.ContainsKey((nNextR, nNextC))) {
      (nRow, nCol) = (nNextR, nNextC);
      break;
    }
  }
}

int nFullRacePS = track.Keys.Count-1;

int nTotalOver99ps = 0;
foreach (var (nR, nC) in track.Keys) {
  foreach (var (dR, dC) in directions) {
    var (nNextR, nNextC) = (nR + 2 * dR, nC + 2 * dC);
    if (nNextR < 0 || nNextC < 0 || nNextR >= nSize || nNextC >= nSize) continue;
    if (area[nNextR][nNextC] == '#') continue;
    if (track[(nR, nC)] > track[(nNextR, nNextC)]) continue;
    int nSaved = track[(nNextR, nNextC)] - track[(nR, nC)] - 2;
    if (nSaved > 99) nTotalOver99ps++;
  }
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nTotalOver99ps);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
int nMaxCheat = 20;
foreach (var (nR, nC) in track.Keys) {
  int nColBoundLeft = Math.Max(1, nC - nMaxCheat);
  int nColBoundRight = Math.Min(nSize - 1, nC + nMaxCheat);
  int nRowBoundUp = Math.Max(1, nR - nMaxCheat);
  int nRowBoundDown = Math.Min(nSize - 1, nR + nMaxCheat);

  for (int nColCheck = nColBoundLeft; nColCheck <= nColBoundRight; ++nColCheck) {
    int nCDist = Math.Abs(nC - nColCheck);
    int nRJump = nMaxCheat - nCDist;
    int nCurRowUp = Math.Max(nRowBoundUp, nR - nRJump);
    int nCurRowDown = Math.Min(nRowBoundDown, nR + nRJump);
    for (int nRowCheck = nCurRowUp; nRowCheck <= nCurRowDown; ++nRowCheck) {
      int nRDist = Math.Abs(nR - nRowCheck);
      if (nRDist + nCDist < 3) continue;
      if (area[nRowCheck][nColCheck] == '#') continue;
      if (track[(nR, nC)] > track[(nRowCheck, nColCheck)]) continue;
      int nSaved = track[(nRowCheck, nColCheck)] - track[(nR, nC)] - (nRDist + nCDist);
      if (nSaved > 99) nTotalOver99ps++;
    }
  }
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nTotalOver99ps);





PrintHelper.ПечатиЕлкаЗаКрај();
