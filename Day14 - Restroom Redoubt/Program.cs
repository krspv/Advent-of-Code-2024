// Day 14 - Restroom Redoubt
using Helpers;
using System.Diagnostics;
using System.Text.RegularExpressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(14, "Restroom Redoubt");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<Robot> robots = [], robotsP2 = [];
foreach (string line in File.ReadAllLines(fileName)) {
  int[] values = Regex.Matches(line, @"^p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)$")[0]
    .Groups.Cast<Group>().Skip(1).Select(g => Int32.Parse(g.Value)).ToArray();

  robots.Add(new() {
    Pos = (values[0], values[1]),
    Vel = (values[2], values[3]),
  });
  robotsP2.Add(new() {
    Pos = (values[0], values[1]),
    Vel = (values[2], values[3]),
  });
}
Robot.Max = (robots.Max(r => 1 + r.Pos.x), robots.Max(r => 1 + r.Pos.y));





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
foreach (Robot robot in robots)
  robot.Move(100);

int[] quads = { 0, 0, 0, 0 };
foreach (Robot robot in robots)
  if (robot.Quadrant != KQuadrant.None)
    quads[(int)robot.Quadrant]++;

int nProduct = quads.Aggregate((acc, q) => acc * q);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nProduct);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2 (It seems that the christmas tree happens when no robots overlap)
int nSteps = 0;

while (true) {
  bool bOverlap = false;
  bool[,] posOverlap = new bool[Robot.Max.x, Robot.Max.y];

  foreach (Robot robot in robotsP2) {
    robot.Move(1);
    bOverlap |= posOverlap[robot.Pos.x, robot.Pos.y];
    posOverlap[robot.Pos.x, robot.Pos.y] = true;
  }

  nSteps++;
  if (!bOverlap) break;
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nSteps);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
[DebuggerDisplay("Pos: ({Pos.x}, {Pos.y}), Vel: ({Vel.x}, {Vel.y})")]
class Robot {
  public static (int x, int y) Max { get; set; }

  public (int x, int y) Pos { get; set; }
  public (int x, int y) Vel { get; set; }

  public void Move(int nSeconds) {
    (int x, int y) nextPos = ((Pos.x + Vel.x * nSeconds) % Max.x, (Pos.y + Vel.y * nSeconds) % Max.y);
    if (nextPos.x < 0) nextPos.x += Max.x;
    if (nextPos.y < 0) nextPos.y += Max.y;
    Pos = nextPos;
  }

  public KQuadrant Quadrant => Pos switch {
    var (x, y) when (x < Max.x / 2 && y < Max.y / 2) => KQuadrant.TopLeft,
    var (x, y) when (x > Max.x / 2 && y < Max.y / 2) => KQuadrant.TopRight,
    var (x, y) when (x < Max.x / 2 && y > Max.y / 2) => KQuadrant.BottomLeft,
    var (x, y) when (x > Max.x / 2 && y > Max.y / 2) => KQuadrant.BottomRight,
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
