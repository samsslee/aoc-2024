using System;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
public static class Day21p2{

    static Dictionary<(char, char), List<string>> numPadMemo = new(){};
    static Dictionary<(char, char), List<string>> dirPadMemo = new(){};
    static List<string> codes = new();

    static Dictionary<(string,int), long> dirSeqMemo = new(){}; //code, depth, result

    static Dictionary<char, (int, int)> numPad = new(){
        {'7',(0,0)},{'8',(0,1)},{'9',(0,2)},
        {'4',(1,0)},{'5',(1,1)},{'6',(1,2)},
        {'1',(2,0)},{'2',(2,1)},{'3',(2,2)},
                    {'0',(3,1)},{'A',(3,2)}
        
    };
    static Dictionary<char, (int, int)> dirPad = new(){
                    {'^',(0,1)},{'A',(0,2)},
        {'<',(1,0)},{'v',(1,1)},{'>',(1,2)},
    };


    public static void Run(){
        Console.WriteLine("Running Day 21 solution...");
        string inputPath = @"inputs/day21.txt";
        StreamReader reader = new(inputPath);

        while(!reader.EndOfStream){
            codes.Add("A" + reader.ReadLine());
        }
        reader.Close();

        Part2();
    }

    public static void Part2(){

        // // List<string> L1 = new();
        // // L1 = Layer1(codes[0],0);
        // Console.WriteLine(L1[0]);

        // Console.WriteLine(dPadLength(L1[0],25));

        long answer = 0;


        foreach(string code in codes){
            List<string> L1 = new();
            L1 = Layer1(code,0);

            long min = L1.Min(l1 => dPadLength(l1, 25));
            Console.WriteLine(code+ " : " + min);
            answer += ExtractAndParseNumber(code)*min;
        }

        Console.WriteLine(answer);

    }


    public static long dPadLength(string code, int robot) {

        if (robot == 0) {
            //Console.WriteLine(code);
            return code.Length; // Return the length of the final sequence.
        }

        if (dirSeqMemo.TryGetValue((code, robot), out long cachedResult)) {
            return cachedResult;
        }

        long minLength = 0;
        List<string> firstPossible = PossibleMovesToNextDir('A',code[0]);
        minLength+= firstPossible.Min( x=> dPadLength(x, robot-1));

        for (int i = 0; i<code.Length-1; i++){
            List<string> allPossible = PossibleMovesToNextDir(code[i], code[i+1]);
            minLength+= allPossible.Min(x => dPadLength(x, robot-1));
        }

        dirSeqMemo[(code, robot)] = minLength;
        return minLength;
    }



    static int ExtractAndParseNumber(string input)
    {
        Match match = Regex.Match(input, @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }

    public static List<string> Layer1(string code, int d) {
        HashSet<string> uniqueMoves = new();  // Use HashSet to remove duplicates

        if (d >= code.Length - 1) {
            return new List<string> { "" };  // Base case
        }

        List<string> possMoves = PossibleMovesToNextNum(code[d], code[d + 1]);
        List<string> nextMoves = Layer1(code, d + 1);

        foreach (string pm in possMoves) {
            foreach (string nm in nextMoves) {
                uniqueMoves.Add(pm + nm);  // Add unique combinations only
            }
        }

        return uniqueMoves.ToList();  // Convert HashSet back to List
    }

    private static List<string> PossibleMovesToNextNum(char from, char to) {
        List<string> moves = new();

        if (numPadMemo.TryGetValue((from, to), out var cachedMoves)) {
            return cachedMoves;
        }

        int vert = numPad[to].Item1 - numPad[from].Item1;
        int horiz = numPad[to].Item2 - numPad[from].Item2;

        if (from == to) {
            moves.Add("A");
        } 
        else if (numPad[to].Item1 == 3 && numPad[from].Item2 == 0) { // Going from left to bottom
            moves.Add(new string('>', Math.Abs(horiz)) + new string('v', Math.Abs(vert)) + "A");
        } 
        else if (numPad[from].Item1 == 3 && numPad[to].Item2 == 0) { // Going from bottom to left
            moves.Add(new string('^', Math.Abs(vert)) + new string('<', Math.Abs(horiz)) + "A");
        } 
        else { // Anywhere else has two options
            string tempvert = vert < 0 ? new string('^', Math.Abs(vert)) : new string('v', Math.Abs(vert));
            string temphoriz = horiz < 0 ? new string('<', Math.Abs(horiz)) : new string('>', Math.Abs(horiz));

            string move1 = tempvert + temphoriz + "A";
            string move2 = temphoriz + tempvert + "A";

            // Only add move2 if it's different from move1
            moves.Add(move1);
            if (move1 != move2)
            {
                moves.Add(move2);
            }
        }

        numPadMemo[(from, to)] = moves; 
        return moves;
    }

    private static List<string> PossibleMovesToNextDir(char from, char to) {
        List<string> moves = new();
        int vert = dirPad[to].Item1 - dirPad[from].Item1;
        int horiz = dirPad[to].Item2 - dirPad[from].Item2;

        if (dirPadMemo.TryGetValue((from, to), out var cachedMoves)) {
            return cachedMoves;
        } else if (from == to){
            moves.Add("A");
        }
        else if (dirPad[to].Item1 == 0 && dirPad[from].Item2 == 0){ //going from left to top
            moves.Add(new string('>', Math.Abs(horiz)) + new string('^', Math.Abs(vert)) + "A");

        } else if(dirPad[from].Item1 == 0 && dirPad[to].Item2 == 0){ //going from top to left
            moves.Add(new string('v', Math.Abs(vert)) + new string('<', Math.Abs(horiz)) + "A");
        } else {
            string tempvert = vert < 0 ? new string('^', Math.Abs(vert)) : new string('v', Math.Abs(vert));
            string temphoriz = horiz < 0 ? new string('<', Math.Abs(horiz)) : new string('>', Math.Abs(horiz));
            string move1 = tempvert + temphoriz + "A";
            string move2 = temphoriz + tempvert + "A";

            // Only add move2 if it's different from move1
            moves.Add(move1);
            if (move1 != move2)
            {
                moves.Add(move2);
            }
        }

        dirPadMemo[(from, to)] = moves;
        return moves;
    }

}

