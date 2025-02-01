using System;
using System.Text.RegularExpressions;

public static class Day13{

    public static void Run(){
        Console.WriteLine("Running Day 13 solution...");
        string inputPath = @"inputs/day13.txt";
        StreamReader reader = new StreamReader(inputPath);
        string pattern = @"\d+";
        long tokens = 0;

        while(!reader.EndOfStream)
        {
            string line = reader.ReadLine()+ reader.ReadLine()+ reader.ReadLine();
            List<int> numbers = new List<int>();
            
            foreach (Match match in Regex.Matches(line, pattern))
            {
                numbers.Add(int.Parse(match.Value));
            }

            Console.WriteLine($"Extracted numbers: {string.Join(", ", numbers)}");

            SolveIntersect solveset = new SolveIntersect(numbers);
            if (solveset.IsIntersecting()){
                if (solveset.CanSolve()){
                    tokens += solveset.TokensUsed();
                    Console.WriteLine(solveset.TokensUsed());
                } else {
                    Console.WriteLine("no integer Solutions");
                }
            } else if(solveset.IsCoincident()){
                Console.WriteLine("coincident");
            } else {
                Console.WriteLine("no solutions");
            }
            reader.ReadLine();
        }
        
        Console.WriteLine(tokens);
        
        reader.Close();
    }

}

public class SolveIntersect{
    int a1;
    int a2;
    int b1;
    int b2;
    long c1;
    long c2;

    long denom;
    long numer;

    long A = 0;
    long B = 0;

    public SolveIntersect(List<int> nums){
        a1 = nums[0];
        a2 = nums[1];
        b1 = nums[2];
        b2 = nums[3];
        //part 1
        // c1 = nums[4];
        // c2 = nums[5];

        //part 2
        c1 = 10000000000000 + nums[4];
        c2 = 10000000000000 + nums[5];
        denom = a1*b2 - a2*b1;
        numer = c1*b2 - c2*b1;
    }

    public bool IsIntersecting(){
        return denom != 0; //return true if it intersects
    }

    public bool IsCoincident(){
        return denom == 0 && numer == 0; 
    }

    public bool IsInt(long n, long d) {
        return n % d == 0;
    }
    
    public long TokensUsed(){
        Console.WriteLine(A+", " +B);
        return A*3 + B;
    }

    public bool CanSolve(){
        if (IsInt(numer, denom)){
            A = numer / denom;
            long numerB  = c1 - A*a1;
            if(IsInt(numerB, b1)){
                B = numerB / b1;
                
                return true;
            } else {
                return false;
            }

        } else {
            return false;
        }
    }

}