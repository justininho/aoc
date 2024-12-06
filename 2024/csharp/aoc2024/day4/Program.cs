using System.Text;

char[,] CreateMatrix() {
  var file = File.ReadAllText("input.txt");

  var rows = file.Split("\n");
  var matrix = new char[rows.Length, rows[0].Length];

  for (var y = 0; y < rows.Length; y++) {
    var cols = rows[y];
    for (var x = 0; x < cols.Length; x++) {
      matrix[y, x] = cols[x];
    }
  }

  return matrix;
}

int Day4Problem1() {
  var matrix = CreateMatrix();
  var count = 0;
  const string searchWord = "XMAS";
  var rows = matrix.GetLength(0);
  var cols = matrix.GetLength(1);
  var directions = new[] {
    (0, 1), (-1, 0), (0, -1), (1, 0),
    (1, -1), (1, 1), (-1, 1), (-1, -1)
  };

  for (var y = 0; y < rows; y++) {
    for (var x = 0; x < cols; x++) {
      if (matrix[y, x] != 'X') continue;

      foreach (var (dx, dy) in directions) {
        var targetY = y + dy * (searchWord.Length - 1);
        var targetX = x + dx * (searchWord.Length - 1);

        if (0 > targetY || targetY >= rows || 0 > targetX || targetX >= cols) continue;
        var word = new StringBuilder(searchWord.Length);
        for (var i = 0; i < searchWord.Length; i++) {
          var currentY = y + dy * i;
          var currentX = x + dx * i;
          word.Append(matrix[currentY, currentX]);
        }

        if (word.ToString() is "XMAS" or "SAMX") count++;
      }
    }
  }

  return count;
}

Console.WriteLine($"Day 4 Problem 1 Solution: {Day4Problem1()}");


int Day4Problem2() {
  var matrix = CreateMatrix();
  var count = 0;
  const string searchWord = "MAS";
  var rows = matrix.GetLength(0);
  var cols = matrix.GetLength(1);
  
  for (var y = 0; y < rows; y++) {
    for (var x = 0; x < cols; x++) {
      if (matrix[y, x] != 'A') continue;
      var topLeft = (y + -1, x + -1);
      var bottomRight = (y + 1, x + 1);

      var bottomLeft = (y + 1, x - 1);
      var topRight = (y - 1, x + 1);
      
      if (!IsInBounds(topLeft) ||
          !IsInBounds(topRight) ||
          !IsInBounds(bottomLeft) ||
          !IsInBounds(bottomRight)
         ) continue;
      
      var word1 = $"{matrix[topLeft.Item1, topLeft.Item2]}{matrix[y, x]}{matrix[bottomRight.Item1, bottomRight.Item2]}";
      var word2 = $"{matrix[bottomLeft.Item1, bottomLeft.Item2]}{matrix[y, x]}{matrix[topRight.Item1, topRight.Item2]}";
      Console.WriteLine($"Word 1: {word1}, Word2: {word2}");

      if (IsSearchWord(word1) && IsSearchWord(word2)) count++;
    }
  }

  return count;

  bool IsInBounds((int, int) coordinate) => coordinate.Item1 >= 0 && coordinate.Item1 < rows && coordinate.Item2 >= 0 &&
                                            coordinate.Item2 < cols;
  
  bool IsSearchWord(string word) => word is "MAS" or "SAM";
} 

Console.WriteLine($"Day 4 Problem 2 Solution: {Day4Problem2()}");