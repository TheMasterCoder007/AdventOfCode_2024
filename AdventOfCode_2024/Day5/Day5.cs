using System.Collections;

namespace AdventOfCode_2024.Day5;

public static class Day5 {
    /// <summary>
    /// This is the main method for this class. This method can be called from other classes to generate
    /// the answers to day 5
    /// </summary>
    public static void PrintQueue() {
    var orderingRules = File
        .ReadAllText(@"Day5/input.txt")
        .Split('\n')
        .Where(str => str.Contains('|'))
        .Select(str => str)
        .ToList();
    var updatedPages = File
        .ReadAllText(@"Day5/input.txt")
        .Split('\n')
        .Where(str => str.Contains(','))
        .Select(str => str)
        .ToList();
        
        var printingInstructions = GetPrintInstructions(orderingRules);
        var updatedPagesPrintOrder = GetUpdatedPagesPrintOrder(updatedPages);
        var correctlyOrderedUpdatesValue =
            CalculateCorrectlyOrderedUpdates(printingInstructions, updatedPagesPrintOrder);
        var incorrectlyOrderedUpdatesValue =
            CalculateIncorrectlyOrderedUpdates(printingInstructions, updatedPagesPrintOrder);
        
        
        Console.WriteLine("The answer for part 1: " + correctlyOrderedUpdatesValue);
        Console.WriteLine("The answer for part 2: " + incorrectlyOrderedUpdatesValue);
    }

    /// <summary>
    /// Parses the print instructions/rules from the input
    /// </summary>
    /// <param name="rules">List of strings pulled in from input text file</param>
    /// <returns>Returns all print instruction sets</returns>
    private static Dictionary<int, List<int>> GetPrintInstructions(List<string> rules) {
        Dictionary<int, List<int>> printRules = new();
        foreach (var str in rules) {
            var instructions = str.Split('|');
            var key = int.Parse(instructions[0]);
            var value = int.Parse(instructions[1]);
            if (printRules.ContainsKey(key)) {
                printRules[key] = printRules[key].Append(value).ToList();
            } else {
                printRules[key] = new List<int> { value };
            }
        }
        
        return printRules;
    }

    /// <summary>
    /// Parses the print order sections from the input
    /// </summary>
    /// <param name="printOrder">List of strings pulled in from input text file</param>
    /// <returns>Returns all print order sections</returns>
    private static List<List<int>> GetUpdatedPagesPrintOrder(List<string> printOrder) {
        return printOrder
            .Select(section => section.Split(','))
            .Select(pageNumbers => pageNumbers.Select(int.Parse).ToList())
            .ToList();
    }

    /// <summary>
    /// Gets the sum of the middle numbers of every correctly ordered update section.
    /// </summary>
    /// <param name="rules">Hashmap containing all page order rules</param>
    /// <param name="printOrder">List that contains all print order sections</param>
    /// <returns>Returns the calculated middle value of the correct ordered updates</returns>
    private static int CalculateCorrectlyOrderedUpdates(Dictionary<int, List<int>> rules, List<List<int>> printOrder) {
        var calculatedValue = 0;

        foreach (var list in printOrder) {
            // checks the current print order list and compares it with the rules to determine if it is good
            var printOrderBad = false;
            for (var index = 0; index < list.Count; index++) {
                if (!rules.ContainsKey(list[index])) continue;
                
                var ruleSet = rules[list[index]];
                for (var checkIndex = 0; checkIndex < index; checkIndex++) {
                    if (!ruleSet.Contains(list[checkIndex])) continue;
                    printOrderBad = true;
                    break;
                }

                if (printOrderBad) break;
            }
            
            // only add middle value of print order if the list was properly ordered
            if (printOrderBad) continue;
            calculatedValue += GetMiddleValue(list);
        }
        
        return calculatedValue;
    }
    
    /// <summary>
    /// Gets the sum of the middle numbers of every incorrectly ordered update section.
    /// The order is fixed before calculating the sum.
    /// </summary>
    /// <param name="rules">Hashmap containing all page order rules</param>
    /// <param name="printOrder">List that contains all print order sections</param>
    /// <returns>Returns the calculated middle value of the incorrectly ordered updates</returns>
    private static int CalculateIncorrectlyOrderedUpdates(Dictionary<int, List<int>> rules, List<List<int>> printOrder) {
        var calculatedValue = 0;

        foreach (var list in printOrder) {
            // create a sorted version of the list based on the rules
            var sortedList = list.OrderBy(x => x, new RuleComparer(rules)).ToList();

            // check if the original list matches the sorted list
            // if it differs, add middle value
            if (!list.SequenceEqual(sortedList)) {
                calculatedValue += GetMiddleValue(sortedList);
            }
        }

        return calculatedValue;
    }
    
    /// <summary>
    /// Class used to compare values to printing rules. Used to sort the improperly ordered lists
    /// </summary>
    private class RuleComparer : IComparer<int> {
        private readonly Dictionary<int, List<int>> _rules;

        // constructor creates private copy of rules for use within class
        public RuleComparer(Dictionary<int, List<int>> rules) {
            _rules = rules;
        }

        // checks values based on rules
        public int Compare(int x, int y) {
            // y must come before x
            if (_rules.ContainsKey(y) && _rules[y].Contains(x)) return 1;
            // x must come before y
            if (_rules.ContainsKey(x) && _rules[x].Contains(y)) return -1;
            
            // order remains the same
            return 0;
        }
    }

    /// <summary>
    /// Gets the middle value from the print order items
    /// </summary>
    /// <param name="list">A list of print order items</param>
    /// <returns>Returns the middle value from the print order items</returns>
    private static int GetMiddleValue(List<int> list) {
        return list[list.Count / 2];
    }
}