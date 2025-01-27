using System;
using System.Collections.Generic;

public static class Day09p2
{

    static List<Fragment> fragmentList = new List<Fragment>();

    public static void Run()
    {
        Console.WriteLine("Running Day 09 solution...");
        string inputPath = @"inputs/day09.txt";
        StreamReader reader = new StreamReader(inputPath);

        string? input = reader.ReadLine();
        reader.Close();
        if (input != null){
            fragmentList = MakeFragmentList(input);
        }
        Console.WriteLine(Part2());
    }

    private static List<Fragment> MakeFragmentList(string input) {

        List<Fragment> fragments = new List<Fragment>();
        int grandIndex = 0;

        for (int i = 0; i<input.Length; i++){
            int boxValue = i % 2 == 0 ? i/2 : 0;
            int spacesLeft = i % 2 == 0 ? 0 : (input[i]-'0');
            fragments.Add(new Fragment(input[i]-'0', grandIndex, boxValue, spacesLeft));
            // Console.WriteLine(fragments[i].StartingIndex());
            // Console.WriteLine(fragments.Count);
            grandIndex += input[i]-'0';

        }

        return fragments;
    }

    private static long Part2(){
        ShuffleNumbers();
        return TallyCheckSum();
    }

    private static void ShuffleNumbers(){
        for(int i = fragmentList.Count-1; i>=0; i-=2){
            // Console.WriteLine(fragmentList[i].TotalSpots());
            //starting at the back of the list, we see if we want to shuffle it
            for(int j = 1; j<i; j+=2){ //this is ok because you can't ever move to a space that was created
                //if there's enough available space somewhere to the left
                if(fragmentList[j].AvailSpace() >= fragmentList[i].TotalSpots()){
                    fragmentList[i].MoveBoxes(fragmentList[j].StartingIndex()); //move the box to that starting index;
                    fragmentList[j].ModifySpace(fragmentList[i].TotalSpots()); //shrink the space by the amount you used
                    break;
                }

            }

        }

    }

    private static long TallyCheckSum(){
        long checkSum = 0;
        for (int i = 0; i<fragmentList.Count; i++){
            checkSum += fragmentList[i].CheckSum();
        }
        return checkSum;
    }

}

public class Fragment
{

    int totalSpots; //total spots
    int startingIndex;
    int boxValue;
    int availSpace;
    public Fragment(int t, int si, int bv, int sl)
    {
        totalSpots = t;
        startingIndex = si;
        boxValue = bv;
        availSpace = sl;
    }

    public int TotalSpots(){
        return totalSpots;
    }
    public int AvailSpace(){
        return availSpace;
    }
    public int StartingIndex(){
        return startingIndex;
    }

    public void ModifySpace(int used){ //before modifying space in the left one
        availSpace -= used;
        startingIndex += used;
    }

    public void MoveBoxes(int newStartingIndex){ //move box first for the right box
        startingIndex = newStartingIndex;
    }

    public int CheckSum(){
        int totalValue = 0;
        if(boxValue != 0){ //speeds up the proces for the ones that are index 0 or spaces or no-longer-existant spaces
            for (int i = 0; i<totalSpots; i++){
                totalValue+=boxValue*(startingIndex+i);
            }
        }
        // Console.WriteLine("Values: {0} {1} {2}",boxValue,startingIndex,totalValue);
        return totalValue;
    }

}