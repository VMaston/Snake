internal class Snake
{
    public readonly List<(int y, int x)> SnakeCoords = new List<(int, int)>();

    public void DrawSnake()
    {
        Console.SetCursorPosition(SnakeCoords[0].x, SnakeCoords[0].y + Helpers.cursorY);
        Helpers.DrawColor(ConsoleColor.Green, "@");
        foreach (var (y, x) in SnakeCoords[1..])
        {
            Console.SetCursorPosition(x, y + Helpers.cursorY);
            Helpers.DrawColor(ConsoleColor.Green, "S");
        }
    }

    public void UpdateSnake()
    {
        if (SnakeCoords.Count > 1)
        {
            Console.SetCursorPosition(SnakeCoords[^1].x, SnakeCoords[^1].y + Helpers.cursorY);
            Console.Write(' ');
            SnakeCoords.RemoveAt(SnakeCoords.Count - 1);
        }

        DrawSnake();
    }

    public void UpdateSnakeHead(int y, int x)
    {
        SnakeCoords.Insert(0, (y, x));
    }
}