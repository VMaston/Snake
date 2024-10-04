internal static class Helpers
{
    public static int cursorY;
    public static void DrawColor(ConsoleColor color, string input)
    {
        Console.ForegroundColor = color;
        Console.Write($"\x1b[1m{input}\x1b[0m");
        Console.ResetColor();
    }

    public static void SetCursorY(int y)
    {
        cursorY = Console.CursorTop - y;
    }
}