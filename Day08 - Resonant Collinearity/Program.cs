// Day 08 - Resonant Collinearity
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(8, "Resonant Collinearity");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> city = File.ReadAllLines(fileName).ToList();
int nSize = city[0].Length;
Debug.Assert(city.All(s => s.Length == nSize));
Debug.Assert(nSize == city.Count);





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
Dictionary<char, List<(int, int)>> antennaLocations = [];
for (int row = 0; row < nSize; ++row)
  for (int col = 0; col < nSize; ++col) {
    char ch = city[row][col];
    if (Char.IsLetterOrDigit(ch)) {
      if (!antennaLocations.ContainsKey(ch))
        antennaLocations[ch] = [];
      antennaLocations[ch].Add((row, col));
    }
  }

HashSet<(int, int)> uniqueAntinodes = [];

foreach (char antenna in antennaLocations.Keys) {
  List<(int, int)> locations = antennaLocations[antenna];
  if (locations.Count > 1) {
    // Check each pair of the same type for antinode locations
    for (int i = 0; i < locations.Count - 1; ++i) {
      (int r1, int c1) = locations[i];
      for (int j = i + 1; j < locations.Count; ++j) {
        (int r2, int c2) = locations[j];

        (int rAnti1, int cAnti1) = (r1 - r2 + r1, c1 - c2 + c1);
        (int rAnti2, int cAnti2) = (r2 - r1 + r2, c2 - c1 + c2);

        if (rAnti1 >= 0 && rAnti1 < nSize&& cAnti1 >= 0 && cAnti1 < nSize)
          uniqueAntinodes.Add((rAnti1, cAnti1));
        if (rAnti2 >= 0 && rAnti2 < nSize && cAnti2 >= 0 && cAnti2 < nSize)
          uniqueAntinodes.Add((rAnti2, cAnti2));
      }
    }
  }
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, uniqueAntinodes.Count);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
foreach (char antenna in antennaLocations.Keys) {
  List<(int, int)> locations = antennaLocations[antenna];
  if (locations.Count > 1) {
    // Check each pair of the same type for antinode locations including resonant harmonics
    for (int i = 0; i < locations.Count - 1; ++i) {
      (int r1, int c1) = locations[i];
      for (int j = i + 1; j < locations.Count; ++j) {
        (int r2, int c2) = locations[j];
        
        (int dR, int dC) = (r2 - r1, c2 - c1);
        (int lineR, int lineC) = (r1, c1);
        bool bMoveRow = true;
        if (dR != 0) {
          int nSteps = lineR / dR;
          lineR -= nSteps * dR;
          lineC -= nSteps * dC;
        } else {
          bMoveRow = false;
          int nSteps = lineC / dC;
          lineC -= nSteps * dC;
          lineR -= nSteps * dR;
        }

        if (bMoveRow) {
          while (lineR < nSize) {
            if (lineC >= 0 && lineC < nSize)
              uniqueAntinodes.Add((lineR, lineC));
            lineR += dR;
            lineC += dC;
          }
        } else {
          while (lineC < nSize) {
            if (lineR >= 0 && lineR < nSize)
              uniqueAntinodes.Add((lineR, lineC));
            lineR += dR;
            lineC += dC;
          }
        }
      }
    }
  }
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, uniqueAntinodes.Count);





PrintHelper.ПечатиЕлкаЗаКрај();
