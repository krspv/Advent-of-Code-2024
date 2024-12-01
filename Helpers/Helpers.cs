using System.Globalization;





namespace Helpers {
  public class PrintHelper {
    public static void ПечатиНаслов(int ден, string име) {
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write($"Ден {ден.ToString("D2")}: ");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine(име);
    }

    public static void ПечатиПрвДел<T>(long милисекунди, T решение) {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("\nПрв дел:");
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine($"  {решение}");
      Console.ResetColor();
      Console.WriteLine($"Време на извршување: {милисекунди.ToString("#,0", new CultureInfo("mk"))} ms");
    }

    public static void ПечатиВторДел<T>(long милисекунди, T решение) {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("\n\nВтор дел:");
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine($"  {решение}");
      Console.ResetColor();
      Console.WriteLine($"Време на извршување: {милисекунди.ToString("#,0", new CultureInfo("mk"))} ms");
    }

    public static void ПечатиЕлкаЗаКрај() {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(@"                                                                         *");
      Console.ForegroundColor = ConsoleColor.Green;
      Console.Write(@"                                                                        /");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.Write(@"x");
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(@"\");
      Console.Write(@"                                                                       /");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.Write(@"x");
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(@"\");
      Console.ForegroundColor = ConsoleColor.Green;
      Console.Write(@"                                                                      /");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.Write(@"x");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.Write(@"x");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(@"\");
      Console.Write(@"                                                                     /");
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.Write(@"o");
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine(@"\");
      Console.ForegroundColor = ConsoleColor.DarkRed;
      Console.WriteLine(@"                                                                        |||");
      Console.WriteLine(@"                                                                        |||");
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      Console.WriteLine(@"====================================================================================");
      Console.ResetColor();
      Console.WriteLine("Притиснете копче за понатаму");

      (int nSaveCursorX, int nSaveCursorY) = Console.GetCursorPosition();
      bool bRunning = true, bColorSwitch = true;
      int nNextSwitch = 0;
      while (bRunning) {
        Console.ForegroundColor = bColorSwitch ? ConsoleColor.Yellow : ConsoleColor.Red;
        for (int i = 72; i < 75; i += 2) {
          Console.SetCursorPosition(i, 12);
          Console.Write("o");
        }
        Console.ForegroundColor = bColorSwitch ? ConsoleColor.Red : ConsoleColor.Cyan;
        for (int i = 71; i < 76; i += 2) {
          Console.SetCursorPosition(i, 13);
          Console.Write("o");
        }
        Console.ForegroundColor = bColorSwitch ? ConsoleColor.Yellow : ConsoleColor.Magenta;
        for (int i = 70; i < 77; i += 2) {
          Console.SetCursorPosition(i, 14);
          Console.Write("o");
        }
        Console.SetCursorPosition(nSaveCursorX, nSaveCursorY);
        Thread.Sleep(50);
        nNextSwitch++;
        if (nNextSwitch % 8 == 0)
          bColorSwitch = !bColorSwitch;

        if (Console.KeyAvailable) {
          Console.ResetColor();
          bRunning = false;
        }
      }

      Console.ReadKey(intercept: true);
    }
  }
}
