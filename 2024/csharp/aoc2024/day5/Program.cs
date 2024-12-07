var input = File.ReadAllText("input.txt").Split("\n\n");
var rules = input[0].Split("\n")
    .Select(r => {
        var s = r.Split("|");
        return (update: int.Parse(s[1]), page: int.Parse(s[0]));
    });
var updates = input[1].Split("\n")
    .Select(u => u.Split(",").Select(int.Parse).ToArray());

int Day5Problem1() => updates.Sum(group => {
    var (graph, inDegrees) = BuildGraph(group);
    var visited = group.Count(node => 
        inDegrees.GetValueOrDefault(node, 0).Equals(0) && ProcessNode(node, graph, inDegrees));
    return visited == group.Length ? group[group.Length / 2] : 0;
});

int Day5Problem2() => updates.Sum(group => {
    var (graph, inDegrees) = BuildGraph(group);
    var ordered = TopologicalSort(graph, inDegrees);
    return group.Zip(ordered).All(p => p.First == p.Second) ? 0 : ordered[ordered.Count / 2];
});

(Dictionary<int, HashSet<int>>, Dictionary<int, int>) BuildGraph(int[] group) {
    var graph = new Dictionary<int, HashSet<int>>();
    var inDegrees = new Dictionary<int, int>();
    
    foreach (var (update, page) in rules.Where(r => 
        group.Contains(r.update) && group.Contains(r.page))) {
        graph.TryAdd(page, []);
        graph[page].Add(update);
        inDegrees[update] = inDegrees.GetValueOrDefault(update) + 1;
        inDegrees.TryAdd(page, 0);
    }
    return (graph, inDegrees);
}

bool ProcessNode(int node, Dictionary<int, HashSet<int>> graph, Dictionary<int, int> inDegrees) {
  if (!graph.TryGetValue(node, out var deps)) return true;
  foreach (var n in deps) inDegrees[n]--;
  return true;
}

List<int> TopologicalSort(Dictionary<int, HashSet<int>> graph, Dictionary<int, int> inDegrees) {
    var queue = new Queue<int>(inDegrees.Where(kv => kv.Value == 0).Select(kv => kv.Key));
    var ordered = new List<int>();
    
    while (queue.TryDequeue(out var node)) {
        ordered.Add(node);
        if (!graph.TryGetValue(node, out var deps)) continue;
        foreach (var n in deps.Where(n => --inDegrees[n] == 0)) queue.Enqueue(n);
    }
    return ordered;
}

Console.WriteLine($"Day 5 Problem 1: {Day5Problem1()}");
Console.WriteLine($"Day 5 Problem 2: {Day5Problem2()}");