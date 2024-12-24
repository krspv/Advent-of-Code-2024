// Day 24 - Crossed Wires
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(24, "Crossed Wires");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
HashSet<string> wires = [];
Dictionary<string, bool> wireValues = [];
HashSet<Gate> gates = [], finishedGates = [];

bool bReadingValues = true;
foreach (string line in File.ReadAllLines(fileName)) {
  if (line.Trim() == "") bReadingValues = false;
  else if (bReadingValues) {
    string[] wVal = line.Split(':', StringSplitOptions.TrimEntries);
    wires.Add(wVal[0]);
    wireValues[wVal[0]] = wVal[1] == "0" ? false : true;
  } else {
    string[] inOut= line.Split("->", StringSplitOptions.TrimEntries);
    string[] inGate = inOut[0].Split(' ', StringSplitOptions.TrimEntries);
    Gate gate = new() { input = [inGate[0], inGate[2]], output = inOut[1], type = Gate.FromTypeString(inGate[1]) };
    gates.Add(gate);
  }
}





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
while (gates.Count > 0) {
  foreach (Gate gate in gates.ToList()) {
    if (wireValues.ContainsKey(gate.input[0]) && wireValues.ContainsKey(gate.input[1])) {
      wireValues[gate.output] = gate.Operate(wireValues);
      gates.Remove(gate);
      finishedGates.Add(gate);
    }
  }
}

Dictionary<int, bool> zValues = [];
foreach (var wVal in wireValues)
  if (wVal.Key.StartsWith('z'))
    zValues[Int32.Parse(wVal.Key[1..])] = wVal.Value;

long nResult = 0;
for (int nZ = zValues.Count - 1; nZ >= 0; --nZ)
  if (zValues[nZ])
    nResult += 1L << nZ;
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nResult);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
Dictionary<string, Gate> gatesByOutput = [];
foreach (Gate gate in finishedGates)
  gatesByOutput[gate.output] = gate;

int nBitCountZ = finishedGates.GroupBy(gate => gate.output).Count(val => val.Key.StartsWith('z'));

HashSet<string> toBeSwapped = [];
for (int i = 0; i < nBitCountZ; ++i) {
  string strNextZ = $"z{i:D2}";

  Gate gate = gatesByOutput[strNextZ];
  if (strNextZ == "z00") {
    if (gate.type != Gate.Type.XOR)
      toBeSwapped.Add(strNextZ);
  } else {
    Gate gateCarry;
    Gate.Type carryType = i > 1 ? Gate.Type.OR : Gate.Type.AND;

    if (i == nBitCountZ - 1) {
      gateCarry = gate;
    } else {
      if (gate.type != Gate.Type.XOR) {
        Debug.Assert(gate.output == strNextZ);
        toBeSwapped.Add(strNextZ);
        continue;
      }
      Gate g1 = gatesByOutput[gate.input[0]];
      Gate g2 = gatesByOutput[gate.input[1]];

      Gate gateAdder = g1.type == Gate.Type.XOR ? g1 : g2;
      gateCarry = g1.type == carryType ? g1 : g2;

      if (gateAdder.type != Gate.Type.XOR && gateCarry.type != carryType) { // Both are bad
        toBeSwapped.Add(gateAdder.output);
        toBeSwapped.Add(gateCarry.output);
        continue;
      }

      if (gateAdder.type == Gate.Type.XOR && gateCarry.type != carryType) {  // Carry is bad
        if (gateCarry == gateAdder)
          gateCarry = gateCarry == g1 ? g2 : g1;  // Get the proper bad one
        toBeSwapped.Add(gateCarry.output);
        gateCarry = null;
      }

      if (gateAdder.type != Gate.Type.XOR && gateCarry.type == carryType) {  // Adder is bad
        if (gateAdder == gateCarry)
          gateAdder = gateAdder == g1 ? g2 : g1;  // Get the proper bad one
        toBeSwapped.Add(gateAdder.output);
        gateAdder = null;
      }
    }

    if (carryType == Gate.Type.OR && gateCarry != null && gatesByOutput.ContainsKey(gateCarry.input[0])) {
      Gate gC1 = gatesByOutput[gateCarry.input[0]];
      Gate gC2 = gatesByOutput[gateCarry.input[1]];

      if (gC1.type != Gate.Type.AND)
        toBeSwapped.Add(gC1.output);

      if (gC2.type != Gate.Type.AND)
        toBeSwapped.Add(gC2.output);
    }
  }
}

string strBadOnes = String.Join(',', toBeSwapped.Order().ToList());
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, strBadOnes);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
[DebuggerDisplay("{input[0]} {type} {input[1]} -> {output}")]
class Gate {
  public enum Type { OR, XOR, AND };
  public string[] input = new string[2];
  public string output;
  public Type type;

  public static Type FromTypeString(string strType) => strType switch {
    "OR" => Type.OR,
    "XOR" => Type.XOR,
    "AND" => Type.AND,
    _ => throw new Exception("Invalid Gate Type"),
  };

  public bool Operate(Dictionary<string, bool> wVals) => type switch {
    Type.OR => wVals[input[0]] || wVals[input[1]],
    Type.AND => wVals[input[0]] && wVals[input[1]],
    Type.XOR => wVals[input[0]] ^ wVals[input[1]],
    _ => throw new Exception("Invalid Gate Type (in Operate)"),
  };
}
