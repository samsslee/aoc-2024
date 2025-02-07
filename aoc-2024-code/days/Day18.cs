using System;
using System.Linq;
using System.Net.Security;

public static class Day18{

    static Dictionary<int, (int, int)> directions = new(){{0,(-1,0)},{1,(0,1)},{2,(1,0)},{3,(0,-1)}};

    static int maxSize = 71; //7 for sample, 71 for actual.
    static int maxBytes = 1024; //12 for sample, 1024 for actual
    static List<List<char>> space = Enumerable.Range(0,maxSize)
    .Select(_ => Enumerable.Repeat('.', maxSize).ToList())
    .ToList();

    public static void Run(){

        Console.WriteLine("Running Day 18 solution...");
        string inputPath = @"inputs/day18.txt";
        string[] input = File.ReadAllLines(inputPath);

        for(int i = 0; i<maxBytes; i++){
            int[] line = input[i].Split(",").Select(int.Parse).ToArray();
            //Console.WriteLine("{0},{1}", line[0],line[1]);
            space[line[1]][line[0]] = '#';
        }
        //Console.WriteLine(string.Join(",", space[5]));

        //part 1
        List<Vertex> path = Walkmap();

        //prep for part 2
        HashSet<(int, int)> stepsTaken = convertPath(path);

        //part 2
        for(int i = maxBytes; i<input.Length; i++){
            //Console.WriteLine(i);
            int[] line = input[i].Split(",").Select(int.Parse).ToArray();
            space[line[1]][line[0]] = '#';
            //Console.WriteLine("add {0},{1}", line[1], line[0]);


            if (stepsTaken.Contains((line[1],line[0]))){
                //Console.WriteLine(i);
                path = Walkmap();

                if (path.Count == 0){
                    Console.WriteLine("x,y:{0},{1}", line[0], line[1]);
                    break;
                }

                stepsTaken = convertPath(path);
            }
        } 

    }

    private static HashSet<(int,int)> convertPath(List<Vertex> path){
        HashSet<(int,int)> steps = new();

        foreach(Vertex p in path){
            steps.Add((p.i, p.j));
        }

        return steps;
    }

    private static List<Vertex> Walkmap(){
        
        List<Vertex> visited = new();

        int minPath = int.MaxValue;

        PriorityQueue<List<Vertex>, int> queue = new(); //location, cost
        Dictionary<Vertex, int> stepsCost = new Dictionary<Vertex, int>();

        Vertex startRight = new Vertex(0,0,1);
        Vertex startDown = new Vertex(0,0,2);
        queue.Enqueue(new List<Vertex>(){startRight}, 0);
        queue.Enqueue(new List<Vertex>() {startDown}, 0);
        stepsCost[startRight] = 0;
        stepsCost[startDown] = 0;

        List<Vertex> currPath;
        int currCost;

        while (queue.TryDequeue(out currPath, out currCost)){
            Vertex currNode = currPath.Last();

            if (currNode.i == maxSize-1 && currNode.j == maxSize-1){
                visited = new List<Vertex>(currPath);
                minPath = currCost;
                break;
            }

            List<int> possibleDirs = findPaths(currNode.i, currNode.j, currNode.dir);
            foreach (int newdir in possibleDirs){
                var (di, dj) = directions[newdir];
                Vertex next = new Vertex(currNode.i+di, currNode.j+dj, newdir);
                
                if (!stepsCost.ContainsKey(next) || currCost+1 < stepsCost[next]){
                    // stepsCost[next] = currCost+1;
                    // //Console.WriteLine("{0},{1}={2}",next.i, next.j, currCost);
                    // queue.Enqueue(next, currCost+1); // Enqueue with priority (cost)

                    List<Vertex> copyList = new List<Vertex>(currPath);
                    copyList.Add(next);
                    stepsCost[next] = currCost+1;
                    queue.Enqueue(copyList, currCost+1); // Enqueue with priority (cost)
                } 
            }
        }

        Console.WriteLine("min Walk:" + minPath);
        return visited;

    }

    private static List<int> findPaths(int i, int j, int dir){

        List<int> dirs = new();
        if(i - 1 >= 0 && dir != 2 && space[i-1][j] != '#'){
            dirs.Add(0);
        }
        if(j + 1 < maxSize && dir != 3 && space[i][j+1] != '#'){
            dirs.Add(1);
        }
        if(i + 1 < maxSize && dir !=0 && space[i+1][j] != '#'){
            dirs.Add(2);
        }
        if(j - 1 >= 0 && dir != 1 && space[i][j-1] != '#'){
            dirs.Add(3);
        }
        return dirs;
    }

}

public class PathStep : IEquatable<PathStep>
{
    public int i;
    public int j;
    public int dir;

    //public List<PathStep> pastSteps { get; set; } = new();

    public PathStep(int ini, int inj, int d)
    {
        i = ini;
        j = inj;
        dir = d;
    }

    public bool Equals(PathStep? other)
    {
        if (other is null) return false;
        return this.i == other.i && 
            this.j == other.j && 
            this.dir == other.dir;
    }
    public override bool Equals(object? obj) => obj is PathStep v && Equals(v);

    public override int GetHashCode() => HashCode.Combine(i, j, dir);
}