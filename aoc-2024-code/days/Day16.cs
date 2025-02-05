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

        List<Vertex>minPaths = Part1();
        Console.WriteLine(Part2(minPaths));

    }

    private static int Part2(List<Vertex> minPaths){
        HashSet<(int, int)> seats = new();

        foreach (Vertex path in minPaths){
            Vertex n = path;
            do {
                seats.Add((n.i, n.j));  // Use `n` instead of `path`
                n = n.parent;  // Move to the parent
            } while (n != null);  // Continue as long as there is a valid parent
        }
        return seats.Count;
    }

    private static List<Vertex> Part1(){
        PriorityQueue<Vertex, int> queue = new();
        Vertex start = new Vertex(height-2, 1, 1);
        queue.Enqueue(start, 0);
        visitedCost[start] = 0;
        List<Vertex> minPaths = new();

        int allMins = int.MaxValue;

        while (queue.Count >0){
            Vertex current = queue.Dequeue();

            if (input[current.i][current.j] == 'E'){

                // Console.WriteLine(current.i);
                // Console.WriteLine(current.j);
                // Console.WriteLine(visitedCost[current]);

                // If it's the first end we encounter, store the cost
                if (allMins == int.MaxValue) {
                    allMins = visitedCost[current];
                    minPaths.Add(current); // Add the first path with minimum cost
                } else if (visitedCost[current] == allMins) {
                    // If the cost matches the minimum cost found, add it
                    minPaths.Add(current);
                }
                // If the cost exceeds the current minimum, break the loop as we don't need higher-cost paths
                else if (visitedCost[current] > allMins) {
                    Console.WriteLine("hey");
                    break;
                }
            }
            List<int> possibleDirs = findPaths(current.i, current.j, current.dir);
            foreach (int newdir in possibleDirs){
                var (di, dj) = directions[newdir];
                Vertex next = new Vertex(current.i+di, current.j+dj, newdir, current);

                if (next.i < 0 || next.i >= height || next.j < 0 || next.j >= width) continue;

                //part 1
                int newcost = visitedCost[current] + (newdir == current.dir ? 1 : 1001);
                
                if (!visitedCost.ContainsKey(next) || newcost < visitedCost[next]){
                    visitedCost[next] = newcost;

                    queue.Enqueue(next, newcost); // Enqueue with priority (cost)
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
    public Vertex? parent;

    public Vertex(int ini, int inj, int d, Vertex? p=null)
    {
        i = ini;
        j = inj;
        dir = d;
        parent = p;
    }

    public bool Equals(Vertex? other)
    {
        if (other is null) return false;
        return this.i == other.i && 
            this.j == other.j && 
            this.dir == other.dir && 
            EqualityComparer<Vertex?>.Default.Equals(this.parent, other.parent);
    }

    public override bool Equals(object? obj) => obj is Vertex v && Equals(v);

    public override int GetHashCode() => HashCode.Combine(i, j, dir);
}