using System.ComponentModel;

namespace AdventOfCode_2024.Day6; 

public static class Day6 {
    private static Dictionary<char, (int newY, int newX)> _updatedCoordinates = new() {
        { '^', (-1, 0) },
        { '>', (0, 1) },
        { 'v', (1, 0) },
        { '<', (0, -1) }
    };
    
    /// <summary>
    /// This is the main method for this class.
    /// This method can be called from other classes to generate the answers to day 6
    /// </summary>
    public static void GuardGallivant() {

        Console.WriteLine("The answer for part 1: " + GetNumberOfUniquePositions());
        Console.WriteLine("The answer for part 2: " + GetNumberOfValidObstructionPoints());
    }
    
    /// <summary>
    /// Parses input data into a hashmap
    /// </summary>
    /// <returns>Returns hashmap containing every position in the grid and its value</returns>
    private static Dictionary<(int y, int x), char> GetMap() {
        return File.ReadLines(@"Day6/input.txt")
            .Select((row, y) => row.Select((value, x) => new { Key = (y, x), Value = value }))
            .SelectMany(item => item)
            .ToDictionary(item => item.Key, item => item.Value);
    }

    /// <summary>
    /// Gets the starting position for the guard
    /// </summary>
    /// <returns>Returns tuple containing the starting x and y coordinates</returns>
    private static (int y, int x) GetGuardPosition() {
        var map = GetMap();
        return map.First(pair => pair.Value is '^' or '>' or 'v' or '<').Key; 
    }

    /// <summary>
    /// Takes current direction and rotates it 90 degrees
    /// </summary>
    /// <param name="direction">Current direction the guard is heading</param>
    /// <returns>Returns updated direction</returns>
    private static char ChangeDir(char direction) {
        return direction switch {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            _ => '^'
        };
    }

    /// <summary>
    /// Used to count number of unique steps taken by the guard 
    /// </summary>
    /// <returns>Returns the total number of unique steps the guard takes</returns>
    private static int GetNumberOfUniquePositions() {
        var numberOfUniquePositions = 1;
        var map = GetMap();
        var (y, x) = GetGuardPosition();
        var direction = map[(y, x)];

        while (map.ContainsKey((y, x))) {
            if (map[(y, x)] is '.') numberOfUniquePositions++;

            (y, x, direction, map) = SimulateMove(y, x, direction, map);
        }
        
        return numberOfUniquePositions;
    }

    /// <summary>
    /// Used to find the total number of obstructions that could be placed which cause
    /// the guard to get stuck in a loop
    /// </summary>
    /// <returns>Returns total number of obstructions that cause the guard to get stuck in a loop</returns>
    private static int GetNumberOfValidObstructionPoints() {
        var numberOfValidObstructionPoints = 0;
        
        // check all possible obstruction points
        var map = GetMap();
        var (y, x) = GetGuardPosition();
        var direction = map[(y, x)];
        while (map.ContainsKey((y, x))) {
            // only check obstruction point if it has not already been checked
            if (map[(y, x)] is '.') {
                var tempMap = GetMap();
                var (yTemp, xTemp) = GetGuardPosition();
                var dirTemp = tempMap[(yTemp, xTemp)];
                tempMap[(y, x)] = '#';
                while (tempMap.ContainsKey((yTemp, xTemp))) {
                    (yTemp, xTemp, dirTemp, tempMap) = SimulateMove(yTemp, xTemp, dirTemp, tempMap);

                    if (yTemp != -1 || xTemp != -1) continue;
                    numberOfValidObstructionPoints++;
                    break;
                }
            }
            
            (y, x, direction, map) = SimulateMove(y, x, direction, map);
        }

        return numberOfValidObstructionPoints;
    }

    /// <summary>
    /// Simulates the guards next move and updates coordinates, map, and direction
    /// </summary>
    /// <param name="y">y axis</param>
    /// <param name="x">x axis</param>
    /// <param name="dir">current direction</param>
    /// <param name="map">map of lab</param>
    /// <returns>Returns updated coordinates, map, and direction</returns>
    private static (int y, int x, char dir, Dictionary<(int y, int x), char> map) 
    SimulateMove(int y, int x, char dir, Dictionary<(int y, int x), char> map) {
        // get next positions to check
        var (newY, newX) = _updatedCoordinates[dir];
        newY += y;
        newX += x;

        // if the map does not contain the key, update coordinates to leave mapped area
        if (!map.ContainsKey((newY, newX)))
        {
            y = newY;
            x = newX;
        }
        // when an obstruction is found guard turns 90 degrees clockwise
        else if (map[(newY, newX)] == '#')
        {
            while (map[(newY, newX)] == '#') {
                dir = ChangeDir(dir);

                // check if the guard is stuck in a loop
                if (map[(y, x)] == dir) return (-1, -1, dir, map);

                // update the map with the new direction and coordinates
                (newY, newX) = _updatedCoordinates[dir];
                newY += y;
                newX += x;
            }
            
            map[(y, x)] = dir;
            y = newY;
            x = newX;
        }
        else
        {
            // Move and update the map with the current direction
            map[(y, x)] = dir;
            y = newY;
            x = newX;
        }

        return (y, x, dir, map);
    }
}