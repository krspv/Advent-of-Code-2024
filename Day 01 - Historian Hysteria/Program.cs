// Day 01 - Historian Hysteria
using Helpers;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Stopwatch stopwatch = new();
PrintHelper.ПечатиНаслов(1, "Historian Hysteria");

string fileName = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName, "input.txt");





// Read the lists
List<int> leftList = [], rightList = [];
foreach (string line in File.ReadAllLines(fileName)) {
  int[] nums = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
  Debug.Assert(nums.Length == 2);
  leftList.Add(nums[0]);
  rightList.Add(nums[1]);
}





stopwatch.Start();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 1
leftList.Sort();
rightList.Sort();

int nSum = 0;
for (int i = 0; i < leftList.Count; ++i)
  nSum += Math.Abs(leftList[i] - rightList[i]);
// Part 1
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();

PrintHelper.ПечатиПрвДел(stopwatch.ElapsedMilliseconds, nSum);





stopwatch.Restart();
////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Part 2
Dictionary<int, int> timesAppears = [];

nSum = 0;
foreach (int num in leftList) {
  if (!timesAppears.ContainsKey(num)) {
    int index = rightList.BinarySearch(num);
    if (index > 0) {
      int lowIndex = index, highIndex = index;
      while (lowIndex > 0 && rightList[lowIndex - 1] == num) lowIndex--;
      while (highIndex < rightList.Count - 1 && rightList[highIndex + 1] == num) highIndex++;
      timesAppears[num] = highIndex - lowIndex + 1;
    } else
      timesAppears[num] = 0;
  }

  nSum += num * timesAppears[num];
}
// Part 2
////////////////////////////////////////////////////////////////////////////////////////////////////////////
stopwatch.Stop();
PrintHelper.ПечатиВторДел(stopwatch.ElapsedMilliseconds, nSum);





PrintHelper.ПечатиЕлкаЗаКрај();
