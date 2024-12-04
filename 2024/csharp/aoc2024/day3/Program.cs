using System.Text.RegularExpressions;


// int Day3Problem1() {
//   const string validSyntax = "mul(x,y)";
//   var sum = 0;
//   foreach (var line in File.ReadLines("input.txt")) {
//     var s = 0;
//     var x = "";
//     var y = "";
//     foreach (var c in line) {
//       if (validSyntax[s] == 'x') {
//         if (char.IsDigit(c)) {
//           x += c;
//           continue;
//         } 
//         if (x.Length > 0) s++;
//       }
//       
//       if (validSyntax[s] == 'y') {
//         if (char.IsDigit(c)) {
//           y += c;
//           continue;
//         }
//       
//         if (y.Length > 0) s++;
//       }
//       
//       if (c == validSyntax[s]) {
//         if (s == validSyntax.Length - 1) {
//           sum += int.Parse(x) * int.Parse(y);
//           s = 0;
//           x = y = "";
//         } else s++;
//       } else {
//         s = 0;
//         x = y = "";
//       }
//     }
//     
//     if (s == validSyntax.Length - 1) sum += int.Parse(x) * int.Parse(y);
//   }
//
//   return sum;
// }

// int SumSegment(string segment) {
//   var sum = 0;
//   const string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
//   foreach (var line in File.ReadLines("input.txt"))
//   {
//     var matches = Regex.Matches(line, pattern);
//     foreach (Match match in matches)
//     {
//       var x = int.Parse(match.Groups[1].Value);
//       var y = int.Parse(match.Groups[2].Value);
//       sum += x * y;
//     }
//   }
//
//   return sum;
// }

int Day3Problem1() {
  var sum = 0;
  const string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
  foreach (var line in File.ReadLines("input.txt"))
  {
    var matches = Regex.Matches(line, pattern);
    foreach (Match match in matches)
    {
      var x = int.Parse(match.Groups[1].Value);
      var y = int.Parse(match.Groups[2].Value);
      sum += x * y;
    }
  }

  return sum;
}


Console.WriteLine($"Day 3 Problem 1 Solution: {Day3Problem1()}");

int Day3Problem2() {
  var sum = 0;
  const string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";
  const string enablePattern = @"do\(\)";
  const string disablePattern = @"don't\(\)";
  
  var file = File.ReadAllText("input.txt");
  var matches = Regex.Matches(file, mulPattern);
  foreach (Match match in matches)
  {
    var beforeMatch = file[..match.Index];
    var lastEnable = Regex.Match(beforeMatch, enablePattern, RegexOptions.RightToLeft);
    var lastDisable = Regex.Match(beforeMatch, disablePattern, RegexOptions.RightToLeft);
    var noDisable = !lastDisable.Success;
    var enableAfterDisable = lastEnable.Success && lastEnable.Index > lastDisable.Index;
    var isEnabled = noDisable || enableAfterDisable;
    if (isEnabled) {
      var x = int.Parse(match.Groups[1].Value);
      var y = int.Parse(match.Groups[2].Value);
      sum += x * y;
    }
  }
    
  return sum;
}

// 89912299 -- too high
// 87163705
Console.WriteLine($"Day 3 Problem 2 Solution: {Day3Problem2()}");