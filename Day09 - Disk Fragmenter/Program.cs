// Day 09 - Disk Fragmenter
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(9, "Disk Fragmenter");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the input
string strInput = File.ReadAllText(fileName) + '0';





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
LinkedList<KFile> fileGap = [];
for (int i = 0, nFileId = 0; i < strInput.Length; i += 2, ++nFileId)
  fileGap.AddLast(new KFile() {
    ID = nFileId,
    Size = Int32.Parse(strInput[i].ToString()),
    Gap = Int32.Parse(strInput[i + 1].ToString()),
  });

LinkedListNode<KFile> front = fileGap.First, back = fileGap.Last;
while (front != back) {
  while (front != back && front.Value.Gap == 0)
    front = front.Next;

  if (front != back) {
    if (back.Value.Size <= front.Value.Gap) {
      fileGap.AddAfter(front, new KFile() {
        ID = back.Value.ID,
        Size = back.Value.Size,
        Gap = front.Value.Gap - back.Value.Size,
      });
      fileGap.RemoveLast();
      back = fileGap.Last;
      front.ValueRef.Gap = 0;
    } else {
      back.ValueRef.Size -= front.Value.Gap;
      fileGap.AddAfter(front, new KFile() {
        ID = back.Value.ID,
        Size = front.Value.Gap,
        Gap = 0,
      });
      front.ValueRef.Gap = 0;
    }
  }
}

front = front.Previous;
if (front.Value.ID == back.Value.ID) {
  front.ValueRef.Size += back.Value.Size;
  fileGap.RemoveLast();
}

ulong nCheckSum = 0;
int nPos = 0;
for (front = fileGap.First; front != null; front = front.Next) {
  for (int size = front.Value.Size; size > 0; --size, ++nPos)
    nCheckSum += (ulong)(front.Value.ID * nPos);
}
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nCheckSum);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
fileGap = [];
for (int i = 0, nFileId = 0; i < strInput.Length; i += 2, ++nFileId)
  fileGap.AddLast(new KFile() {
    ID = nFileId,
    Size = Int32.Parse(strInput[i].ToString()),
    Gap = Int32.Parse(strInput[i + 1].ToString()),
  });

for (back = fileGap.Last; back != null;) {
  front = fileGap.First;
  while (front != back && front.Value.Gap < back.Value.Size)
    front = front.Next;
  if (front != back) {
    fileGap.AddAfter(front, new KFile() {
      ID = back.Value.ID,
      Size = back.Value.Size,
      Gap = front.Value.Gap - back.Value.Size,
    });
    front.ValueRef.Gap = 0;
    back.Previous.ValueRef.Gap += back.Value.Size + back.Value.Gap;
    LinkedListNode<KFile> prev = back.Previous;
    fileGap.Remove(back);
    back = prev;
  } else
    back = back.Previous;
}

nCheckSum = 0;
nPos = 0;
for (front = fileGap.First; front != null; front = front.Next) {
  for (int size = front.Value.Size; size > 0; --size, ++nPos)
    nCheckSum += (ulong)(front.Value.ID * nPos);
  nPos += front.Value.Gap;
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nCheckSum);





PrintHelper.ПечатиЕлкаЗаКрај();





// Types
[DebuggerDisplay("ID: {ID}, Size: {Size}, Gap: {Gap}")]
class KFile {
  public int ID { get; set; }
  public int Size { get; set; }
  public int Gap { get; set; }
}
