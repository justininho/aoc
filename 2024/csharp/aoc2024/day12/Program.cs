using Matrix = char[,];
using Point = (int x, int y);

int Day12Problem1() {
  var price = 0;
  var matrix = CreateMatrix("input.txt");
  Point[] directions = [(1, 0), (0, -1), (-1, 0), (0, 1)];

  var plots = new Dictionary<char, (int area, int perimeter)>();
  for (var y = 0; y < matrix.GetLength(0); y += 1) {
    for (var x = 0; x < matrix.GetLength(1); x += 1) {
      var neighbors = 0;
      var plot = matrix[y, x];
      foreach (var dir in directions) {
        if (!TryGetNeighbor(matrix, (x, y), dir, out var neighbor)) continue;
        if (plot == neighbor) {
          neighbors += 1;
        }
      }

      if (!plots.TryGetValue(plot, out var plotValue)) {
        plots.Add(plot, (1, 4 - neighbors));
      } else {
        plotValue.area += 1;
        plotValue.perimeter += 4 - neighbors;
        plots[plot] = plotValue;
      }
    }
  }

  foreach (var (key, value) in plots) {
    Console.WriteLine($"key: {key}, value: {value}");
  }

  foreach (var (area, perimeter) in plots.Values) {
    price += area * perimeter;
  }

  return price;
}

Console.WriteLine($"Day 12 Probelm 1 Solution: {Day12Problem1()}");
return;

bool TryGetNeighbor(Matrix matrix, Point point, Point dir, out char? neighbor) {
  var newPoint = new Point(point.x + dir.x, point.y + dir.y);
  if (0 <= newPoint.x && newPoint.x < matrix.GetLength(1) && 0 <= newPoint.y && newPoint.y < matrix.GetLength(0)) {
    neighbor = matrix[newPoint.y, newPoint.x];
    return true;
  }

  neighbor = null;
  return false;
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