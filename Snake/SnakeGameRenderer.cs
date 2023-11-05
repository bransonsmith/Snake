namespace Snake
{

    public interface ISnakeGameRenderer
    {
        public void RenderCells(List<Cell> cells);
        public void RenderEntireSnakeGameArea(ISnakeGameArea snakeGameArea);
    }
    public interface IConsoleSnakeGameRenderer : ISnakeGameRenderer
    {
    }

    public class ConsoleSnakeGameRenderer : IConsoleSnakeGameRenderer
    {
        public IConsoleCellRenderer ConsoleCellRenderer { get; set; }
        public ConsoleSnakeGameRenderer(IConsoleCellRenderer consoleCellRenderer)
        {
            ConsoleCellRenderer = consoleCellRenderer;
        }

        public ConsoleSnakeGameRenderer()
        {
            Console.CursorVisible = false;
        }

        public void RenderCells(List<Cell> cells)
        {
            foreach (var cell in cells)
            {
                ConsoleCellRenderer.Render(cell);
            }
        }

        public void RenderEntireSnakeGameArea(ISnakeGameArea snakeGameArea)
        {
            foreach (var cellRow in snakeGameArea.Cells)
            {
                foreach (var cell in cellRow)
                {
                    ConsoleCellRenderer.Render(cell);
                }
            }
        }
    }
}
