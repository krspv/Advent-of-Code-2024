// Day 14 - Restroom Redoubt
using Helpers;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(14, "Restroom Redoubt");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");
string fileOut = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "output.txt");





// Read the input
List<Robot> robots = [], robotsForPart2 = [];
foreach (string line in File.ReadAllLines(fileName)) {
  int[] values = Regex.Matches(line, @"^p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)$")[0]
    .Groups.Cast<Group>().Skip(1).Select(g => Int32.Parse(g.Value)).ToArray();
  robots.Add(new Robot() {
    Pos = (values[0], values[1]),
    Vel = (values[2], values[3]),
  });
  robotsForPart2.Add(new Robot() {
    Pos = (values[0], values[1]),
    Vel = (values[2], values[3]),
  });
}
(long x, long y) maxPos = (robots.Max(r => 1 + r.Pos.x), robots.Max(r => 1 + r.Pos.y));





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
foreach (Robot robot in robots)
  robot.Move(100, maxPos);

int[] quads = { 0, 0, 0, 0 };
foreach (Robot robot in robots) {
  int nQuadrant = (int)robot.Quadrant(maxPos);
  if (nQuadrant >= 0)
    quads[nQuadrant]++;
}

int nProduct = quads.Aggregate((acc, q) => acc * q);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nProduct);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
long nSteps = 98;
foreach (Robot robot in robotsForPart2)
  robot.Move(98, maxPos);

while (true) {
  bool[,] mat = new bool[maxPos.x, maxPos.y];
  foreach (Robot robot in robotsForPart2)
    mat[robot.Pos.x, robot.Pos.y] = true;
  using (StreamWriter writer = new StreamWriter(fileOut)) {
    writer.WriteLine(nSteps);
    for (int y = 0; y < maxPos.y; ++y) {
      for (int x = 0; x < maxPos.x; ++x) {
        writer.Write(mat[x, y] ? '#' : '.');
      }
      writer.WriteLine();
    }
  }

  if (nSteps == 7572) break;

  nSteps += maxPos.x;
  foreach (Robot robot in robotsForPart2)
    robot.Move(maxPos.x, maxPos);
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nSteps);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
[DebuggerDisplay("Pos: ({Pos.x}, {Pos.y}), Vel: ({Vel.x}, {Vel.y})")]
class Robot {
  public (long x, long y) Pos { get; set; }
  public (long x, long y) Vel { get; set; }

  public void Move(long nSeconds, (long x, long y) mxPos) {
    (long x, long y) nextPos = ((Pos.x + Vel.x * nSeconds) % mxPos.x, (Pos.y + Vel.y * nSeconds) % mxPos.y);
    if (nextPos.x < 0)
      nextPos.x += mxPos.x;
    if (nextPos.y < 0)
      nextPos.y += mxPos.y;
    Pos = nextPos;
  }

  public KQuadrant Quadrant((long x, long y) mxPos) => Pos switch {
    var (x, y) when (x < mxPos.x / 2 && y < mxPos.y / 2) => KQuadrant.TopLeft,
    var (x, y) when (x > mxPos.x / 2 && y < mxPos.y / 2) => KQuadrant.TopRight,
    var (x, y) when (x < mxPos.x / 2 && y > mxPos.y / 2) => KQuadrant.BottomLeft,
    var (x, y) when (x > mxPos.x / 2 && y > mxPos.y / 2) => KQuadrant.BottomRight,
    _ => KQuadrant.None,
  };
}

enum KQuadrant {
  None = -1,
  TopLeft = 0,
  TopRight,
  BottomLeft,
  BottomRight,
}
