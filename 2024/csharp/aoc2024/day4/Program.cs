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
        var targetY = y + dy * 3;
        var targetX = x + dx * 3;

        if (0 > targetY || targetY >= rows || 0 > targetX || targetX >= cols) continue;
        var word = new StringBuilder(4);
        for (var i = 0; i < 4; i++) {
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

int Day4Problem2()
{
  var matrix = CreateMatrix();
  var rows = matrix.GetLength(0);
  var cols = matrix.GetLength(1);
    
  // Pre-define the diagonal directions for checking
  var diagonalPairs = new[] {
    ((y: -1, x: -1), (y: 1, x: 1)),   // top-left to bottom-right
    ((y: 1, x: -1), (y: -1, x: 1))    // bottom-left to top-right
  };
    
  var count = 0;
  for (var y = 0; y < rows; y++)
  for (var x = 0; x < cols; x++)
  {
    if (matrix[y, x] != 'A') continue;

    // Check if both diagonals form valid words
    if (HasValidDiagonals(y, x)) count++;
  }
  return count;

  bool HasValidDiagonals(int centerY, int centerX)
  {
    foreach (var (start, end) in diagonalPairs)
    {
      var startPos = (y: centerY + start.y, x: centerX + start.x);
      var endPos = (y: centerY + end.y, x: centerX + end.x);
            
      if (!IsInBounds(startPos.y, startPos.x) || !IsInBounds(endPos.y, endPos.x))
        return false;
    }

    var word1 = GetDiagonalWord(centerY, centerX, diagonalPairs[0].Item1, diagonalPairs[0].Item2);
    var word2 = GetDiagonalWord(centerY, centerX, diagonalPairs[1].Item1, diagonalPairs[1].Item2);
        
    return IsSearchWord(word1) && IsSearchWord(word2);
  }

  string GetDiagonalWord(int centerY, int centerX, (int y, int x) start, (int y, int x) end)
  {
    var startChar = matrix[centerY + start.y, centerX + start.x];
    var centerChar = matrix[centerY, centerX];
    var endChar = matrix[centerY + end.y, centerX + end.x];
    return $"{startChar}{centerChar}{endChar}";
  }

  bool IsInBounds(int y, int x) => 
    y >= 0 && y < rows && x >= 0 && x < cols;

  bool IsSearchWord(string word) => 
    word is "MAS" or "SAM";
}
Console.WriteLine($"Day 4 Problem 2 Solution: {Day4Problem2()}");