using System;
using System.Collections;

public static class Day08{

    private static Dictionary<char, List<(int, int)>> antennas = new Dictionary<char, List<(int, int)>>();
    private static HashSet<(int, int)> aNodes = new HashSet<(int, int)>();
    private static int width;
    private static int height; 

    public static void Run(){
        Console.WriteLine("Running Day 08 solution...");
        string inputPath = @"inputs/day08.txt";

        string[] map = File.ReadAllLines(inputPath);
        height = map.Length;
        width = map[0].Length;
        Console.WriteLine(Part1(map));
    }

    private static int Part1(string[] map)
    {
        for (int i=0;i<height;i++){
            for(int j=0; j<width; j++){
                if(map[i][j] != '.'){
                    //check if it's the dictionary
                    if (antennas.ContainsKey(map[i][j])){
                        
                        //part 2
                        //if it's the first time we see a repeat antenna
                        //then add the one that's in the list as well as the new one
                        if (antennas[map[i][j]].Count == 1){
                            aNodes.Add(antennas[map[i][j]][0]);
                        }
                        //otherwise, we've already added all the antennas in the list
                        //so just add the new antenna to the node list
                        aNodes.Add((i,j));
                        
                        
                        makeANodes(i, j, antennas[map[i][j]]);
                        antennas[map[i][j]].Add((i,j));



                    } else {
                        antennas.Add(map[i][j], new List<(int,int)>{(i, j)});
                    };
                }
            }
        }

        return aNodes.Count;
    }

    private static void makeANodes(int curri, int currj, List<(int, int)> ants){

        foreach ((int, int) ant in ants){
            //make forward and backward nodes differently in each part

            //part1 only
            // addAntinode(curri, currj, ant.Item1, ant.Item2);
            // addAntinode(ant.Item1, ant.Item2, curri, currj);


            //part2 only
            keepAddingAntinodes(curri, currj, ant.Item1, ant.Item2);
            keepAddingAntinodes(ant.Item1, ant.Item2, curri, currj);
        }

    }

    private static void addAntinode(int i0, int j0, int i1, int j1){
        int inew = i0 - i1 + i0;
        int jnew = j0 - j1 + j0;
        if(inew < height && inew >=0 && jnew <width && jnew >=0){
            aNodes.Add((inew, jnew));
        };
    }

    private static void keepAddingAntinodes(int i0, int j0, int i1, int j1){
        int idiff = i0-i1;
        int jdiff = j0-j1;
        
        while (true){
            int inew = idiff + i0;
            int jnew = jdiff + j0;
            if(inew < height && inew >=0 && jnew <width && jnew >=0){
                aNodes.Add((inew, jnew));
            } else {
                break;
            }
            i0 = inew;
            j0 = jnew;
        }
    }

}