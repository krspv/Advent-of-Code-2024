// Day 23 - LAN Party
using Helpers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(23, "LAN Party");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input2.txt");





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
int nMaxEdges = edges.Max(edge => edge.Value.Count);  // This is how many computers the maximum LAN party will have

foreach (string v1 in vertices)
  foreach (string v2 in vertices) {
    if (edges[v1].Contains(v2)) {
      int nCount = 0; // Count how many indirect connections between v1 and v2 exist (v1 - v3 - v2) [i.e. v3 is between v1 and v2]
      foreach (string v3 in vertices) {
        if (v3 != v1 && v3 != v2) {
          if (edges[v1].Contains(v3) && edges[v3].Contains(v2))
            nCount++;
        }
      }
      if (nCount < nMaxEdges - 2) { // This edge (v1 - v2) does not belong to the maximum LAN party, so remove it
        edges[v1].Remove(v2);
        edges[v2].Remove(v1);
      }
    }
  }

string vertexMax = vertices.First(vert => edges[vert].Count > 0); // Member of the maximum LAN party
HashSet<string> setMax = [vertexMax, .. edges[vertexMax]];  // The maximum LAN party
string password = String.Join(',', setMax.OrderBy(s => s));
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, password);





PrintHelper.ПечатиЕлкаЗаКрај();
