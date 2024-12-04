// See https://aka.ms/new-console-template for more information

// int Day2Problem1()
// {
//   var safeCount = 0;
//   foreach (var line in File.ReadLines("input.txt")) {
//     var levels = line
//       .Split(" ", StringSplitOptions.RemoveEmptyEntries)
//       .Select(int.Parse)
//       .ToArray();
//
//     var isSafe = true;
//     var direction = 0;
//     // only go to length - 1 because we are always checking ahead
//     for (var i = 0; i < levels.Length - 1; i++) {
//       var delta = levels[i] - levels[i + 1];
//       if (direction == 0) {
//         direction = Math.Sign(delta);
//       }
//       // invariants:
//       // 1. Any two adjacent levels differ by at least one and at most three.
//       // 2. The levels are either all increasing or all decreasing.
//       var outOfRange = int.Abs(delta) is < 1 or > 3;
//       var wrongDirection = Math.Sign(delta) != direction;
//
//       if (!outOfRange && !wrongDirection) continue;
//       isSafe = false;
//       break;
//     }
//
//     if (isSafe) safeCount++;
//   }
//
//   return safeCount;
// }

int Day2Problem1()
{
  return File.ReadLines("input.txt")
    .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
      .Select(int.Parse)
      .ToArray())
    .Count(IsValidSequence);
}

Console.WriteLine($"Day 2 Problem 1 Solution: {Day2Problem1()}");

// bool IsValid(int delta, int direction) {
//   var outOfRange = int.Abs(delta) is < 1 or > 3;
//   var wrongDirection = direction != 0 && Math.Sign(delta) != direction;
//   return !outOfRange && !wrongDirection;
// }

// int Day2Problem2()
// {
//   var safeCount = 0;
//   foreach (var line in File.ReadLines("input.txt")) {
//     var levels = line
//       .Split(" ", StringSplitOptions.RemoveEmptyEntries)
//       .Select(int.Parse)
//       .ToArray();
//
//     var isSafe = true;
//     var direction = 0;
//     var hasSkip = true;
//
//     for (var i = 0; i < levels.Length - 1; i++) {
//       var delta = levels[i + 1] - levels[i]; // Note: reversed the subtraction
//
//       if (IsValid(delta, direction)) {
//         direction = direction == 0 ? Math.Sign(delta) : direction;
//         continue;
//       }
//
//       if (hasSkip) {
//         hasSkip = false;
//         
//         // var isFirst = i == 0;
//         var isLast = i + 1 >= levels.Length - 1;
//         if (isLast) {
//           continue;
//         }
//
//         // Try skipping current number (i)
//         var hasTwoAhead = i + 2 < levels.Length;
//         if (hasTwoAhead) {
//           var newDelta = levels[i + 2] - levels[i];
//           // var newDirection = direction == 0 ? Math.Sign(newDelta) : direction;
//           if (IsValid(newDelta, 0)) {
//             direction = Math.Sign(newDelta);
//             i++;
//             continue;
//           }
//         }
//
//         // Try skipping previous number (look back)
//         var hasBeforeAndAfter = i > 0 && i + 1 < levels.Length;
//         if (hasBeforeAndAfter) {
//           var newDelta = levels[i + 1] - levels[i - 1];
//           // var newDirection = direction == 0 ? Math.Sign(newDelta) : direction;
//           if (IsValid(newDelta, 0)) {
//             direction = Math.Sign(newDelta);
//             continue;
//           }
//         } 
//       }
//
//       isSafe = false;
//       break;
//     }
//
//     if (isSafe) safeCount++;
//     Console.WriteLine($"{string.Join(" ", levels)}");
//     Console.WriteLine($"is Safe: {isSafe}");
//   }
//
//   return safeCount;
// }

int Day2Problem2()
{
  var safeCount = 0;
  foreach (var line in File.ReadLines("input.txt")) {
    var values = line
      .Split(" ", StringSplitOptions.RemoveEmptyEntries)
      .Select(int.Parse)
      .ToArray();

    // First check if valid without skips
    if (IsValidSequence(values)) {
      safeCount++;
      continue;
    }

    // Try removing each number
    var isSafe = values
      .Select((_, skip) => values.Take(skip).Concat(values.Skip(skip + 1)).ToArray()).Any(IsValidSequence);
    if (isSafe) safeCount++;
  }
    
  return safeCount;
}

bool IsValidSequence(int[] values)
{
  var safePos = new HashSet<int> { 1, 2, 3 };
  var safeNeg = new HashSet<int> { -1, -2, -3 };
    
  for (var i = 1; i < values.Length; i++) {
    var delta = values[i] - values[i - 1];
    safePos.Add(delta);
    safeNeg.Add(delta);
  }
    
  return safePos.Count == 3 || safeNeg.Count == 3;
}

Console.WriteLine($"Day 2 Problem 2 Solution: {Day2Problem2()}");