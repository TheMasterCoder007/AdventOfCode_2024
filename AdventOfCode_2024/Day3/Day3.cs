using System.Runtime.InteropServices;

namespace AdventOfCode_2024.Day3;

public static class Day3 {
    private static string _input = File.ReadAllText(@"Day3/input.txt");
    private static MulInstructionStage _mulInstructionStage = MulInstructionStage.M;
    private static DoInstruction _doInstruction = DoInstruction.D;
    private static DontInstruction _dontInstruction = DontInstruction.D;
    private static bool mulInstructionsDisabled = false;
    private static string _number1 = "";
    private static string _number2 = "";
    private static List<int> _validMulInstructionResults = new();
    private static List<int> _validMulInstructionResults2 = new();

    private enum DoInstruction {
        D,
        O,
        LeftRoundBracket,
        RightRoundBracket,
        DoInstructionComplete
    }
    private enum DontInstruction {
        D,
        O,
        N,
        Apostrophe,
        T,
        LeftRoundBracket,
        RightRoundBracket,
        DontInstructionComplete
    } 
    private enum MulInstructionStage {
        M,
        U,
        L,
        LeftRoundBracket,
        Digit1,
        Digit2, 
        Digit3,
        Comma,
        Digit4,
        Digit5,
        Digit6,
        RightRoundBracket,
        CompleteMulInstruction
    }
    
    public static void MullItOver() {
        RunValidMulInstructions();
        Console.WriteLine("The answer for part 1: " + _validMulInstructionResults.Sum());
        Console.WriteLine("The answer for part 2: " + _validMulInstructionResults2.Sum());
    }

    private static void RunValidMulInstructions() {
        foreach (var c in _input) {
            // do instructions
            if (_doInstruction == DoInstruction.DoInstructionComplete) {
                mulInstructionsDisabled = false;
                _doInstruction = DoInstruction.D;
            }
            
            if (!CheckDoInstruction(c)) _doInstruction = DoInstruction.D;
            
            // dont instructions
            if (_dontInstruction == DontInstruction.DontInstructionComplete) {
                mulInstructionsDisabled = true;
                _dontInstruction = DontInstruction.D;
            }
            
            if (!CheckDontInstruction(c)) _dontInstruction = DontInstruction.D;
            
            // mul instructions
            if (_mulInstructionStage == MulInstructionStage.CompleteMulInstruction) {
                var result = int.Parse(_number1) * int.Parse(_number2);
                _validMulInstructionResults.Add(result);
                _mulInstructionStage = MulInstructionStage.M;
                _number1 = "";
                _number2 = "";

                if (!mulInstructionsDisabled) {
                    _validMulInstructionResults2.Add(result);
                }
            } 
            
            if (ValidInstructionStage(c)) continue;

            _mulInstructionStage = MulInstructionStage.M;
            _number1 = "";
            _number2 = "";
        }
    }

    private static bool ValidInstructionStage(char c) {
        switch (_mulInstructionStage) {
            case MulInstructionStage.M when c == 'm':
                _mulInstructionStage = MulInstructionStage.U;
                return true;
            case MulInstructionStage.U when c == 'u':
                _mulInstructionStage = MulInstructionStage.L;
                return true;
            case MulInstructionStage.L when c == 'l':
                _mulInstructionStage = MulInstructionStage.LeftRoundBracket;
                return true;
            case MulInstructionStage.LeftRoundBracket when c == '(':
                _mulInstructionStage = MulInstructionStage.Digit1;
                return true;
            case MulInstructionStage.Digit1 when char.IsDigit(c):
                _number1 += c;
                _mulInstructionStage = MulInstructionStage.Digit2;
                return true;
            case MulInstructionStage.Digit2 when char.IsDigit(c):
                _number1 += c;
                _mulInstructionStage = MulInstructionStage.Digit3;
                return true;
            case MulInstructionStage.Digit3 when char.IsDigit(c):
                _number1 += c;
                _mulInstructionStage = MulInstructionStage.Comma;
                return true;
            case MulInstructionStage.Digit2 or MulInstructionStage.Digit3 or MulInstructionStage.Comma when c == ',':
                _mulInstructionStage = MulInstructionStage.Digit4;
                return true;
            case MulInstructionStage.Digit4 when char.IsDigit(c):
                _number2 += c;
                _mulInstructionStage = MulInstructionStage.Digit5;
                return true;
            case MulInstructionStage.Digit5 when char.IsDigit(c):
                _number2 += c;
                _mulInstructionStage = MulInstructionStage.Digit6;
                return true;
            case MulInstructionStage.Digit6 when char.IsDigit(c):
                _number2 += c;
                _mulInstructionStage = MulInstructionStage.RightRoundBracket;
                return true;
            case MulInstructionStage.Digit5 or MulInstructionStage.Digit6 or MulInstructionStage.RightRoundBracket when c == ')':
                _mulInstructionStage = MulInstructionStage.CompleteMulInstruction;
                return true;
            default:
                return false;
        }
    }
    
    private static bool CheckDoInstruction(char c) {
        switch (_doInstruction) {
            case DoInstruction.D when c == 'd':
                _doInstruction = DoInstruction.O;
                return true;
            case DoInstruction.O when c == 'o':
                _doInstruction = DoInstruction.LeftRoundBracket;
                return true;
            case DoInstruction.LeftRoundBracket when c == '(':
                _doInstruction = DoInstruction.RightRoundBracket;
                return true;
            case DoInstruction.RightRoundBracket when c == ')':
                _doInstruction = DoInstruction.DoInstructionComplete;
                return true;
            default:
                return false;
        }
    }
    
    private static bool CheckDontInstruction(char c) {
        switch (_dontInstruction) {
            case DontInstruction.D when c == 'd':
                _dontInstruction = DontInstruction.O;
                return true;
            case DontInstruction.O when c == 'o':
                _dontInstruction = DontInstruction.N;
                return true;
            case DontInstruction.N when c == 'n':
                _dontInstruction = DontInstruction.Apostrophe;
                return true;
            case DontInstruction.Apostrophe when c == '\'':
                _dontInstruction = DontInstruction.T;
                return true;
            case DontInstruction.T when c == 't':
                _dontInstruction = DontInstruction.LeftRoundBracket;
                return true;
            case DontInstruction.LeftRoundBracket when c == '(':
                _dontInstruction = DontInstruction.RightRoundBracket;
                return true;
            case DontInstruction.RightRoundBracket when c == ')':
                _dontInstruction = DontInstruction.DontInstructionComplete;
                return true;
            default:
                return false;
        }
    }
}