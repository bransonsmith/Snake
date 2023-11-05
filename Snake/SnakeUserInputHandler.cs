namespace Snake
{

    public interface ISnakeUserInputHandler
    {
        public Direction CurrentDirection { get; set; }
        public void CheckUserInput();
    }

    public interface IConsoleSnakeUserInputHandler: ISnakeUserInputHandler { }

    public class ConsoleSnakeUserInputHandler : IConsoleSnakeUserInputHandler {
        public Direction CurrentDirection { get; set; } = Direction.Down;

        public Queue<Direction> DirectionInputQueue { get; set; }

        public void CheckUserInput()
        {
            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        CurrentDirection = Direction.Up; break;
                    case ConsoleKey.DownArrow:
                        CurrentDirection = Direction.Down; break;
                    case ConsoleKey.LeftArrow:
                        CurrentDirection = Direction.Left; break;
                    case ConsoleKey.RightArrow:
                        CurrentDirection = Direction.Right; break;
                    default:
                        break;
                }
            }
        }
    }

}
