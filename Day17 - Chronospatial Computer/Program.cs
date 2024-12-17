// Day 17 - Chronospatial Computer
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
string strTarget = lines[4].Split(':', StringSplitOptions.TrimEntries)[1];
long A = 0;
while (true) {
  while (true) {
    computer = new(lines);
    computer.SetA(A);
    computer.Run();
    if (strTarget.EndsWith(computer.Output)) break;
    A++;
  }
  if (strTarget == computer.Output) break;
  A *= 8;
  if (A == 0) A = 1;
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
