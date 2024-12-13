// Day 13 - Claw Contraption
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(13, "Claw Contraption");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> inputLines = [..File.ReadAllLines(fileName)];
int nLineNumber = 0;
List<KMachine> machines = [];
while (nLineNumber < inputLines.Count) {
  KMachine machine = new();

  string lineA = inputLines[nLineNumber++];
  string[] xy = lineA.Split(':')[1].Trim().Split(',', StringSplitOptions.TrimEntries);
  machine.A.X = Int32.Parse(xy[0].Split('+', StringSplitOptions.TrimEntries)[1]);
  machine.A.Y = Int32.Parse(xy[1].Split('+', StringSplitOptions.TrimEntries)[1]);

  string lineB = inputLines[nLineNumber++];
  xy = lineB.Split(':')[1].Trim().Split(',', StringSplitOptions.TrimEntries);
  machine.B.X = Int32.Parse(xy[0].Split('+', StringSplitOptions.TrimEntries)[1]);
  machine.B.Y = Int32.Parse(xy[1].Split('+', StringSplitOptions.TrimEntries)[1]);

  string linePrize = inputLines[nLineNumber++];
  xy = linePrize.Split(':')[1].Trim().Split(',', StringSplitOptions.TrimEntries);
  machine.Prize.X = Int32.Parse(xy[0].Split('=', StringSplitOptions.TrimEntries)[1]);
  machine.Prize.Y = Int32.Parse(xy[1].Split('=', StringSplitOptions.TrimEntries)[1]);

  nLineNumber++;
  machines.Add(machine);
}





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
int nTokens = 0;
foreach (KMachine machine in machines) {
  int nMinCost = -1;

  for (int nPressA = 0; nPressA <= 100; ++nPressA) {
    Tuple movementA = new Tuple() { X = machine.A.X * nPressA, Y = machine.A.Y * nPressA };
    if (movementA.X > machine.Prize.X || movementA.Y > machine.Prize.Y)
      break;
    if (movementA.X == machine.Prize.X && movementA.Y == machine.Prize.Y) {
      int nCost = nPressA * 3;
      if (nMinCost == -1 || (nCost < nMinCost))
        nMinCost = nCost;
      break;
    }

    for (int nPressB = 1; nPressB <= 100; ++nPressB) {
      Tuple movementAB = new Tuple() { X = machine.B.X * nPressB + movementA.X, Y = machine.B.Y * nPressB + movementA.Y };
      if (movementAB.X > machine.Prize.X || movementAB.Y > machine.Prize.Y)
        break;
      if (movementAB.X == machine.Prize.X && movementAB.Y == machine.Prize.Y) {
        int nCost = nPressA * 3 + nPressB;
        if (nMinCost == -1 || (nCost < nMinCost))
          nMinCost = nCost;
        break;
      }
    }
  }

  if (nMinCost != -1)
    nTokens += nMinCost;
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nTokens);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
long nTokens2 = 0;
foreach (KMachine machine in machines) {
  machine.Prize.X += 10_000_000_000_000L;
  machine.Prize.Y += 10_000_000_000_000L;

  long nDenominator = machine.B.Y * machine.A.X - machine.B.X * machine.A.Y;
  if (nDenominator != 0) {
    long nNumerator = machine.Prize.Y * machine.A.X - machine.Prize.X * machine.A.Y;
    if (nNumerator % nDenominator == 0) {
      long nBPresses = nNumerator / nDenominator;

      nNumerator = machine.Prize.X - nBPresses * machine.B.X;
      nDenominator = machine.A.X;
      if (nNumerator % nDenominator == 0) {
        long nAPresses = (machine.Prize.X - nBPresses * machine.B.X) / machine.A.X;
        nTokens2 += 3 * nAPresses + nBPresses;
      }
    }
  }
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nTokens2);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
struct Tuple {
  public long X;
  public long Y;
}

class KMachine {
  public Tuple A;
  public Tuple B;
  public Tuple Prize;
}
