using System;
using System.Text.RegularExpressions;
public static class Day21{

    static Dictionary<(char, char), List<string>> numPadMemo = new(){};
    static Dictionary<(char, char), List<string>> dirPadMemo = new(){};

    static List<string> codes = new();

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

        Part1();
    }

    public static void Part1(){

        List<string> L1 = new();
        int answer = 0;

        
        foreach(string c in codes){
            int minLength = int.MaxValue;
            int maxLength = int.MinValue;
            List<string> possibleL1 = Layer1(c,0);
            //Console.WriteLine(string.Join(", ", possibleL1));
            L1 = FindShortestStrings(possibleL1); //prune
            List<string> L2 = new();
            foreach(string l1 in L1){
                List<string> possibleL2 = Layer2and3("A"+l1,0);
                //Console.WriteLine(string.Join(", ", possibleL2));
                L2 = FindShortestStrings(possibleL2); //prune
                foreach(string l2 in L2){
                    List<string> possibleL3 = Layer2and3("A"+l2,0);
                    //Console.WriteLine("NEW: " + string.Join(", ", possibleL3));
                    //Console.WriteLine("before prune" + possibleL3.Count);
                    Console.WriteLine("after prune" + FindShortestStrings(possibleL3).Count); //prune

                    
                    minLength = Math.Min(minLength, possibleL3.Min(s => s.Length));
                    maxLength = Math.Max(maxLength, possibleL3.Max(s => s.Length));
                }
            }
            Console.WriteLine(c + ":" + minLength);
            Console.WriteLine(c + ":" + maxLength);
            answer += ExtractAndParseNumber(c)*minLength;
        }

        Console.WriteLine("Part 1: " + answer);
    }

    static int ExtractAndParseNumber(string input)
    {
        Match match = Regex.Match(input, @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }

    public static List<string> Layer2and3(string code, int d){
        HashSet<string> uniqueMoves = new();  // Use HashSet to remove duplicates

        if (d >= code.Length - 1) {
            return new List<string> { "" };  // Base case
        }

        List<string> possMoves = PossibleMovesToNextDir(code[d], code[d + 1]);
        List<string> nextMoves = Layer2and3(code, d + 1);

        foreach (string pm in possMoves) {
            foreach (string nm in nextMoves) {
                uniqueMoves.Add(pm + nm);  // Add unique combinations only
            }
        }

        return uniqueMoves.ToList();  // Convert HashSet back to List

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

    private static List<string> FindShortestStrings(List<string> possibleL3) {
        Console.WriteLine("here " + possibleL3.Count);
        int minLength = possibleL3.Min(s => s.Length);  // Find the shortest length
        Console.WriteLine("min " + minLength);
        List<string> tobeRet = possibleL3.Where(s => s.Length == minLength).ToList();
        Console.WriteLine("toberet " + tobeRet.Count);
        return tobeRet; // Get all shortest strings
    }

    // private static List<string> LeastChanges(List<string> possMoves) {
    //     int minChanges = int.MaxValue;
    //     List<string> minChangesList = new();

    //     for (int i = 0; i < possMoves.Count; i++) {
    //         int changes = Changes(possMoves[i]);

    //         if (changes < minChanges) {
    //             minChanges = changes;  // 🔥 Update the minChanges value
    //             minChangesList = new List<string> { possMoves[i] }; // Overwrite with new list
    //         } else if (changes == minChanges) {
    //             minChangesList.Add(possMoves[i]); // Add to existing min list
    //         }
    //     }

    //     return minChangesList;
    // }

    private static int Changes(string moves){
        int changes = 0;

        for(int i = 1; i<moves.Length; i++){
            if (moves[i-1] != moves[i]){
                changes++;
            }
        }
        return changes;
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

            moves.Add(tempvert + temphoriz + "A");
            moves.Add(temphoriz + tempvert + "A");
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
        }
        if (from == to){
            moves.Add("A");
        }
        else if (dirPad[to].Item1 == 0 && dirPad[from].Item2 == 0){ //going from left to top
            moves.Add(new string('>', Math.Abs(horiz)) + new string('^', Math.Abs(vert)) + "A");

        } else if(dirPad[from].Item1 == 0 && dirPad[to].Item2 == 0){ //going from top to left
            moves.Add(new string('v', Math.Abs(vert)) + new string('<', Math.Abs(horiz)) + "A");
        } else {
            string tempvert = vert < 0 ? new string('^', Math.Abs(vert)) : new string('v', Math.Abs(vert));
            string temphoriz = horiz < 0 ? new string('<', Math.Abs(horiz)) : new string('>', Math.Abs(horiz));
            moves.Add(tempvert + temphoriz + "A");
            moves.Add(temphoriz + tempvert + "A");
        }

        dirPadMemo[(from, to)] = moves;
        return moves;

    }

}

