using System;
using System.Runtime.CompilerServices;

public static class Day16{

    static Dictionary<int, (int, int)> directions = new(){{0,(-1,0)},{1,(0,1)},{2,(1,0)},{3,(0,-1)}};
    static Dictionary<Vertex, int> visitedCost = new Dictionary<Vertex, int>();
    //(location, direction), cost

    static int width;
    static int height;
    static List<List<char>> input = new(); 


    public static void Run(){
        Console.WriteLine("Running Day 16 solution...");
        string inputPath = @"inputs/day16.txt";

        var rawInput = File.ReadAllLines(inputPath);
        height = rawInput.Length;
        width = rawInput[0].Length;
        input = rawInput.Select(line => line.ToList()).ToList();

        HashSet<Vertex>minPaths = Part1();
        Console.WriteLine(Part2(minPaths));

    }

    private static int Part2(HashSet<Vertex> minPaths){
        HashSet<(int, int)> seats = new();
        foreach(Vertex v in minPaths){
            seats.Add((v.i,v.j));
        }

        return seats.Count;
    }

    private static HashSet<Vertex> Part1(){
        PriorityQueue<List<Vertex>, int> queue = new();
        Vertex start = new Vertex(height-2, 1, 1);
        queue.Enqueue(new List<Vertex>(){start}, 0);
        visitedCost[start] = 0;
        HashSet<Vertex> minPaths = new();

        int allMins = int.MaxValue;
        List<Vertex> currentPath;
        int currentCost;

        while (queue.TryDequeue(out currentPath, out currentCost)){
            Vertex current = currentPath.Last();

            if (input[current.i][current.j] == 'E'){

                // If it's the first end we encounter, store the cost
                if (allMins == int.MaxValue) {
                    allMins = visitedCost[current];
                    minPaths.UnionWith(currentPath);

                } else if (visitedCost[current] == allMins) {
                    // If the cost matches the minimum cost found, add it
                    minPaths.UnionWith(currentPath);
                }
                // If the cost exceeds the current minimum, break the loop as we don't need higher-cost paths
                else if (visitedCost[current] > allMins) {
                    break;
                }
            }
            List<int> possibleDirs = findPaths(current.i, current.j, current.dir);
            foreach (int newdir in possibleDirs){
                var (di, dj) = directions[newdir];
                Vertex next = new Vertex(current.i+di, current.j+dj, newdir);

                if (next.i <= 0 || next.i >= height-1 || next.j <= 0 || next.j >= width-1) continue;

                int newcost = currentCost + (newdir == current.dir ? 1 : 1001);
                
                if (!visitedCost.ContainsKey(next) || newcost <= visitedCost[next]){
                    List<Vertex> copyList = new List<Vertex>(currentPath);
                    copyList.Add(next);

                    visitedCost[next] = newcost;
                    queue.Enqueue(copyList, newcost); // Enqueue with priority (cost)
                } 

            }

        }
        return minPaths;
    }


    private static List<int> findPaths(int i, int j, int dir){
        List<int> dirs = new();
        if(dir != 2 && input[i-1][j] != '#'){
            dirs.Add(0);
        }
        if(dir != 3 && input[i][j+1] != '#'){
            dirs.Add(1);
        }
        if(dir !=0 && input[i+1][j] != '#'){
            dirs.Add(2);
        }
        if(dir != 1 && input[i][j-1] != '#'){
            dirs.Add(3);
        }
        return dirs;
    }

}

public class Vertex : IEquatable<Vertex>
{
    public int i;
    public int j;
    public int dir;

    //public List<Vertex> pastSteps { get; set; } = new();

    public Vertex(int ini, int inj, int d)
    {
        i = ini;
        j = inj;
        dir = d;
    }

    public bool Equals(Vertex? other)
    {
        if (other is null) return false;
        return this.i == other.i && 
            this.j == other.j && 
            this.dir == other.dir;
    }
    public override bool Equals(object? obj) => obj is Vertex v && Equals(v);

    public override int GetHashCode() => HashCode.Combine(i, j, dir);
}