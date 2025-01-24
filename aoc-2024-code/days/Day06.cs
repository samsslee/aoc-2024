using System;
using System.IO;
using System.Collections;

public static class Day06
{

    private static string[] floorplan;
    private static int currDir = 0;
        private static Dictionary<int, int[]> directions = new Dictionary<int, int[]>
    {
        { 0, new int[] { -1, 0 } }, // "Up" direction
        { 1, new int[] { 0, 1 } },  // "Right" direction
        { 2, new int[] { 1, 0 } },
        { 3, new int[] { 0, -1 }}
    };
    private static HashSet<(int,int)> spots = new HashSet<(int, int)>();
    private static (int, int) start;

    public static void Run()
    {
        Console.WriteLine("Running Day 06 solution...");
        string inputPath = @"inputs/day06.txt";
        floorplan = File.ReadAllLines(inputPath);
        List<List<char>> writableFloorplan = floorplan
            .Select(line => line.ToList())
            .ToList();
        

        //find carat
        int i = 0;
        int j = 0;
        for (i = 0; i<floorplan.Length; i++){
            if (floorplan[i].Contains('^'))
            {
                j = floorplan[i].IndexOf('^');
                break;
            }
        }
        //save the starting coordinates first
        start = (i,j);

        //part 1
        while (InBounds(i, j)){
            spots.Add((i,j));
            (i, j) = NextStep(i, j, writableFloorplan);
        }

        Console.WriteLine(spots.Count);
        spots.Remove(start); //we don't put hash on the first on in the path so it's not one of the spots to test
        Console.WriteLine("obstacles placed:"+ Part2(writableFloorplan, floorplan[0].Length));

    }

    private static int Part2(List<List<char>> wfp, int width){

        // Console.WriteLine(start);
        int obstacleCount = 0;
        //for every spot in the path, change the . to a #
            //List<int> visited = new List<int>();
            //start a counter for how many numbers are added to the visited list
            //flag the cycle found as false
            //while it is in bounds, add to the visited, increment the counter, and go to next step
                //every 10 visited try detect cycle
                //if detect cycle finds there is a cycle, mark cycle found as true
                //increment obstacleCount
                //break the while loop
            //change the # back to a .
        //

        foreach ((int x, int y) in spots){
            int i = start.Item1;
            int j = start.Item2;
            currDir = 0;
            wfp[x][y] = '#';
            //Console.WriteLine("New Placement:"+ x+","+y);
            List<int> visited = new List<int>();
            while(InBounds(i,j)){
                int loc = (i * width) + j;
                visited.Add(loc);
                if (visited.Count % 10 == 8){
                    //Console.WriteLine("trying:" +x+","+y);

                    if(DetectCycle(visited)){
                        //Console.WriteLine("\n Success at:"+x+","+y+"\n");
                        
                        obstacleCount++;
                        break;
                    };
                }
                (i,j) = NextStep(i, j, wfp);
                //Console.Write("["+i+","+j+"], ");
            }
            wfp[x][y] = '.';
            //Console.WriteLine("\n==========\n");

        }
    

        return obstacleCount;
    }

    private static bool DetectCycle (List<int> visited)
    {
        int length = visited.Count;
        int minimumCycleLength = 4; //technically this is the shorted possible cycle

        // Only start checking for cycles if the list is large enough
        if (length < 2 * minimumCycleLength)
        {
            throw new Exception("cycle length too short");
        }

        for (int cycleLength = minimumCycleLength; cycleLength<= length/2; cycleLength++)
        {
            bool isCycle = true;
            for (int i = 0; i<cycleLength;i++){
            if (visited[length - 1 - i] != visited[length - 1 - i - cycleLength])
                {
                    isCycle = false;
                    break;
                }                
            }
            if (isCycle)
            {
                // Console.WriteLine($"Cycle detected with length {cycleLength}:");
                // for (int i = 0; i < cycleLength; i++)
                // {
                //     Console.Write(visited[length - cycleLength + i] + " ");
                // }
                // Console.WriteLine();
                
                
                return true;
            }
        }
        return false;
    }

    private static (int, int) NextStep(int i, int j, List<List<char>> floorplan)
    {
        int nexti = i+directions[currDir][0];
        int nextj = j+directions[currDir][1];
        if (!InBounds(nexti, nextj))
        {
            return (nexti, nextj);
        } else {

            if (floorplan[nexti][nextj] == '#')
            {
                currDir = (currDir+1) % 4;
                nexti = i+directions[currDir][0];
                nextj = j+directions[currDir][1];
            }
            return (nexti, nextj);
        }

    }

    private static bool InBounds(int i, int j){
        bool result = i >= 0 && i < floorplan.Length && j >= 0 && j < floorplan[0].Length;
        return result;
    }


}