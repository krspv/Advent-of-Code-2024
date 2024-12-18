// Day 18 - RAM Run
using Helpers;
using System.Diagnostics;
using System.Text;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(18, "RAM Run");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<(int, int)> memoryFall = [];
foreach (string line in File.ReadAllLines(fileName))
  memoryFall.Add(line.Split(',').Select(Int32.Parse).ToList() switch { var L => (L[0], L[1]) });





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
(int nSize, int nTotalFalls) = fileName.EndsWith("input.txt") ? (73, 1024) : (9, 12);
int[,] mem = new int[nSize,nSize];

// Initialize the memory space with a border
for (int x = 0; x < nSize; ++x)
  for (int y = 0; y < nSize; ++y)
    mem[y, x] = (x == 0 || x == nSize - 1 || y == 0 || y == nSize - 1) ? -1 : Int32.MaxValue;
// string mv = DebugMemorySpace();

// First fall
for (int n = 0; n < nTotalFalls; ++n) {
  (int x, int y) = memoryFall[n];
  mem[y + 1, x + 1] = -1;
}

(int, int)[] directions = [(-1, 0), (1, 0), (0, 1), (0, -1)];
// mv = DebugMemorySpace();

void MinPath((int x, int y) pos, int nCurrentScore) {
  mem[pos.y, pos.x] = nCurrentScore;
  if (pos.x == nSize - 2 && pos.y == nSize - 2) return;

  foreach (var (dY, dX) in directions) {
    var (nextY, nextX) = (pos.y + dY, pos.x + dX);
    if (mem[nextY,nextX]>nCurrentScore+1) MinPath((nextX, nextY), nCurrentScore + 1);
  }
}

MinPath((1, 1), 0);
// mv = DebugMemorySpace();
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, mem[nSize - 2, nSize - 2]);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
bool bCanReach;
void CanReach((int x, int y) pos, HashSet<(int, int)> set) {
  if (pos == (1, 1)) bCanReach = true;
  if (!bCanReach && mem[pos.y, pos.x] >= 0 && !set.Contains(pos)) {
    set.Add(pos);
    foreach (var (dY, dX) in directions) CanReach((pos.x + dX, pos.y + dY), set);
  }
}

string strSolution = "";
for (int n = nTotalFalls; n < memoryFall.Count; ++n) {
  var (x, y) = memoryFall[n];
  mem[y + 1, x + 1] = -1;
  // mv = DebugMemorySpace();
  bCanReach = false;
  CanReach((nSize - 2, nSize - 2), []);
  if (!bCanReach) {
    strSolution = $"{x},{y}";
    break;
  }
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, strSolution);





PrintHelper.ПечатиЕлкаЗаКрај();





// Debug utils
#pragma warning disable CS8321
string DebugMemorySpace() {
  StringBuilder sb = new();
  for (int x = 0; x < mem.GetLength(0); ++x) {
    for (int y = 0; y < mem.GetLength(1); ++y) {
      sb.Append(mem[x, y] switch { -1 => '#', > 0 => '.', _ => 'O' });
    }
    sb.Append('\n');
  }
  return sb.ToString();
}
#pragma warning restore CS8321
