// using System;
// using System.IO;
// using System.Collections.Generic;
// using System.Net.NetworkInformation;

// public static class Day05
// {
//     // Declare class-level static fields
//     private static HashSet<string> sets = new HashSet<string>();
//     private static List<string> updates = new List<string>();

//     public static void Run()
//     {
//         Console.WriteLine("Running Day 05 solution...");
//         string inputPath = @"inputs/day05.txt";
//         StreamReader reader = new StreamReader(inputPath);

//         while (true)
//         {
//             string line = reader.ReadLine();
            
//             if (string.IsNullOrEmpty(line)) // Better check for null or empty
//             {
//                 break;
//             }
//             sets.Add(line!);
//         }

//         while (!reader.EndOfStream)
//         {
//             string line = reader.ReadLine();
//             updates.Add(line!);
//         }
//         reader.Close();

//         //Console.WriteLine(Part1());
//         Console.WriteLine(Part2());
//     }

//     private static int Part1()
//     {
//         // Access the static fields directly
//         // Console.WriteLine(string.Join(", ", sets));
//         // Console.WriteLine(string.Join(", ", updates));
//         int midSums = 0;

//         foreach (string update in updates){
//             string [] numbers = update.Split(",");
//             //Console.WriteLine(update);
//             if (isValid(numbers)){
//                 midSums +=FindMiddleInt(numbers);
//             }
//         }

//         return midSums;
//     }

//     private static bool flag = false;

//     private static int Part2()
//     {
//         int midSums = 0;

//         foreach (string update in updates){
//             string [] numbers = update.Split(",");
//             //Console.WriteLine(update);
//             if (!isValid(numbers)){
//                 flag = false;
//                 while (!flag)
//                 {
//                     numbers = MakeValid(numbers);
//                 }
//                 midSums += FindMiddleInt(numbers);
//             }
//         }

//         return midSums;

//     }

//     private static string[] MakeValid(string[] numbers){
//         string[] deepCopy = (string[]) numbers.Clone();
//         for (int i = 0; i<numbers.Length-1; i++){
//             for (int j = i+1; j<numbers.Length; j++){
//                 if (sets.Contains(numbers[j]+"|"+numbers[i])){
//                     deepCopy = doSwap(numbers, j, i);
//                     return deepCopy;
//                 }
//             }

//         }
//         flag = true;
//         return deepCopy;
//     }
//     private static string[] doSwap(string[] numbers, int j, int i){
//         string[] deepCopy = (string[]) numbers.Clone();
//         deepCopy[i] = numbers[j];
//         deepCopy[j] = numbers[i];
//         return deepCopy;
//     }



//     private static bool isValid(string[] numbers){

//         for (int i = 0; i<numbers.Length-1; i++){
//             for (int j = i+1; j<numbers.Length; j++){
//                 if (!sets.Contains(numbers[i]+"|"+numbers[j])){
//                     //Console.WriteLine(numbers[i]+"|"+numbers[j]);
//                     return false;
//                 }
//             }

//         }
//         return true;

//     }

//     private static int FindMiddleInt(string [] numbers){

//         int middle = (int) Math.Floor(numbers.Length/2.0);
//         //Console.WriteLine(numbers[middle]);
//         return int.Parse(numbers[middle]);
//     }
// }
