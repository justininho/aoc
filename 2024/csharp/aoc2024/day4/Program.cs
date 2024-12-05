int Day4Problem1() {
  var file = File.ReadAllText("input.txt");
  var matrix = file.Split("\n").ToArray();
  var rotations = new[] {
    (1, -1), (1, 1), (-1, 1), (-1, -1)
  };
  
  var foundXmas = new HashSet<(int startX, int startY, int endX, int endY)>();
  for (var y = 0; y < matrix.Length; y++) {
    for (var x = 0; x < matrix[y].Length; x++) {
      if (matrix[y][x] == 'X') {
        foreach (var (rx, ry) in rotations) {
          var maxX = matrix[y].Length;
          var maxY = matrix.Length;
          var distanceX = x + rx * 3;
          var distanceY = y + ry * 3;
          var validX = 0 <= distanceX && distanceX < maxX;
          var validY = 0 <= distanceY && distanceY < maxY;
          
          var pathX = (x, y, distanceX, y);
          if (validX && !foundXmas.Contains(pathX)) {
            var startX = Math.Min(x, distanceX);
            var endX = Math.Max(x, distanceX) + 1;
            var horizontalSlice = matrix[y][startX..endX];
            if (horizontalSlice is "XMAS" or "SAMX") foundXmas.Add(pathX);
          }
          
          var pathY = (x, y, x, distanceY);
          if (validY && !foundXmas.Contains(pathY)) {
            var verticalSlice = "";
            var startY = Math.Min(y, distanceY);
            var endY = Math.Max(y, distanceY) + 1;
            for (var i = startY; i < endY; i++) {
              verticalSlice += matrix[i][x];
            }
            if (verticalSlice is "XMAS" or "SAMX") foundXmas.Add(pathY);
          }
          
          var pathDiagonal = (x, y, distanceX, distanceY);
          if (validX && validY && !foundXmas.Contains(pathDiagonal)) {
            var diagonalSlice = "";
            for (var i = 0; i < 4; i++) {
              var currentX = x + (rx * i);
              var currentY = y + (ry * i);
              diagonalSlice += matrix[currentY][currentX];
            }
            if (diagonalSlice is "XMAS" or "SAMX") foundXmas.Add(pathDiagonal);
          }
        }
      }
    }
  }

  return foundXmas.Count;
}

Console.WriteLine($"Day 4 Problem 1 Solution: {Day4Problem1()}");