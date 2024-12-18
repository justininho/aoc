using System.Collections;
using Matrix = char[,];
using Point = (int x, int y);

int Day12Problem1() {
  var price = 0;
  var matrix = CreateMatrix("input.txt");
  Point[] directions = [(1, 0), (0, -1), (-1, 0), (0, 1)];
  if (matrix.Length == 0) return price;
  var visited = new HashSet<Point>();
  
  for (var y = 0; y < matrix.GetLength(0); y += 1) {
    for (var x = 0; x < matrix.GetLength(1); x += 1) {
      if (visited.Contains((x, y))) continue;
      var queue = new Queue<Point>();
      queue.Enqueue((x, y));
      var perimeter = 0;
      var plot = new HashSet<Point>();
      while (queue.Count > 0) {
        var neighbors = 0;
        var point = queue.Dequeue();
        if (!visited.Add(point)) continue;
        plot.Add(point);
        foreach (var dir in directions) {
          if (!TryGetNeighbor(matrix, point, dir, out var c, out var n) || matrix[point.y, point.x] != c) continue;
          neighbors += 1;
          if (!visited.Contains(n)) queue.Enqueue(n);
        }
        perimeter += 4 - neighbors;
      }

      price += plot.Count * perimeter;
    }
  }

  return price;
}

Console.WriteLine($"Day 12 Problem 1 Solution: {Day12Problem1()}");
return;

bool TryGetNeighbor(Matrix matrix, Point point, Point dir, out char? neighbor, out Point newPoint) {
  newPoint = new Point(point.x + dir.x, point.y + dir.y);
  if (IsInBounds(matrix, newPoint)) {
    neighbor = matrix[newPoint.y, newPoint.x];
    return true;
  }

  neighbor = null;
  return false;
}

bool IsInBounds(Matrix matrix, Point point) {
  return 0 <= point.x && point.x < matrix.GetLength(1) && 0 <= point.y && point.y < matrix.GetLength(0);
}

Matrix CreateMatrix(string inputPath) {
  var file = File.ReadAllText(inputPath);
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