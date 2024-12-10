// Day 10 - Hoof It
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(10, "Hoof It");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> map = File.ReadAllLines(fileName).ToList();
int nSize = map.Count;
Debug.Assert(map.All(x => x.Length == nSize));





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
int Trail(int row, int col, char v, HashSet<(int,int)> nines) {
  if (row < 0 || col < 0 || row >= nSize || col >= nSize) return 0;
  if (map[row][col] != v) return 0;
  if (v == '9') {
    if (nines.Contains((row, col))) return 0;
    else {
      nines.Add((row, col));
      return 1;
    }
  }

  char chNext = (char)(v + 1);
  return Trail(row - 1, col, chNext, nines) + Trail(row + 1, col, chNext, nines) + Trail(row, col - 1, chNext, nines) + Trail(row, col + 1, chNext, nines);
}

int nCount = 0;
for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < nSize; ++col)
    nCount += Trail(row, col, '0', []);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nCount);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
int Trail2(int row, int col, char v) {
  if (row < 0 || col < 0 || row >= nSize || col >= nSize) return 0;
  if (map[row][col] != v) return 0;
  if (v == '9') return 1;

  char chNext = (char)(v + 1);
  return Trail2(row - 1, col, chNext) + Trail2(row + 1, col, chNext) + Trail2(row, col - 1, chNext) + Trail2(row, col + 1, chNext);
}

nCount = 0;
for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < nSize; ++col)
    nCount += Trail2(row, col, '0');
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nCount);





PrintHelper.ПечатиЕлкаЗаКрај();
