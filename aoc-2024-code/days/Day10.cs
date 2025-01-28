using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Day10 {

    static List<List<int>> input = new List<List<int>>();
    static int inputLength;
    static int inputWidth;

    public static void Run(){
        Console.WriteLine("Running Day 10 solution...");
        string inputPath = @"inputs/day10.txt";
        var rawInput = File.ReadAllLines(inputPath);
        inputLength = rawInput.Length;
        inputWidth = rawInput[0].Length;

        input = rawInput.Select(line => line.Select(ch => ch - '0').ToList()).ToList();
        Console.WriteLine(Part1and2());

    }

    private static long Part1and2() {

        long hikeScore = 0;
        HikeFinder findHikes = new HikeFinder(input, inputLength, inputWidth);

        for (int i = 0; i<inputLength; i++){
            for (int j = 0; j<inputWidth; j++){
                if (input[i][j] == 0){
                    //Console.WriteLine("** starting at {0} {1}", i, j);
                    int eachHikeScore = findHikes.Hike(i,j);
                    //Console.WriteLine("-----------Hikescore of {0}", eachHikeScore);
                    hikeScore += eachHikeScore;
                }
            }
        }

        return hikeScore;
    }

}

public class HikeFinder {

    static List<List<int>> input = new List<List<int>>();
    HashSet<(int, int)> trailEnds = new HashSet<(int, int)>();

    int inputLength;
    int inputWidth;
    
    public HikeFinder(List<List<int>> i, int length, int width){
        input = i;
        inputLength = length;
        inputWidth = width;
    }

    public int Hike(int i, int j)
    {
        // Clear the HashSet to reset the state for this hike
        trailEnds.Clear();

        // Start the hike from the given position
        return Hiker(i, j);
    }

    private int Hiker(int i, int j) {
        
        //find valid neighbors, i think current direction doesn't matter because we can only increase
        //so you don't end up going back and forth

        if(input[i][j] == 9){

            //part 2
            return 1;

            //part 1
            // if(trailEnds.Contains((i,j))){
            //     //Console.WriteLine("repeat 9 at: {0} {1}", i, j);

            //     return 0;
            // } else {
            //     //Console.WriteLine("found 9 at: {0} {1}", i, j);
            //     trailEnds.Add((i,j));
            //     return 1;
            // }
            
        }

        List<(int,int)> nextSteps = findNextSteps(i, j);
        int count = 0;

        foreach ((int,int) step in nextSteps){
            //Console.WriteLine("Hiking to: {0} {1}", step.Item1, step.Item2);
            count += Hiker(step.Item1, step.Item2);
        }

        return count;

    }

    private List<(int, int)> findNextSteps(int i, int j){

        List<(int, int)> nextSteps = new List<(int, int)>();

        if (i < 0 || i>= inputLength || j<0 || j>=inputWidth){ //out of bounds
            return nextSteps;
        }

        //look in all 4 directions
        int current = input[i][j];

        if (i-1>= 0 && current+1 == input[i-1][j]){ //up neibor
            nextSteps.Add((i-1, j));
        }
        if (i+1<inputLength && current+1 == input[i+1][j]){ //down neighbor
            nextSteps.Add((i+1, j));
        }
        if (j-1>= 0 && current+1 == input[i][j-1]){ //left neighbor
            nextSteps.Add((i, j-1));
        }
        if (j+1<inputWidth && current+1 == input[i][j+1]){ //right neighbor
            nextSteps.Add((i, j+1));
        }

        return nextSteps;

    }


}