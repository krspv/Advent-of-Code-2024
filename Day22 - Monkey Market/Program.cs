﻿// Day 22 - Monkey Market
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(22, "Monkey Market");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<long> secrets = [.. File.ReadAllLines(fileName).Select(Int64.Parse)];





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
static long Evolve(long nSecret) {
  nSecret ^= nSecret * 64;
  nSecret %= 16777216;
  nSecret ^= nSecret / 32;
  nSecret %= 16777216;
  nSecret ^= nSecret * 2048;
  nSecret %= 16777216;

  return nSecret;
}

long nSum = 0;
foreach (long nNum in secrets) {
  long nTemp = nNum;
  for (int i = 0; i < 2000; ++i)
    nTemp = Evolve(nTemp);
  nSum += nTemp;
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nSum);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
List<Dictionary<int, int>> nest = [];  // Sequence for num
HashSet<int> allSeqs = [];
foreach (long nNum in secrets) {
  nest.Add([]);
  var dict = nest.Last();

  long nTemp = nNum;
  Queue<int> q = [];
  for (int i = 0; i < 2000; ++i) {
    long n = Evolve(nTemp);
    int n1 = (int)(n % 10);
    int x1 = (int)(nTemp % 10);
    q.Enqueue(n1 - x1 + 9); // 0..18
    if (q.Count == 4) {
      if (n1 > 0) {
        List<int> lst = [.. q];
        int val20 = lst[0] * 8000 + lst[1] * 400 + lst[2] * 20 + lst[3];
        if (!dict.ContainsKey(val20))
          dict[val20] = n1;
        allSeqs.Add(val20);
      }
      q.Dequeue();
    }
    nTemp = n;
  }
}

int nMax = 0;
foreach (int nSeq in allSeqs) {
  int nTotal = 0;
  foreach (var dict in nest) {
    if (dict.TryGetValue(nSeq, out int val))
      nTotal += val;
  }
  nMax = Math.Max(nMax, nTotal);
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nMax);





PrintHelper.ПечатиЕлкаЗаКрај();
