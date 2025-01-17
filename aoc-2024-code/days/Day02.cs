using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

public static class Day02
{
    public static void Run()
    {
        Console.WriteLine("Running Day 02 solution...");

        // Start the stopwatch to measure runtime
        Stopwatch stopwatch = Stopwatch.StartNew();

        string inputPath = @"inputs/day02s.txt";

        try
        {
            foreach (string line in File.ReadLines(inputPath))
            {
                // Split line into two columns and parse as integers
                int[] parts = Array.ConvertAll(line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);

                var helper = new Helper();
                bool result = helper.IsSafe(parts);

                Console.WriteLine($"Line: {line} -> Safe: {result}");
            }
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
            int lpoint = 0;
            int rpoint = 1;
            bool inc = parts[rpoint]-parts[lpoint] > 0;


            while (rpoint < parts.Length)
            {
                if (parts[lpoint] == parts[rpoint]){
                    return false;
                }
                
                if (parts[rpoint] - parts[lpoint] > 2)
                {
                    return false;
                }
                lpoint++;
                rpoint++;
            }

            return true;
        }
    }
}
