﻿// Day 19 - Linen Layout
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
// Part 1 (Could have used caching for performance, however it runs relatively fast even without caching)
bool MatchPattern(string pattern) {
  if (pattern == "") return true;

  foreach (string towel in towels)
    if (pattern.StartsWith(towel) && MatchPattern(pattern[towel.Length..]))
      return true;

  return false;
}

long nCount = patterns.Count(MatchPattern);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nCount);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2 (Caching is essential)
Dictionary<string, long> cache = [];

long CountPatterns(string pattern) {
  if (cache.TryGetValue(pattern, out long value)) return value;

  long nTotal = towels.Contains(pattern) ? 1 : 0;

  for (int i = 1; i < pattern.Length; ++i) {
    if (towels.Contains(pattern[..i]))
      nTotal += CountPatterns(pattern[i..]);
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
