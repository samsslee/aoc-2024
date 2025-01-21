using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;

public static class Day02
{
    public static void Run()
    {
        Console.WriteLine("Running Day 02 solution...");

        // Start the stopwatch to measure runtime
        Stopwatch stopwatch = Stopwatch.StartNew();

        string inputPath = @"inputs/day02.txt";
        int safeCount = 0;


        try
        {
            foreach (string line in File.ReadLines(inputPath))
            {
                // Split line into two columns and parse as integers
                List<int> parts = Array.ConvertAll(line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries), int.Parse).ToList();

                // bool result = Part1.IsSafe(parts);

                // Console.WriteLine($"Line: {line} -> Safe: {result}");
                bool result2 = Part2.IsSafeWithException(parts);
                //Console.WriteLine($"Line: {line} -> Safe: {result2}");
                

                if (result2)
                {
                    safeCount++;
                }

            }
            Console.WriteLine(safeCount);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        // Stop the stopwatch and display runtime
        stopwatch.Stop();
        Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
    }

    private class Part1
    {
        public static bool IsSafe(List<int> parts)
        {
            int [] diffs = Utils.makeDiffs(parts);
            bool inc = diffs[0] > 0;
            foreach (int diff in diffs)
            {
                if(!Utils.checkConditions(diff, inc))
                {
                    return false;
                }
            }

            return true;
        }
    }

    private class Part2
    {
        public static bool IsSafeWithException(List<int> parts)
        {
            int [] diffs = Utils.makeDiffs(parts);
            bool inc = diffs[0] > 0;
            for (int i = 0; i <diffs.Length; i++)
            {
                //if it's unsafe AND you're not already at the last diff
                if(!Utils.checkConditions(diffs[i], inc) && i != diffs.Length-1)
                {
                    //always check if dropping the first one helps
                    //RIP this will recalculate diffs whatever
                    if (Part1.IsSafe(parts[1..]))
                    {
                        return true;
                    }

                    List<int> leftRemove = [.. parts];
                    leftRemove.RemoveAt(i);
                    if (Part1.IsSafe(leftRemove))
                    {
                        return true;
                    }


                    List<int> rightRemove = [.. parts];
                    rightRemove.RemoveAt(i+1);
                    if (Part1.IsSafe(rightRemove))
                    {
                        return true;
                    }

                    
                    return false;
                }
            }
            return true;
        }


    }

    private class Utils
    {
        public static int [] makeDiffs(List<int> parts)
        {
            int [] diffs = new int[parts.Count-1];

            for (int i = 0; i< parts.Count-1; i++)
            {
                diffs[i] = parts[i+1] - parts[i];
            }

            return diffs;
        }

        public static bool checkConditions( int diff, bool inc)
        {
            if (diff == 0 || Math.Abs(diff) >3 || diff > 0 != inc)
            {
                return false;
            }

            return true;
        }
    }

    // private class Helper
    // {
    //     public bool IsSafe(int[] parts)
    //     {

    //         int [] diffs = makeDiffs(parts);
    //         bool inc = parts[1]-parts[0] > 0;
    //         bool success = true;
    //         int i = 0;

    //         while (success && i<diffs.Length){
    //             success = checkConditions(diffs[i++], inc);
    //         }

    //         if (success || i==diffs.Length)
    //         {
    //             return true;
    //         } 
    //         else {
    //             i--;
    //             success = true;
    //             //add it right
    //             int j = 0;

    //             if (i == 0){
    //                 inc = diffs[0]+diffs[1] > 0;
    //             }

    //             while (success && j<diffs.Length){

    //                 if (j == i-1){
    //                     success = checkConditions(diffs[j]+diffs[j+1], inc);
    //                     j++;
    //                 } else {
    //                     success = checkConditions(diffs[j], inc);
    //                 }
    //                 j++;
    //             }

    //             //add it left
    //             if (!success){
                    
    //                 if (i==0){ //drop leftmost diff
    //                     j = 1;
    //                     inc = diffs[1] > 0;
    //                     do {
    //                         success = checkConditions(diffs[j++], inc);
    //                     } while (success && j<diffs.Length);
    //                 } else {
    //                     j = 0;
    //                     if (i == 1){
    //                         inc = diffs[0]+diffs[1] > 0;
    //                     }

    //                     while (success && j<diffs.Length){
    //                         if (j == i){
    //                             success = checkConditions(diffs[j]+diffs[j+1], inc);
    //                             j++;
    //                         } else {
    //                             success = checkConditions(diffs[j], inc);
    //                         }
    //                         j++;
    //                     }
    //                 }
    //             }

    //         }

    //         return success;

    //     }

    //     private int [] makeDiffs(int[] parts)
    //     {
    //         int [] diffs = new int[parts.Length-1];

    //         for (int i = 0; i< parts.Length-1; i++)
    //         {
    //             diffs[i] = parts[i+1] - parts[i];
    //         }

    //         return diffs;
    //     }

    //     private bool checkConditions( int diff, bool inc)
    //     {
    //         if (diff == 0 || Math.Abs(diff) >3 || diff > 0 != inc)
    //         {
    //             return false;
    //         }

    //         return true;
    //     }

    // }

    
}
