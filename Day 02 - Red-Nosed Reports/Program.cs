// Day 02 - Red-Nosed Reports
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(2, "Red-Nosed Reports");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the reports
List<List<int>> reports = [];
foreach (string line in File.ReadAllLines(fileName))
  reports.Add( line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() );





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
bool IsSafe(List<int> report) {
  bool bGrowing = report[1] > report[0];
  for (int i = 1; i < report.Count; ++i) {
    int diff = bGrowing
      ? report[i] - report[i - 1]
      : report[i - 1] - report[i];
    if (diff < 1 || diff > 3) return false;
  }
  return true;
}

int nCount = reports.Count(report => IsSafe(report));
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nCount);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
bool IsDampenedSafe(List<int> report) {
  if (IsSafe(report)) return true;

  for (int nPos = 0; nPos < report.Count; ++nPos) {
    List<int> subReport = report.Where((_, index) => index != nPos).ToList(); // Remove 1 element at position nPos from the original report
    if (IsSafe(subReport)) return true; // Check if the sub-report is safe
  }

  return false;
}

nCount = reports.Count(report => IsDampenedSafe(report));
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nCount);





PrintHelper.ПечатиЕлкаЗаКрај();
