using System.Drawing;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Intrinsics.Arm;
using Map = char[,];
using Point = (int x, int y);

int Day6Problem1() {
  var posTraveled = new HashSet<Point>();
  var map = CreateMap(File.ReadAllText("input.txt"));
  var guardPos = FindGuard(map);

  while (IsInBounds(map, guardPos)) {
    // PrintMatrix(map);
    posTraveled.Add(guardPos);
    guardPos = MoveGuard(map, guardPos);
  }

  return posTraveled.Count;
}

// Console.WriteLine($"Day 6 Problem 1 Solution: {Day6Problem1()}");

void Day6Problem2() {
  var visitedPoints = new HashSet<Point>();
  var map = CreateMap(File.ReadAllText("input.txt"));
  var guardPos = FindGuard(map);

  while (IsInBounds(map, guardPos)) {
    visitedPoints.Add(guardPos);
    guardPos = MoveGuard(map, guardPos);
  }

  // var rectangles = new List<Rectangle>();
  // var corners = visitedPoints.Where(point => IsCorner(map, point)).ToArray();
  var rects = FindRectangles(map, visitedPoints);
  
  // foreach (var c1 in corners) {
  // foreach (var c2 in corners) {
  // if (c1 == c2) continue;

  // same row
  // if (c1.y == c2.y) ;

  // same column
  // if (c1.x == c2.x) ;
  // }

  // }

  PrintMatrix(map);
}

Day6Problem2();
// Console.WriteLine($"Day 6 Problem 2 Solution: {Day6Problem2()}");
return;

Point FindGuard(Map map) {
  for (var y = 0; y < map.GetLength(0); y++) {
    for (var x = 0; x < map.GetLength(1); x++) {
      if (map[y, x] is '^' or '>' or 'v' or '<')
        return (x, y);
    }
  }

  throw new Exception("Guard not found");
}

Point MoveGuard(Map map, Point guardPos) {
  var (x, y) = guardPos;
  var guard = map[y, x];
  var guardDir = GetGuardDirection(guard);
  var newPos = new Point(x + guardDir.x, y + guardDir.y);


  if (!IsColliding(map, newPos)) {
    // replace guard char with X
    map[y, x] = guard is 'v' or '^' ? '|' : '-';

    // update . char on map with guard
    if (IsInBounds(map, newPos)) map[newPos.y, newPos.x] = guard;
    return newPos;
  }

  // replace guard char with C (corner)
  map[y, x] = '+';

  // clockwise rotation
  var rotatedGuardDir = new Point(-guardDir.y, guardDir.x);
  var rotatedPos = new Point(x + rotatedGuardDir.x, y + rotatedGuardDir.y);

  // replace . on map with new guard (rotated)
  if (IsInBounds(map, rotatedPos)) map[rotatedPos.y, rotatedPos.x] = GetGuardChar(rotatedGuardDir);
  return rotatedPos;
}

bool IsInBounds(Map map, Point pos) {
  var (x, y) = pos;
  return 0 <= x && x < map.GetLength(1) && 0 <= y && y < map.GetLength(0);
}

bool IsColliding(Map map, Point pos) {
  if (IsInBounds(map, pos)) return map[pos.y, pos.x] == '#';
  return false;
}

Point[] FindVisitedCorners(Map map, HashSet<Point> visitedPoints) {
  return visitedPoints.Where(point => map[point.y, point.x] == '+').ToArray();
}

List<Point> FindCornersInRow(Map map, int y) {
  var corners = new List<Point>();
  for (var x = 0; x < map.GetLength(0); x++) {
    if (map[y, x] == '+') corners.Add((x, y));
  }

  return corners;
}

List<Point> FindCornersInColumn(Map map, int x) {
  var corners = new List<Point>();
  for (var y = 0; y < map.GetLength(1); y++) {
    if (map[y, x] == '+') corners.Add((x, y));
  }

  return corners;
}

bool IsCorner(Map map, Point point) {
  throw new NotImplementedException();
}

Point GetGuardDirection(char guard) {
  return guard switch {
    '^' => (0, -1),
    '>' => (1, 0),
    'v' => (0, 1),
    '<' => (-1, 0),
    _ => throw new Exception("Unknown guard character")
  };
}

