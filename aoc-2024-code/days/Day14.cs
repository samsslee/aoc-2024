using System;
using System.Text.RegularExpressions;

public static class Day14{

    public static void Run(){
        Console.WriteLine("Running Day 14 solution...");

        long max = 0;
        
        //Part 2
        for (int i = 0; i<103*101; i++){
            long lots = Part1(i);
            if (lots > max){
                max = lots;
                Console.WriteLine(i);
                Console.WriteLine(max);
            }
        }
    }

    private static long Part1(int iter){
        string inputPath = @"inputs/day14.txt";

        StreamReader reader = new StreamReader(inputPath);
        string pattern = @"-?\d+";
        List<int> quadrants = new List<int>(){0,0,0,0,0};

        while(!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            List<int> numbers = new List<int>();
            foreach (Match match in Regex.Matches(line, pattern))
            {
                numbers.Add(int.Parse(match.Value));
            }
            //Console.WriteLine($"Extracted numbers: {string.Join(", ", numbers)}");


            placeRobot pr = new placeRobot(numbers, iter);
            quadrants[pr.whichQuadrant()]++;
            
        }
        //Console.WriteLine("{0}, {1}, {2}, {3}", quadrants[1], quadrants[2], quadrants[3], quadrants[4]);
        quadrants.RemoveAt(0);
        return quadrants.Max();

    }
}

public class placeRobot{

    static int width = 101; //101 //11
    static int height = 103; //103 //7

    static int iter;

    int x;
    int y;
    int dx;
    int dy;

    public placeRobot(List<int> nums, int iters){
        x = nums[0];
        y = nums[1];
        dx = nums[2];
        dy = nums[3];
        iter = iters;
    }

    public int whichQuadrant(){
        (int x, int y) = MoveRobot();
        
        if (y < height/2){
            if (x<width/2){
                return 1;
            } else if(x>width/2) {
                return 2;
            }
        } else if (y> height/2) {
            if (x<width /2){
                return 3;
            } else if (x>width/2) {
                return 4;
            }
        } 
        return 0;

    }

    private (int, int) MoveRobot(){
        int totaldx = Modulo(dx*iter, width);
        int totaldy = Modulo(dy*iter, height);

        //Console.WriteLine(totaldx);
        //Console.WriteLine(totaldy);

        int finalx = (totaldx + x) % width;
        int finaly = (totaldy + y) % height;


        return (finalx, finaly);
    }

    private int Modulo(int a, int b)
    {
        return ((a % b) + b) % b;
    }


}