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

        Console.WriteLine(Part1and2());
    }

    public static int Part1and2(){
        int totalCost = 0;
        List<List<char>> inputcopy = new List<List<char>>(input);
        
        for (int i = 0; i < inputLength; i++){
            for (int j = 0; j<inputWidth; j++ ){
                if(inputcopy[i][j] != '.'){
                    //Console.WriteLine("{0}, {1} {2}",inputcopy[i][j],i,j);
                    PlotFinder plot = new PlotFinder(inputcopy, i, j);
                    plot.FindArea(i,j);
                    totalCost += plot.CostP2();
                    //Part 1:
                    //totalCost += plot.Cost();
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
    Dictionary<string, List<int>> perimTypes = new Dictionary<string, List<int>>();

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

    public int CostP2(){
        int perims = 0;
        foreach(var type in perimTypes){
            perims += CountRegions(type.Value);
        }
        return perims*area;
    }

    public List<List<char>> markedGarden(){
        return input;
    }

    private void AddPerim(string key, int value){
        if (!perimTypes.TryGetValue(key, out var list))
        {
            list = new List<int> { value };
            perimTypes[key] = list;
        }
        else
        {
            list.Add(value);
        }
    }

    private int CountRegions(List<int> list){
        if (list.Count == 0) return 0;
        list.Sort();

        int count = 1;

        for(int i = 1; i<list.Count; i++){
            if (list[i] != list[i-1]+1){
                count++;
            }
        }
        return count;

    }

    public void FindArea(int i, int j){
        int countPerimeter = 0;
        input[i][j] = '.';
        area++;
        seen.Add((i,j));

        if (i-1 < 0){
            countPerimeter++;
            AddPerim(i+"U",j);
        } else {
            if (FindNext(i-1,j) && !seen.Contains((i-1,j))){
                FindArea(i-1, j);
            } else if (!seen.Contains((i-1,j))){
                countPerimeter++;
                AddPerim(i+"U",j);

            }
        }
        if (i+1 >= inputLength){
            countPerimeter++;
            AddPerim(i+"D",j);

        } else {
            if (FindNext(i+1,j) && !seen.Contains((i+1,j))){
                FindArea(i+1, j);
            } else if (!seen.Contains((i+1,j))){
                countPerimeter++;
                AddPerim(i+"D",j);
            }
        }
        if (j-1 < 0){
            countPerimeter++;
            AddPerim(j+"L",i);
        } else {
            if (FindNext(i,j-1) && !seen.Contains((i,j-1))){
                FindArea(i, j-1);
            } else if (!seen.Contains((i,j-1))){
                countPerimeter++;
                AddPerim(j+"L",i);
            }
        }
        if (j+1 >= inputWidth){
            countPerimeter++;
            AddPerim(j+"R",i);
        } else {
            if (FindNext(i,j+1) && !seen.Contains((i,j+1))){
                FindArea(i, j+1);
            } else if (!seen.Contains((i,j+1))){
                countPerimeter++;
                AddPerim(j+"R",i);
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