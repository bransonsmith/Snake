using ConsoleGame;

namespace Snake
{
    public class SnakeGameArea : IGameArea
    {
        public ICell[][] Cells { get; set; }

        public SnakeGameArea(int height, int width)
        {
            Cells = new Cell[height][];
            for (int y = 0; y < height; y++)
            {
                Cells[y] = new Cell[width];
                for (int x = 0; x < width; x++)
                {
                    Cells[y][x] = new Cell { X = x, Y = y, State = (int)SnakeCellState.Empty };
                }
            }
        }

        public ICell GetEmptyCell()
        {
            var emptyCells = Cells
                .SelectMany(subArray => subArray)
                .Where(cell => cell.State == (int)SnakeCellState.Empty)
                .ToList();

            if (!emptyCells.Any())
            {
                return null;
            }

            return emptyCells[new Random().Next(emptyCells.Count())];
        }

    }
}