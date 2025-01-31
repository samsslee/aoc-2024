using System;

public static class Day12{

    static List<List<char>> input = new List<List<char>>();
    static int inputLength;
    static int inputWidth;
    public static void Run(){
        Console.WriteLine("Running Day 12 solution...");
        string inputPath = @"inputs/day12.txt";
        var rawInput = File.ReadAllLines(inputPath);
        inputLength = rawInput.Length;
        inputWidth = rawInput[0].Length;

        input = rawInput.Select(line => line.ToList()).ToList();

        Console.WriteLine(Part1());
    }

    public static int Part1(){
        int totalCost = 0;
        List<List<char>> inputcopy = new List<List<char>>(input);
        
        for (int i = 0; i < inputLength; i++){
            for (int j = 0; j<inputWidth; j++ ){
                if(inputcopy[i][j] != '.'){
                    //Console.WriteLine("{0}, {1} {2}",inputcopy[i][j],i,j);
                    PlotFinder plot = new PlotFinder(inputcopy, i, j);
                    plot.FindArea(i,j);
                    totalCost += plot.Cost();
                    inputcopy = plot.markedGarden();
                }
            }
        }

        return totalCost;
    }



}

public class PlotFinder{


    int area = 0;
    int perimeter = 0;
    List<List<char>> input = new List<List<char>>();
    HashSet<(int, int)> seen = new HashSet<(int, int)>();

    char letter;
    int inputLength;
    int inputWidth;

    public PlotFinder(List<List<char>> garden, int i, int j){
        input = garden;
        letter = input[i][j];
        inputLength = input.Count;
        inputWidth = input[0].Count;
    }

    public int Cost(){
        //Console.WriteLine("A*P {0}, {1}", area, perimeter);
        return perimeter*area;
    }

    public List<List<char>> markedGarden(){
        return input;
    }

    public void FindArea(int i, int j){
        int countPerimeter = 0;
        input[i][j] = '.';
        area++;
        seen.Add((i,j));

        if (i-1 < 0){
            countPerimeter++;
        } else {
            if (FindNext(i-1,j) && !seen.Contains((i-1,j))){
                FindArea(i-1, j);
            } else if (!seen.Contains((i-1,j))){
                countPerimeter++;
            }
        }
        if (i+1 >= inputLength){
            countPerimeter++;
        } else {
            if (FindNext(i+1,j) && !seen.Contains((i+1,j))){
                FindArea(i+1, j);
            } else if (!seen.Contains((i+1,j))){
                countPerimeter++;
            }
        }
        if (j-1 < 0){
            countPerimeter++;
        } else {
            if (FindNext(i,j-1) && !seen.Contains((i,j-1))){
                FindArea(i, j-1);
            } else if (!seen.Contains((i,j-1))){
                countPerimeter++;
            }
        }
        if (j+1 >= inputWidth){
            countPerimeter++;
        } else {
            if (FindNext(i,j+1) && !seen.Contains((i,j+1))){
                FindArea(i, j+1);
            } else if (!seen.Contains((i,j+1))){
                countPerimeter++;
            }
        }

        //Console.WriteLine("{0},{1}: {2}", i,j,countPerimeter);

        perimeter += countPerimeter;

    }

    private bool FindNext(int i, int j){
        if (input[i][j] == letter){
            return true;
        } else {
            return false;
        }
    
    }



}