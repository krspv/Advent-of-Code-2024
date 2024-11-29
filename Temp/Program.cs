// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Stopwatch stopwatch = new();

stopwatch.Start();
Thread.Sleep(100);
stopwatch.Stop();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\nПрв дел:");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("  Решение");
Console.ResetColor();
Console.WriteLine($"Време на извршување: {stopwatch.ElapsedMilliseconds}ms");


stopwatch.Restart();
Thread.Sleep(300);
stopwatch.Stop();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\n\nВтор дел:");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("  Решение");
Console.ResetColor();
Console.WriteLine($"Време на извршување: {stopwatch.ElapsedMilliseconds}ms");

Console.WriteLine("\n\n\n\n");
