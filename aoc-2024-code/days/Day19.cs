using System;
using System.IO.Pipelines;
using System.Linq;

public static class Day19{

    static int maxPattern;

    static Dictionary<string, bool> memoArrangement = new();
    static Dictionary<string, long> memoArrangementCount = new();

    static HashSet<string> towels = new();
    static List<string> patterns = new();

    public static void Run(){
        //make the towels and count what is the biggest towel pattern is, max towel pattern
        
        Console.WriteLine("Running Day 19 solution...");
        string inputPath = @"inputs/day19.txt";

        StreamReader reader = new(inputPath);
        towels.UnionWith(reader?.ReadLine()?.Split(", "));
        maxPattern = towels.Max(t => t.Length);

        //skip blank
        reader.ReadLine();
        //make a list of the patterns
        while (!reader.EndOfStream){
            patterns.Add(reader.ReadLine());
        }
        reader.Close();

        Part1();
        Part2();

    }

    private static int Part1(){
        int count = 0;

        foreach(string pattern in patterns){
            if(canArrange(pattern)){
                count++;
            }
        }

        Console.WriteLine(count);
        return count;
    }


    private static bool canArrange(string pattern){

        if (memoArrangement.ContainsKey(pattern)){
                return memoArrangement[pattern];
        }

        if (pattern.Length == 0) return true;
        if (towels.Contains(pattern)) return true;

        //choose all the combinations from (1,the rest) to (maxPattern,the rest)
        for (int i = 1; i <= Math.Min(pattern.Length, maxPattern); i++)
        {
            string left = pattern[..i];
            string right = pattern[i..];

            if (towels.Contains(left) && canArrange(right))
            {
                memoArrangement[pattern] = true;
                return true;
            }
        }
        
        memoArrangement[pattern] = false;
        return false;

    }


    private static long Part2(){
        long count = 0;

        foreach(string pattern in patterns){
            count += Arrangements(pattern);
        }

        Console.WriteLine(count);
        return count;

    }

    private static long Arrangements(string pattern){

        if (memoArrangementCount.ContainsKey(pattern)){
                return memoArrangementCount[pattern];
        }

        if (pattern.Length == 0) return 1;
        //removed the original base case of finding "any" towel
        //just let the recursion handle it

        long totalArrangements = 0;

        //choose all the combinations from (1,the rest) to (maxPattern,the rest)
        for (int i = 1; i <= Math.Min(pattern.Length, maxPattern); i++)
        {
            string left = pattern[..i];
            string right = pattern[i..];

            if (towels.Contains(left)) // Only recurse if `left` is valid
            {
                totalArrangements += Arrangements(right);
            }

        }
        
        memoArrangementCount[pattern] = totalArrangements;
        return totalArrangements;

    }

}