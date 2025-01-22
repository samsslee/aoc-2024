using System;
using System.Diagnostics;
using System.Text.RegularExpressions;


public static class Day03
{
    public static void Run()
    {
        Console.WriteLine("Running Day 03 solution...");

        // Start the stopwatch to measure runtime
        Stopwatch stopwatch = Stopwatch.StartNew();

        string inputPath = @"inputs/day03.txt";
        string inputString = File.ReadAllText(inputPath);

        int total = Part1(inputString);
        Console.WriteLine(total);

        int total2 = Part2(inputString);
        Console.WriteLine(total2);


        stopwatch.Stop();
        Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");

    }

    private static int Part1(string inputString)
    {
        string pattern = @"mul\(\-?\d+,\-?\d+\)";
        Regex rg = new Regex(pattern);

        MatchCollection muls = rg.Matches(inputString);
        Console.WriteLine(muls.Count);
        int total = 0;
        
        for (int i=0; i<muls.Count; i++){
            //Console.WriteLine(muls[i].Value);
            total += Utils.Multiply(muls[i].Value);
        }

        return total;
    }

    private static int Part2(string inputString)
    {
        string dopattern = @"do\(\)";
        string dontpattern = @"don\'t\(\)";

        string[] parts = Regex.Split(inputString, dopattern);
        int total = 0;

        foreach (string part in parts)
        {
            string[] partAgain = Regex.Split(part, dontpattern);
            Console.WriteLine(part);
            total += Part1(partAgain[0]);
        }

        return total;

    }
}



public static class Utils
{
    public static int Multiply(string mulLine)
    {
        Regex num = new Regex(@"\-?\d+");
        MatchCollection nums = num.Matches(mulLine);

        if (nums.Count < 2)
        {
            throw new ArgumentException("Input line must contain at least two numbers.");
        }

        return Convert.ToInt32(nums[0].Value) * Convert.ToInt32(nums[1].Value);
    }
}