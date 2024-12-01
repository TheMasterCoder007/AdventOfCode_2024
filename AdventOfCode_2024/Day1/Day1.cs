
namespace AdventOfCode_2024;

public static class Day1 {
    private static IEnumerable<string> _input = File.ReadLines(@"Day1/input.txt");
    private static List<int> _leftList = new List<int>();
    private static List<int> _rightList = new List<int>();
    
    public static void HistorianHysteria() {
        UpdateLists();
        Console.WriteLine("The answer for part 1: " + CalculateListDistance());
        Console.WriteLine("The answer for part 2: " + CalculateSimilarityScore());
    }

    private static void UpdateLists() {
        foreach (var line in _input) {
            var splitLine = line.Split(' ');
            var values = splitLine
                .Where(str => int.TryParse(str, out _))
                .Select(c => int.Parse(c.ToString()))
                .ToArray();
            
            _leftList.Add(values[0]);
            _rightList.Add(values[1]);
        }
        
        _leftList.Sort();
        _rightList.Sort();
    }

    private static int CalculateListDistance() {
        var totalDistance = 0;
        for (var index = 0; index < _leftList.Count; index++) {
            if (_leftList[index] <= _rightList[index]) {
                totalDistance += _rightList[index] - _leftList[index];
            } else {
                totalDistance += _leftList[index] - _rightList[index];
            }
        }

        return totalDistance;
    }

    private static int CalculateSimilarityScore() {
        var similarityScore = 0;
        for (var index = 0; index < _leftList.Count; index++) {
            var count = _rightList.Count(val => val == _leftList[index]);
            similarityScore += count * _leftList[index];
        }
        
        return similarityScore;
    }
}