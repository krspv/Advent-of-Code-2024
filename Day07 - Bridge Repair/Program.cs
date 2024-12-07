// Day 07 - Bridge Repair
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(7, "Bridge Repair");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<ulong> testValues = [];
List<List<ulong>> numbers = [];
foreach (string line in File.ReadAllLines(fileName)) {
  string[] arr = line.Split(':', StringSplitOptions.TrimEntries);
  testValues.Add(UInt64.Parse(arr[0]));
  numbers.Add( arr[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(UInt64.Parse).ToList() );
}





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
ulong nSum = 0;
bool[] validLines = new bool[testValues.Count];  // Save the valid ones for Part 2, so we can skip those lines
int nMaxCombinationArrLength = 0;  // Also needed for Part 2
for (int line = 0; line < testValues.Count; ++line) {
  int nCombinationArrayLength = numbers[line].Count - 1;
  int nTotalCombinations = 1 << nCombinationArrayLength;
  if (nCombinationArrayLength > nMaxCombinationArrLength)
    nMaxCombinationArrLength = nCombinationArrayLength;

  for (int nCurComb = 0; nCurComb < nTotalCombinations; ++nCurComb) {
    int nTempComb = nCurComb;
    ulong nCalc = numbers[line][0];

    for (int i = 0; i < numbers[line].Count - 1; ++i) {
      if ((nTempComb % 2) != 0)
        nCalc *= numbers[line][i + 1];
      else
        nCalc += numbers[line][i + 1];
      if (nCalc > testValues[line])
        break;
      nTempComb >>= 1;
    }

    if (nCalc == testValues[line]) {  // Found a match
      nSum += nCalc;
      validLines[line] = true;
      break;
    }
  }
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nSum);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
ulong Concat(ulong nCalc, ulong val) {
  Stack<ulong> digits = [];
  while (val > 0) {
    digits.Push(val % 10);
    val /= 10;
  }

  while (digits.Count > 0)
    nCalc = nCalc * 10 + digits.Pop();

  return nCalc;
}

// Calculate powers of 3
int[] powersOf3 = new int[nMaxCombinationArrLength];
powersOf3[0] = 3;
for (int i = 1; i < nMaxCombinationArrLength; ++i)
  powersOf3[i] = 3 * powersOf3[i - 1];

for (int line = 0; line < testValues.Count; ++line) {
  if (!validLines[line]) {
    int nCombinationArrayLength = numbers[line].Count - 1;
    int nTotalCombinations = powersOf3[nCombinationArrayLength - 1];

    for (int nCurComb = 0; nCurComb < nTotalCombinations; ++nCurComb) {
      int nTempComb = nCurComb;
      ulong nCalc = numbers[line][0];

      for (int i = 0; i < numbers[line].Count - 1; ++i) {
        switch (nTempComb % 3) {
          case 0: // +
            nCalc += numbers[line][i + 1];
            break;

          case 1: // *
            nCalc *= numbers[line][i + 1];
            break;

          case 2: // ||
            nCalc = Concat(nCalc, numbers[line][i + 1]);
            break;
        }
        if (nCalc > testValues[line])
          break;
        nTempComb /= 3;
      }

      if (nCalc == testValues[line]) {  // Found a match
        nSum += nCalc;
        break;
      }
    }
  }
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nSum);





PrintHelper.ПечатиЕлкаЗаКрај();
