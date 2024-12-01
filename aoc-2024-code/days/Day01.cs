using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

public static class Day01
{
    public static void Run()
    {
        Console.WriteLine("Running Day 01 solution...");

        // Start the stopwatch to measure runtime
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        string inputPath = @"inputs/day01.txt";
        
        // Min-heaps for the two columns
        PriorityQueue<int, int> leftHeap = new();
        PriorityQueue<int, int> rightHeap = new();

        try
        {
            foreach (string line in File.ReadLines(inputPath))
            {
                // Split line into two columns
                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                {
                    throw new FormatException($"Invalid line format: {line}");
                }

                int leftValue = int.Parse(parts[0]);
                int rightValue = int.Parse(parts[1]);

                // Add to respective heaps
                leftHeap.Enqueue(leftValue, leftValue);
                rightHeap.Enqueue(rightValue, rightValue);
            }

            Console.WriteLine("Heaps built successfully!");

            // Now delegate the calculation of the sum of differences to the Solver class
            // var solverP1 = new Solver();
            // int sumOfDifferences = solverP1.CalculateSumOfDifferences(leftHeap, rightHeap);

            var solverP2 = new Solver();
            int similarityScore = solverP2.SimilarityScore(leftHeap, rightHeap);

            // Stop the stopwatch and print the elapsed time
            stopwatch.Stop();
            // Console.WriteLine($"Sum of absolute differences between respective values: {sumOfDifferences}");
            Console.WriteLine($"Similarity Score is: {similarityScore}");
            Console.WriteLine($"Runtime: {stopwatch.ElapsedMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}

// Solver class in the same file
public class Solver
{
    public int CalculateSumOfDifferences(PriorityQueue<int, int> leftHeap, PriorityQueue<int, int> rightHeap)
    {
        int sumOfDifferences = 0;

        // Process both heaps, summing the differences between corresponding elements
        while (leftHeap.Count > 0 && rightHeap.Count > 0)
        {
            int leftVal = leftHeap.Dequeue();
            int rightVal = rightHeap.Dequeue();

            // Calculate the difference and add to the sum
            sumOfDifferences += Math.Abs(leftVal - rightVal);
        }

        return sumOfDifferences;
    }

    public int SimilarityScore(PriorityQueue<int, int> leftHeap, PriorityQueue<int, int> rightHeap)
    {
        int similarityScore = 0;

        while (leftHeap.Count > 0 && rightHeap.Count > 0)
        {
            int currLeft = leftHeap.Peek();
            int currRight = rightHeap.Peek();

            if (currLeft < currRight)
            {
                leftHeap.Dequeue();
            }
            else if (currLeft > currRight)
            {
                rightHeap.Dequeue();
            }
            else // currLeft == currRight
            {
                int countLeft = 0;
                int countRight = 0;
                
                // Count occurrences of currLeft in leftHeap
                while (leftHeap.Count > 0 && leftHeap.Peek() == currLeft)
                {
                    countLeft++;
                    leftHeap.Dequeue();
                }

                // Count occurrences of currRight in rightHeap
                while (rightHeap.Count > 0 && rightHeap.Peek() == currRight)
                {
                    countRight++;
                    rightHeap.Dequeue();
                }

                similarityScore += countLeft * countRight * currLeft;
            }
        }

        return similarityScore;
    }

}
