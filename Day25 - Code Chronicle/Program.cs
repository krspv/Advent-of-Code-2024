// Day 25 - Code Chronicle
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(24, "Code Chronicle");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<int[]> keys = [], locks = [];
string[] lines = File.ReadAllLines(fileName);
for (int nLineIdx = 0; nLineIdx < lines.Length; nLineIdx += 8) {
  int[] columnValues = new int[5];
  for (int nCol = 0; nCol < 5; ++nCol)
    for (int nRow = 0; nRow < 7; ++nRow)
      if (lines[nLineIdx + nRow][nCol] == '#')
        columnValues[nCol]++;

  if (lines[nLineIdx] == "#####")
    locks.Add(columnValues);
  else
    keys.Add(columnValues);
}




stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
bool Match(int[] key, int[] lok) {
  for (int nCol = 0; nCol < 5; ++nCol)
    if (key[nCol] + lok[nCol] > 7) return false;
  return true;
}

int nTotal = 0;
foreach (int[] key in keys)
  nTotal += locks.Count(lck => Match(key, lck));
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nTotal);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, "Нема");





PrintHelper.ПечатиЕлкаЗаКрај();
