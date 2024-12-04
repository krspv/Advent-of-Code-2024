// Day 04 - Ceres Search
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(4, "Ceres Search");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> mat = [.. File.ReadAllLines(fileName)];





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
int nCountXmas = 0;

for (int row = 0; row < mat.Count; ++row) {
  for (int col = 0; col < mat[row].Length; ++col) {
    if (mat[row][col] == 'X') {
      if (mat[row].Substring(col).StartsWith("XMAS")) nCountXmas++;
      if (col - 3 >= 0 && mat[row].Substring(col - 3).StartsWith("SAMX")) nCountXmas++;
      if (row - 3 >= 0 && mat[row - 1][col] == 'M' && mat[row - 2][col] == 'A' && mat[row - 3][col] == 'S') nCountXmas++;
      if (row + 3 < mat.Count && mat[row + 1][col] == 'M' && mat[row + 2][col] == 'A' && mat[row + 3][col] == 'S') nCountXmas++;
      if (row - 3 >= 0 && col + 3 < mat[row].Length && mat[row - 1][col + 1] == 'M' && mat[row - 2][col + 2] == 'A' && mat[row - 3][col + 3] == 'S') nCountXmas++;
      if (row + 3 < mat.Count && col + 3 < mat[row].Length && mat[row + 1][col + 1] == 'M' && mat[row + 2][col + 2] == 'A' && mat[row + 3][col + 3] == 'S') nCountXmas++;
      if (row + 3 < mat.Count && col - 3 >= 0 && mat[row + 1][col - 1] == 'M' && mat[row + 2][col - 2] == 'A' && mat[row + 3][col - 3] == 'S') nCountXmas++;
      if (row - 3 >= 0 && col - 3 >= 0 && mat[row - 1][col - 1] == 'M' && mat[row - 2][col - 2] == 'A' && mat[row - 3][col - 3] == 'S') nCountXmas++;
    }
  }
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nCountXmas);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
nCountXmas = 0;

for (int row = 1; row < mat.Count - 1; ++row) {
  for (int col = 1; col < mat[row].Length - 1; ++col) {
    if (mat[row][col] == 'A') {
      if (((mat[row - 1][col - 1] == 'M' && mat[row + 1][col + 1] == 'S') || (mat[row - 1][col - 1] == 'S' && mat[row + 1][col + 1] == 'M'))
        && ((mat[row - 1][col + 1] == 'M' && mat[row + 1][col - 1] == 'S') || (mat[row - 1][col + 1] == 'S' && mat[row + 1][col - 1] == 'M')))
        nCountXmas++;
    }
  }
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nCountXmas);





PrintHelper.ПечатиЕлкаЗаКрај();
