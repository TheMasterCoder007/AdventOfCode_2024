namespace AdventOfCode_2024.Day4;

public static class Day4 {
    private static IEnumerable<string> _input = File.ReadLines(@"Day4/input.txt");
    private static List<List<char>> _grid = new();
    private static int _count = 0;
    private static int _count2 = 0;

    public static void CeresSearch() {
        UpdateGrid();
        GetMatchCount();
        FindMasCrosses();
        Console.WriteLine("The answer for part 1: " + _count);
        Console.WriteLine("The answer for part 2: " + _count2);
    }

    private static void UpdateGrid() {
        foreach (var line in _input) {
            _grid.Add(line.ToList());
        }
    }

    // PART 1 - BRUTE FORCE 
    private static void GetMatchCount() {
        // parse through each horizontal row
        foreach (var str in _grid.Select(row => string.Join("", row))) {
            CheckSection(str);
            var revStr = string.Join("", str.Reverse());
            CheckSection(revStr);
        }
        
        // parse through each vertical column
        for (var col = 0; col < _grid[0].Count; col++) {
            var column = _grid.Select(row => row[col]).ToList();
            var str = string.Join("", column);
            CheckSection(str);
            var revStr = string.Join("", str.Reverse());
            CheckSection(revStr);
        }
        
        // parse through each left diagonal column
        // first half
        for (var diagRow = 3; diagRow < _grid.Count; diagRow++) {
            var leftDiagonal = new List<char>();
            var col = _grid[0].Count - 1;
            for (var row = diagRow; row >= 0; row--) {
                if (col >= 0) {
                    leftDiagonal.Add(_grid[row][col--]);
                }
            }

            var str = string.Join("", leftDiagonal);
            CheckSection(str);
            var revStr = string.Join("", str.Reverse());
            CheckSection(revStr);
        }
        // second half
        for (var diagCol = _grid[0].Count - 2; diagCol >= 3; diagCol--) {
            var leftDiagonal = new List<char>();
            var row = _grid.Count - 1;
            for (var col = diagCol; col >= 0; col--) {
                if (row >= 0) {
                    leftDiagonal.Add(_grid[row--][col]);
                }
            }
            
            var str = string.Join("", leftDiagonal);
            CheckSection(str);
            var revStr = string.Join("", str.Reverse());
            CheckSection(revStr);
        }
        
        // parse through each right diagonal column
        // first half
        for (var diagRow = 3; diagRow < _grid.Count; diagRow++) {
            var rightDiagonal = new List<char>();
            var col = 0;
            for (var row = diagRow; row >= 0; row--) {
                if (col < _grid[0].Count) {
                    rightDiagonal.Add(_grid[row][col++]);
                }
            }
            
            var str = string.Join("", rightDiagonal);
            CheckSection(str);
            var revStr = string.Join("", str.Reverse());
            CheckSection(revStr);
        }
        // second half
        for (var diagCol = 1; diagCol < _grid[0].Count - 3; diagCol++) {
            var rightDiagonal = new List<char>();
            var row = _grid.Count - 1;
            for (var col = diagCol; col < _grid[0].Count; col++) {
                if (row >= 0) {
                    rightDiagonal.Add(_grid[row--][col]);
                }
            }
            
            var str = string.Join("", rightDiagonal);
            CheckSection(str);
            var revStr = string.Join("", str.Reverse());
            CheckSection(revStr);
        }
    }
    
    private static void CheckSection(string section) {
        var index = 0;

        while (index != -1) {
            index = section.IndexOf("XMAS", 0);
            if (index == -1) continue;
            
            section = section[(index + 4)..];
            _count++;
        }
    }
    
    // PART 2 - FORCE ISN'T ALWAYS BETTER
    private static void FindMasCrosses() {
        for (var row = 1; row < _grid.Count - 1; row++) {
            for (var col = 1; col < _grid[0].Count - 1; col++) {
                if (_grid[row][col] != 'A') continue;
                
                var word1 = "";
                var word2 = "";

                word1 += _grid[row - 1][col - 1];
                word1 += _grid[row + 1][col + 1];
                word2 += _grid[row - 1][col + 1];
                word2 += _grid[row + 1][col - 1];

                if (word1.Contains('M') && word1.Contains('S') && word2.Contains('M') && word2.Contains('S')) {
                    _count2++;
                }
            }
        }
    }
}