using System;
using System.Text.RegularExpressions;
using System.Linq;


public static class Day17{

    public static void Run(){
        Console.WriteLine("Running Day 17 solution...");
        string inputPath = @"inputs/day17.txt";

        string[] rawInput = File.ReadAllLines(inputPath);
        string pattern = @"\d+";
        List<long> numbers = new();

        foreach(string line in rawInput){
            foreach (Match match in Regex.Matches(line, pattern))
            {
                numbers.Add(long.Parse(match.Value));
            }
        }

        Operation ops = new Operation(numbers);
        
        //part 1
        Console.WriteLine(ops.RunProgram());

        //part 2
        ops.RunPart2();

    }

}

public class Operation{

    private long regA {get; set;}
    private long regB {get; set;}
    private long regC {get; set;}
    List<long> program;
    private long pointer = 0;
    public List<long> output = new();

    public Operation(List<long>nums){
        regA = nums[0];
        regB = nums[1];
        regC = nums[2];

        program = nums.Skip(3).Select(x => x).ToList();
    }
    
    public string RunProgram(){
        while (pointer < program.Count-1){
            PickProgram(program[(int)pointer], program[(int)pointer+1]);
        }
        return string.Join(",", output.ToArray());
    }

    public void RunPart2(){
        List<long> found = FindGoalRegA(1,0);
        found.Sort();

        Console.WriteLine("ans: {0}", found[0]);
        //Console.WriteLine(string.Join(", ",found));
    }

    public List<long> FindGoalRegA(long curr, int digits){

        //generate the next ones to look for 
        List<long> nextBaselines = LookNext8(curr, program[program.Count-digits-1]);
        List<long> results = new List<long>();

        foreach(long baseline in nextBaselines){
            if(digits >= program.Count-1){
                return new List<long>() { baseline };
            }
            results.AddRange(FindGoalRegA(baseline*8, digits+1));
        }
        return results;
    }

    private List<long> LookNext8(long guess, long goal){

        List<long> baselines = new();

        for (long i = guess; i<guess+8; i++){
            ResetRegs(i);
            RunProgram();

            if (output[0] == goal){
                baselines.Add(i);
            }
        }
        return baselines;
    }


    private void ResetRegs(long a){
        regA = a;
        regB = 0;
        regC = 0;
        pointer = 0;
        output = new List<long>();
    }


    private long Combo(long opcode){
        if(opcode <= 3){return opcode;}
        else if(opcode == 4){return regA;}
        else if (opcode == 5){return regB;}
        else if (opcode == 6){return regC;}
        else {throw new Exception("combo issue");}
    }


    private void PickProgram(long opcode, long operand){
        switch (opcode)
        {
            case 0:
                P0(operand);
                break;
            case 1:
                P1(operand);
                break;
            case 2:
                P2(operand);
                break;
            case 3:
                P3(operand);
                break;
            case 4:
                P4();
                break;
            case 5:
                P5(operand);
                break;
            case 6:
                P6(operand);
                break;
            case 7:
                P7(operand);
                break;
            default:
                Console.WriteLine("what the heck");
                break;
        }
    }

    private void P0(long operand){
        regA /= (long) Math.Pow(2,Combo(operand));
        pointer+=2;
    }

    private void P1(long operand){
        regB ^= operand;
        pointer+=2;
    }

    private void P2(long operand){
        regB = Combo(operand) % 8;
        //Console.WriteLine(regB);
        pointer+=2;
    }

    private void P3(long operand){
        if (regA != 0){
            pointer = operand;
        } else {
            pointer+=2;
        }
    }

    private void P4(){
        regB = regB^regC;
        pointer+=2;
    }

    private void P5(long operand){
        output.Add(Combo(operand) % 8);
        pointer+=2;
    }
    private void P6(long operand){
        regB = regA/(long) Math.Pow(2,Combo(operand));
        pointer+=2;
    }
    private void P7(long operand){
        regC = regA/(long) Math.Pow(2,Combo(operand));
        pointer+=2;
    }


}