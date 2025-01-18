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
                int[] parts = Array.ConvertAll(line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);

                var helper = new Helper();
                bool result = helper.IsSafe(parts);

                Console.WriteLine($"Line: {line} -> Safe: {result}");

                if (result)
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

    private class Helper
    {
        public bool IsSafe(int[] parts)
        {

            int [] diffs = makeDiffs(parts);
            bool inc = parts[1]-parts[0] > 0;
            bool success = true;
            int i = 0;

            while (success && i<diffs.Length){
                success = checkConditions(diffs[i++], inc);
            }

            if (success || i==diffs.Length)
            {
                return true;
            } 
            else {
                i--;
                success = true;
                //try dropping the left number
                if(i==0){
                    inc = diffs[1] > 0;
                    
                    for (int j = 1; j<diffs.Length; j++){
                        success = checkConditions(diffs[j],inc);
                        if (!success)
                        {
                            break;
                        }
                    }

                } else {
                    
                    for (int j = 0; j<diffs.Length; j++){
                        if (j == i-1){
                            success = checkConditions(diffs[j]+diffs[j+1],inc);
                            j++;
                        } else {
                            success = checkConditions(diffs[j],inc);
                        }
                        if (!success)
                        {
                            break;
                        }
                    }

                }

                if (!success) {
                        for (int j = 0; j<diffs.Length; j++){
                            if (j == i){
                                success = checkConditions(diffs[j]+diffs[j+1],inc);
                                j++;
                            } else {
                                success = checkConditions(diffs[j],inc);
                            }
                            if (!success)
                            {
                                break;
                            }
                        }
                    }


            }

            return success;

        }

        private int [] makeDiffs(int[] parts)
        {
            int [] diffs = new int[parts.Length-1];

            for (int i = 0; i< parts.Length-1; i++)
            {
                diffs[i] = parts[i+1] - parts[i];
            }

            return diffs;
        }

        private bool checkConditions( int diff, bool inc)
        {
            if (diff == 0 || Math.Abs(diff) >3 || diff > 0 != inc)
            {
                return false;
            }

            return true;
        }

    }

    
}
