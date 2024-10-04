internal class Game
{
    public bool inPlay;

    public bool completedTurn;
    public int TurnTime;
    private readonly Board Board;
    private readonly Fruit Fruit;
    private Snake Snake { get; set; }

    public Game(int turnTime, Board board, Snake snake, Fruit fruit) {
        TurnTime = turnTime;
        Board = board;
        Snake = snake;
        Fruit = fruit;
    }

    public void UpdateGame()
    {
        //Win condition: if the snake equals the total amount of playable tiles.
        int totalTiles = (Board.dimensions.y - 2) * (Board.dimensions.x - 2);
        if (Snake.SnakeCoords.Count >= totalTiles)
        {
            Snake.DrawSnake();
            Console.SetCursorPosition(0, Helpers.cursorY + Board.dimensions.y);
            Console.WriteLine($"You won! {Snake.SnakeCoords.Count - 1} / {totalTiles - 1}");
            inPlay = false;
            return;
        }
        // Loss condition: if the snake collides with the wall.
        else if (Snake.SnakeCoords[0].y == 0 || Snake.SnakeCoords[0].y == Board.dimensions.y - 1 || Snake.SnakeCoords[0].x == 0 || Snake.SnakeCoords[0].x == Board.dimensions.x - 1)
        {
            DrawLoss();
            inPlay = false;
            return;
        }
        // Loss condition: if the snake collides with itself.
        else
        {
            if (Snake.SnakeCoords[1..].Contains((Snake.SnakeCoords[0].y, Snake.SnakeCoords[0].x)))
            {
                DrawLoss();
                inPlay = false;
                return;
            };
        }

        if (Snake.SnakeCoords[0].y == Fruit.fruitCoords.y && Snake.SnakeCoords[0].x == Fruit.fruitCoords.x)
        {
            UpdateAndDrawFruit();
        }

        Snake.UpdateSnake();
    }


    public void UpdateAndDrawFruit()
    {
        if (Snake.SnakeCoords.Count > 1)
        {
            Snake.SnakeCoords.Add((Snake.SnakeCoords[^1].y, Snake.SnakeCoords[^1].x));
        }
        Board.availablePlaces = Board.availablePlaces.Except(Snake.SnakeCoords).ToList();

        Random rand = new Random();

        int fruitXY = rand.Next(0, Board.availablePlaces.Count - 1);
        Console.SetCursorPosition(Board.availablePlaces[fruitXY].x, Board.availablePlaces[fruitXY].y + Helpers.cursorY);
        Helpers.DrawColor(ConsoleColor.Yellow, "?");

        Fruit.fruitCoords = Board.availablePlaces[fruitXY];
    }

    void DrawLoss()
    {
        Snake.UpdateSnake();
        int totalTiles = (Board.dimensions.y - 2) * (Board.dimensions.x - 2);
        Console.SetCursorPosition(Snake.SnakeCoords[0].x, Snake.SnakeCoords[0].y + Helpers.cursorY);
        Helpers.DrawColor(ConsoleColor.Red, "@");
        Console.SetCursorPosition(0, Helpers.cursorY + Board.dimensions.y);
        Console.WriteLine($"You lost! {Snake.SnakeCoords.Count - 1} / {totalTiles}");
    }
}