partial class SnakeGame
{
    static void Main(string[] args)
    {
        //Create instance of Board with board dimensions
        Board board = new Board(10, 20);

        //Create instance of Snake
        Snake snake = new Snake();
        Random rand = new Random();
        snake.UpdateSnakeHead(rand.Next(1, board.dimensions.y - 1), rand.Next(1, board.dimensions.x - 1));

        //Create instance of Fruit
        Fruit fruit = new Fruit();

        //Establish console length for accurate cursor placement.
        //This way, we can directly move the cursor to the part that needs to change, reducing the need to redraw the board each tick.
        Helpers.SetCursorY(board.dimensions.y);

        //Game Loop Initializers
        Game game = new Game(200, board, snake, fruit);
        game.inPlay = true;

        snake.UpdateSnake();
        game.UpdateAndDrawFruit();

        ConsoleKeyInfo keyPress = default, lastPress = default;

        //Game Loop
        while (game.inPlay)
        {
            //Ensure a turn completes before taking new input.
            game.completedTurn = false;

            //Buffer runs through the entire input stream to find the first non-duplicate input.
            //If not, the snake will lock-in the same input for multiple iterations if the button is held down.
            ConsoleKeyInfo buffer = default;
            while (Console.KeyAvailable && (buffer == default || buffer == lastPress))
            {
                buffer = Console.ReadKey(true);
            }

            //Checking on default is asking if it's the first key press of the game, as there'd be no input stored in lastPress to check for.
            //These nested if statements ensure that you can only make valid moves and can not reverse your current direction into your own body.
            if (lastPress == default || lastPress.Key == ConsoleKey.UpArrow || lastPress.Key == ConsoleKey.DownArrow)
            {
                if (buffer.Key == ConsoleKey.LeftArrow || buffer.Key == ConsoleKey.RightArrow)
                {
                    keyPress = buffer;
                    lastPress = keyPress;
                }
            }
            else if (lastPress == default || lastPress.Key == ConsoleKey.LeftArrow || lastPress.Key == ConsoleKey.RightArrow)
            {
                if (buffer.Key == ConsoleKey.UpArrow || buffer.Key == ConsoleKey.DownArrow)
                {
                    keyPress = buffer;
                    lastPress = keyPress;
                }
            }

            while (game.inPlay && !Console.KeyAvailable || !game.completedTurn)
            {
                switch (keyPress.Key)
                {
                    case ConsoleKey.UpArrow:
                        snake.UpdateSnakeHead(snake.SnakeCoords[0].y - 1, snake.SnakeCoords[0].x);
                        break;
                    case ConsoleKey.DownArrow:
                        snake.UpdateSnakeHead(snake.SnakeCoords[0].y + 1, snake.SnakeCoords[0].x);
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.UpdateSnakeHead(snake.SnakeCoords[0].y, snake.SnakeCoords[0].x - 1);
                        break;
                    case ConsoleKey.RightArrow:
                        snake.UpdateSnakeHead(snake.SnakeCoords[0].y, snake.SnakeCoords[0].x + 1);
                        break;
                    default:
                        break;
                }
                game.UpdateGame();
                Thread.Sleep(game.TurnTime);
                game.completedTurn = true;
            }
        }
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
}