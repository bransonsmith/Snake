using ConsoleGame;

namespace Snake
{
    /// <summary>
    /// Current Direction updated to most recent user input in a CheckUserInput call.
    /// Generally, CheckUserInput should be called in every game loop cycle.
    /// </summary>
    public interface ISnakeInputHandler
    {
        public Direction CurrentDirection { get; set; }
        public void CheckUserInput();
    }

    public class SnakeKeyboardInputHandler : ISnakeInputHandler
    {
        public Direction CurrentDirection { get; set; } = Direction.Down;

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
