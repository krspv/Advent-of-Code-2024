// Day 23 - LAN Party
using Helpers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(23, "LAN Party");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
Dictionary<string, HashSet<string>> edges = [];
HashSet<string> vertices = [];
foreach (string line in File.ReadAllLines(fileName)) {
  string[] v = [.. line.Split('-', StringSplitOptions.TrimEntries)];
  vertices.Add(v[0]);
  vertices.Add(v[1]);
  if (!edges.ContainsKey(v[0])) edges[v[0]] = [];
  if (!edges.ContainsKey(v[1])) edges[v[1]] = [];
  edges[v[0]].Add(v[1]);
  edges[v[1]].Add(v[0]);
}





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
List<string> listVertices = [.. vertices];

int nTotal = 0;
for (int i = 0; i < listVertices.Count - 2; ++i) {
  string vert1 = listVertices[i];
  for (int j = i + 1; j < listVertices.Count - 1; ++j) {
    string vert2 = listVertices[j];
    if (!edges[vert1].Contains(vert2)) continue;
    for (int k = j + 1; k < listVertices.Count; ++k) {
      string vert3 = listVertices[k];
      if (vert3[0] == 't' || vert2[0] == 't' || vert1[0] == 't') {
        if (edges[vert3].Contains(vert1) && edges[vert3].Contains(vert2))
          nTotal++;
      }
    }
  }
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nTotal);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
HashSet<HashSet<string>> groups = new(new KSetComparer());  // Initial group of 2 elements each
foreach (string vertice in vertices) {
  foreach (string next in edges[vertice]) {
    groups.Add([vertice, next]);
  }
}

while (true) {
  HashSet<HashSet<string>> nextGroups = new(new KSetComparer());  // Groups of 3, 4, 5 etc elements each
  foreach (string vertice in vertices) {
    foreach (HashSet<string> group in groups) {
      if (!group.Contains(vertice) && group.IsSubsetOf(edges[vertice])) {
        nextGroups.Add([vertice, .. group]);
      }
    }
  }
  groups = nextGroups;
  if (groups.Count < 2) break;
}

List<string> list = [.. groups.First()];
list.Sort();
string password = string.Join(',', list);
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, password);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
class KSetComparer : IEqualityComparer<HashSet<string>> {
  public bool Equals(HashSet<string> x, HashSet<string> y) => x.SetEquals(y);
  public int GetHashCode([DisallowNull] HashSet<string> obj) => obj.Aggregate(0, (hash, item) => hash ^ item.GetHashCode());
}
