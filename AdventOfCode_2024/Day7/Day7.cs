namespace AdventOfCode_2024.Day7; 

public static class Day7 {
    private static List<long> _calibrationResults = new();
    
    /// <summary>
    /// This is the main method for this class.
    /// This method can be called from other classes to generate the answers to day 7
    /// </summary>
    public static void BridgeRepair() {
        var equations = GetEquations();
        
        // PART 1 **********************************************************************************
        // check each equation for validity
        foreach (KeyValuePair<long, int[]> entry in equations) {
            GetCalibrationResult(entry.Key, entry.Value, 1, entry.Value[0], false);
        }
        
        Console.WriteLine("The answer for part 1: " + _calibrationResults.Sum());
        
        // PART 2 **********************************************************************************
        // check each equation for validity with updated parameters
        _calibrationResults.Clear();
        foreach (KeyValuePair<long, int[]> entry in equations) {
            GetCalibrationResult(entry.Key, entry.Value, 1, entry.Value[0], true);   
        }
        
        Console.WriteLine("The answer for part 2: " + _calibrationResults.Sum());
    }
    
    /// <summary>
    /// Parse input to a hashmap
    /// </summary>
    /// <returns>Returns hashmap containing the test value as well as all of the equation values</returns>
    private static Dictionary<long, int[]> GetEquations() {
        return File.ReadLines(@"Day7/input.txt")
            .Select(line => line.Split(':'))
            .Select(equation => {
                var key = long.Parse(equation[0]);
                var rawValues = equation[1].Split(' ');
                var values = rawValues
                    .Where(value => int.TryParse(value, out _))
                    .Select(int.Parse)
                    .ToArray();
                
                return new { Key = key, Value = values };
            })
            .ToDictionary(item => item.Key, item => item.Value);
    }

    /// <summary>
    /// Recursively checks every possible combination using the missing operators. The recursive checks
    /// ensure that every possible tree is explored. If at the end of the equation, one of the trees match
    /// the test value, then the value is added to the calibration results list.
    /// </summary>
    /// <param name="testValue">The test value is used to see if any of the combinations can provide the right result</param>
    /// <param name="numbers">Array of equation values in order</param>
    /// <param name="index">current index in array</param>
    /// <param name="currentValue">The total calculated value based on the current progress through that path</param>
    /// <param name="checkConcat">If true, the concat operator will be used. This is used for part 2 only</param>
    private static void GetCalibrationResult(
        long testValue, int[] numbers, int index, long currentValue, bool checkConcat) {
        if (index == numbers.Length) {
            if (currentValue == testValue && !_calibrationResults.Contains(testValue)) {
                _calibrationResults.Add(testValue);
            }
            return;
        }
        
        // check validity of addition operator
        GetCalibrationResult(testValue, numbers, index + 1, (currentValue + numbers[index]), checkConcat);
        
        // check validity of multiplication operator
        GetCalibrationResult(testValue, numbers, index + 1, (currentValue * numbers[index]), checkConcat);
        
        // check validity of concat operator
        if (checkConcat) {
            var concatValue = currentValue + numbers[index].ToString();
            GetCalibrationResult(testValue, numbers, index + 1, long.Parse(concatValue), true);
        }
    }
}