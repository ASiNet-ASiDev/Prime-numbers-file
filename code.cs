using System;

int j = 1_000_000_000;
var i = j / 256;
var offsStart = 0;
var offsEnd = i;
var locker = new object();

var path = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "result.txt");

Range[] ranges = new Range[256];

Console.WriteLine($"[{DateTime.Now:T}] => [START]");

for (int t = 0; t < ranges.Length; t++)
{
    var range = new Range(offsStart, offsEnd);
    ranges[t] = range;
    offsStart += i;
    offsEnd += i;
}


using (var stream = File.Create(path))
{
    using var writer = new StreamWriter(stream);
    Parallel.ForEach(ranges, r =>
    {
        Calc(r, writer, locker);
    });
}

Console.WriteLine($"[{DateTime.Now:T}] => [END]");
Console.WriteLine($"[{DateTime.Now:T}] => [FILE: {path}]");

Console.ReadKey();


void Calc(Range range, StreamWriter writer, object locker)
{
    for (int i = range.Start.Value; i < range.End.Value; i++)
    {
        if (IsSimp(i))
        {
            lock (locker)
            {
                writer.WriteLine(i);
            }
        }
    }
}


bool IsSimp(int n)
{
    for (int i = 2; i * i <= n; i++)
    {
        if (n % i == 0)
            return false;
        while (n % i == 0)
            n /= i;
    }
    return true;
}
