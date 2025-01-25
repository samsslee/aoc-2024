using System;
using System.Collections.Generic;

public static class Day09
{

    private static int length;
    private static List<int> spaces = new List<int>();
    private static List<int> boxes = new List<int>();
    private static int totalFilled;
    

    public static void Run()
    {
        Console.WriteLine("Running Day 09 solution...");
        string inputPath = @"inputs/day09.txt";
        StreamReader reader = new StreamReader(inputPath);

        string? input = reader.ReadLine();
        if (input != null){
            length = input.Length;
        }
        reader.Close();

        int i = 0;
        while (i<length){
            int b = int.Parse(input[i++].ToString());
            boxes.Add(b);
            totalFilled += b;
            if (i<length){
                spaces.Add(int.Parse(input[i++].ToString()));
            }
        }

        Console.WriteLine("answer: "+ Part1());
    }

    private static long Part1()
{
    int grandIndex = 0; // grand admiral index lol
    int lp;
    int sp; // space pointer sp
    long answer = 0;
    bool flag = false;

    // Left numbers
    for (lp = 0; lp < boxes.Count; lp++)
    {
        for (int k = 0; k < boxes[lp]; k++)
        {
            if (grandIndex >= totalFilled)
            {
                if (k != boxes[lp]-1){
                    boxes[lp] = boxes[lp]-k;
                }
                
                flag = true;
                break;
            }
            answer += checkSum(grandIndex, lp);
            grandIndex++;
        }
        if (flag)
        {
            break;
        }
        grandIndex += spaces[lp];
    }

    // Reset variables for right numbers
    grandIndex = 0;
    flag = false;
    int rp = boxes.Count - 1;

    // Right numbers
    for (sp = 0; sp < spaces.Count; sp++)
    {
        grandIndex += boxes[sp];
        int i = 0; // Tracks progress within the current space

        while (i < spaces[sp] && lp<=rp)
        {

            for (int k = 0; k < boxes[rp]; k++)
            {
                answer += checkSum(grandIndex, rp); 
                grandIndex++; // Increment global index
                i++; // Increment within space
                if (i >= spaces[sp]) // Check if out of space
                {
                    if (k < boxes[rp] - 1) // Handle leftover box values
                    {
                        boxes[rp] -= (k + 1); // Properly subtract used values
                    }
                    else // All values in the box are used
                    {
                        rp--; // Move to the previous box
                    }
                    break;
                }
                else if (k == boxes[rp] - 1)
                {
                    rp--; // Move to the previous box
                    break;
                }
            }
        }

        // Exit the outer loop if the total is filled
        if (grandIndex >= totalFilled)
        {
            break;
        }
    }

    return answer;
}


    private static int checkSum(int gI, int p){
        return gI*p;
    }
}