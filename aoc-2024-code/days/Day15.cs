using System;

public static class Day15{
    static int mapSize=0;

    static Dictionary<char, (int, int)> directions = new (){
        {'^', (-1,0)},{'>', (0,1)},{'v', (1,0)},{'<', (0,-1)}
    };

    static List<List<char>> map = new ();
    static List<List<char>> mapp2 = new ();
    static List<char> moves = new();
    static (int, int) starter;
    public static void Run(){

        Console.WriteLine("Running Day 15 solution...");
        string inputPath = @"inputs/day15.txt";
        StreamReader reader = new StreamReader(inputPath);


        while(true){
            mapSize++;
            string line = reader.ReadLine();
            if (line == ""){
                mapSize--;
                break;
            }
            if (line.Contains("@")){
                starter = (mapSize-1, line.IndexOf("@"));
            }
            map.Add(line.ToList());
        }

        while(!reader.EndOfStream){
            string line = reader.ReadLine();
            foreach(char c in line){
                moves.Add(c);
            }
        }
        //Console.WriteLine(Part1());
        Part2();
    }

    private static int Part2(){
    
        //reconstruct map
        for(int i = 0; i<mapSize; i++){
            mapp2.Add(new List<char>());
            for (int j=0; j<mapSize; j++){
                //Console.WriteLine(map[i][j]);
                switch(map[i][j]){
                    case '#':
                        mapp2[i].Add('#');
                        mapp2[i].Add('#');
                        break;
                    case '.':
                        mapp2[i].Add('.');
                        mapp2[i].Add('.');
                        break;
                    case '@':
                        mapp2[i].Add('@');
                        mapp2[i].Add('.');
                        starter = (i, j*2);
                        break;
                    case 'O':
                        mapp2[i].Add('[');
                        mapp2[i].Add(']');
                        break;
                }
            }
        }

        mapSize *=2;
        (int curri, int currj) = starter;
        Console.WriteLine(starter);

        foreach(char dir in moves){
            
            if (dir == '>' || dir == '<'){
                (curri, currj) = moveBoxSideways(curri, currj, dir);
            } else {
                (curri, currj) = moveBoxVertical(curri, currj, dir);
            }

            // foreach (var row in mapp2)
            // {
            //     Console.WriteLine(string.Join("", row));
            // }
        }

        int boxPosSum = sumBoxesp2();
        Console.WriteLine(boxPosSum);

        return boxPosSum;
    }

    private static (int, int) moveBoxVertical(int i, int j, char dir){
        if (!canMoveVertical(i,j,dir)){
            return (i,j);
        } else {
            moveBoxVert(i, j, dir);
        }
        
        (int di, int dj) = directions[dir];
        return (i+di, j+dj);
    }

    private static void moveBoxVert(int i, int j, char dir){
        (int di, int dj) = directions[dir];

        int newi = i + di;
        int newj = j + dj;

        if(mapp2[newi][newj] == '['){
            moveBoxVert(newi, newj, dir);
            moveBoxVert(newi, newj+1, dir);
        }
        else if(mapp2[newi][newj] == ']'){
            moveBoxVert(newi, newj, dir);
            moveBoxVert(newi, newj-1, dir);
        }

        if(mapp2[newi][newj] == '.'){
            mapp2[newi][newj] = mapp2[i][j];
            mapp2[i][j] = '.';
        } else {
            //Console.WriteLine("error: {0}", mapp2[newi][newj]);
        }

    }

    private static bool canMoveVertical(int i, int j, char dir)
    {
        (int di, int dj) = directions[dir];
        int newi = i + di;
        int newj = j + dj;

        if (newi < 0 || newi >= mapSize/2 || newj < 0 || newj >= mapSize )
        {
            return false;
        }

        if (mapp2[newi][newj] == '#')
        {
            return false;
        }
        if (mapp2[newi][newj] == '[')
        {
            return canMoveVertical(newi, newj, dir) && canMoveVertical(newi, newj+1, dir);
        }
        else if (mapp2[newi][newj] == ']')
        {
            return canMoveVertical(newi, newj, dir) && canMoveVertical(newi, newj-1, dir);
        }
        return true; //if it's a '.'
    }

    private static (int, int) moveBoxSideways(int i, int j, char dir){
        (int di, int dj) = directions[dir];

        int newi = i + di;
        int newj = j + dj;

        if (newi < 0 || newi >= mapSize/2 || newj < 0 || newj >= mapSize)
        {
            return (i, j);
        }

        if(mapp2[newi][newj] == '#'){
            return (i, j);
        } 
        if(mapp2[newi][newj] == '[' || mapp2[newi][newj] == ']' ){
            (int finali, int finalj) = moveBoxSideways(newi, newj, dir);
            if ((finali, finalj) == (newi, newj)) {
                return (i,j);
            } //if you didnt end up moving it
        }
        if(mapp2[newi][newj] == '.'){
            mapp2[newi][newj] = mapp2[i][j];
            mapp2[i][j] = '.';
        }
        return (newi, newj);

    }

    private static int sumBoxesp2(){

        int total = 0;
        for (int i = 0; i<mapSize/2; i++){
            for (int j = 0; j<mapSize; j++){
                if (mapp2[i][j] == '['){
                    total+= i*100+j;
                }
            }
        }
        return total;
    }




    private static int Part1(){
        (int curri, int currj) = starter;

        foreach(char dir in moves){
            (curri, currj) = moveBox(curri, currj, dir);
        }
        int boxPosSum = sumBoxes();
        return boxPosSum;
    }


    private static (int, int) moveBox(int i, int j, char dir){

        (int di, int dj) = directions[dir];

        int newi = i + di;
        int newj = j + dj;

        if (newi < 0 || newi >= mapSize || newj < 0 || newj >= mapSize)
        {
            return (i, j);
        }

        if(map[newi][newj] == '#'){
            return (i, j);
        } 
        if(map[newi][newj] == 'O'){
            (int finali, int finalj) = moveBox(newi, newj, dir);
            if ((finali, finalj) == (newi, newj)) {
                return (i,j);
            } //if you didnt end up moving it
        }
        if(map[newi][newj] == '.'){
            map[newi][newj] = map[i][j];
            map[i][j] = '.';
        }
        return (newi, newj);

    }

    private static int sumBoxes(){

        int total = 0;
        for (int i = 0; i<mapSize; i++){
            for (int j = 0; j<mapSize; j++){
                if (map[i][j] == 'O'){
                    total+= i*100+j;
                }
            }
        }
        return total;
    }





}