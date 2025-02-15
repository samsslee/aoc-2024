using System;
using System.Linq;

public static class Day22{

    static List<long> secrets = new();
    static Dictionary<int, string> encode = new(){
        {0,"0"}, {1,"1"}, {2,"2"}, {3,"3"}, {4,"4"}, 
        {5,"5"}, {6,"6"}, {7,"7"}, {8,"8"}, {9,"9"},
            {-1,"A"}, {-2,"B"}, {-3,"C"}, {-4,"D"}, 
        {-5,"E"}, {-6,"F"}, {-7,"G"}, {-8,"H"}, {-9,"I"}, 
    };

    static Dictionary<string,int> firstEncounter = new();


    static Dictionary<long, long> memo = new();

    public static void Run(){
        Console.WriteLine("Running Day 22 solution...");
        string inputPath = @"inputs/day22.txt";
        StreamReader reader = new(inputPath);

        while(!reader.EndOfStream){
            secrets.Add(long.Parse(reader.ReadLine()));
        }
        reader.Close();

        Part2();

    }

    // private static void Part1(){
    //     long answer = 0;

    //     foreach (long secret in secrets){
    //         long s = secret;
    //         int nth = 2000;

    //         while(nth >0){
    //             s = Evolve(s);
    //             nth--;
    //         }
    //         answer +=s;
    //     }

    //     Console.WriteLine("answer part 1: "+ answer);
    // }


    private static void Part2(){
        //long answer = 0;

        foreach (long secret in secrets){
            long s = secret;
            int nth = 2000;
            Queue<int> last4 = new();
            HashSet<string> seenEncodesThisTime = new();

            while(nth >0){

                if(last4.Count==4){
                    last4.Dequeue();
                }
                long newS = Evolve(s);
                last4.Enqueue((int) newS % 10 - (int) s %10);
                if(last4.Count == 4){
                    List<string> list = last4.ToList().Select(x => encode[x]).ToList();
                    string encoded = string.Join("",list);

                    if(!seenEncodesThisTime.Contains(encoded)){
                        seenEncodesThisTime.Add(encoded);
                        firstEncounter[encoded] = firstEncounter.GetValueOrDefault(encoded, 0) + (int) newS % 10;

                    }
                }

                s = newS;
                nth--;
            }


            //answer +=s;
        }

        long max = firstEncounter.Values.Max();

        Console.WriteLine("answer part 2: " + max);
        //Console.WriteLine("answer part 1: "+ answer);
    }

    private static long Evolve(long secret){

        if (memo.TryGetValue(secret, out long cachedValue)){
            return cachedValue;
        }

        long s1 = step1(secret);
        long s2 = step2(s1);
        long new_Secret = step3(s2);

        memo[secret] = new_Secret;
        return new_Secret;
    }

    private static long step1(long secret){
        long new_Secret = mix(secret, secret*64);
        return prune(new_Secret);
    }

    private static long step2(long secret){
        long new_Secret = mix(secret, secret/32);
        return prune(new_Secret);
    }

    private static long step3(long secret){
        long new_Secret = mix(secret, secret*2048);
        return prune(new_Secret);
    }

    private static long mix(long s, long mixin){
        return mixin^s;
    }

    private static long prune(long s){
        return s % 16777216;
    }

}