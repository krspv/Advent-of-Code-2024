// Day 11 - Plutonian Pebbles
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(11, "Plutonian Pebbles");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<long> numbers = File.ReadAllText(fileName)
  .Split(' ', StringSplitOptions.RemoveEmptyEntries)
  .Select(Int64.Parse)
  .ToList();





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
long[] arrPowers10 = new long[10];
arrPowers10[0] = 10;
for (int i = 1; i < 10; ++i)
  arrPowers10[i] = 10 * arrPowers10[i - 1];

Dictionary<(long, int), long> cache = []; // Added caching after Part2 was revealed

long Blink(long nStone, int nTimes) {
  if (cache.ContainsKey((nStone, nTimes)))
    return cache[(nStone, nTimes)];

  long nRetVal;
  if (nTimes == 0)
    nRetVal = 1;
  else if (nStone == 0)
    nRetVal = Blink(1, nTimes - 1);
  else {
    int nLen = nStone.ToString().Length;
    if (nLen % 2 == 0) {
      long nRight = nStone % arrPowers10[nLen / 2 - 1];
      long nLeft = nStone / arrPowers10[nLen / 2 - 1];
      nRetVal = Blink(nLeft, nTimes - 1) + Blink(nRight, nTimes - 1);
    } else
      nRetVal = Blink(nStone * 2024, nTimes - 1);
  }

  cache[(nStone, nTimes)] = nRetVal;
  return nRetVal;
}

long nTotal = numbers.Sum(num => Blink(num, 25));
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nTotal);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
nTotal = numbers.Sum(num => Blink(num, 75));
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nTotal);





PrintHelper.ПечатиЕлкаЗаКрај();
