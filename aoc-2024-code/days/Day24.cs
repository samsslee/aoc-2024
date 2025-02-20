using System;
using System.Text.RegularExpressions;
using System.Linq;

public static class Day24
{

    static Dictionary<string, bool?> gates = new();
    static List<Calc> input = new();

    private class Calc
    {
        public string A { get; }
        public string B { get; }
        public string Res { get; }
        public string Op { get; }

        public Calc(string a, string b, string res, string op)
        {
            A = a;
            B = b;
            Res = res;
            Op = op;
        }
    }

    public static void Run()
    {
        Console.WriteLine("Running Day 24 solution...");
        string inputPath = @"inputs/day24.txt";
        StreamReader reader = new(inputPath);

        while (true)
        {
            string line = reader.ReadLine();
            if (line == "")
            {
                break;
            }
            string[] x = line.Split(": ");
            bool xval = int.Parse(x[1]) == 1;
            gates[x[0]] = xval;
        }
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] x = line.Split(" "); // 0: A, 1: what, 2: B, 3: ->, 4: result
            if (!gates.ContainsKey(x[0])) gates[x[0]] = null;
            if (!gates.ContainsKey(x[2])) gates[x[2]] = null;
            if (!gates.ContainsKey(x[4])) gates[x[4]] = null;
            input.Add(new Calc(x[0], x[2], x[4], x[1]));
        }

        // Console.WriteLine("\n=== Initial Gates State ===");
        // foreach (var kvp in gates)
        // {
        //     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        // }

        Part1();
        Part2();
    }

    private static void Part1()
    {
        string pattern = @"^z\d{2}$";
        var regex = new Regex(pattern);

        int nullCount = gates.Count(kv => regex.IsMatch(kv.Key) && kv.Value == null);

        while (nullCount > 0) // while some are still unfilled
        {
            int i = 0;
            while (i < input.Count)
            {
                // Ensure keys exist before accessing
                if (gates.ContainsKey(input[i].A) && gates.ContainsKey(input[i].B))
                {
                    if (gates[input[i].A] != null && gates[input[i].B] != null)
                    {
                        gates[input[i].Res] = computeValue((bool)gates[input[i].A], (bool)gates[input[i].B], input[i].Op);
                    }
                }
                i++; // Increment to avoid infinite loop
            }

            // Update nullCount after processing
            nullCount = gates.Count(kv => regex.IsMatch(kv.Key) && kv.Value == null);
            //Console.WriteLine($"Remaining null values: {nullCount}");
        }

        // // Print final results
        // foreach (var kvp in gates)
        // {
        //     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        // }

        var matchingKeys = gates.Keys
        .Where(key => regex.IsMatch(key))
        .OrderBy(key => key)  // Sorting the keys alphabetically
        .ToList();
        long ans = 0;

        for (int k = 0; k<matchingKeys.Count; k++){
            //Console.WriteLine(matchingKeys[k] + ": " + gates[matchingKeys[k]]);
            if (gates[matchingKeys[k]] == true) {
                ans += (long) Math.Pow(2,k);
            }
        }
        Console.WriteLine("Part 1: " + ans);

    }

    private static void Part2(){

        string xyz = @"^[a-zA-Z]\d{2}$"; //for x and y or i guess even z
        var xyzregex = new Regex(xyz);

        List<string> ORletters = new();
        Dictionary<string, int> stringLetters = new();

        HashSet<string> problems = new();

        foreach (Calc line in input){
            if (line.Op == "OR"){
                ORletters.Add(line.A);
                ORletters.Add(line.B);
            } else if (!xyzregex.IsMatch(line.A) && line.Op != "OR"){
                stringLetters[line.A] = stringLetters.GetValueOrDefault(line.A, 0) + 1;
                stringLetters[line.B] = stringLetters.GetValueOrDefault(line.B, 0) + 1;
            }
        }

        foreach(Calc line in input){
            
            //if it's an x_n or y_n it must be a string result
            if (xyzregex.IsMatch(line.A)){ // if it's an x y line then it needs to be letters
                if(xyzregex.IsMatch(line.Res) && line.Res != "z00"){
                    Console.WriteLine(line.A + " " + line.Op + " " + line.B + " -> " + line.Res);
                    problems.Add(line.Res);
                }
            } else if(xyzregex.IsMatch(line.Res)) { //if it's a not x y line and it has a Z, it better have an xor
                if (line.Op != "XOR" && line.Res !="z45"){
                    Console.WriteLine(line.A + " " + line.Op + " " + line.B + " -> " + line.Res);
                    problems.Add(line.Res);
                }
            }
            if (!xyzregex.IsMatch(line.A) && !xyzregex.IsMatch(line.Res)){ // if it's an xor line that isn't an x y line then it better have z
                if (line.Op == "XOR"){
                    Console.WriteLine(line.A + " " + line.Op + " " + line.B + " -> " + line.Res);
                    problems.Add(line.Res);
                }
            }
            if (line.Op == "AND" && line.A != "x00"){
                if (!ORletters.Contains(line.Res)){ //if it's an AND result but it's not in the OR list then it's an issue
                    if(!problems.Contains(line.Res)){
                        Console.WriteLine(line.A + " " + line.Op + " " + line.B + " -> " + line.Res);
                        problems.Add(line.Res);
                    }
                }
            }
            if (xyzregex.IsMatch(line.A) && line.Op == "XOR" && line.Res != "z00"){
                if(!stringLetters.ContainsKey(line.Res) || stringLetters[line.Res] != 2){
                    Console.WriteLine(line.A + " " + line.Op + " " + line.B + " -> " + line.Res);
                    problems.Add(line.Res);
                }
            }
            if (xyzregex.IsMatch(line.A) && line.Op == "OR" && line.Res != "z45"){
                if(!stringLetters.ContainsKey(line.Res) || stringLetters[line.Res] != 2){
                    Console.WriteLine(line.A + " " + line.Op + " " + line.B + " -> " + line.Res);
                    problems.Add(line.Res);
                }
            }

        }

        List<string> tempZs = new();
        List<string> temp = new();

        foreach(string p in problems){
            if (xyzregex.IsMatch(p)){
                tempZs.Add(p);
            } else {
                temp.Add(p);
            }
        }
        temp.Sort();
        tempZs.Sort();

        Console.WriteLine("Part 2: " + string.Join(",", temp)+","+string.Join(",", tempZs));

    }


    private static bool? computeValue(bool A, bool B, string Op){

        bool? ans = null;

        switch (Op){

            case "AND":
                ans = A&&B;
                break;
            case "OR":
                ans = A||B;
                break;
            case "XOR":
                ans = A^B;
                break;

            default:
                Console.WriteLine("issue: " + Op);
                break;

        }

        return ans;

    }
}

    