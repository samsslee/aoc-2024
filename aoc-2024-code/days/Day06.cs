// using System;
// using System.IO;
// using System.Collections;

// public static class Day06
// {

//     private static int outbound = 0;
//     private static string[] floorplan;
//     private static int currDir = 0;
//     private static Dictionary<int, int[]> directions = new Dictionary<int, int[]>
//     {
//         { 0, new int[] { -1, 0 } }, // "Up" direction
//         { 1, new int[] { 0, 1 } },  // "Right" direction
//         { 2, new int[] { 1, 0 } },
//         { 3, new int[] { 0, -1 }}
//     };
//     private static HashSet<(int,int)> spots = new HashSet<(int, int)>();
//     private static (int, int) start;

//     public static void Run()
//     {
//         Console.WriteLine("Running Day 06 solution...");
//         string inputPath = @"inputs/day06.txt";
//         floorplan = File.ReadAllLines(inputPath);
//         List<List<char>> writableFloorplan = floorplan
//             .Select(line => line.ToList())
//             .ToList();
        

//         //find carat
//         int i = 0;
//         int j = 0;
//         for (i = 0; i<floorplan.Length; i++){
//             if (floorplan[i].Contains('^'))
//             {
//                 j = floorplan[i].IndexOf('^');
//                 break;
//             }
//         }
//         //save the starting coordinates first
//         start = (i,j);

//         //part 1
//         while (InBounds(i, j)){
//             spots.Add((i,j));
//             (i, j) = NextStep(i, j, writableFloorplan);
//         }

//         Console.WriteLine(spots.Count);
//         spots.Remove(start); //we don't put hash on the first on in the path so it's not one of the spots to test
//         Console.WriteLine("obstacles placed:"+ Part2(writableFloorplan, floorplan[0].Length));

//     }

//     private static int Part2(List<List<char>> wfp, int width){

//         // Console.WriteLine(start);
//         int obstacleCount = 0;


//         foreach ((int x, int y) in spots){
//             int i = start.Item1;
//             int j = start.Item2;
//             currDir = 0;
//             wfp[x][y] = '#';
//             List<int> visited = new List<int>();

//             while(InBounds(i,j)){
//                 int loc = ((i * width) + j)*4+currDir;
//                 if (visited.Contains(loc)){
//                     obstacleCount++;
//                     break;
//                 } 
//                 visited.Add(loc);

//                 (i,j) = NextStep(i, j, wfp);
//             }

//             wfp[x][y] = '.';

//         }
    
//         Console.WriteLine(outbound);
//         return obstacleCount;
//     }

//     private static (int, int) NextStep(int i, int j, List<List<char>> fp)
//     {
//         int nexti = i+directions[currDir][0];
//         int nextj = j+directions[currDir][1];

//         if (!InBounds(nexti, nextj))
//         {
//             ++outbound;
//             return (nexti, nextj);
//         } else {
            
//             while (fp[nexti][nextj] == '#')
//             {
//                 currDir = (currDir+1) % 4;
//                 nexti = i+directions[currDir][0];
//                 nextj = j+directions[currDir][1];
//             }
//             return (nexti, nextj);
//         }

//     }

//     private static bool InBounds(int i, int j){
//         return i >= 0 && i < floorplan.Length && j >= 0 && j < floorplan[0].Length;
//     }


// }