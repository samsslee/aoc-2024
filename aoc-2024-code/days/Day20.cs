using System;

public static class Day20{

    static List<(int,int)> raceTrack = new();
    static List<List<char>> map = new();
    static (int, int) start;
    static int height;
    static int width;

    static bool found = false;

    static int target = 5;
    static int targetp2 = 100;
    static int taxicab = 20;

    public static void Run(){

        Console.WriteLine("Running Day 20 solution...");
        string inputPath = @"inputs/day20.txt";

        //go line by line and convert into list of list
        //see if S or E are in the list

        string[] input = File.ReadAllLines(inputPath);
        height = input.Length;
        width = input[0].Length;

        for (int i = 0; i<height; i++){
            List<char> newLine = [.. input[i]];
            if(newLine.Contains('S')){
                start = (i, newLine.IndexOf('S'));
            }
            map.Add(newLine);
        }

        Part1();

    }

    private static int Part1(){
        int save100 = 0;

        //set up start as the first node and it's directions
        raceTrack.Add(start);

        //do maze and store maze as path:walls
        NumberTrack(start.Item1, start.Item2, 1); //start north for sample, east for actual
        Console.WriteLine(raceTrack.Count);

        //go through each node and find it's walls
        foreach((int, int) track in raceTrack){
            save100+= BreakWallAt(track.Item1, track.Item2);
        }

        Console.WriteLine("saved more than target: "+ save100);
        //break each wall and see if it makes paths to the main path


        int save100CheatMore = 0;
        //Part 2
        //pair them via step distance over target ie 100 or 50 or whatever
        for(int a = 0; a<raceTrack.Count-targetp2-1; a++){
            for (int b = a+targetp2; b< raceTrack.Count; b++){

                //is it taxicab 20
                int xdist = Math.Abs(raceTrack[a].Item1 - raceTrack[b].Item1);
                int ydist = Math.Abs(raceTrack[a].Item2 - raceTrack[b].Item2);
                int currenttaxi = xdist+ydist;

                if (currenttaxi <= taxicab && b-a-currenttaxi >=targetp2){
                    save100CheatMore++;
                }
            }
        }
        Console.WriteLine("part2: "+ save100CheatMore);

        return save100;
    }

    private static void NumberTrack(int i, int j, int dir){
        bool found = false;

        while(!found){

            if(map[i][j] == 'E'){
                found = true;
            } else {
                if(dir!=2){
                    if(map[i-1][j] != '#'){
                        raceTrack.Add((i-1,j));
                        i -= 1;
                        dir = 0;
                    }
                }
                if(dir !=0 && map[i+1][j] !='#'){
                    raceTrack.Add((i+1,j));
                    i +=1;
                    dir = 2;
                }
                if( dir !=3 && map[i][j+1] !='#'){
                    raceTrack.Add((i,j+1));
                    j+=1;
                    dir = 1;
                }
                if (dir !=1 && map[i][j-1] !='#'){
                    raceTrack.Add((i, j-1));
                    j-=1;
                    dir = 3;
                }
            }

        }
        
    }

    private static int BreakWallAt(int i, int j){

        int savingsOver100 = 0;
        int currIndex = raceTrack.IndexOf((i,j));
        
        // look up
        if (map[i-1][j] == '#'){
            if(raceTrack.Contains((i-2,j))){
                int savings = raceTrack.IndexOf((i-2,j)) - currIndex - 2;
                //Console.WriteLine(savings);
                if (savings>=target){
                    savingsOver100++;
                }
            }
        }
        //look down
        if (map[i+1][j] == '#'){
            if(raceTrack.Contains((i+2,j))){
                int savings = raceTrack.IndexOf((i+2,j)) - currIndex - 2;
                //Console.WriteLine(savings);

                if (savings>=target){
                    savingsOver100++;
                }
            }
        }
        //look right
        if (map[i][j+1] == '#'){
            if(raceTrack.Contains((i,j+2))){
                int savings = raceTrack.IndexOf((i,j+2)) - currIndex - 2;
                //Console.WriteLine(savings);

                if (savings>=target){
                    savingsOver100++;
                }
            }
        }
        //look left
        if (map[i][j-1] == '#'){
            if(raceTrack.Contains((i,j-2))){
                int savings = raceTrack.IndexOf((i,j-2)) - currIndex - 2;
                //Console.WriteLine(savings);

                if (savings>=target){
                    savingsOver100++;
                }
            }
        }

        return savingsOver100;
    }

}