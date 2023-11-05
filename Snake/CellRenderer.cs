namespace Snake
{
    public interface ICellRenderer
    {
        public void Render(Cell cell);
    }

    public interface IConsoleCellRenderer : ICellRenderer
    {

    }

    public class ConsoleCellRenderer : IConsoleCellRenderer
    {

        private Dictionary<CellState, ConsoleColor> CellStateColors = new Dictionary<CellState, ConsoleColor> {
                { CellState.Empty, ConsoleColor.Blue },
                { CellState.Snake, ConsoleColor.Green },
                { CellState.SnakeHead, ConsoleColor.Green },
                { CellState.Food, ConsoleColor.Red },
                { CellState.Obstacle, ConsoleColor.DarkGray }
            };

        private Dictionary<CellState, string> CellStateChars = new Dictionary<CellState, string> {
                { CellState.Empty, "  " },
                { CellState.Snake, "++" },
                { CellState.SnakeHead, "()" },
                { CellState.Food, "@@" },
                { CellState.Obstacle, "><" }
            };

        public void Render(Cell cell)
        {
            Console.SetCursorPosition(cell.X * 2, cell.Y);
            Console.BackgroundColor = CellStateColors[cell.State];
            Console.ForegroundColor = CellStateColors[cell.State];
            var cellChar = CellStateChars[cell.State];
            Console.Write(cellChar);
            Console.CursorVisible = false;
        }
    }
}
