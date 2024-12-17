﻿// Day 17 - Chronospatial Computer
using Helpers;
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(17, "Chronospatial Computer");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> lines = [.. File.ReadAllLines(fileName)];




stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
KComputer computer = new(lines);
computer.Run();
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, computer.Output);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
List<char> list = (lines[4].Split(':', StringSplitOptions.TrimEntries)[1].Split(',')).Select(Char.Parse).ToList();
list.Reverse();
List<int> lstUsed = [];
long A = 0;
StringBuilder sb = new();

foreach (char ch in list) {
  A = 0;
  long nShifter = 8;
  for (int i = lstUsed.Count - 1; i >= 0; --i) {
    A += nShifter * lstUsed[i];
    nShifter <<= 3;
  }

  if (sb.Length > 0) sb.Insert(0, ',');
  sb.Insert(0, ch);
  string strExpected = sb.ToString();
  long tmp = A;

  while (true) {
    computer = new(lines);
    computer.SetA(A);
    computer.Run();
    if (computer.Output == strExpected)
      break;
    A++;
  }

  lstUsed.Add((int)(A - tmp));
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, A);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
enum KOpcode { ADV = 0, BXL, BST, JNZ, BXC, OUT, BDV, CDV };

class KComputer {
  private List<(KOpcode, int)> program;
  private long[] Reg = new long[3]; // Registers
  private int nHead;
  private List<char> output = [];
  public string Output => String.Join(',', output);

  public KComputer(List<string> inputLines) {
    nHead = 0;

    for (int i = 0; i < 3; ++i) {
      string[] strReg = inputLines[i].Split(':', StringSplitOptions.TrimEntries);
      Debug.Assert(strReg.Length == 2 && strReg[0].StartsWith("Register ") && strReg[0].Last() == "ABC"[i]);
      Reg[i] = Int64.Parse(strReg[1]);
    }

    string[] strProg = inputLines[4].Split(':', StringSplitOptions.TrimEntries);
    Debug.Assert(strProg.Length == 2 && strProg[0] == "Program");
    strProg = strProg[1].Split(',', StringSplitOptions.TrimEntries);

    program = [];
    for (int i = 0; i < strProg.Length; i += 2)
      program.Add(((KOpcode)Int32.Parse(strProg[i]), Int32.Parse(strProg[i + 1])));
  }

  private long GetComboVal(int operand) => operand < 4 ? operand : Reg[operand - 4];

  public void Run() {
    while (nHead < program.Count) {
      var (opCode, operand) = program[nHead++];
      long opCombo = GetComboVal(operand);
      switch (opCode) {
        case KOpcode.ADV: Reg[0] = Reg[0] / (1L << (int)opCombo); break;
        case KOpcode.BXL: Reg[1] = Reg[1] ^ operand;              break;
        case KOpcode.BST: Reg[1] = opCombo % 8;                   break;
        case KOpcode.JNZ: if (Reg[0] != 0) nHead = operand >> 1;  break;
        case KOpcode.BXC: Reg[1] = Reg[1] ^ Reg[2];               break;
        case KOpcode.OUT: output.Add((char)('0' + opCombo % 8));  break;
        case KOpcode.BDV: Reg[1] = Reg[0] / (1L << (int)opCombo); break;
        case KOpcode.CDV: Reg[2] = Reg[0] / (1L << (int)opCombo); break;
      }
    }
  }

  public void SetA(long val) { Reg[0] = val; }
}
