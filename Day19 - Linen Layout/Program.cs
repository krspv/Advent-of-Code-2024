// Day 19 - Linen Layout
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(19, "Linen Layout");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> lines = [.. File.ReadAllLines(fileName)];
HashSet<string> towels = [.. lines[0].Split(',', StringSplitOptions.TrimEntries)];
List<string> patterns = lines[2..];





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
int nMaxLen = towels.Max(p => p.Length);

bool MatchPattern(string pattern) {
  int nPos = 0, nEndPos = Math.Min(nPos + nMaxLen, pattern.Length);

  Stack<int> stack = [];
  while (nPos < pattern.Length) {
    while (nEndPos > nPos && !towels.Contains(pattern[nPos..nEndPos]))
      nEndPos--;

    if (nEndPos > nPos) {
      stack.Push(nEndPos - nPos);
      nPos = nEndPos;
      nEndPos = Math.Min(nPos + nMaxLen, pattern.Length);
    } else {
      if (stack.Count == 0) return false;
      int nPrevMax = stack.Pop();
      nEndPos = nPos - 1;
      nPos -= nPrevMax;
    }
  }

  return true;
}

long nCount = patterns.Count(MatchPattern);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nCount);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
Dictionary<string, long> cache = [];

long CountPatterns(string pattern) {
  if (cache.TryGetValue(pattern, out long value)) return value;

  long nTotal = 0;
  if (MatchPattern(pattern)) {
    foreach (string towel in towels) {
      if (towel.Length < pattern.Length && pattern.StartsWith(towel))
        nTotal += CountPatterns(pattern[towel.Length..]);
    }
    if (towels.Contains(pattern)) nTotal++;
  }
  cache.Add(pattern, nTotal);
  
  return cache[pattern];
}

nCount = patterns.Sum(CountPatterns);
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nCount);





PrintHelper.ПечатиЕлкаЗаКрај();
