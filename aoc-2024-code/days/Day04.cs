using System;

public static class Day04
{
    public static void Run()
    {
        Console.WriteLine("Running Day 03 solution...");
        string inputPath = @"inputs/day04.txt";
        string[] wordSearch = File.ReadAllLines(inputPath);
        // int xmasCount = Part1.searchXmas(wordSearch);
        // Console.WriteLine(xmasCount);

        int xmasCount2 = Part2.searchXmas(wordSearch);
        Console.WriteLine(xmasCount2);
    }
}

public static class Part1
{
    static int[,] directions = { {-1, -1}, {0, -1}, {1, -1}, {1, 0}, {1, 1}, {0, 1}, {-1, 1}, {-1, 0} };
    
    public static int searchXmas(string[] wordSearch)
    {
        int height = wordSearch.Length;
        int width = wordSearch[0].Length;
        int xmasCount = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (wordSearch[i][j] == 'X') // Fix: Compare with 'X' since we are dealing with characters
                {
                    for (int k = 0; k < directions.GetLength(0); k++) // Fix: Iterate over rows of directions
                    {
                        bool xmasFound = FollowDirection(wordSearch, i, j, directions[k, 0], directions[k, 1], height, width);
                        if (xmasFound)
                        {
                            xmasCount++;
                        }
                    }
                }
            }
        }

        return xmasCount; // Fix: Ensure return happens after all iterations
    }

    public static bool FollowDirection(string[] wordSearch, int cx, int cy, int dx, int dy, int height, int width)
    {
        // Check if there's enough space to follow that direction
        if (cx + 3 * dx >= height || cx + 3 * dx < 0 || cy + 3 * dy >= width || cy + 3 * dy < 0)
        {
            return false;
        }

        // Fix: Access characters correctly within the string
        if (wordSearch[cx + dx][cy + dy] == 'M' &&
            wordSearch[cx + 2 * dx][cy + 2 * dy] == 'A' &&
            wordSearch[cx + 3 * dx][cy + 3 * dy] == 'S')
        {
            return true;
        }

        return false;
    }
}

public static class Part2
{
    public static int searchXmas(string[] wordSearch)
    {
        int height = wordSearch.Length;
        int width = wordSearch[0].Length;
        int xmasCount = 0;
    

    for (int i = 1; i < height-1; i++)
        {
            for (int j = 1; j < width-1; j++)
            {
                if (wordSearch[i][j] == 'A') //look for the middle
                {
                    if(wordSearch[i-1][j-1].Equals('M')
                    && wordSearch[i+1][j-1].Equals('M')
                    && wordSearch[i+1][j+1].Equals('S')
                    && wordSearch[i-1][j+1].Equals('S'))
                    {
                        xmasCount++;
                    }
                    if(wordSearch[i-1][j-1].Equals('S')
                    && wordSearch[i+1][j-1].Equals('M')
                    && wordSearch[i+1][j+1].Equals('M')
                    && wordSearch[i-1][j+1].Equals('S'))
                    {
                        xmasCount++;
                    }
                    if(wordSearch[i-1][j-1].Equals('S')
                    && wordSearch[i+1][j-1].Equals('S')
                    && wordSearch[i+1][j+1].Equals('M')
                    && wordSearch[i-1][j+1].Equals('M'))
                    {
                        xmasCount++;
                    }
                    if(wordSearch[i-1][j-1] == 'M'
                    && wordSearch[i+1][j-1].Equals('S')
                    && wordSearch[i+1][j+1].Equals('S')
                    && wordSearch[i-1][j+1].Equals('M'))
                    {
                        xmasCount++;
                    }
                }
            }
        }

        return xmasCount; // Fix: Ensure return happens after all iterations

    }
}

