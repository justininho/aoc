int Day1Problem1()
{
  Console.WriteLine($"Starting Day1Problem1 at {DateTime.Now:HH:mm:ss.fff}");
  var lineCount = File.ReadLines("input.txt").Count();
  var leftNums = new int[lineCount];
  var rightNums = new int[lineCount];
 
  var readStart = DateTime.Now;
  var i = 0;
  foreach (var line in File.ReadLines("input.txt")) {
    var split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    var ok = int.TryParse(split[0], out var left);
    if (!ok) throw new Exception($"Unable to parse {split[0]}");
    leftNums[i] = left;
    ok = int.TryParse(split[1], out var right);
    if(!ok) throw new Exception($"Unable to parse {split[1]}");
    rightNums[i] = right;
    i++;
  }
  Console.WriteLine($"Reading took: {(DateTime.Now - readStart).TotalMilliseconds}ms");

  var sortStart = DateTime.Now;
  Array.Sort(leftNums);
  Array.Sort(rightNums);
  Console.WriteLine($"Sorting took: {(DateTime.Now - sortStart).TotalMilliseconds}ms");

  var sumStart = DateTime.Now;
  var sum = 0;
  for (i = 0; i < leftNums.Length; i++)
  {
    sum += int.Abs(leftNums[i] - rightNums[i]);
  }
  Console.WriteLine($"Summing took: {(DateTime.Now - sumStart).TotalMilliseconds}ms");
 
  Console.WriteLine($"Total time: {(DateTime.Now - readStart).TotalMilliseconds}ms");
  return sum;
}

Console.WriteLine($"Day 1 Problem 1 Solution: {Day1Problem1()}");

int Day1Problem2()
{
  Console.WriteLine($"Starting Day1Problem2 at {DateTime.Now:HH:mm:ss.fff}");
  var lineCount = File.ReadLines("input.txt").Count();
  var leftNums = new int[lineCount];
  var rightCount = new Dictionary<int, int>();
 
  var readStart = DateTime.Now;
  var i = 0;
  foreach (var line in File.ReadLines("input.txt"))
  {
    var split = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
    var ok = int.TryParse(split[0], out var left);
    if (!ok) throw new Exception($"Unable to parse {split[0]}");
    leftNums[i] = left;
    i++;
    ok = int.TryParse(split[1], out var right);
    if (!ok) throw new Exception($"Unable to parse {split[1]}");
    rightCount[right] = rightCount.GetValueOrDefault(right) + 1;
  }
  Console.WriteLine($"Reading took: {(DateTime.Now - readStart).TotalMilliseconds}ms");

  var calcStart = DateTime.Now;
  var similarityScore = 0;
  var memo = new Dictionary<int, int>();
  foreach (var n in leftNums)
  {
    if (memo.TryGetValue(n, out var val))
    {
      similarityScore += val;
    }
    else if (rightCount.TryGetValue(n, out var count))
    {
      val = n * count;
      similarityScore += val;
      memo[n] = val;
    }
  }
  Console.WriteLine($"Calculation took: {(DateTime.Now - calcStart).TotalMilliseconds}ms");
 
  Console.WriteLine($"Total time: {(DateTime.Now - readStart).TotalMilliseconds}ms");
  return similarityScore;
}

Console.WriteLine($"Day 1 Problem 2 Solution: {Day1Problem2()}");
