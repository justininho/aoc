// parse input

var input = File.ReadAllText("input.txt");
var splitInput = input.Split("\n\n");
var rules = splitInput[0].Split("\n").Select(r => {
  var s = r.Split("|");
  var page = int.Parse(s[0]);
  var update = int.Parse(s[1]);
  return (update, page);
}).ToArray();
var updates = splitInput[1].Split("\n").Select(
  update => update
    .Split(",")
    .Select(int.Parse)
    .ToArray()
).ToArray();

int Day5Problem1() {
  var sum = 0;
  foreach (var group in updates) {
    var graph = new Dictionary<int, HashSet<int>>();
    var inDegrees = new Dictionary<int, int>();
    foreach (var (update, page) in rules) {
      if (!group.Contains(update) || !group.Contains(page)) continue;
      if (graph.TryGetValue(page, out var set)) set.Add(update);
      else graph[page] = [update];

      if (inDegrees.TryGetValue(update, out var degree)) inDegrees[update] = degree + 1;
      else inDegrees.TryAdd(update, 1);
      inDegrees.TryAdd(page, 0);
    }

    var visited = 0;
    foreach (var node in group) {
      if (inDegrees.TryGetValue(node, out var value) && value > 0) continue;
      visited++;
      if (!graph.TryGetValue(node, out var dependencies)) continue;
      foreach (var neighbor in dependencies) inDegrees[neighbor] -= 1;
    }

    var isValid = visited == group.Length;
    if (!isValid) continue;
    var mid = group.Length / 2;
    sum += group[mid];
  }

  return sum;
}


Console.WriteLine($"Day 5 Problem 1 Solution: {Day5Problem1()}");


int Day5Problem2() {
  var sum = 0;
  foreach (var group in updates) {
    var graph = new Dictionary<int, List<int>>();
    var inDegrees = new Dictionary<int, int>();
    foreach (var (update, page) in rules) {
      if (!group.Contains(update) || !group.Contains(page)) continue;
      if (graph.TryGetValue(page, out var list)) list.Add(update);
      else graph[page] = [update];

      if (inDegrees.TryGetValue(update, out var degree)) inDegrees[update] = degree + 1;
      else inDegrees.TryAdd(update, 1);
      inDegrees.TryAdd(page, 0);
    }

    var queue = new Queue<int>();
    foreach (var (page, degree) in inDegrees) {
      if (degree != 0) continue;
      queue.Enqueue(page);
    }
    
    var ordered = new List<int>();
    while(queue.Count > 0) {
      var node = queue.Dequeue();
      ordered.Add(node);
      if (!graph.TryGetValue(node, out var dependencies)) continue;
      foreach (var neighbor in dependencies) {
        inDegrees[neighbor] -= 1;
        if (inDegrees[neighbor] != 0) continue;
        queue.Enqueue(neighbor);
      }
    }

    var i = 0;
    while (i < group.Length && ordered[i] == group[i]) i++;
    var inOrder = i == group.Length;
    if (inOrder) continue;
    var mid = ordered.Count / 2;
    sum += ordered[mid];
  }

  return sum;
}

Console.WriteLine($"Day 5 Problem 2 Solution: {Day5Problem2()}");