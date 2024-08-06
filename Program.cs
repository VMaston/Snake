class SnakeGame
{
    static void DrawColor(ConsoleColor color, string input)
    {
        Console.ForegroundColor = color;
        Console.Write($"\x1b[1m{input}\x1b[0m");
        Console.ResetColor();
    }

    static List<(int y, int x)> DrawBoard((int boardY, int boardX) boardDimensions)
    {
        List<(int y, int x)> availablePlaces = new List<(int y, int x)>();
        for (int y = 0; y < boardDimensions.boardY; y++)
        {
            for (int x = 0; x < boardDimensions.boardX; x++)
            {
                if (x == 0 || x == boardDimensions.boardX - 1)
                {
                    Console.Write('|');
                }
                else if (y == 0 || y == boardDimensions.boardY - 1)
                {
                    Console.Write('-');
                }
                else
                {
                    availablePlaces.Add((y, x));
                    Console.Write(' ');
                }
                if (x == boardDimensions.boardX - 1)
                {
                    Console.Write("\n");
                }
            }
        }

        return availablePlaces;
    }

    static void DrawLoss(List<(int y, int x)> snake, int cursorY, (int y, int x) boardDimensions)
    {
        UpdateSnake(snake, cursorY);
        int totalTiles = (boardDimensions.y - 2) * (boardDimensions.x - 2);
        Console.SetCursorPosition(snake[0].x, snake[0].y + cursorY);
        DrawColor(ConsoleColor.Red, "@");
        Console.SetCursorPosition(0, cursorY + boardDimensions.y);
        Console.WriteLine($"You lost! {snake.Count - 1} / {totalTiles}");
        Environment.Exit(1);
    }

    static (int y, int x) UpdateAndDrawFruit(List<(int y, int x)> snake, List<(int y, int x)> availablePlaces, int cursorY)
    {
        if (snake.Count > 1)
        {
            snake.Add((snake[^1].y, snake[^1].x));
        }
        availablePlaces = availablePlaces.Except(snake).ToList();

        Random rand = new Random();

        int fruitXY = rand.Next(0, availablePlaces.Count - 1);
        Console.SetCursorPosition(availablePlaces[fruitXY].x, availablePlaces[fruitXY].y + cursorY);
        DrawColor(ConsoleColor.Yellow, "?");

        return availablePlaces[fruitXY];
    }

    static void DrawSnake(List<(int y, int x)> snake, int cursorY)
    {
        Console.SetCursorPosition(snake[0].x, snake[0].y + cursorY);
        DrawColor(ConsoleColor.Green, "@");
        foreach (var (y, x) in snake[1..])
        {
            Console.SetCursorPosition(x, y + cursorY);
            DrawColor(ConsoleColor.Green, "S");
        }
    }

    static void UpdateSnake(List<(int y, int x)> snake, int cursorY)
    {
        if (snake.Count > 1)
        {
            Console.SetCursorPosition(snake[^1].x, snake[^1].y + cursorY);
            Console.Write(' ');
            snake.RemoveAt(snake.Count - 1);
        }
        DrawSnake(snake, cursorY);
    }


    static (int y, int x) UpdateGame(List<(int y, int x)> snake, int cursorY, (int y, int x) boardDimensions, List<(int y, int x)> availablePlaces, (int y, int x) fruit)
    {
        //Win condition: if the snake equals the total amount of playable tiles.
        int totalTiles = (boardDimensions.y - 2) * (boardDimensions.x - 2);
        if (snake.Count >= totalTiles)
        {
            DrawSnake(snake, cursorY);
            Console.SetCursorPosition(0, cursorY + boardDimensions.y);
            Console.WriteLine($"You won! {snake.Count - 1} / {totalTiles - 1}");
            Environment.Exit(1);
        }
        // Loss condition: if the snake collides with the wall.
        else if (snake[0].y == 0 || snake[0].y == boardDimensions.y - 1 || snake[0].x == 0 || snake[0].x == boardDimensions.x - 1)
        {
            DrawLoss(snake, cursorY, boardDimensions);
        }
        // Loss condition: if the snake collides with itself.
        else
        {
            if (snake[1..].Contains((snake[0].y, snake[0].x)))
            {
                DrawLoss(snake, cursorY, boardDimensions);
            };
        }

        if (snake[0].y == fruit.y && snake[0].x == fruit.x)
        {
            fruit = UpdateAndDrawFruit(snake, availablePlaces, cursorY);
        }

        UpdateSnake(snake, cursorY);

        return fruit;
    }
    static void Main(string[] args)
    {
        (int y, int x) boardDimensions = (10, 20);
        List<(int y, int x)> availablePlaces = DrawBoard(boardDimensions);

        //Create Tuple List for snake co-ordinates
        List<(int y, int x)> snake = new List<(int, int)>();
        Random rand = new Random();
        snake.Insert(0, (rand.Next(1, boardDimensions.y - 1), rand.Next(1, boardDimensions.x - 1)));

        //Establish console length for accurate cursor placement.
        //This way, we can directly move the cursor to the part that needs to change, reducing the need to redraw the board each tick.
        int cursorY = Console.CursorTop - boardDimensions.y;

        UpdateSnake(snake, cursorY);
        (int y, int x) fruit = UpdateAndDrawFruit(snake, availablePlaces, cursorY);


        //Game Loop Initializers
        bool completeTurn;
        int sleep = 0;
        ConsoleKeyInfo keyPress = default, lastPress = default;

        //Game Loop
        while (true)
        {
            //Ensure a turn completes before taking new input.
            completeTurn = false;

            //Buffer runs through the entire input stream to find the first non-duplicate input.
            //If not, the snake will lock-in the same input for multiple iterations if the button is held down.
            ConsoleKeyInfo buffer = default;
            while (Console.KeyAvailable && (buffer == default || buffer == lastPress))
            {
                buffer = Console.ReadKey(true);
            }

            if (lastPress == default || lastPress.Key == ConsoleKey.UpArrow || lastPress.Key == ConsoleKey.DownArrow)
            {
                if (lastPress == default || buffer.Key == ConsoleKey.LeftArrow || buffer.Key == ConsoleKey.RightArrow)
                {
                    sleep = 200;
                    keyPress = buffer;
                    lastPress = keyPress;
                }
            }
            else if (lastPress == default || lastPress.Key == ConsoleKey.LeftArrow || lastPress.Key == ConsoleKey.RightArrow)
            {
                if (lastPress == default || buffer.Key == ConsoleKey.UpArrow || buffer.Key == ConsoleKey.DownArrow)
                {
                    sleep = 200;
                    keyPress = buffer;
                    lastPress = keyPress;
                }
            }


            while (!Console.KeyAvailable || !completeTurn)
            {
                switch (keyPress.Key)
                {
                    case ConsoleKey.UpArrow:
                        snake.Insert(0, (snake[0].y - 1, snake[0].x));
                        break;
                    case ConsoleKey.DownArrow:
                        snake.Insert(0, (snake[0].y + 1, snake[0].x));
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.Insert(0, (snake[0].y, snake[0].x - 1));
                        break;
                    case ConsoleKey.RightArrow:
                        snake.Insert(0, (snake[0].y, snake[0].x + 1));
                        break;
                    default:
                        break;
                }
                fruit = UpdateGame(snake, cursorY, boardDimensions, availablePlaces, fruit);
                Thread.Sleep(sleep);
                completeTurn = true;
            }
        }
    }
}