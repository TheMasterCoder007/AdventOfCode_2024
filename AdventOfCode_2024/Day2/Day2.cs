using static System.Int32;

namespace AdventOfCode_2024.Day2;

public static class Day2 {
    private static IEnumerable<string> _input = File.ReadLines(@"Day2/input.txt");

    public static void RedNosedReports() {
        var numberOfSafeReports = 0;
        var numberOfSafeReportsDampened = 0;
        foreach (var line in _input) {
            var report = line
                .Split(' ')
                .Where(str => TryParse(str, out _))
                .Select(int.Parse)
                .ToList();

            if (CheckIfReportSafe(report)) numberOfSafeReports++;
            if (RunProblemDampener(report)) numberOfSafeReportsDampened++;
        }
        
        Console.WriteLine("The answer for part 1: " + numberOfSafeReports);
        Console.WriteLine("The answer for part 2: " + numberOfSafeReportsDampened);
    }

    private static bool CheckIfReportSafe(List<int> report) {
        if (report.Count < 2) return true;
        
        var isAscending = report[0] < report[1];
        var prevVal = report[0];
        var currentVal = report[1];
        for (var index = 1; index < report.Count; index++) {
            currentVal = report[index];
            if (
                prevVal == currentVal ||
                isAscending && prevVal > currentVal ||
                !isAscending && prevVal < currentVal ||
                (isAscending && currentVal - prevVal > 3) ||
                (!isAscending && prevVal - currentVal > 3)
            ) {
                return false;
            }

            prevVal = currentVal;
        }

        return true;
    }
    
    private static bool RunProblemDampener(List<int> report) {
        if (CheckIfReportSafe(report)) return true;
        
        for (var index = 0; index < report.Count; index++) {
            var updatedReport = new List<int>();
            updatedReport.AddRange(report);
            updatedReport.RemoveAt(index);
            if (CheckIfReportSafe(updatedReport)) return true;
        }

        return false;
    }
}