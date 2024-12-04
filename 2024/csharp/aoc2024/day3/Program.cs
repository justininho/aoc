using System.Text.RegularExpressions;

int Day3Problem1() {
  const string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
  return (from line in File.ReadLines("input.txt")
    from Match match in Regex.Matches(line, pattern)
    let x = int.Parse(match.Groups[1].Value)
    let y = int.Parse(match.Groups[2].Value)
    select x * y).Sum();
}

Console.WriteLine($"Day 3 Problem 1 Solution: {Day3Problem1()}");


int Day3Problem2() {
  var sum = 0;
  const string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";

  var file = File.ReadAllText("input.txt");
  var matches = Regex.Matches(file, mulPattern);
  foreach (Match match in matches) {
    var beforeMatch = file[..match.Index];
    var doIndex = beforeMatch.LastIndexOf("do()", StringComparison.Ordinal);
    var dontIndex =
      beforeMatch.LastIndexOf("don't()", StringComparison.Ordinal);

    var noDisable = dontIndex == -1;
    var enableAfterDisable =
      doIndex >
      dontIndex;

    var isEnabled = noDisable || enableAfterDisable;
    if (!isEnabled) continue;
    var x = int.Parse(match.Groups[1].Value);
    var y = int.Parse(match.Groups[2].Value);
    sum += x * y;
  }

  return sum;
}

int Day3Problem2Lines() {
  var sum = 0;
  const string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";
  var carryOverDont = false;
  foreach (var line in File.ReadLines("input.txt")) {
    var matches = Regex.Matches(line, mulPattern);
    int lastDoIndex;
    int lastDontIndex;
    foreach (Match match in matches) {
      var beforeMatch = line[..match.Index];
      lastDoIndex = beforeMatch.LastIndexOf("do()", StringComparison.Ordinal);
      lastDontIndex = beforeMatch.LastIndexOf("don't()", StringComparison.Ordinal);

      var noDisable = lastDontIndex == -1 && !carryOverDont;
      var enableAfterDisable =
        lastDoIndex >
        lastDontIndex;

      var isEnabled = noDisable || enableAfterDisable;
      if (!isEnabled) continue;
      var x = int.Parse(match.Groups[1].Value);
      var y = int.Parse(match.Groups[2].Value);
      sum += x * y;
    }

    lastDoIndex = line.LastIndexOf("do()", StringComparison.Ordinal);
    lastDontIndex = line.LastIndexOf("don't()", StringComparison.Ordinal);
    carryOverDont = lastDontIndex > lastDoIndex;
  }

  return sum;
}

// // 89912299 -- too high
// // 87163705
Console.WriteLine($"Day 3 Problem 2 Solution: {Day3Problem2()}");