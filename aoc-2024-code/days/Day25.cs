using System;

public static class Day25{

    static Dictionary< int, int[] > KeySpaces = new();
    static Dictionary< int, int[] > Locks = new();

    public static void Run(){

        Console.WriteLine("Running Day 25 solution...");
        string inputPath = @"inputs/day25.txt";
        StreamReader reader = new(inputPath);

        int KCount = 0;
        int LCount = 0;

        while(!reader.EndOfStream){

            while(true){
                string line = reader.ReadLine();
                if(line == ""){
                    break;
                }
                if(line == "....."){ //then it's a key
                    KeySpaces[KCount] = [0,0,0,0,0];
                    for(int i = 0; i<6; i++){
                        string ks = reader.ReadLine();
                        for (int j = 0; j<5; j++){
                            if(ks[j] == '.'){
                                KeySpaces[KCount][j]++;
                            }
                        }    
                    }
                    KCount++;
                    break;
                } else if (line == "#####"){
                    Locks[LCount] = [0,0,0,0,0];
                    for(int i = 0; i<6; i++){
                        string locks = reader.ReadLine();
                        for (int j = 0; j<5; j++){
                            if(locks[j] == '#'){
                                Locks[LCount][j]++;
                            }
                        }    
                    }
                    LCount++;
                    break;
                }
                
            }

        }
        int valid = 0;

        foreach( var locks in Locks){
            Console.WriteLine($"Locks: {locks.Key}, Values: [{string.Join(", ", locks.Value)}]");
            foreach (var ks in KeySpaces){
                bool isValid = true;
                for (int i = 0; i<5; i++){
                    if(locks.Value[i]>ks.Value[i]){
                        isValid = false;
                        break;
                    }
                }
                if(isValid){
                    valid++;
                }
            }
        }

        Console.WriteLine("day 25: " + valid);

        


    }


}