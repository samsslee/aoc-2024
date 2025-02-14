using System;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        // Call the specific day's solution directly
                // Start the stopwatch to measure runtime
        Stopwatch stopwatch = Stopwatch.StartNew();
        Day21.Run();
        stopwatch.Stop();
        Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
    }
}