char GetGuardChar(Point dir) {
  return dir switch {
    (0, -1) => '^',
    (1, 0) => '>',
    (0, 1) => 'v',
    (-1, 0) => '<',
    _ => throw new Exception("Unknown guard character")
  };
}

Map CreateMap(string input) {
  var lines = input.Split("\n");
  var maxY = lines.Length;
  var maxX = lines[0].Length;
  var map = new char[maxY, maxX];
  for (var y = 0; y < maxY; y++) {
    for (var x = 0; x < maxX; x++) {
      map[y, x] = lines[y][x];
    }
  }

  return map;
}

HashSet<Rectangle> FindRectangles(Map map, HashSet<Point> visitedPoints) {
  var rectangles = new HashSet<Rectangle>();
  // var visitedCorners = FindVisitedCorners(map, visitedPoints);
  var visitedCorners = new[] { new Point(4, 1), new Point(8,1), new Point(8,6)};
  foreach (var corner1 in visitedCorners) {
    var cornersInRow = FindCornersInRow(map, corner1.y);
    foreach (var corner2 in cornersInRow) {
      if (corner1 == corner2) continue;

      if (corner1.y != corner2.y || !HasHorizontalPath(map, corner1, corner2)) continue;
      
      var cornersInColumn1 = FindCornersInColumn(map, corner1.x);
      foreach (var c1 in cornersInColumn1) {
        Console.WriteLine($"c1: {c1}");
        map[c1.y, c1.x] = '1';
        // var lastCorner = (corner2.x, c1.y);
        // if (IsInBounds(map, lastCorner) && map[lastCorner.y, lastCorner.x] != '#') {
        //   rectangles.Add(new Rectangle(corner1, corner2, c1, lastCorner));
        // }
      }

      var cornersInColumn2 = FindCornersInColumn(map, corner2.x);
      foreach (var c2 in cornersInColumn2) {
        Console.WriteLine($"c1: {c2}");
        map[c2.y, c2.x] = '2';
        var lastCorner = (corner1.x, c2.y);
        
        // if (IsInBounds(map, lastCorner) && map[lastCorner.y, lastCorner.x] != '#') {
        //   rectangles.Add(new Rectangle(corner1, corner2, lastCorner, c2));
        // }
      }
    }
  }

  return rectangles;
}

bool HasHorizontalPath(Map map, Point p1, Point p2) {
  var start = Math.Min(p1.x, p2.x);
  var end = Math.Max(p1.x, p2.x);

  for (var x = start; x < end; x++) {
    if (map[p1.y, x] is not ('-' or '+')) return false;
  }

  return true;
}

bool HasVerticalPath(Map map, Point p1, Point p2) {
  // Check if all positions between start and end are '|' or '+'
  var minY = Math.Min(p1.y, p2.y);
  var maxY = Math.Max(p1.y, p2.y);

  for (var y = minY + 1; y < maxY; y++) {
    if (map[y, p1.x] is not ('|' or '+')) return false;
  }

  return true;
}

// bool HasLastCorner(Map map, Point d)

void PrintMatrix<T>(T[,] matrix) {
  int rows = matrix.GetLength(0);
  int cols = matrix.GetLength(1);

  // Find the maximum width needed for any element
  int maxWidth = 0;
  for (int i = 0; i < rows; i++) {
    for (int j = 0; j < cols; j++) {
      int width = matrix[i, j]?.ToString().Length ?? 0;
      maxWidth = Math.Max(maxWidth, width);
    }
  }

  // Print the matrix with consistent spacing
  for (int i = 0; i < rows; i++) {
    // Console.Write("[");
    for (int j = 0; j < cols; j++) {
      string value = matrix[i, j]?.ToString() ?? "null";
      Console.Write(value.PadLeft(maxWidth));

      // Add spacing between elements, except for the last element
      if (j < cols - 1)
        Console.Write(" ");
    }

    Console.WriteLine();
    // Console.WriteLine("]");
  }
}

record Rectangle(Point TopLeft, Point TopRight, Point BottomLeft, Point BottomRight);