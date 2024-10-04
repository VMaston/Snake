internal class Board
{
    public (int y, int x) dimensions;
    public List<(int y, int x)> availablePlaces = new List<(int y, int x)>();

    public Board(int boardY, int boardX)
    {
        dimensions = (boardY, boardX);
        DrawBoard();
    }

    public void DrawBoard()
    {
        for (int y = 0; y < dimensions.y; y++)
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                if (x == 0 || x == dimensions.x - 1)
                {
                    Console.Write('|');
                }
                else if (y == 0 || y == dimensions.y - 1)
                {
                    Console.Write('-');
                }
                else
                {
                    availablePlaces.Add((y, x));
                    Console.Write(' ');
                }
                if (x == dimensions.x - 1)
                {
                    Console.Write("\n");
                }
            }
        }
    }
}