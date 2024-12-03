// Day 03 - Mull It Over
using Helpers;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(3, "Mull It Over");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
string input = File.ReadAllText(fileName);





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
int SolveForInput(string input) =>
  Regex.Matches(input, @"mul\(\d{1,3},\d{1,3}\)")
  .Aggregate(0, (acc, match) => acc + Regex.Matches(match.Value, @"\d{1,3}")
      .Select(match => Int32.Parse(match.Value))
      .Aggregate(1, (acc, n) => acc * n));

int nSolution = SolveForInput(input);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nSolution);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2

// Reduce the input (by removing the "don't()" parts)
StringBuilder sb = new();
bool bPrevDo = true;
while (true) {
  int idxDo = input.IndexOf("do()");
  int idxDont = input.IndexOf("don't()");

  if (idxDo == -1 && idxDont == -1) {
    if (bPrevDo) sb.Append(input);
    break;
  }

  bool bNextDo = idxDont == -1 || (idxDont > idxDo);
  int idx = bNextDo ? idxDo : idxDont;
  if (bPrevDo) sb.Append(input[..idx]);
  input = input[(idx + (bNextDo ? 4 : 7))..];
  bPrevDo = bNextDo;
}

nSolution = SolveForInput(sb.ToString());
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nSolution);





PrintHelper.ПечатиЕлкаЗаКрај();
