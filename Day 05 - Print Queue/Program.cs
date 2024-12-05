// Day 05 - Print Queue
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(5, "Print Queue");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
Dictionary<int, HashSet<int>> lessThan = [];
List<List<int>> sections = [];
bool bReadingTheRules = true;
foreach (string line in File.ReadAllLines(fileName)) {
  if (line.Trim() == "")
    bReadingTheRules = false;
  else {
    if (bReadingTheRules) {
      int[] rule = line.Split('|').Select(Int32.Parse).ToArray();
      Debug.Assert(rule.Length == 2);
      if (!lessThan.ContainsKey(rule[0]))
        lessThan.Add(rule[0], [rule[1]]);
      else
        lessThan[rule[0]].Add(rule[1]);
    } else {
      sections.Add(line.Split(',').Select(Int32.Parse).ToList());
      Debug.Assert(sections.Last().Count % 2 == 1);
    }
  }
}





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
bool IsCorrectlyOrdered(List<int> section) {
  for (int i = 0; i < section.Count - 1; ++i) {
    int nItemCur = section[i], nItemNext = section[i + 1];
    if (!lessThan.ContainsKey(nItemCur) || !lessThan[nItemCur].Contains(nItemNext))
      return false;
  }
  return true;
}

int nSum = 0;
List<List<int>> incorrectSections = [];
foreach (var section in sections) {
  if (IsCorrectlyOrdered(section))
    nSum += section[section.Count >> 1];
  else
    incorrectSections.Add(section); // Saving for Part 2
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nSum);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
nSum = 0;
var comparer = Comparer<int>.Create((x, y) => (lessThan.ContainsKey(x) && lessThan[x].Contains(y)) ? -1 : 1);
foreach (var section in incorrectSections) {
  section.Sort(comparer);
  nSum += section[section.Count >> 1];
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nSum);





PrintHelper.ПечатиЕлкаЗаКрај();
