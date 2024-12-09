long Day7Problem1() {
  long sum = 0;
  var equations = ParseInput("input.txt");

  foreach (var (answer, vars) in equations) {
    if (IsValid(answer, vars, 0)) sum += answer;
  }

  return sum;
}

long Day7Problem2() {
  long sum = 0;
  var equations = ParseInput("input.txt");

  foreach (var (answer, vars) in equations) {
    if (IsValid(answer, vars, 0, true)) sum += answer;
  }

  return sum;
}

List<(long answer, int[] vars)> ParseInput(string inputPath) {
  var equations = new List<(long answer, int[] vars)>();
  foreach (var line in File.ReadLines(inputPath)) {
    var s = line.Split(":", StringSplitOptions.TrimEntries);
    var answer = long.Parse(s[0]);
    var vars = s[1].Split(" ").Select(int.Parse).ToArray();
    equations.Add((answer, vars));
  }

  return equations;
}

bool IsValid(long answer, int[] vars, long acc, bool includeConcat = false) {
  if (vars.Length == 0) {
    return acc == answer;
  }

  var currentVar = vars[0];
  var rest = vars[1..vars.Length];
  var add = IsValid(answer, rest, acc + currentVar, includeConcat);
  var mult = IsValid(answer, rest, acc * currentVar, includeConcat);
  
  if (!includeConcat) return add || mult;
  
  // concatenate acc and currentVar
  var digits = currentVar == 0 ? 1 : (int)Math.Floor(Math.Log10(currentVar) + 1);
  var result = acc * (int)Math.Pow(10, digits) + currentVar;
  var concat = IsValid(answer, rest, result, includeConcat);
  return add || mult || concat;
}

Console.WriteLine($"Day 7 Problem 1 Solution: {Day7Problem1()}");
Console.WriteLine($"Day 7 Problem 2 Solution: {Day7Problem2()}");