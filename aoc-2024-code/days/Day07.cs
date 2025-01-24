using System;

public static class Day07
{

    // Arrays or lists to store parts of the split lines
    static List<long> sums = new List<long>();
    static List<long[]> nums = new List<long[]>();
    public static void Run(){
        Console.WriteLine("Running Day 07 solution...");
        string inputPath = @"inputs/day07.txt";

        StreamReader reader = new StreamReader(inputPath);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(": ");
            sums.Add(long.Parse(parts[0]));
            nums.Add(parts[1].Split(" ").Select(long.Parse).ToArray());

        }
        reader.Close();
        Console.WriteLine(Part1());

    }

    private static long Part1(){
        long total = 0;

        for(int i = 0; i<nums.Count;i++)
        {
            //Console.WriteLine(nums[i][0]);
            NumbersProcessor processor = new NumbersProcessor(nums[i], sums[i]);
            if(processor.AddOrMultiply(nums[i][0],1)){
                total += sums[i];
            }; // Call a method to process the numbers
        }
        return total;
    }
}

public class NumbersProcessor
{
    private long[] nums;      // Array of numbers
    private long targetSum;   // Target sum to reach
    private int nLength;     // Length of the numbers array

    // Constructor to initialize the fields
    public NumbersProcessor(long[] numbers, long sum)
    {
        nums = numbers;
        targetSum = sum;
        nLength = numbers.Length;
    }

    // Recursive method to check if targetSum can be reached
    public bool AddOrMultiply(long currentSum, long currentIndex)
    {
        // Base condition: If the current sum matches the target, return true
        if (currentSum == targetSum)
        {
            return true;
        }
        if (currentSum > targetSum)
        {
            return false;
        }
        // Base condition: If we've processed all numbers, return false
        if (currentIndex >= nLength)
        {
            return false;
        }

        // Recursive case: Try adding and multiplying the next number
        long nextNum = nums[currentIndex];
        bool addedResult = AddOrMultiply(currentSum + nextNum, currentIndex + 1);
        bool multipliedResult = AddOrMultiply(currentSum * nextNum, currentIndex + 1);

        // Return true if either operation yields the target sum
        return addedResult || multipliedResult;
    }
}