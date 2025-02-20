// using System;
// using System.Text.RegularExpressions;
// using System.Linq;

// public static class Day23{

//     static Dictionary<string, HashSet<string>> nodes = new();
//     static HashSet<string> triplets = new();

//     static string inputPath = @"inputs/day23.txt";

//     public static void Run(){
//             Console.WriteLine("Running Day 23 solution...");
//             StreamReader reader = new(inputPath);

//             while(!reader.EndOfStream){
//                 string[] temp = reader.ReadLine().Split('-');
//                 if (!nodes.ContainsKey(temp[0])) {
//                     nodes[temp[0]] = new HashSet<string>();
//                 }
//                 nodes[temp[0]].Add(temp[1]);
//                 if (!nodes.ContainsKey(temp[1])) {
//                     nodes[temp[1]] = new HashSet<string>();
//                 }
//                 nodes[temp[1]].Add(temp[0]);
                
//             }
//             reader.Close();

//             //Part1();
//             Part2();
//     }

//     private static void Part1(){
            
//             StreamReader reader = new(inputPath);

//             while(!reader.EndOfStream){
//                 string[] temp = reader.ReadLine().Split('-');


//                 if (Regex.IsMatch(temp[0], @"^t") || Regex.IsMatch(temp[1], @"^t")) { //i don't think i even need the or
//                     HashSet<string> intersecting = new HashSet<string>(nodes[temp[0]]);
//                     intersecting.IntersectWith(nodes[temp[1]]);

//                     foreach (string i in intersecting){
//                         List<string> tempList = new List<string>(){temp[0], temp[1], i};
//                         tempList.Sort();
//                         string newTriplet = string.Join(",", tempList);
//                         triplets.Add(newTriplet);
//                     }
//                 }
                
//             }
//             reader.Close();

//             Console.WriteLine("Part 1: " + triplets.Count);

//     }

//     private static void Part2(){
//         HashSet<string> R = new();
//         HashSet<string> P = new HashSet<string>(nodes.Keys);  // Start with all nodes in P
//         HashSet<string> X = new();
//         HashSet<HashSet<string>> cliques = bronKerbosch(R, P, X);
//         long maxSize = 0;
//         List<string> members = new();

//         // Print the size of each maximal clique
//         foreach (var clique in cliques)
//         {
//             if (clique.Count > maxSize){
//                 maxSize = clique.Count;
//                 members = clique.ToList();
//             }
//         }

//         members.Sort();
//         Console.WriteLine("Part 2: " + maxSize);
//         Console.WriteLine(string.Join(",", members));
//     }


//     private static HashSet<HashSet<string>> bronKerbosch(HashSet<string> R, HashSet<string> P, HashSet<string> X) {
    
//         HashSet<HashSet<string>> cliques = new();
//         if (P.Count == 0 && X.Count ==0){
//             cliques.Add(new HashSet<string>(R));
//         }
    
//         foreach (string v in P.ToList()){
//             HashSet<string> newR = [.. R, v];
//             HashSet<string> newP = new HashSet<string>(P.Where(x =>nodes[v].Contains(x)));
//             HashSet<string> newX = new HashSet<string>(P.Where(x =>nodes[v].Contains(x)));
//             // Recursively find cliques in the new sets
//             var newCliques = bronKerbosch(newR, newP, newX);
            
//             // Merge the new cliques into the result set
//             foreach (var clique in newCliques)
//             {
//                 cliques.Add(clique);
//             }
//             P.Remove(v);
//             X.Add(v);

//         }
//         return cliques;
//     }


// }