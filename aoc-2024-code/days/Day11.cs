using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Day11 {

    static Dictionary<(long, int), long> memo = new Dictionary<(long, int), long>();

    public static void Run(){
        Console.WriteLine("Running Day 11 solution...");
        string inputPath = @"inputs/day11.txt";
        StreamReader reader = new StreamReader(inputPath);
        List<long>? stones = reader.ReadLine()?.Split(" ").Select(s => long.Parse(s)).ToList();
        reader.Close();

        Console.WriteLine(Part1(stones));
    }



    private static long Part1(List<long> stones){
        long total = 0;
        int blinks = 75;

        foreach (long stone in stones){
            //Console.WriteLine(thing);
            total += Blink(stone, blinks);
        }
        return total;
    }


    private static long Blink(long stone, int blinks){

        if (memo.TryGetValue((stone, blinks), out long cachedResult))
        {
            //Console.WriteLine($"Using cached result for ({stone}, {blinks}): {cachedResult}");
            return cachedResult;
        }

        long result;
        if (blinks == 0)
        {
            result = 1;
        }
        else if (stone == 0)
        {
            result = Blink(1, blinks - 1);
        }
        else if (NumOfDigits(stone) is int digits && digits % 2 == 0)
        {
            (long first, long second) = SplitNumber(stone, digits / 2);
            result = Blink(first, blinks - 1) + Blink(second, blinks - 1);
        }
        else
        {
            result = Blink(stone * 2024, blinks - 1);
        }

        // Store the computed result in memoization dictionary
        memo.TryAdd((stone, blinks), result);
        return result;
   }

    private static int NumOfDigits(long n)
    {
        if (n == 0) return 1;  // Zero has 1 digit, which is odd
        int digits = (int)Math.Floor(Math.Log10(Math.Abs(n))) + 1;
        return digits;
    }

    private static (long, long) SplitNumber(long stone, long half){

        long splitter = (long) Math.Pow(10,half);
        //Console.WriteLine(splitter);
        long second = stone % splitter;
        long first = stone / splitter;
        //Console.WriteLine("first second {0} {1} stone {2}", first, second, stone);
        return (first, second);

    }


}