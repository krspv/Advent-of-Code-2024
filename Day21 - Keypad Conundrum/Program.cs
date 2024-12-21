﻿// Day 21 - Keypad Conundrum
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(21, "Keypad Conundrum");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> lines = [.. File.ReadAllLines(fileName)];





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
Robot rDir2 = new Robot(Robot.KeypadType.Directional);
Robot rDir1 = new Robot(Robot.KeypadType.Directional, rDir2);
Robot rNum = new Robot(Robot.KeypadType.Numeric, rDir1);

long CalcComplexity(Robot R) {
  long nSum = 0;

  foreach (string line in lines) {
    long nTotal = 0;
    foreach (char ch in line)
      nTotal += R.Move(ch);
    nSum += nTotal * Int32.Parse(line[..^1]);
  }

  return nSum;
}

long nResult = CalcComplexity(rNum);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nResult);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
Robot rDirNext = new Robot(Robot.KeypadType.Directional);
Robot rDirCur = null;
for (int i = 0; i < 24; ++i) {
  rDirCur = new Robot(Robot.KeypadType.Directional, rDirNext);
  rDirNext = rDirCur;
}
rNum = new Robot(Robot.KeypadType.Numeric, rDirCur);

nResult = CalcComplexity(rNum);
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nResult);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
class Robot(Robot.KeypadType type, Robot child = null) {
  public enum KeypadType { Numeric, Directional };
  private int nRow = type == KeypadType.Numeric ? 3 : 0, nCol = 2;
  private readonly List<string> Keypad = type == KeypadType.Numeric ? ["789", "456", "123", "X0A"] : ["X^A", "<v>"];
  private readonly Robot child = child;
  private readonly Dictionary<((int r, int c) from, (int r, int c) to), long> cache = [];

  public long Move(char chTarget) {
    int nTargetRow = Keypad.FindIndex(row => row.Contains(chTarget));
    int nTargetCol = Keypad[nTargetRow].IndexOf(chTarget);

    var key = ((nRow, nCol), (nTargetRow, nTargetCol));
    if (!cache.ContainsKey(key)) {
      string strVert = nTargetRow switch {
        var n when n < nRow => new string('^', nRow - n),
        var n when n > nRow => new string('v', n - nRow),
        _ => "",
      };
      string strHorz = nTargetCol switch {
        var n when n < nCol => new string('<', nCol - n),
        var n when n > nCol => new string('>', n - nCol),
        _ => "",
      };

      HashSet<string> setComb = Combinations(strVert, strHorz);
      List<string> listComb = [];
      if (setComb.Count > 1) {
        foreach (string comb in setComb) {
          if (IsGood(comb))
            listComb.Add(comb);
        }
      } else
        listComb = [.. setComb];
      listComb = listComb.Select(comb => comb + "A").ToList();

      if (child == null)
        cache[key] = listComb[0].Length;
      else {
        long nRet = Int64.MaxValue;
        foreach (string comb in listComb) {
          long nCur = 0;
          foreach (char ch in comb)
            nCur += child.Move(ch);
          nRet = Math.Min(nRet, nCur);
        }
        cache[key] = nRet;
      }
    }


    nRow = nTargetRow;
    nCol = nTargetCol;
    return cache[key];
  }

  private HashSet<string> Combinations(string s1, string s2) {
    if (s1.Length == 0) return [s2];
    if (s2.Length == 0) return [s1];
    char ch = s2[0];
    string s3 = s2[1..];

    HashSet<string> ret = [];
    for (int i = 0; i <= s1.Length; ++i)
      ret = [.. ret, .. Combinations(s1[..i] + ch + s1[i..], s3)];

    return ret;
  }

  private bool IsGood(string comb) {
    int nR1 = nRow, nC1 = nCol;
    foreach (char ch in comb) {
      switch (ch) {
        case '^': nR1--; break;
        case 'v': nR1++; break;
        case '<': nC1--; break;
        case '>': nC1++; break;
      }
      if (Keypad[nR1][nC1] == 'X') return false;
    }

    return true;
  }
}