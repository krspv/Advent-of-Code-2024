// Day 16 - Reindeer Maze
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(16, "Reindeer Maze");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
List<string> maze = [.. File.ReadAllLines(fileName)];
int nSize = maze.Count;
Debug.Assert(maze.All(line => line.Length == nSize));
Debug.Assert(maze[nSize - 2][1] == 'S');
Debug.Assert(maze[1][nSize - 2] == 'E');





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
Queue<(int, int, Orientation, int)> queue = [];
queue.Enqueue((nSize - 2, 1, Orientation.East, 0));  // The S position
int[,] costMap = new int[nSize, nSize];

(int, int) NextRowCol(int nRow, int nCol, Orientation orient) => orient switch {
  Orientation.East => (nRow, nCol + 1),
  Orientation.West => (nRow, nCol - 1),
  Orientation.North => (nRow - 1, nCol),
  Orientation.South => (nRow + 1, nCol),
  _ => throw new Exception("Bad orientation!"),
};

void EnqueueNextMove(Queue<(int, int, Orientation, int)> q, int row, int col, Orientation orient, int cost) {
  if (maze[row][col] != '#' && (costMap[row, col] == 0 || costMap[row, col] > cost)) {
    costMap[row, col] = cost;
    if (maze[row][col] == '.')
      q.Enqueue((row, col, orient, cost));
  }
}

while (queue.Count > 0) {
  Queue<(int, int, Orientation, int)> nextQueue = [];

  while (queue.Count > 0) {
    var (row, col, orient, cost) = queue.Dequeue();

    // Same direction
    var (nextR, nextC) = NextRowCol(row, col, orient);
    EnqueueNextMove(nextQueue, nextR, nextC, orient, cost + 1);

    // Clockwise turn
    int nNextOrientationCW = (int)orient + 1;
    if (nNextOrientationCW == 4) nNextOrientationCW = 0;
    Orientation nextOrient_CW = (Orientation)nNextOrientationCW;
    var (nextR_CW, nextC_CW) = NextRowCol(row, col, nextOrient_CW);
    EnqueueNextMove(nextQueue, nextR_CW, nextC_CW, nextOrient_CW, cost + 1001);

    // Counterclockwise turn
    int nNextOrientationCCW = (int)orient - 1;
    if (nNextOrientationCCW < 0) nNextOrientationCCW = 3;
    Orientation nextOrient_CCW = (Orientation)nNextOrientationCCW;
    var (nextR_CCW, nextC_CCW) = NextRowCol(row, col, nextOrient_CCW);
    EnqueueNextMove(nextQueue, nextR_CCW, nextC_CCW, nextOrient_CCW, cost + 1001);
  }

  queue = nextQueue;
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, costMap[1, nSize - 2]);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
HashSet<(int, int)> history = [];
Queue<(int, int)> queueBack = [];
queueBack.Enqueue((1, nSize - 2));  // The E position

while (queueBack.Count > 0) {
  Queue<(int, int)> nextQueue = [];

  while (queueBack.Count > 0) {
    var (row, col) = queueBack.Dequeue();
    if (!history.Contains((row, col))) {
      history.Add((row, col));

      (int, int)[] directions = [(-1, 0), (1, 0), (0, 1), (0, -1)];
      foreach (var (dR, dC) in directions)
        if (maze[row + dR][col + dC] != '#') {
          if (costMap[row + dR, col + dC] == costMap[row, col] - 1)
            nextQueue.Enqueue((row + dR, col + dC));
          else if (costMap[row + dR, col + dC] == costMap[row, col] - 1001) {
            nextQueue.Enqueue((row + dR, col + dC));
            if (maze[row + 2 * dR][col + 2 * dC] != '#' && costMap[row + 2 * dR, col + 2 * dC] == costMap[row, col] - 2)
              nextQueue.Enqueue((row + 2 * dR, col + 2 * dC));
          }
        }
    }
  }

  queueBack = nextQueue;
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, history.Count);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
enum Orientation { East = 0, South, West, North }
