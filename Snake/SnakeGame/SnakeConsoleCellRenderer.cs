using ConsoleGame;

namespace Snake
{
    public class SnakeConsoleCellRenderer : ICellRenderer
    {
        private Dictionary<SnakeCellState, ConsoleColor> CellStateColors = new Dictionary<SnakeCellState, ConsoleColor> {
                { SnakeCellState.Empty, ConsoleColor.White },
                { SnakeCellState.Snake, ConsoleColor.Green },
                { SnakeCellState.SnakeHead, ConsoleColor.Green },
                { SnakeCellState.Food, ConsoleColor.Red },
                { SnakeCellState.Obstacle, ConsoleColor.DarkGray }
            };

        private Dictionary<SnakeCellState, string> CellStateChars = new Dictionary<SnakeCellState, string> {
                { SnakeCellState.Empty, "  " },
                { SnakeCellState.Snake, "++" },
                { SnakeCellState.SnakeHead, "()" },
                { SnakeCellState.Food, "@@" },
                { SnakeCellState.Obstacle, "><" }
            };

        public void Render(ICell cell)
        {
            Console.SetCursorPosition(cell.X * 2, cell.Y);
            Console.BackgroundColor = CellStateColors[(SnakeCellState)cell.State];
            Console.ForegroundColor = CellStateColors[(SnakeCellState)cell.State];
            if (cell.State == (int)SnakeCellState.Empty)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            var cellChar = CellStateChars[(SnakeCellState)cell.State];
            Console.Write(cellChar);
            Console.CursorVisible = false;
        }
    }
}
